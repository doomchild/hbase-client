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

using FluentAssertions;

using HBase.Stargate.Client.Models;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class Identification
	{
		private readonly HBaseContext _hBase;

		public Identification(HBaseContext hBase)
		{
			_hBase = hBase;
		}

		[Given(@"I have an identifier consisting of a ([^,]*)")]
		public void SetIdentifier(string table)
		{
			_hBase.Identifier = new Identifier {Table = table};
		}

		[Given(@"I have an identifier consisting of a ([^,]*), a ([^,]*), a ([^,]*), a ([^,]*), and a ([^,]*)")]
		public void SetIdentifier(string table, string row, string column, string qualifier, string timestamp)
		{
			_hBase.Identifier = new Identifier
			{
				Table = table,
				Row = row,
				CellDescriptor = new HBaseCellDescriptor
				{
					Column = column,
					Qualifier = qualifier
				},
				Timestamp = timestamp.ToNullableInt64()
			};
		}

		[Given(@"I have an identifier consisting of a ([^,]*), a ([^,]*), a ([^,]*), and a ([^,]*)")]
		public void SetIdentifier(string table, string row, string column, string qualifier)
		{
			_hBase.Identifier = new Identifier
			{
				Table = table,
				Row = row,
				CellDescriptor = new HBaseCellDescriptor
				{
					Column = column,
					Qualifier = qualifier
				}
			};
		}

		[Given(@"I have an identifier consisting of the following values:")]
		public void SetIdentifier(Table options)
		{
			_hBase.Identifier = GetIdentifier(options);
		}

		[Given(@"I have a second identifier consisting of the following values:")]
		public void SetSecondIdentifier(Table options)
		{
			_hBase.SecondIdentifier = GetIdentifier(options);
		}

		[When(@"I match the second identifier to the first one")]
		public void MatchIdentifiers()
		{
			_hBase.IdentifierMatchResult = _hBase.Identifier.Matches(_hBase.SecondIdentifier);
		}

		[Then(@"the identifier match should( not)? have succeeded")]
		public void AssertMatchResults(string negativeModifier)
		{
			_hBase.IdentifierMatchResult.Should().Be(string.IsNullOrEmpty(negativeModifier));
		}

		private static Identifier GetIdentifier(Table options)
		{
			var descriptor = options.CreateInstance<TestDescriptor>();

			var identifier = new Identifier
			{
				Table = descriptor.Table,
				Row = descriptor.Row,
				CellDescriptor = new HBaseCellDescriptor
				{
					Column = descriptor.Column,
					Qualifier = descriptor.Qualifier
				},
				Timestamp = descriptor.Timestamp
			};
			return identifier;
		}
	}
}