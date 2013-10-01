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
using System.Collections.Generic;

using RestSharp.Injection;

namespace HBase.Stargate.Client.Api
{
	/// <summary>
	///    Defines the default implementation of <see cref="IStargateScanner" />.
	/// </summary>
	public class StargateScanner : StargateBase, IStargateScanner
	{
		private readonly string _tableName;
		private string _scannerId;

		/// <summary>
		/// Initializes a new instance of the <see cref="StargateScanner" /> class.
		/// </summary>
		/// <param name="serverUrl">The server URL.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="scannerId">The scanner id.</param>
		/// <param name="restFactory">The rest factory.</param>
		public StargateScanner(string serverUrl, string tableName, string scannerId, IRestSharpFactory restFactory)
			: base(serverUrl, restFactory)
		{
			_tableName = tableName;
			_scannerId = scannerId;
		}

		/// <summary>
		///    Creates the current scanner.
		/// </summary>
		public void Create()
		{

		}

		/// <summary>
		///    Reads the records returned by the current scanner.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public IEnumerable<CellSet> Read()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///    Emits each record returned by the current scanner into an observable sequence.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public IObservable<CellSet> Results()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///    Deletes the current scanner.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Delete()
		{
			throw new NotImplementedException();
		}
	}
}