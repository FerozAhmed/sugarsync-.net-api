using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SugarSyncApi.Api
{
    internal static class RequestParser
    {
        internal static UserInfo ParseUserInfo (string responseData)
        {
            UserInfo userInfo = new UserInfo();

            XDocument doc = XDocument.Parse(responseData);

            XElement root = doc.Element("user", true);
            userInfo.UserName = root.Element("username", true).Value;
            userInfo.NickName = root.Element("nickname", true).Value;
            userInfo.Salt = root.Element("salt", true).Value;
            userInfo.Workspaces = root.Element("workspaces", true).Value;
            userInfo.Syncfolders = root.Element("syncfolders", true).Value;
            userInfo.Deleted = root.Element("deleted", true).Value;
            userInfo.MagicBriefcase = root.Element("magicBriefcase", true).Value;
            userInfo.WebArchive = root.Element("webArchive", true).Value;
            userInfo.MobilePhotos = root.Element("mobilePhotos", true).Value;
            userInfo.Albums = root.Element("albums", true).Value;
            userInfo.RecentActivities = root.Element("recentActivities", true).Value;
            userInfo.ReceivedShares = root.Element("receivedShares", true).Value;
            userInfo.PublicLinks = root.Element("publicLinks", true).Value;
            userInfo.MaximumPublicLinkSize = root.ElementAsInt64("maximumPublicLinkSize");

            XElement quota = root.Element("quota", true);

            userInfo.Quota = new UserInfo.UserQuota
            {
                Limit = quota.ElementAsUInt64("limit"),
                Usage = quota.ElementAsUInt64("usage")
            };
            return userInfo;
        }

        private static Sharing ParseSharing(XElement sharingElement)
        {
            Sharing sharing = new Sharing
            {
                Enabled = sharingElement.AttributeAsBool("enabled")
            };
            if (sharing.Enabled)
            {
                sharing.ReadAllowed = sharingElement.Element("readAllowed").AttributeAsBool("enabled");
                sharing.WriteAllowed = sharingElement.Element("writeAllowed").AttributeAsBool("enabled");
                sharing.ShareList = sharingElement.Element("shareList", true).Value;
            }
            return sharing;
        }

        private static Content.CollectionItem ParseCollectiontItem(XElement element)
        {
            Content.CollectionItem item = new Content.CollectionItem
            {
                Type = element.AttributeAsItemType("type"),
                DisplayName = element.Element("displayName", true).Value,
                Ref = element.Element("ref", true).Value,
                Contents = element.Element("contents", true).Value
            };
            return item;
        }

        private static Content.FileItem ParseFileItem(XElement element)
        {
            XElement fileData = element.Element("fileData");
            Content.FileItem item = new Content.FileItem
            {
                DisplayName = element.Element("displayName", true).Value,
                Ref = element.Element("ref", true).Value,
                Size = element.ElementAsInt64("size"),
                LastModified = element.ElementAsDateTime("lastModified"),
                MediaType = element.Element("mediaType", true).Value,
                FileData = fileData == null ? null : fileData.Value,
                PresentOnServer = element.ElementAsBool("presentOnServer")
            };
            return item;
        }

        internal static ContainerInfo ParseContainerInfo(string responseData)
        {
            ContainerInfo containerInfo = new ContainerInfo();

            XDocument doc = XDocument.Parse(responseData);
            XElement root = doc.Element("folder", true);

            containerInfo.DisplayName = root.Element("displayName", true).Value;
            containerInfo.DsId = root.Element("dsid", true).Value;
            containerInfo.Created = root.ElementAsDateTime("timeCreated");
            containerInfo.Collections = root.Element("collections", true).Value;
            containerInfo.Files = root.Element("files", true).Value;
            containerInfo.Contents = root.Element("contents", true).Value;

            containerInfo.Sharing = ParseSharing(root.Element("sharing", true));

            return containerInfo;
        }

        internal static Content ParseContent(string responseData)
        {
            XDocument doc = XDocument.Parse(responseData);
            XElement root = doc.Element("collectionContents", true);

            var content = new Content
            {
                HasMore = root.AttributeAsBool("hasMore"),
                Start = root.AttributeAsUInt64("start"),
                End = root.AttributeAsUInt64("end")
            };

            var collectionItems = new List<Content.CollectionItem>();
            var fileItems = new List<Content.FileItem>();
            content.CollectionItems = collectionItems;
            content.FileItems = fileItems;

            collectionItems.AddRange(root.Elements("collection").Select(e=>ParseCollectiontItem(e)));

            fileItems.AddRange(root.Elements("file").Select(e=>ParseFileItem(e)));

            return content;
        }
    }
}
