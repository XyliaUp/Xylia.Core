using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Xylia.Xml;
public static partial class Files
{
	public static string GetValue(this XmlNode node) => node.Attributes["value"]?.Value;


	public static string Text(this XmlDocument XmlDoc, Formatting Formatting = Formatting.Indented)
	{
		XmlTextWriter writer = new(new MemoryStream(), null)
		{
			Formatting = Formatting,
		};

		XmlDoc.Save(writer);


		writer.BaseStream.Position = 0;
		StreamReader streamReader = new(writer.BaseStream);

		string xmlString = streamReader.ReadToEnd();
		streamReader.Close();

		return xmlString;
	}

	public static XmlDocument GetXmlDocument(this string FullPath)
	{
		XmlDocument doc = new();
		doc.Load(FullPath);

		return doc;
	}




	public static TResult GetObject<TResult>(this string xmlString)
	{
		var serializer = new XmlSerializer(typeof(TResult));
		return (TResult)serializer.Deserialize(new StringReader(xmlString));
	}

	public static XmlElement LinqTo(this XElement element)
	{
		if (element == null) return null;

		using var reader = element.CreateReader();
		return new XmlDocument().ReadNode(reader) as XmlElement;
	}

	public static XElement LinqTo(this XmlElement element)
	{
		if (element == null) return null;

		var doc = new XmlDocument();
		doc.AppendChild(doc.ImportNode(element, true));
		return XElement.Parse(doc.InnerXml);
	}
}