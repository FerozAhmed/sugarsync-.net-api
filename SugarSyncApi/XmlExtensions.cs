using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SugarSyncApi
{
	internal static class XmlExtensions
	{
		internal const string FieldRequiredFormat = "Field '{0}' was expected";
		internal const string FieldInvalidFormat = "Field '{0}' is invalid";
		internal const string AttributeRequiredFormat = "Attribute '{0}' is required for field '{1}'";
		internal const string AttributeInvalidFormat = "Attribute '{0}' for field '{1}' is invalid";

		internal static byte[] Encode(this XDocument doc, Encoding encoding)
		{
			if (doc == null)			
				throw new ArgumentNullException("doc");
			

			using (var memoryStream = new MemoryStream())
			using (var streamWriter = new StreamWriter(memoryStream, encoding))
			{
				doc.Save(streamWriter, SaveOptions.DisableFormatting);
				return memoryStream.ToArray();
			}
		}	
		

		internal static XElement Element(this XContainer element, string name, bool throwErrorIfNotPresent)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			if (name == null)
				throw new ArgumentNullException("name");

			var elementToReturn = element.Element(name);

			if (elementToReturn == null && throwErrorIfNotPresent)
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, FieldRequiredFormat, name));

			return elementToReturn;
		}

		internal static Int64 ElementAsInt64(this XContainer element, string name)
		{
			var elementToReturn = element.Element(name, true);

			Int64 value;
			if (!Int64.TryParse(elementToReturn.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, FieldInvalidFormat, name));

			return value;
		}

		internal static UInt64 ElementAsUInt64(this XContainer element, string name)
		{		
			var elementToReturn = element.Element(name, true);

			UInt64 value;
			if (!UInt64.TryParse(elementToReturn.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, FieldInvalidFormat, name));

			return value;
		}

		internal static DateTime ElementAsDateTime(this XContainer element, string name)
		{			
			var elementToReturn = element.Element(name, true);

			DateTime value;
			if (!DateTime.TryParse(elementToReturn.Value, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite| DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AdjustToUniversal, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, FieldInvalidFormat, name));

			return value;
		}

		internal static bool ElementAsBool(this XElement element, string name)
		{
			var elementToReturn = element.Element(name, true);

			bool value;
			if (!bool.TryParse(elementToReturn.Value, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, FieldInvalidFormat, name));

			return value;
		}


		internal static XAttribute Attribute(this XElement element, string name, bool throwErrorIfNotPresent)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			if (name == null)
				throw new ArgumentNullException("name");

			XAttribute attribute = element.Attribute(name);

			if (attribute == null && throwErrorIfNotPresent)
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, AttributeRequiredFormat, name, element.Name.LocalName));

			return attribute;
		}

		internal static bool AttributeAsBool(this XElement element, string name)
		{
			XAttribute attribute = element.Attribute(name, true);

			bool value;
			if (!bool.TryParse(attribute.Value, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, AttributeInvalidFormat, name, element.Name.LocalName));

			return value;
		}

		internal static UInt64 AttributeAsUInt64(this XElement element, string name)
		{
			XAttribute attribute = element.Attribute(name, true);

			UInt64 value;
			if (!UInt64.TryParse(attribute.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
				throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, AttributeInvalidFormat, name, element.Name.LocalName));

			return value;
		}

		internal static Content.CollectionItem.CollectionItemType AttributeAsItemType(this XElement element, string name)
		{
			string type = element.Attribute(name, true).Value;
			if (type.Equals("folder", StringComparison.Ordinal))
				return Content.CollectionItem.CollectionItemType.Folder;

            if (type.Equals("workspace", StringComparison.Ordinal))
                return Content.CollectionItem.CollectionItemType.Workspace;
			
			throw new SugarSyncException(string.Format(CultureInfo.InvariantCulture, AttributeInvalidFormat, name, element.Name.LocalName));			
		}
	}
}
