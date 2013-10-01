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
	///    Defines an identifier in the HBase system.
	/// </summary>
	public class Identifier
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="Identifier" /> class.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="qualifier">The qualifier.</param>
		/// <param name="timestamp">The timestamp.</param>
		public Identifier(string row, string column = null, string qualifier = null, long? timestamp = null)
		{
			Row = row;
			Column = column;
			Qualifier = qualifier;
			Timestamp = timestamp;
		}

		/// <summary>
		///    Gets the row.
		/// </summary>
		/// <value>
		///    The row.
		/// </value>
		public string Row { get; private set; }

		/// <summary>
		///    Gets the column.
		/// </summary>
		/// <value>
		///    The column.
		/// </value>
		public string Column { get; private set; }

		/// <summary>
		///    Gets the qualifier.
		/// </summary>
		/// <value>
		///    The qualifier.
		/// </value>
		public string Qualifier { get; private set; }

		/// <summary>
		///    Gets the timestamp.
		/// </summary>
		/// <value>
		///    The timestamp.
		/// </value>
		public long? Timestamp { get; private set; }
	}
}