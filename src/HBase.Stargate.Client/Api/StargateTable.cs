﻿#region FreeBSD

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

namespace HBase.Stargate.Client.Api
{
	/// <summary>
	///    Defines a default implementation of <see cref="IStargateTable" />.
	/// </summary>
	public class StargateTable : IStargateTable
	{
		private readonly string _serverUrl;
		private readonly string _tableName;
		private readonly Func<string, string, string, IStargateScanner> _scannerFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="StargateTable" /> class.
		/// </summary>
		/// <param name="serverUrl">The server URL.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="scannerFactory">The scanner factory.</param>
		public StargateTable(string serverUrl, string tableName, Func<string, string, string, IStargateScanner> scannerFactory)
		{
			_serverUrl = serverUrl;
			_tableName = tableName;
			_scannerFactory = scannerFactory;
		}

		/// <summary>
		/// Creates a scanner context for the current table.
		/// </summary>
		/// <param name="scannerId"></param>
		public IStargateScanner ForScanner(string scannerId = null)
		{
			return _scannerFactory(_serverUrl, _tableName, scannerId);
		}
	}
}