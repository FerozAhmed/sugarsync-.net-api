using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SugarSyncApi.Api
{
	internal class FileRequests
	{
		internal class DownloadFilePacket : BaseRequestPacket
		{
			internal String FileUrl { get; set; }
		}

        internal class UploadFilePacket : BaseRequestPacket
        {
            internal String FileUrl { get; set; }
        }

        internal class CreateFilePacket : BaseRequestPacket
        {
            internal String DisplayName { get; set; }
            internal String MediaType { get; set; }
            internal String FolderUrl { get; set; }
        }

		internal static ApiResponse GetFileResponse(DownloadFilePacket data)
		{
            if (!data.FileUrl.EndsWith("/data", StringComparison.OrdinalIgnoreCase))
                data.FileUrl += "/data";

			data.RequestUri = new Uri(data.FileUrl);
			data.Method = "GET";
			data.ResponseType = BaseRequestPacket.ResponseTypeEnum.Stream;
			return ApiRequestSender.SendPacket(data);
		}

        private static byte[] GetCreateFileRequestContent(String displayName, String mediaType, Encoding encoding)
        {
            XDocument document = new XDocument(new XDeclaration("1.0", encoding.BodyName, "yes"));
            XElement root = new XElement("file");

            XElement displayNameElement = new XElement("displayName");
            displayNameElement.SetValue(displayName);
            root.Add(displayNameElement);

            XElement mediaTypeElement = new XElement("mediaType");
            mediaTypeElement.SetValue(mediaType);
            root.Add(mediaTypeElement);

            document.Add(root);
            return document.Encode(encoding);
        }

        internal static ApiResponse GetCreateFileResponse(CreateFilePacket data)
        {
            data.RawData = GetCreateFileRequestContent(data.DisplayName, data.MediaType, data.Encoding);
            data.RequestUri = new Uri(data.FolderUrl);
            data.Method = "POST";
            return ApiRequestSender.SendPacket(data);
        }

        internal static ApiResponse GetUploadFileResponse(UploadFilePacket data)
        {
            if (!data.FileUrl.EndsWith("/data", StringComparison.OrdinalIgnoreCase))
                data.FileUrl += "/data";
          
            data.RequestUri = new Uri(data.FileUrl);
            data.Method = "PUT";
            return ApiRequestSender.SendPacket(data);
        }
	}
}
