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
using System.Linq;

using HBase.Stargate.Client.Models;

namespace HBase.Stargate.Client.Config
{
	/// <summary>
	///    Provides an XML configuration-based <see cref="CellQuery" /> implementation.
	/// </summary>
	public class ConfigCellQuery : ConfigDescriptor
	{
		private const string _beginTimestampName = "beginTimestamp";
		private const string _endTimestampName = "endTimestamp";
		private const string _maxVersionsName = "maxVersions";
		private const string _cellsName = "filters";
		private const string _addCellName = "filter";
		private const string _removeCellName = "remove";
		private const string _clearCellsName = "clear";

		/// <summary>
		///    Gets or sets the begin timestamp (exclusive).
		/// </summary>
		/// <value>
		///    The timestamp.
		/// </value>
		[ConfigurationProperty(_beginTimestampName, IsRequired = false)]
		public long? BeginTimestamp
		{
			get { return this[_beginTimestampName] as long?; }
			set { this[_beginTimestampName] = value; }
		}

		/// <summary>
		///    Gets or sets the end timestamp (exclusive).
		/// </summary>
		/// <value>
		///    The timestamp.
		/// </value>
		[ConfigurationProperty(_endTimestampName, IsRequired = false)]
		public long? EndTimestamp
		{
			get { return this[_endTimestampName] as long?; }
			set { this[_endTimestampName] = value; }
		}

		/// <summary>
		///    Gets or sets the max versions.
		/// </summary>
		/// <value>
		///    The max versions.
		/// </value>
		[ConfigurationProperty(_maxVersionsName, IsRequired = false)]
		public int? MaxVersions
		{
			get { return this[_maxVersionsName] as int?; }
			set { this[_maxVersionsName] = value; }
		}

		/// <summary>
		///    Gets or sets the cells used as filters in the query.
		/// </summary>
		/// <value>
		///    The cells to use as filters in the query.
		/// </value>
		[ConfigurationProperty(_cellsName)]
		[ConfigurationCollection(typeof (ConfigCellDescriptor), AddItemName = _addCellName, RemoveItemName = _removeCellName,
			ClearItemsName = _clearCellsName)]
		public ConfigCellDescriptorCollection Cells
		{
			get { return this[_cellsName] as ConfigCellDescriptorCollection; }
			set { this[_cellsName] = value; }
		}

		/// <summary>
		///    Converts the config-based cell query to a normal one.
		/// </summary>
		/// <param name="query">The query.</param>
		public static implicit operator CellQuery(ConfigCellQuery query)
		{
			return new CellQuery
			{
				Table = query.Table,
				Row = query.Row,
				BeginTimestamp = query.BeginTimestamp,
				EndTimestamp = query.EndTimestamp,
				MaxVersions = query.MaxVersions,
				Cells = query.Cells.Cast<ConfigCellDescriptor>().Select(cell => (HBaseCellDescriptor) cell)
			};
		}
	}
}