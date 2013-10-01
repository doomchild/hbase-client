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

using HBase.Stargate.Client;
using HBase.Stargate.Client.MimeConversion;

namespace _specs.Models
{
	public class ContentConverter : IMimeConverter
	{
		private IMimeConverter _converter;

		public void SetConversionToXml()
		{
			MimeType = HBaseMimeTypes.Xml;
			_converter = new XmlMimeConverter();
		}

		public void SetConversionToJson()
		{
			MimeType = HBaseMimeTypes.Json;
			//TODO: _converter = new JsonMimeConverter();
		}

		public void SetConversionToProtobuf()
		{
			MimeType = HBaseMimeTypes.Protobuf;
			//TODO: _converter = new ProtobufMimeConverter();
		}

		public void SetConversionToBinary()
		{
			MimeType = HBaseMimeTypes.Stream;
			//TODO: _converter = new BinaryMimeConverter();
		}

		public string MimeType { get; private set; }

		public string Convert(CellSet cells)
		{
			if (_converter != null) return _converter.Convert(cells);

			throw new System.NotImplementedException();
		}

		public string Convert(Cell cell)
		{
			if (_converter != null) return _converter.Convert(cell);

			throw new System.NotImplementedException();
		}

		public CellSet Convert(string data)
		{
			if (_converter != null) return _converter.Convert(data);

			throw new System.NotImplementedException();
		}
	}
}