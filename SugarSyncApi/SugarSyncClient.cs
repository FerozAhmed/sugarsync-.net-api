using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using SugarSyncApi.Api;

namespace SugarSyncApi
{
	public class SugarSyncClient
	{		
		private String _userAgent = "SugarSync .NET API/v1.0";
		
		public String UserAgent
		{
			get { return _userAgent; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_userAgent = value;
			}
		}

		public IWebProxy Proxy { get; set; }

		private T CreatePacket<T>() where T : BaseRequestPacket
		{
			T packet =  Activator.CreateInstance<T>();
			packet.UserAgent = UserAgent;
			packet.Proxy = Proxy;
			return packet;
		}

		private T CreatePacket<T>(string accesToken) where T : BaseRequestPacket
		{
			if (string.IsNullOrEmpty(accesToken))
				throw new ArgumentException("accesToken");

			T packet = CreatePacket<T>();
			packet.AccessToken = accesToken;
			return packet;
		}

		public string GetRefreshToken(String username, String password, String application, String accessKey, String privateAccessKey)
		{
			if (username == null)
				throw new ArgumentException("username");

			if (password == null)
				throw new ArgumentException("password");

			if (application == null)
				throw new ArgumentException("application");

			if (accessKey == null)
				throw new ArgumentException("accessKey");

			if (privateAccessKey == null)
				throw new ArgumentException("privateAccessKey");
			
			RefreshTokenRequest.RefreshTokenPacket packet = CreatePacket<RefreshTokenRequest.RefreshTokenPacket>();
			packet.Username = username;
			packet.Password = password;
			packet.Application = application;
			packet.AccessKey = accessKey;
			packet.PrivateAccessKey = privateAccessKey;

			ApiResponse response = RefreshTokenRequest.GetAuthorizationResponse(packet);			
			return response.Headers.Get("Location");			
		}

		public string GetAccessToken(String accessKeyId, String privateAccessKey, String refreshToken)
		{
			if (accessKeyId == null)
				throw new ArgumentException("accessKeyId");

			if (privateAccessKey == null)
				throw new ArgumentException("privateAccessKey");

			if (refreshToken == null)
				throw new ArgumentException("refreshToken");
			
			AccessTokenRequest.AccessTokenPacket packet = CreatePacket<AccessTokenRequest.AccessTokenPacket>();
			packet.AccessKeyId = accessKeyId;
			packet.PrivateAccessKey = privateAccessKey;
			packet.RefreshToken = refreshToken;

			ApiResponse response = AccessTokenRequest.GetAccessTokenResponse(packet);			
			return response.Headers.Get("Location");
		}

		public UserInfo GetUserInfo(string accessToken)
		{
			if (accessToken == null)
				throw new ArgumentException("accessToken");

			UserInfoRequest.UserInfoPacket packet = CreatePacket<UserInfoRequest.UserInfoPacket>(accessToken);
			
			ApiResponse response = UserInfoRequest.GetUserInfoResponse(packet);

            return RequestParser.ParseUserInfo(response.Body);
		}

		public ContainerInfo GetContainerInfo(string accessToken, string containerUrl)
		{
			if (string.IsNullOrEmpty(containerUrl))
				throw new ArgumentException("containerUrl");

			ContentInfoRequest.ContentInfoPacket packet = CreatePacket<ContentInfoRequest.ContentInfoPacket>(accessToken);			
			packet.InfoUrl = containerUrl;
			ApiResponse response = ContentInfoRequest.GetContainerInfoResponse(packet);

		    return RequestParser.ParseContainerInfo(response.Body);
		}

		

		public Content GetContent(string accessToken, string contentUrl)
		{
			if (string.IsNullOrEmpty(contentUrl))
				throw new ArgumentException("contentUrl");

			ContentInfoRequest.ContentInfoPacket packet = CreatePacket<ContentInfoRequest.ContentInfoPacket>(accessToken);
			packet.InfoUrl = contentUrl;
			ApiResponse response = ContentInfoRequest.GetContainerInfoResponse(packet);

		    return RequestParser.ParseContent(response.Body);
		}

		public Stream GetFile(string accessToken, string fileUrl)
		{
			if (string.IsNullOrEmpty(fileUrl))
				throw new ArgumentException("fileUrl");

            FileRequests.DownloadFilePacket packet = CreatePacket<FileRequests.DownloadFilePacket>(accessToken);
			packet.FileUrl = fileUrl;

			ApiResponse response = FileRequests.GetFileResponse(packet);

			return response.ResponseStream;
		}

        public String CreateFile(string accessToken, string folderUrl, string fileName, string mediaType)
        {
            if (string.IsNullOrEmpty(folderUrl))
                throw new ArgumentException("folderUrl");
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("fileName");
            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            FileRequests.CreateFilePacket packet = CreatePacket<FileRequests.CreateFilePacket>(accessToken);
            packet.FolderUrl = folderUrl;
            packet.DisplayName = fileName;
            packet.MediaType = mediaType;

            ApiResponse response = FileRequests.GetCreateFileResponse(packet);

            return response.Headers.Get("Location");
        }

        public void UploadFile(string accessToken, string fileData, Stream fileStream)
        {
            if (string.IsNullOrEmpty(fileData))
                throw new ArgumentException("fileData");

            FileRequests.UploadFilePacket packet = CreatePacket<FileRequests.UploadFilePacket>(accessToken);
            packet.FileUrl = fileData;
            packet.InputStream = fileStream;

            FileRequests.GetUploadFileResponse(packet);
        }
	}
}
