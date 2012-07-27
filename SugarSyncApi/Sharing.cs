using System;

namespace SugarSyncApi
{
	public class Sharing
	{
		public bool Enabled {get;set;}
		public bool ReadAllowed {get;set;}
		public bool WriteAllowed { get; set; }
		public String ShareList {get;set;}
	}
}