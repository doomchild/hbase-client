#region FreeBSD

// Copyright (c) 2014, The Tribe
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

using HBase.Stargate.Client.Models;

namespace _specs.Models
{
	public class TestCell : TestDescriptor
	{
		public TestCell() {}

		public TestCell(string row, string column, string qualifier, string timestamp, string value)
		{
			Row = row;
			Column = column;
			Qualifier = qualifier;
			Timestamp = timestamp.ToNullableInt64();
			Value = value;
		}

		public string Value { get; set; }

		public static implicit operator Cell(TestCell instance)
		{
			return new Cell(new Identifier
			{
				Table = instance.Table,
				Row = instance.Row,
				CellDescriptor = new HBaseCellDescriptor
				{
					Column = instance.Column,
					Qualifier = instance.Qualifier
				},
				Timestamp = instance.Timestamp
			}, instance.Value);
		}

		public static implicit operator TestCell(Cell instance)
		{
			return new TestCell
			{
				Table = instance.Identifier.Table,
				Row = instance.Identifier.Row,
				Column = instance.Identifier.CellDescriptor.Column,
				Qualifier = instance.Identifier.CellDescriptor.Qualifier,
				Timestamp = instance.Identifier.Timestamp,
				Value = instance.Value
			};
		}
	}
}