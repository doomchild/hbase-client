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
	///    Provides an XML configuration-based implementation <see cref="ConfigurationElement" />.
	/// </summary>
	public class ConfigCellDescriptor : ConfigurationElement
	{
		private const string _columName = "column";
		private const string _qualifierName = "qualifier";

		/// <summary>
		///    Gets or sets the column.
		/// </summary>
		/// <value>
		///    The column.
		/// </value>
		[ConfigurationProperty(_columName, IsRequired = true)]
		public string Column
		{
			get { return this[_columName] as string; }
			set { this[_columName] = value; }
		}

		/// <summary>
		///    Gets or sets the qualifier.
		/// </summary>
		/// <value>
		///    The qualifier.
		/// </value>
		[ConfigurationProperty(_qualifierName, IsRequired = false)]
		public string Qualifier
		{
			get { return this[_qualifierName] as string; }
			set { this[_qualifierName] = value; }
		}

		/// <summary>
		///    Converts the config-based cell descriptor to a normal one.
		/// </summary>
		/// <param name="descriptor">The cell descriptor.</param>
		public static implicit operator HBaseCellDescriptor(ConfigCellDescriptor descriptor)
		{
			return new HBaseCellDescriptor
			{
				Column = descriptor.Column,
				Qualifier = descriptor.Qualifier
			};
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