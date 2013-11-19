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

namespace HBase.Stargate.Client.Config
{
	/// <summary>
	///    Provides a collection of <see cref="ConfigCellDescriptor" /> elements.
	/// </summary>
	public class ConfigCellDescriptorCollection : ConfigurationElementCollection
	{
		/// <summary>
		///    Creates a new <see cref="ConfigCellDescriptor" />.
		/// </summary>
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConfigCellDescriptor();
		}

		/// <summary>
		///    Gets the element key for a specified configuration element.
		/// </summary>
		/// <param name="element">
		///    The <see cref="ConfigurationElement" /> to return the key for.
		/// </param>
		protected override object GetElementKey(ConfigurationElement element)
		{
			var descriptor = element as ConfigCellDescriptor;

			return descriptor == null ? new object() : string.Format("{0}:{1}", descriptor.Column, descriptor.Qualifier);
		}

		/// <summary>
		///    Indicates whether the <see cref="T:System.Configuration.ConfigurationElementCollection" /> object is read only.
		/// </summary>
		/// <returns>
		///    true if the <see cref="T:System.Configuration.ConfigurationElementCollection" /> object is read only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}
	}
}