using System;

namespace SugarSyncApi.Api
{
	internal class UserInfoRequest
	{
		internal class UserInfoPacket : BaseRequestPacket
		{			
		}

		private const String UserInfoApiUrl = "https://api.sugarsync.com/user";

		internal static ApiResponse GetUserInfoResponse(UserInfoPacket data)
		{
			data.RequestUri = new Uri(UserInfoApiUrl);
			data.Method = "GET";			
			return ApiRequestSender.SendPacket(data);
		}
	}
}