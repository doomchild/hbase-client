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

using System;
using System.Linq;
using System.Text;

using HBase.Stargate.Client.Properties;

namespace HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides a basic implementation of <see cref="IResourceBuilder" />.
	/// </summary>
	public class ResourceBuilder : IResourceBuilder
	{
		private readonly IStargateOptions _options;
		private const string _wildCard = "*";
		private const string _appendSegmentFormat = "/{0}";
		private const string _appendQualifierFormat = ":{0}";
		private const string _appendRangeFormat = ",{0}";

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceBuilder"/> class.
		/// </summary>
		/// <param name="options">The HBase Stargate options.</param>
		public ResourceBuilder(IStargateOptions options)
		{
			_options = options;
		}

		/// <summary>
		///    Builds a cell or row query URI.
		/// </summary>
		/// <param name="query"></param>
		public string BuildCellOrRowQuery(CellQuery query)
		{
			if (!query.CanDescribeTable())
			{
				throw new ArgumentException(Resources.ResourceBuilder_MinimumForCellOrRowQueryNotMet);
			}

			return BuildFromCellQuery(query).ToString();
		}

		/// <summary>
		///    Builds a single value storage URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public string BuildSingleValueAccess(Identifier identifier)
		{
			if (!identifier.CanDescribeCell())
			{
				throw new ArgumentException(Resources.ResourceBuilder_MinimumForSingleValueAccessNotMet);
			}

			return BuildFromIdentifier(identifier).ToString();
		}

		/// <summary>
		///    Builds a delete-item URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		public string BuildDeleteItem(Identifier identifier)
		{
			if (!identifier.CanDescribeRow())
			{
				throw new ArgumentException(Resources.ResourceBuilder_MinimumForDeleteItemNotMet);
			}

			return BuildFromIdentifier(identifier).ToString();
		}

		/// <summary>
		/// Builds a batch insert URI.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public string BuildBatchInsert(Identifier identifier)
		{
			if (!identifier.CanDescribeTable())
			{
				throw new ArgumentException(Resources.ResourceBuilder_MinimumForBatchInsertNotMet);
			}

			return new StringBuilder(identifier.Table).AppendFormat(_appendSegmentFormat, _options.FalseRowKey).ToString();
		}

		private static StringBuilder BuildFromIdentifier(Identifier identifier)
		{
			bool hasTimestamp = identifier.Timestamp.HasValue;
			var uriBuilder = BuildFromDescriptor(identifier);

			bool columnMissing = identifier.Cell == null || string.IsNullOrEmpty(identifier.Cell.Column);
			if (columnMissing && !hasTimestamp)
			{
				return uriBuilder;
			}

			uriBuilder.AppendFormat(_appendSegmentFormat, columnMissing ? _wildCard : identifier.Cell.Column);

			if (!columnMissing && !string.IsNullOrEmpty(identifier.Cell.Qualifier))
			{
				uriBuilder.AppendFormat(_appendQualifierFormat, identifier.Cell.Qualifier);
			}

			if (hasTimestamp)
			{
				uriBuilder.AppendFormat(_appendSegmentFormat, identifier.Timestamp);
			}

			return uriBuilder;
		}

		private static StringBuilder BuildFromCellQuery(CellQuery query)
		{
			bool hasTimestamp = query.EndTimestamp.HasValue
				&& (!query.BeginTimestamp.HasValue || query.BeginTimestamp.Value < query.EndTimestamp.Value);

			var uriBuilder = BuildFromDescriptor(query);

			bool columnsMissing = query.Cells == null || query.Cells.All(cell => string.IsNullOrEmpty(cell.Column));
			if (columnsMissing && !hasTimestamp) return uriBuilder;

			if (!columnsMissing)
			{
				var validCells = query.Cells.Where(cell => !string.IsNullOrEmpty(cell.Column)).ToArray();

				var firstCell = validCells.First();
				uriBuilder.AppendFormat(_appendSegmentFormat, firstCell.Column);

				if (!string.IsNullOrEmpty(firstCell.Qualifier))
				{
					uriBuilder.AppendFormat(_appendQualifierFormat, firstCell.Qualifier);
				}

				foreach (HBaseCellDescriptor cell in validCells.Skip(1))
				{
					uriBuilder.AppendFormat(_appendRangeFormat, cell.Column);
					if (!string.IsNullOrEmpty(cell.Qualifier))
					{
						uriBuilder.AppendFormat(_appendQualifierFormat, cell.Qualifier);
					}
				}
			}
			else
			{
				uriBuilder.AppendFormat(_appendSegmentFormat, _wildCard);
			}

			if (hasTimestamp)
			{
				if (query.BeginTimestamp.HasValue)
				{
					uriBuilder.AppendFormat(_appendSegmentFormat, query.BeginTimestamp);
					uriBuilder.AppendFormat(_appendRangeFormat, query.EndTimestamp);
				}
				else
				{
					uriBuilder.AppendFormat(_appendSegmentFormat, query.EndTimestamp);
				}
			}

			return uriBuilder;
		}

		private static StringBuilder BuildFromDescriptor(HBaseDescriptor identifier)
		{
			return new StringBuilder(identifier.Table)
				.AppendFormat(_appendSegmentFormat, string.IsNullOrEmpty(identifier.Row) ? _wildCard : identifier.Row);
		}
	}
}