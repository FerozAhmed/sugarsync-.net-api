using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SugarSyncApi.Api
{
    internal static class ApiRequestSender
    {
        internal static ApiResponse SendPacket(BaseRequestPacket packet)
        {
            ApiResponse response;
            try
            {
                response = SendPacketPrivate(packet);
            }
            catch (HttpException httpException)
            {
                throw new SugarSyncException("Request error", httpException);
            }
            catch (WebException webexception)
            {
                throw new SugarSyncException("Request error", webexception);
            }

            int statusCode = (int)response.StatusCode;
            if (statusCode > 299)
            {
                if (response.ResponseStream != null)
                    response.ResponseStream.Dispose();
                string exceptionMessage = string.Format(CultureInfo.InvariantCulture, "Request error. Response code: {0}. Response body: {1}", statusCode, response.Body);
                throw new SugarSyncException(exceptionMessage);
            }
            return response;
        }

        private static ApiResponse SendPacketPrivate(BaseRequestPacket packet)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(packet.RequestUri);

            if (packet.Proxy != null)
                request.Proxy = packet.Proxy;


            if (packet.RawData == null && packet.InputStream == null)
                request.ContentLength = 0;
            else if (packet.RawData != null)
                request.ContentLength = packet.RawData.Length;

            request.Headers["Encoding"] = packet.Encoding.BodyName;
            request.UserAgent = packet.UserAgent;
            request.ContentType = packet.ContentType;
            request.Method = packet.Method;

            foreach (var header in packet.Headers)
                request.Headers.Add(header.Key, header.Value);

            if (!string.IsNullOrEmpty(packet.AccessToken))
                request.Headers["Authorization"] = packet.AccessToken;

            if (!packet.Method.EndsWith("GET", StringComparison.OrdinalIgnoreCase))
            {
                using (var requestStream = request.GetRequestStream())
                {
                    if (packet.RawData != null)
                        requestStream.Write(packet.RawData, 0, packet.RawData.Length);

                    if (packet.InputStream != null)
                    {
                        byte[] tmp = new byte[4096];
                        int read;
                        while ((read = packet.InputStream.Read(tmp, 0, 4096)) > 0)
                        {
                            requestStream.Write(tmp, 0, read);
                        }
                    }
                }
            }



            WebResponse response = null;
            Exception error = null;
            string responseBody = null;
            Stream responseStream = null;
            ApiResponse apiResponse = null;
            try
            {
                response = request.GetResponse();

                switch (packet.ResponseType)
                {
                    case BaseRequestPacket.ResponseTypeEnum.Content:
                        using (responseStream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                            responseBody = reader.ReadToEnd();
                        responseStream = null;
                        break;
                    case BaseRequestPacket.ResponseTypeEnum.Stream:
                        responseStream = response.GetResponseStream();
                        break;
                }

                apiResponse = new ApiResponse
                {
                    StatusCode = ((HttpWebResponse)response).StatusCode,
                    Body = responseBody,
                    Headers = response.Headers,
                    ResponseStream = responseStream
                };
            }
            catch (Exception ex)
            {
                error = ex;
            }

            IDisposable disposable = response;
            if (error != null && disposable != null)
            {
                disposable.Dispose();
                throw error;
            }
            if (packet.ResponseType != BaseRequestPacket.ResponseTypeEnum.Stream && disposable != null)
                disposable.Dispose();

            return apiResponse;
        }
    }
}
