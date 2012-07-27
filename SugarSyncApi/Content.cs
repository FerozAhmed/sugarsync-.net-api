using System;
using System.Collections.Generic;

namespace SugarSyncApi
{
	public class Content
	{		
		public class CollectionItem
		{
			public enum CollectionItemType { Folder, Workspace }

			public String DisplayName { get; set; }
			public String Ref { get; set; }
			public String Contents { get; set; }
			public CollectionItemType Type { get; set; } 
		}

		public class FileItem
		{
			public String DisplayName { get; set; }
			public String Ref { get; set; }
			public Int64 Size { get; set; }
			public DateTime LastModified { get; set; }
			public String MediaType { get; set; }
			public bool PresentOnServer { get; set; }
			public String FileData { get; set; }
		}

		public UInt64 Start {get;set;} 
		public UInt64 End {get;set;} 
		public bool HasMore {get;set;}
		public IEnumerable<CollectionItem> CollectionItems { get; set; }
		public IEnumerable<FileItem> FileItems { get; set; } 
	}
}
