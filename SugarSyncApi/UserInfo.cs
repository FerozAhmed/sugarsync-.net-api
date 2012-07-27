using System;

namespace SugarSyncApi
{
	public class UserInfo
	{
		public class UserQuota
		{
			public UInt64 Limit { get; set; }
			public UInt64 Usage { get; set; }
		}

		public String UserName { get; set; }
		public String NickName { get; set; }
		public String Salt { get; set; }
		public UserQuota Quota { get; set; }
		public String Workspaces { get; set; }
		public String Syncfolders { get; set; }
		public String Deleted { get; set; }
		public String MagicBriefcase { get; set; }
		public String WebArchive { get; set; }
		public String MobilePhotos { get; set; }
		public String Albums { get; set; }
		public String RecentActivities { get; set; }
		public String ReceivedShares { get; set; }
		public String PublicLinks { get; set; }
		public Int64 MaximumPublicLinkSize { get; set; }
	}
}