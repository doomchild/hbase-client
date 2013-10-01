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

using RestSharp;
using RestSharp.Injection;

namespace HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides a base type for Stargate components.
	/// </summary>
	public abstract class StargateBase
	{
		private readonly IRestSharpFactory _restSharp;
		private readonly IRestClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="StargateBase"/> class.
		/// </summary>
		/// <param name="serverUrl">The server URL.</param>
		/// <param name="restSharp">The rest sharp factory.</param>
		protected StargateBase(string serverUrl, IRestSharpFactory restSharp)
		{
			_restSharp = restSharp;
			_client = _restSharp.CreateClient(serverUrl);
		}

		/// <summary>
		/// Sends the request.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="resource">The resource.</param>
		/// <param name="acceptType">Type of the accept.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		protected IRestResponse SendRequest(Method method, string resource, string acceptType, string contentType = null, string content = null)
		{
			var request = _restSharp.CreateRequest(resource, method)
				.AddHeader(RestConstants.AcceptHeader, acceptType);

			if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(contentType))
			{
				request.AddParameter(contentType, content, ParameterType.RequestBody);
			}

			return _client.Execute(request);
		}
	}
}