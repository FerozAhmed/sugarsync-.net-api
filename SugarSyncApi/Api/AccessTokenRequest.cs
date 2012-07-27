using System;
using System.Text;
using System.Xml.Linq;

namespace SugarSyncApi.Api
{	
	internal class AccessTokenRequest
	{
		private const String AuthAccessTokenApiUrl = "https://api.sugarsync.com/authorization";

		internal class AccessTokenPacket : BaseRequestPacket
		{
			public String AccessKeyId { get; set; }
			public String PrivateAccessKey { get; set; }
			public String RefreshToken { get; set; }			
		}

		private static byte[] GetRequestContent(String accessKeyId, String privateAccessKey, String refreshToken, Encoding encoding)
		{
			XDocument document = new XDocument(new XDeclaration("1.0", encoding.BodyName, "yes"));
			XElement root = new XElement("tokenAuthRequest");

			XElement accessKeyElement = new XElement("accessKeyId");
			accessKeyElement.SetValue(accessKeyId);
			root.Add(accessKeyElement);		

			XElement privateAccessKeyElement = new XElement("privateAccessKey");
			privateAccessKeyElement.SetValue(privateAccessKey);
			root.Add(privateAccessKeyElement);		

			XElement refreshTokenElement = new XElement("refreshToken");
			refreshTokenElement.SetValue(refreshToken);
			root.Add(refreshTokenElement);	

			document.Add(root);
			return document.Encode(encoding);
		}

		internal static ApiResponse GetAccessTokenResponse(AccessTokenPacket data)
		{
			data.RawData = GetRequestContent(data.AccessKeyId, data.PrivateAccessKey, data.RefreshToken, data.Encoding);
			data.RequestUri = new Uri(AuthAccessTokenApiUrl);
			return ApiRequestSender.SendPacket(data);
		}
	}
}