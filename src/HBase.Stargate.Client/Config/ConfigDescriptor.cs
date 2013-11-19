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

using System.Configuration;

using HBase.Stargate.Client.Models;

namespace HBase.Stargate.Client.Config
{
	/// <summary>
	///    Provides a base type for XML configuration-based <see cref="HBaseDescriptor" /> implementations.
	/// </summary>
	public abstract class ConfigDescriptor : ConfigurationElement
	{
		private const string _tableName = "table";
		private const string _rowName = "row";

		/// <summary>
		///    Gets or sets the table.
		/// </summary>
		/// <value>
		///    The table.
		/// </value>
		[ConfigurationProperty(_tableName, IsRequired = true)]
		public string Table
		{
			get { return this[_tableName] as string; }
			set { this[_tableName] = value; }
		}

		/// <summary>
		///    Gets or sets the row.
		/// </summary>
		/// <value>
		///    The row.
		/// </value>
		[ConfigurationProperty(_rowName, IsRequired = true)]
		public string Row
		{
			get { return this[_rowName] as string; }
			set { this[_rowName] = value; }
		}

		/// <summary>
		///    Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only.
		/// </summary>
		/// <returns>
		///    true if the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}
	}
}