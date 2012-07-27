using System;

namespace SugarSyncApi.Api
{
	internal class ContentInfoRequest
	{
		internal class ContentInfoPacket : BaseRequestPacket
		{
			public String InfoUrl { get; set; }
		}

		internal static ApiResponse GetContainerInfoResponse(ContentInfoPacket data)
		{
			data.RequestUri = new Uri(data.InfoUrl);			
			data.Method = "GET";
			return ApiRequestSender.SendPacket(data);
		}
	}
}
