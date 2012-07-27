using System;
using System.Runtime.Serialization;

namespace SugarSyncApi
{
	[Serializable]
	public class SugarSyncException : Exception
	{
		public SugarSyncException()
		{
		}

		public SugarSyncException(string message)
			: base(message)
		{
		}

		public SugarSyncException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected SugarSyncException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}


