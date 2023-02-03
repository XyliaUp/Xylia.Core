using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using HtmlAgilityPack;

namespace Xylia.Attribute
{
	/// <summary>
	/// 文本属性集合
	/// </summary>
	public sealed class StringAttributeCollection : AttributeCollection
	{
		#region 构造
		public StringAttributeCollection()
		{

		}


		public StringAttributeCollection(XmlAttributeCollection attributes)
		{
			foreach (XmlAttribute attribute in attributes)
				this.Add(new MyAttribute(attribute.Name, attribute.Value));
		}

		public StringAttributeCollection(HtmlAttributeCollection attributes)
		{
			foreach (HtmlAttribute attribute in attributes)
				this.Add(new MyAttribute(attribute.Name, attribute.Value));
		}

		public StringAttributeCollection(IEnumerable<XAttribute> attributes)
		{
			foreach (XAttribute attribute in attributes)
				this.Add(new MyAttribute(attribute.Name.LocalName, attribute.Value));
		}
		#endregion



		#region 方法
		public new string this[params string[] param] => this[param.ToList(), true]?.ToString();

		public new string this[object AttrOrIndex, bool IngoreCase = true] => base[AttrOrIndex, IngoreCase]?.ToString();

		public new string this[bool IgnoreCase, params string[] param] => this[param.ToList(), IgnoreCase]?.ToString();
		#endregion
	}

	public static class AttributeCollectionUtil
	{
		#region 创建属性集合
		public static StringAttributeCollection CreateAttributeCollection(this XmlNode xmlNode) => new(xmlNode.Attributes);

		public static StringAttributeCollection CreateAttributeCollection(this HtmlNode HtmlNode) => new(HtmlNode.Attributes);

		public static StringAttributeCollection CreateAttributeCollection(this XElement xElement) => new(xElement.Attributes());
		#endregion
	}
}