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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HBase.Stargate.Client.MimeConversion
{
	/// <summary>
	///    Defines an XML implementation of <see cref="IMimeConverter" />.
	/// </summary>
	public class XmlMimeConverter : MimeConverterBase
	{
		private const string _cellSetName = "CellSet";
		private const string _rowName = "Row";
		private const string _keyName = "key";
		private const string _columnFormat = "{0}:{1}";
		private const string _columnName = "column";
		private const string _qualifierName = "qualifier";
		private const string _timestampName = "timestamp";
		private const string _cellName = "Cell";
		private const string _columnParserFormat = "(?<{0}>[^:]+):(?<{1}>.+)?";
		private static readonly Regex _columnParser = new Regex(string.Format(_columnParserFormat, _columnName, _qualifierName));

		/// <summary>
		///    Gets the current MIME type.
		/// </summary>
		/// <value>
		///    The MIME type.
		/// </value>
		public override string MimeType
		{
			get { return HBaseMimeTypes.Xml; }
		}

		/// <summary>
		///    Converts the specified cells to text according to the current MIME type.
		/// </summary>
		/// <param name="cells">The cells.</param>
		public override string Convert(IEnumerable<Cell> cells)
		{
			IDictionary<string, Cell[]> rows = cells
				.GroupBy(cell => cell.Identifier.Row)
				.ToDictionary(group => group.Key, group => group.ToArray());

			XElement xml = XmlForCellSet(rows.Keys
				.Select(row => XmlForRow(row, rows[row]
					.Select(cell => XmlForCell(cell.Identifier.Cell.Column, cell.Identifier.Cell.Qualifier, cell.Identifier.Timestamp, cell.Value)))));

			return xml.ToString();
		}

		/// <summary>
		///    Converts the specified cell to text according to the current MIME type.
		/// </summary>
		/// <param name="cell"></param>
		public override string Convert(Cell cell)
		{
			XElement xml = XmlForCellSet(new[]
			{
				XmlForRow(cell.Identifier.Row, new[]
				{
					XmlForCell(cell.Identifier.Cell.Column, cell.Identifier.Cell.Qualifier, cell.Identifier.Timestamp, cell.Value)
				})
			});

			return xml.ToString();
		}

		/// <summary>
		/// Converts the specified data to a set of cells according to the current MIME type.
		/// </summary>
		/// <param name="data">The data.</param>
		public override IEnumerable<Cell> Convert(string data)
		{
			if (string.IsNullOrEmpty(data)) return new CellSet(Enumerable.Empty<Cell>());

			return XElement.Parse(data).Elements(_rowName)
				.SelectMany(row => row.Elements(_cellName))
				.Select(CellForXml);
		}

		private static Cell CellForXml(XElement cell)
		{
			XElement parent = cell.Parent;
			if (parent == null) return null;

			XAttribute keyAttribute = parent.Attribute(_keyName);
			if (keyAttribute == null) return null;

			string row = Decode(keyAttribute.Value);
			ParsedColumn parsedColumn = ParseColumn(cell);

			XAttribute timestampAttribute = cell.Attribute(_timestampName);
			long? timestamp = timestampAttribute != null ? timestampAttribute.Value.ToNullableLong() : null;
			string value = Decode(cell.Value);

			return new Cell(new Identifier
			{
				Row = row,
				Cell = new HBaseCellDescriptor
				{
					Column = parsedColumn.Column,
					Qualifier = parsedColumn.Qualifier
				},
				Timestamp = timestamp
			}, value);
		}

		private static ParsedColumn ParseColumn(XElement cell)
		{
			XAttribute columnAttribute = cell.Attribute(_columnName);
			if (columnAttribute == null) return new ParsedColumn();

			string columnValue = Decode(columnAttribute.Value);
			Match match = _columnParser.Match(columnValue);
			if (!match.Success) return new ParsedColumn();

			string column = match.Groups[_columnName].Value;
			string qualifier = match.Groups[_qualifierName].Value;

			return new ParsedColumn(column, qualifier);
		}

		private static XElement XmlForCellSet(IEnumerable<XElement> rows)
		{
			return new XElement(_cellSetName, rows);
		}

		private static XElement XmlForRow(string row, IEnumerable<XElement> cells)
		{
			return new XElement(_rowName, new XAttribute(_keyName, Encode(row)), cells);
		}

		private static XElement XmlForCell(string column, string qualifier, long? timestamp, string value)
		{
			var cell = new XElement(_cellName, new XText(Encode(value)));
			if (!string.IsNullOrEmpty(column)) cell.Add(new XAttribute(_columnName, Encode(string.Format(_columnFormat, column, qualifier))));
			if (timestamp.HasValue) cell.Add(new XAttribute(_timestampName, timestamp.Value));
			return cell;
		}

		private struct ParsedColumn
		{
			public ParsedColumn(string column, string qualifier) : this()
			{
				Column = column;
				Qualifier = qualifier;
			}

			public string Column { get; private set; }
			public string Qualifier { get; private set; }
		}
	}
}