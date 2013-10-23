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

using System.Net;

using HBase.Stargate.Client.Models;

using Patterns.Testing.Moq;

using RestSharp;

using TechTalk.SpecFlow;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class Administration
	{
		private readonly HBaseContext _hBase;
		private readonly ResourceContext _resources;
		private readonly IMoqContainer _container;

		public Administration(HBaseContext hBase, ResourceContext resources, IMoqContainer container)
		{
			_hBase = hBase;
			_resources = resources;
			_container = container;
		}

		[Given(@"I have created a new table schema")]
		public void CreateTableSchema()
		{
			_hBase.TableSchema = new TableSchema();
		}

		[Given(@"I have set my table schema's name to ""(.*)""")]
		public void SetTableSchemaName(string name)
		{
			_hBase.TableSchema.Name = name;
		}

		[Given(@"I have added a column named ""(.*)"" to my table schema")]
		public void AddColumnSchema(string name)
		{
			_hBase.TableSchema.Columns.Add(new ColumnSchema {Name = name});
		}

		[When(@"I create a table using my table schema")]
		public void CreateTableUsingSchema()
		{
			_hBase.Stargate.CreateTable(_hBase.TableSchema);
		}

		[When(@"I delete the ""(.*)"" table")]
		public void DeleteTable(string tableName)
		{
			_hBase.Stargate.DeleteTable(tableName);
		}

		[When(@"I get the schema of the ""(.*)"" table")]
		public void GetTableSchema(string tableName)
		{
			_hBase.Stargate.GetTableSchema(tableName);
		}

		[Given(@"I will always get a response with a status of ""(.*)"" and content equivalent to the resource called ""(.*)""")]
		public void SetupFakeResponse(HttpStatusCode responseStatus, string responseContentResource)
		{
			string content = _resources.GetString(responseContentResource);
			_container.Mock<IRestResponse>().SetupGet(response => response.StatusCode).Returns(responseStatus);
			_container.Mock<IRestResponse>().SetupGet(response => response.Content).Returns(content);
		}

	}
}