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

namespace HBase.Stargate.Client
{
	/// <summary>
	/// Provides general extensions for Stargate Client components.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		///    Converts the current text to a nullable long value.
		/// </summary>
		/// <param name="text">The text.</param>
		public static long? ToNullableLong(this string text)
		{
			if (string.IsNullOrWhiteSpace(text)) return null;

			long value;
			return long.TryParse(text, out value) ? value : (long?) null;
		}

		/// <summary>
		/// Determines whether the descriptor can describe a table.
		/// </summary>
		/// <param name="descriptor">The descriptor.</param>
		public static bool CanDescribeTable(this HBaseDescriptor descriptor)
		{
			return descriptor != null && !string.IsNullOrEmpty(descriptor.Table);
		}

		/// <summary>
		/// Determines whether the descriptor can describe a row.
		/// </summary>
		/// <param name="descriptor">The descriptor.</param>
		public static bool CanDescribeRow(this HBaseDescriptor descriptor)
		{
			return descriptor.CanDescribeTable() && !string.IsNullOrEmpty(descriptor.Row);
		}

		/// <summary>
		/// Determines whether the identifier can describe a cell.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public static bool CanDescribeCell(this Identifier identifier)
		{
			return identifier.CanDescribeRow() && identifier.Cell != null && !string.IsNullOrEmpty(identifier.Cell.Column);
		}

		/// <summary>
		/// Converts the identifier into a cell query.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public static CellQuery ToQuery(this Identifier identifier)
		{
			return new CellQuery
			{
				Table = identifier.Table,
				Row = identifier.Row,
				Cells = new[]
				{
					new HBaseCellDescriptor
					{
						Column = identifier.Cell != null ? identifier.Cell.Column : null,
						Qualifier = identifier.Cell != null ? identifier.Cell.Qualifier : null
					}
				},
				BeginTimestamp = identifier.Timestamp.HasValue ? identifier.Timestamp - 1 : null,
				EndTimestamp = identifier.Timestamp.HasValue ? identifier.Timestamp + 1 : null
			};
		}
	}
}