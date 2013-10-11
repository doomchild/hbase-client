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
using System.Linq;

using FluentAssertions;

using HBase.Stargate.Client;
using HBase.Stargate.Client.Api;
using HBase.Stargate.Client.MimeConversion;

using Patterns.Testing.Moq;

using RestSharp;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class RequestVerification
	{
		private readonly IMimeConverterFactory _converterFactory;
		private readonly RestContext _rest;

		public RequestVerification(RestContext rest, IMoqContainer container)
		{
			_rest = rest;
			_converterFactory = container.Create<IMimeConverterFactory>();
		}

		[Then(@"a REST request should have been submitted with the following values:")]
		public void CheckRequest(Table values)
		{
			var properties = values.CreateInstance<ExpectedRequestProperties>();
			_rest.Request.Method.Should().Be(properties.Method);
			_rest.Request.Resource.Should().Be(properties.Resource);
		}

		[Then(@"a REST request should have been submitted with the correct (.+) and (.+)")]
		public void CheckRequest(Method method, string resource)
		{
			_rest.Request.Method.Should().Be(method);
			_rest.Request.Resource.Should().Be(resource);
		}

		[Then(@"the REST request should have contained (.*) cells?")]
		public void CheckRequestCellCount(int count)
		{
			IEnumerable<Cell> row = GetRowFromRequest();
			row.Should().HaveCount(count);
		}

		[Then(@"one of the cells in the REST request should have had the value ""(.*)""")]
		public void CheckAnyCellValue(string value)
		{
			IEnumerable<Cell> row = GetRowFromRequest();
			row.SingleOrDefault(cell => cell.Value == value).Should().NotBeNull();
		}

		[Then(@"one of the cells in the REST request should have had the following values:")]
		public void CheckAnyCellValues(Table values)
		{
			IEnumerable<Cell> row = GetRowFromRequest();
			var expected = values.CreateInstance<TestCell>();
			row.SingleOrDefault(cell => cell.Identifier.Row == expected.Row
				&& cell.Identifier.Cell.Column == expected.Column
				&& cell.Identifier.Cell.Qualifier == expected.Qualifier
				&& cell.Value == expected.Value).Should().NotBeNull();
		}

		private IEnumerable<Cell> GetRowFromRequest()
		{
			IMimeConverter converter = CreateRequestConverter();
			string content = _rest.Request.Parameters
				.Where(cell => cell.Type == ParameterType.RequestBody)
				.Select(cell => cell.Value.ToString())
				.FirstOrDefault();
			IEnumerable<Cell> row = converter.Convert(content).ToArray();
			row.Should().NotBeNull();
			return row;
		}

		private IMimeConverter CreateRequestConverter()
		{
			string mimeType = _rest.Request.Parameters
				.Where(parameter => parameter.Type == ParameterType.HttpHeader && IsAcceptOrContentType(parameter.Name))
				.Select(parameter => parameter.Value.ToString())
				.FirstOrDefault();

			mimeType.Should().NotBeEmpty();

			return _converterFactory.CreateConverter(mimeType);
		}

		private static bool IsAcceptOrContentType(string name)
		{
			return name == RestConstants.ContentTypeHeader
				|| name == RestConstants.AcceptHeader;
		}
	}
}