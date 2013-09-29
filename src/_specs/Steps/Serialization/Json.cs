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

using TechTalk.SpecFlow;

namespace _specs.Steps.Serialization
{
	[Binding]
	public class Json
	{
		[When(@"I convert my cell to JSON")]
		public void ConvertCellToJson()
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"my JSON content should be equivalent to the resource called ""(.*)""")]
		public void CompareJsonToResource(string p0)
		{
			ScenarioContext.Current.Pending();
		}

		[Given(@"I have JSON content equal to the resource called ""(.*)""")]
		public void SetJsonToResource(string p0)
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I convert my JSON to a cell")]
		public void ConvertJsonToCell()
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I convert my set of cells to JSON")]
		public void ConvertCellSetToJson()
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I convert my JSON to a set of cells")]
		public void ConvertJsonToCellSet()
		{
			ScenarioContext.Current.Pending();
		}
	}
}