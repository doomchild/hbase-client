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

using HBase.Stargate.Client;
using HBase.Stargate.Client.Api;

using RestSharp.Injection;

namespace _specs.Models
{
	public class HBaseContext
	{
		public CellSet CellSet { get; set; }

		public Cell Cell { get; set; }

		public string RawContent { get; set; }

		public HBaseOptions Options { get; set; }

		public IStargate Stargate { get; private set; }

		public IStargateTable Table { get; set; }

		public Identifier Identifier { get; set; }

		public IStargateScanner Scanner { get; set; }

		public void SetupClientInIsolation()
		{
			//TODO: setup anything needed?
		}

		public void SetClient()
		{
			Stargate = new Stargate(Options.ServerUrl, (s, s1) => new StargateTable(s, s1, (s2, s3, arg3) => new StargateScanner(s2, s3, arg3, new DefaultRestSharpFactory())));
		}
	}
}