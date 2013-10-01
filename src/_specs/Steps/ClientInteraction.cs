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

using HBase.Stargate.Client;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using _specs.Models;

namespace _specs.Steps
{
	[Binding]
	public class ClientInteraction
	{
		private readonly HBaseContext _hBase;

		public ClientInteraction(HBaseContext hBase)
		{
			_hBase = hBase;
		}

		[Given(@"I have everything I need to test an HBase client in isolation, with the following options:")]
		public void SetupClient(Table options)
		{
			_hBase.Options = options.CreateInstance<HBaseOptions>();
		}

		[Given(@"I have an HBase client")]
		public void CreateClient()
		{
			_hBase.SetClient();
		}

		[Given(@"I have set my context to a table called ""(.*)""")]
		public void SetTableContextTo(string tableName)
		{
			_hBase.Table = _hBase.Stargate.ForTable(tableName);
		}

		[Given(@"I have set my context to a new scanner")]
		public void SetScannerContext()
		{
			_hBase.Scanner = _hBase.Table.ForScanner();
		}

		[Given(@"I have an identifier consisting of a (.+), a (.*), a (.*), and a (.*)")]
		public void SetIdentifier(string row, string column, string qualifier, string timestamp)
		{
			_hBase.Identifier = new Identifier(row, column, qualifier, timestamp.ToNullableLong());
		}
	}
}