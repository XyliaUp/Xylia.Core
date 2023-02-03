using System;
using System.Xml;
using System.Xml.Linq;

using Xylia.Attribute;

namespace Xylia.Files.XmlEx
{
	public sealed class XmlProperty : IDisposable
	{
		#region 构造
		public XmlProperty(XmlNode xmlNode)
		{
			//读取属性
			this.Attributes = xmlNode.CreateAttributeCollection();

			//
			this.SourceText = xmlNode.OuterXml;

			this.InnerText = xmlNode.InnerText;

			//传递名字
			this.Name = xmlNode.Name;
		}

		public XmlProperty(XElement xElement)
		{
			//传递名字
			this.Name = xElement.Name.LocalName;

			//读取属性
			this.Attributes = xElement.CreateAttributeCollection();

			this.SourceText = xElement.ToString();


			#region 获取内部文本
			var TempNode = xElement.FirstNode;
			if (TempNode != null && TempNode.NodeType == XmlNodeType.CDATA) this.InnerText = (TempNode as XCData).Value;
			else this.InnerText = xElement.Value;
			#endregion
		}

		public XmlProperty(string Html, bool IsHtml = true)
		{
			if (IsHtml)
			{
				XElement xElement = XElement.Parse(Html);

				//原始文本
				this.SourceText = Html;

				//传递名字
				this.Name = xElement.Name.LocalName;

				//读取属性
				this.Attributes = xElement.CreateAttributeCollection();
			}
			else
			{
				//传递名字
				this.Name = Html;

				//读取属性
				this.Attributes = new StringAttributeCollection();
			}
		}

		public XmlProperty(string name, StringAttributeCollection attributes)
		{
			this.Name = name;
			this.Attributes = attributes;
		}
		#endregion


		#region 字段	
		/// <summary>
		/// XmlProperty的属性结构
		/// </summary>
		public readonly StringAttributeCollection Attributes;

		/// <summary>
		/// 字段名字
		/// </summary>
		public string Name;

		/// <summary>
		/// 原始文本
		/// </summary>
		public readonly string SourceText;

		/// <summary>
		/// 内部文本
		/// </summary>
		public string InnerText;

		/// <summary>
		/// 外部文本
		/// </summary>
		public string OuterText
		{
			get
			{
				string AttrInfo = null;
				if (this.Attributes != null)
				{
					foreach (var Attr in this.Attributes)
						AttrInfo += $"{ Attr.Name }=\"{ Attr.Value }\" ";
				}

				return $"<{ this.Name } { AttrInfo }/>";
			}
		}
		#endregion

		#region 方法
		/// <summary>
		/// 是否包含指定名称属性
		/// </summary>
		/// <param name="AttrName"></param>
		/// <param name="IgnoreCase"></param>
		/// <param name="Extension"></param>
		/// <returns></returns>
		public bool ContainAttribute(string AttrName, bool IgnoreCase = true, bool Extension = false)
			=> this.Attributes.ContainsName(AttrName, IgnoreCase, Extension);

		/// <summary>
		/// 是否包含指定名称属性
		/// </summary>
		/// <param name="AttrName"></param>
		/// <param name="AttrValue"></param>
		/// <param name="IgnoreCase"></param>
		/// <param name="Extension"></param>
		/// <returns>返回对象</returns>
		public bool ContainAttribute(string AttrName, out object AttrValue, bool IgnoreCase = true, bool Extension = false)
			=> this.Attributes.ContainsName(AttrName, out AttrValue, IgnoreCase, Extension);

		/// <summary>
		/// 是否包含指定名称属性
		/// </summary>
		/// <param name="AttrName"></param>
		/// <param name="AttrValue"></param>
		/// <param name="IgnoreCase"></param>
		/// <param name="Extension"></param>
		/// <returns>返回对象的文本值</returns>
		public bool ContainAttribute(string AttrName, out string AttrValue, bool IgnoreCase = true, bool Extension = false)
			 => this.Attributes.ContainsName(AttrName, out AttrValue, IgnoreCase, Extension);
		#endregion

		#region IDispose
		private bool disposedValue;

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)
					this.Attributes.Dispose();
				}

				// TODO: 释放未托管的资源(未托管的对象)并替代终结器
				// TODO: 将大型字段设置为 null
				disposedValue = true;
			}
		}

		// // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
		// ~XmlProperty()
		// {
		//     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}