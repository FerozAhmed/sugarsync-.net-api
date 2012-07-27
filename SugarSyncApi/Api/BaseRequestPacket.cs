using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SugarSyncApi.Api
{
	internal abstract class BaseRequestPacket
	{
		internal enum ResponseTypeEnum
		{
			Content,
			Stream
		}

		private ResponseTypeEnum _responseType = ResponseTypeEnum.Content;
		internal ResponseTypeEnum ResponseType
		{
			get { return _responseType; }
			set { _responseType = value; }
		}


		private String _userAgent = string.Empty;
		internal String UserAgent
		{
			get { return _userAgent; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_userAgent = value;
			}
		}

		private Encoding _encoding = Encoding.UTF8;
		internal Encoding Encoding
		{
			get { return _encoding; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_encoding = value;
			}
		}

		private String _method = "POST";
		internal String Method
		{
			get { return _method; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_method = value;
			}
		}

		private String _contentType;
		internal String ContentType
		{
			get { return _contentType; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_contentType = value;
			}
		}

		internal byte[] RawData { get; set; }

		internal Uri RequestUri { get; set; }

		internal IWebProxy Proxy { get; set; }

		private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();
		internal IDictionary<string, string> Headers { get { return _headers; } }

		internal String AccessToken { get; set; }

        internal Stream InputStream { get; set; }
	}
}
