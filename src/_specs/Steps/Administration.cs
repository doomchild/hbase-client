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

using System;
using System.Collections.Generic;
using System.Net;

using HBase.Stargate.Client.Api;
using HBase.Stargate.Client.Models;

using Moq;

using Patterns.Testing.Moq;

using RestSharp;

using TechTalk.SpecFlow;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class Administration
	{
		private readonly IMoqContainer _container;
		private readonly HBaseContext _hBase;
		private readonly ErrorContext _errors;
		private readonly ResourceContext _resources;

		public Administration(HBaseContext hBase, ErrorContext errors, ResourceContext resources, IMoqContainer container)
		{
			_hBase = hBase;
			_errors = errors;
			_resources = resources;
			_container = container;
		}

		[Given(@"I have created a new table schema")]
		public void CreateTableSchema()
		{
			_hBase.TableSchema = new TableSchema();
		}

		[Given(@"I have set my table schema's name to ""(.*)""")]
		public void SetTableSchemaName(TestString name)
		{
			_hBase.TableSchema.Name = name;
		}

		[Given(@"I have added a column named ""(.*)"" to my table schema")]
		public void AddColumnSchema(TestString name)
		{
			List<ColumnSchema> columns = _hBase.TableSchema.Columns ?? (_hBase.TableSchema.Columns = new List<ColumnSchema>());
			columns.Add(new ColumnSchema {Name = name});
		}

		[When(@"I create a table using my table schema")]
		public void CreateTableUsingSchema()
		{
			try
			{
				_hBase.Stargate.CreateTable(_hBase.TableSchema);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] {error};
			}
		}

		[When(@"I delete the ""(.*)"" table")]
		public void DeleteTable(string tableName)
		{
			_hBase.Stargate.DeleteTable(tableName);
		}

		[When(@"I get the schema of the ""(.*)"" table")]
		public void GetTableSchema(string tableName)
		{
			try
			{
				_hBase.Stargate.GetTableSchema(tableName);
			}
			catch (Exception error)
			{
				_errors.CaughtErrors = new[] {error};
			}
		}

		[Given(@"I will always get a response with a status of ""(.*)"" and content equivalent to the resource called ""(.*)""")]
		public void SetupFakeResponseWithContent(HttpStatusCode responseStatus, string responseContentResource)
		{
			string content = _resources.GetString(responseContentResource);
			Mock<IRestResponse> responseMock = _container.Mock<IRestResponse>();
			responseMock.SetupGet(response => response.StatusCode).Returns(responseStatus);
			responseMock.SetupGet(response => response.Content).Returns(content);
		}

		[Given(@"I will always get a response with a status of ""(.*)"" and a location header of ""(.*)""")]
		public void SetupFakeResponseWithLocation(HttpStatusCode responseStatus, string location)
		{
			Mock<IRestResponse> responseMock = _container.Mock<IRestResponse>();
			responseMock.SetupGet(response => response.StatusCode).Returns(responseStatus);
			responseMock.SetupGet(response => response.Headers).Returns(new List<Parameter>
			{
				new Parameter {Name = RestConstants.LocationHeader, Type = ParameterType.HttpHeader, Value = location}
			});
		}

		[Given(@"I will always get a response with a response status of ""(.*)"" and error message equivalent to the resource called ""(.*)""")]
		public void SetupFakeResponseWithError(ResponseStatus status, string messageResource)
		{
			string message = _resources.GetString(messageResource);
			Mock<IRestResponse> responseMock = _container.Mock<IRestResponse>();
			responseMock.SetupGet(response => response.ResponseStatus).Returns(status);
			responseMock.SetupGet(response => response.ErrorException).Returns(new WebException(message));
		}
	}
}