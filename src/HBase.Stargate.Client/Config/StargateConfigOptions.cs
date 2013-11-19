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

using HBase.Stargate.Client.Api;

namespace HBase.Stargate.Client.Config
{
	/// <summary>
	///    Provides an XML configuration-based implementation of <see cref="IStargateOptions" />.
	/// </summary>
	public class StargateConfigOptions : ConfigurationElement, IStargateOptions
	{
		private const string _serverUrlName = "serverUrl";
		private const string _contentTypeName = "contentType";
		private const string _falseRowKeyName = "falseRowKey";

		/// <summary>
		///    Gets or sets the server URL.
		/// </summary>
		/// <value>
		///    The server URL.
		/// </value>
		[ConfigurationProperty(_serverUrlName, IsRequired = true)]
		public string ServerUrl
		{
			get { return this[_serverUrlName] as string; }
			set { this[_serverUrlName] = value; }
		}

		/// <summary>
		///    Gets or sets the type of the content.
		/// </summary>
		/// <value>
		///    The type of the content.
		/// </value>
		[ConfigurationProperty(_contentTypeName, IsRequired = false, DefaultValue = Api.Stargate.DefaultContentType)]
		public string ContentType
		{
			get { return this[_contentTypeName] as string; }
			set { this[_contentTypeName] = value; }
		}

		/// <summary>
		///    Gets or sets the false row key.
		/// </summary>
		/// <value>
		///    The false row key.
		/// </value>
		[ConfigurationProperty(_falseRowKeyName, IsRequired = false, DefaultValue = Api.Stargate.DefaultFalseRowKey)]
		public string FalseRowKey
		{
			get { return this[_falseRowKeyName] as string; }
			set { this[_falseRowKeyName] = value; }
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