using System;
using System.Net;
using System.IO;

namespace SugarSyncApi.Api
{
	internal class ApiResponse
	{
		internal HttpStatusCode StatusCode { get; set; }
		internal String Body { get; set; }
		internal WebHeaderCollection Headers { get; set; }
		internal Stream ResponseStream { get; set; }
	}
}
