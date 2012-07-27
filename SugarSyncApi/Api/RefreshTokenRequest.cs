using System;
using System.Text;
using System.Xml.Linq;

namespace SugarSyncApi.Api
{
	internal class RefreshTokenRequest
	{
		internal class RefreshTokenPacket : BaseRequestPacket
		{
			public String Username { get; set; }
			public String Password { get; set; }
			public String Application { get; set; }
			public String AccessKey { get; set; }
			public String PrivateAccessKey { get; set; }			
		}

		private const String AppAuthRefreshTokenApiUrl = "https://api.sugarsync.com/app-authorization";
		
		private static byte[] GetRequestContent(String username, String password, String application, String accessKey, String privateAccessKey, Encoding encoding)
		{
			XDocument document = new XDocument(new XDeclaration("1.0", encoding.BodyName, "yes"));
			XElement root = new XElement("appAuthorization");

			XElement userNameElement = new XElement("username");
			userNameElement.SetValue(username);
			root.Add(userNameElement);

			XElement passwordElement = new XElement("password");
			passwordElement.SetValue(password);
			root.Add(passwordElement);

			XElement appElement = new XElement("application");
			appElement.SetValue(application);
			root.Add(appElement);

			XElement accessKeyElement = new XElement("accessKeyId");
			accessKeyElement.SetValue(accessKey);
			root.Add(accessKeyElement);

			XElement privateAccessKeyElement = new XElement("privateAccessKey");
			privateAccessKeyElement.SetValue(privateAccessKey);
			root.Add(privateAccessKeyElement);


			document.Add(root);
			return document.Encode(encoding);
		}


		internal static ApiResponse GetAuthorizationResponse(RefreshTokenPacket data)
		{
			data.RawData = GetRequestContent(data.Username, data.Password, data.Application, data.AccessKey, data.PrivateAccessKey, data.Encoding);
			data.RequestUri = new Uri(AppAuthRefreshTokenApiUrl);
            data.Method = "POST";
			return ApiRequestSender.SendPacket(data);
		}
	}
}