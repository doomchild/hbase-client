#region FreeBSD

// Copyright (c) 2013, The Tribe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using HBase.Stargate.Client.MimeConversion;
using HBase.Stargate.Client.Models;

using RestSharp;
using RestSharp.Injection;

namespace HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides a default implementation <see cref="IStargate" />.
	/// </summary>
	public class Stargate : IStargate
	{
		/// <summary>
		///    The default false row key
		/// </summary>
		public const string DefaultFalseRowKey = "row";

		/// <summary>
		///    The default content type
		/// </summary>
		public const string DefaultContentType = HBaseMimeTypes.Xml;

		private readonly IRestClient _client;
		private readonly IMimeConverter _converter;
		private readonly IErrorProvider _errorProvider;
		private readonly IResourceBuilder _resourceBuilder;
		private readonly IRestSharpFactory _restSharp;

		/// <summary>
		///    Initializes a new instance of the <see cref="Stargate" /> class.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <param name="resourceBuilder">The resource builder.</param>
		/// <param name="restSharp">The RestSharp factory.</param>
		/// <param name="converterFactory">The converter factory.</param>
		/// <param name="errorProvider">The error provider.</param>
		public Stargate(IStargateOptions options, IResourceBuilder resourceBuilder, IRestSharpFactory restSharp, IMimeConverterFactory converterFactory,
			IErrorProvider errorProvider)
		{
			_resourceBuilder = resourceBuilder;
			_restSharp = restSharp;
			_errorProvider = errorProvider;
			_client = _restSharp.CreateClient(options.ServerUrl);
			options.ContentType = string.IsNullOrEmpty(options.ContentType) ? DefaultContentType : options.ContentType;
			_converter = converterFactory.CreateConverter(options.ContentType);
			options.FalseRowKey = string.IsNullOrEmpty(options.FalseRowKey) ? DefaultFalseRowKey : options.FalseRowKey;
			Options = options;
		}

		/// <summary>
		///    Gets the options.
		/// </summary>
		/// <value>
		///    The options.
		/// </value>
		protected virtual IStargateOptions Options { get; private set; }

		/// <summary>
		///    Writes the value to HBase using the identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="value">The value.</param>
		public virtual async Task WriteValueAsync(Identifier identifier, string value)
		{
			string contentType = Options.ContentType;
			string resource = _resourceBuilder.BuildSingleValueAccess(identifier);
			string content = _converter.ConvertCell(new Cell(identifier, value));
			IRestResponse response = await SendRequest(Method.POST, resource, contentType, contentType, content);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
		}

		/// <summary>
		///    Writes the value to HBase using the identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <param name="value">The value.</param>
		public void WriteValue(Identifier identifier, string value)
		{
			WriteValueAsync(identifier, value).Wait();
		}

		/// <summary>
		///    Writes the cells to HBase.
		/// </summary>
		/// <param name="cells">The cells.</param>
		public async Task WriteCellsAsync(CellSet cells)
		{
			string contentType = Options.ContentType;
			var tableIdentifier = new Identifier {Table = cells.Table};
			string resource = _resourceBuilder.BuildBatchInsert(tableIdentifier);
			IRestResponse response = await SendRequest(Method.POST, resource, contentType, contentType, _converter.ConvertCells(cells));
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
		}

		/// <summary>
		///    Writes the cells to HBase.
		/// </summary>
		/// <param name="cells">The cells.</param>
		public void WriteCells(CellSet cells)
		{
			WriteCellsAsync(cells).Wait();
		}

		/// <summary>
		///    Deletes the item with the matching identifier from HBase.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public async Task DeleteItemAsync(Identifier identifier)
		{
			string resource = _resourceBuilder.BuildDeleteItem(identifier);
			IRestResponse response = await SendRequest(Method.DELETE, resource, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
		}

		/// <summary>
		///    Deletes the item with the matching identifier from HBase.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public void DeleteItem(Identifier identifier)
		{
			DeleteItemAsync(identifier).Wait();
		}

		/// <summary>
		///    Reads the value with the matching identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public async Task<string> ReadValueAsync(Identifier identifier)
		{
			string resource = identifier.Timestamp.HasValue
				? _resourceBuilder.BuildCellOrRowQuery(identifier.ToQuery())
				: _resourceBuilder.BuildSingleValueAccess(identifier, true);

			IRestResponse response = await SendRequest(Method.GET, resource, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK, HttpStatusCode.NotFound);

			return _converter.ConvertCells(response.Content)
				.Select(cell => cell.Value)
				.FirstOrDefault();
		}

		/// <summary>
		///    Reads the value with the matching identifier.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public string ReadValue(Identifier identifier)
		{
			return ReadValueAsync(identifier).Result;
		}

		/// <summary>
		///    Finds the cells matching the query.
		/// </summary>
		/// <param name="query"></param>
		public async Task<CellSet> FindCellsAsync(CellQuery query)
		{
			string resource = _resourceBuilder.BuildCellOrRowQuery(query);
			IRestResponse response = await SendRequest(Method.GET, resource, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK, HttpStatusCode.NotFound);

			return new CellSet(_converter.ConvertCells(response.Content))
			{
				Table = query.Table
			};
		}

		/// <summary>
		///    Finds the cells matching the query.
		/// </summary>
		/// <param name="query"></param>
		public CellSet FindCells(CellQuery query)
		{
			return FindCellsAsync(query).Result;
		}

		/// <summary>
		///    Creates the table.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		public void CreateTable(TableSchema tableSchema)
		{
			CreateTableAsync(tableSchema).Wait();
		}

		/// <summary>
		///    Creates the table.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		public async Task CreateTableAsync(TableSchema tableSchema)
		{
			string resource = _resourceBuilder.BuildTableSchemaAccess(tableSchema);
			string data = _converter.ConvertSchema(tableSchema);
			IRestResponse response = await SendRequest(Method.PUT, resource, Options.ContentType, Options.ContentType, data);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
		}

		/// <summary>
		///    Gets the table names.
		/// </summary>
		public IEnumerable<string> GetTableNames()
		{
			return GetTableNamesAsync().Result;
		}

		/// <summary>
		///    Gets the table names.
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<string>> GetTableNamesAsync()
		{
			IRestResponse response = await SendRequest(Method.GET, string.Empty, HBaseMimeTypes.Text);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
			return ParseLines(response.Content);
		}

		/// <summary>
		///    Deletes the table.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public void DeleteTable(string tableName)
		{
			DeleteTableAsync(tableName).Wait();
		}

		/// <summary>
		///    Deletes the table.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public async Task DeleteTableAsync(string tableName)
		{
			string resource = _resourceBuilder.BuildTableSchemaAccess(new TableSchema {Name = tableName});
			IRestResponse response = await SendRequest(Method.DELETE, resource, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
		}

		/// <summary>
		///    Gets the table schema async.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public async Task<TableSchema> GetTableSchemaAsync(string tableName)
		{
			string resource = _resourceBuilder.BuildTableSchemaAccess(new TableSchema {Name = tableName});
			IRestResponse response = await SendRequest(Method.GET, resource, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK);
			return _converter.ConvertSchema(response.Content);
		}

		/// <summary>
		///    Gets the table schema.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public TableSchema GetTableSchema(string tableName)
		{
			return GetTableSchemaAsync(tableName).Result;
		}

		/// <summary>
		///    Creates a new stargate with the specified options.
		/// </summary>
		/// <param name="serverUrl">The server URL.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="falseRowKey">The false row key.</param>
		public static IStargate Create(string serverUrl, string contentType = DefaultContentType, string falseRowKey = DefaultFalseRowKey)
		{
			return Create(new StargateOptions {ServerUrl = serverUrl, ContentType = contentType, FalseRowKey = falseRowKey});
		}

		/// <summary>
		///    Creates a new stargate with the specified options.
		/// </summary>
		/// <param name="options">The options.</param>
		public static IStargate Create(IStargateOptions options)
		{
			var resourceBuilder = new ResourceBuilder(options);
			var restSharp = new RestSharpFactory(url => new RestClient(url), (resource, method) => new RestRequest(resource, method));
			var mimeConverters = new MimeConverterFactory(new[]
			{
				new XmlMimeConverter(new SimpleValueConverter())
			});
			var errors = new ErrorProvider();

			options.ContentType = string.IsNullOrEmpty(options.ContentType)
				? DefaultContentType
				: options.ContentType;

			options.FalseRowKey = string.IsNullOrEmpty(options.FalseRowKey)
				? DefaultFalseRowKey
				: options.FalseRowKey;

			return new Stargate(options, resourceBuilder, restSharp, mimeConverters, errors);
		}

		/// <summary>
		///    Sends the request.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="resource">The resource.</param>
		/// <param name="acceptType">Type of the accept.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="content">The content.</param>
		protected virtual Task<IRestResponse> SendRequest(Method method, string resource, string acceptType,
			string contentType = null, string content = null)
		{
			return Task.Run(() =>
			{
				IRestRequest request = _restSharp.CreateRequest(resource, method)
					.AddHeader(RestConstants.AcceptHeader, acceptType);

				if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(contentType))
				{
					request.AddParameter(contentType, content, ParameterType.RequestBody);
				}

				return _client.Execute(request);
			});
		}

		private static IEnumerable<string> ParseLines(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}

			using (var reader = new StringReader(text))
			{
				string line;
				while ((line = reader.ReadLine()) != null) yield return line;
			}
		}
	}
}