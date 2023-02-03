using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Xylia.Files.XmlEx;

namespace Xylia.Extension
{
	public static partial class Files
	{
		/// <summary>
		/// 获得文档文本
		/// </summary>
		/// <param name="XmlDoc"></param>
		/// <param name="Formatting"></param>
		/// <returns></returns>
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


		#region 转换为 XmlProperty
		public static List<XmlProperty> Properties(this XmlNodeList xmlNodeList) => xmlNodeList.OfType<XmlElement>().Select(x => x.Property()).ToList();

		/// <summary>
		/// XmlNode => XmlProperty
		/// </summary>
		/// <param name="xmlNode"></param>
		/// <returns></returns>
		public static XmlProperty Property(this XmlNode xmlNode) => new XmlProperty(xmlNode);

		public static XmlProperty Property(this XElement xElement) => new XmlProperty(xElement);

		public static XmlProperty Property(this string Html)
		{
			try
			{
				return new XmlProperty(Html);
			}
			catch
			{
				throw new Exception("初始化错误：" + Html);
			}
		}

		/// <summary>
		/// Html文本转换为XmlProperty实例
		/// </summary>
		/// <param name="Html"></param>
		/// <returns></returns>
		public static List<XmlProperty> ToProperties(this string Html)
		{
			List<XmlProperty> List = new List<XmlProperty>();

			foreach (Match match in new Regex(@"<.*?>").Matches(Html))
			{
				var tmp = match.Value.ToProperty();
				if (tmp != null) List.Add(tmp);
			}

			return List;
		}

		/// <summary>
		/// Html文本转换为XmlProperty实例
		/// </summary>
		/// <param name="Html"></param>
		/// <returns></returns>
		public static XmlProperty ToProperty(this string Html)
		{
			string Info = new Regex(@"<.*?" + "/" + @"\s*" + ">").Match(Html)?.Value;
			return Info?.Property();
		}
		#endregion
	}
}