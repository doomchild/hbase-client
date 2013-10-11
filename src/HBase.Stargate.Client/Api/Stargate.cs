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

using System.Linq;
using System.Net;
using System.Threading.Tasks;

using HBase.Stargate.Client.MimeConversion;

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
		private readonly IResourceBuilder _resourceBuilder;
		private readonly IRestSharpFactory _restSharp;
		private readonly IErrorProvider _errorProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="Stargate" /> class.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <param name="resourceBuilder">The resource builder.</param>
		/// <param name="restSharp">The RestSharp factory.</param>
		/// <param name="converterFactory">The converter factory.</param>
		/// <param name="errorProvider">The error provider.</param>
		public Stargate(IStargateOptions options, IResourceBuilder resourceBuilder, IRestSharpFactory restSharp, IMimeConverterFactory converterFactory, IErrorProvider errorProvider)
		{
			_resourceBuilder = resourceBuilder;
			_restSharp = restSharp;
			_errorProvider = errorProvider;
			_client = _restSharp.CreateClient(options.ServerUrl);
			_converter = converterFactory.CreateConverter(options.ContentType);
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
			string content = _converter.Convert(new Cell(identifier, value));
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
			IRestResponse response = await SendRequest(Method.POST, resource, contentType, contentType, _converter.Convert(cells));
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
			string hbaseIdentifier = identifier.Timestamp.HasValue
				? _resourceBuilder.BuildCellOrRowQuery(identifier.ToQuery())
				: _resourceBuilder.BuildSingleValueAccess(identifier);

			IRestResponse response = await SendRequest(Method.GET, hbaseIdentifier, Options.ContentType);
			_errorProvider.ThrowIfStatusMismatch(response, HttpStatusCode.OK, HttpStatusCode.NotFound);

			return _converter.Convert(response.Content)
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

			return new CellSet(_converter.Convert(response.Content))
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
				new XmlMimeConverter()
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
	}
}