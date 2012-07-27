using System;

namespace SugarSyncApi
{
	public class ContainerInfo
	{
		public String DisplayName { get; set; }
		public DateTime Created { get; set; }
		public String DsId { get; set; }
		public String Collections { get; set; }
		public String Files { get; set; }
		public String Contents { get; set; }
		public Sharing Sharing { get; set; }
	}
}

