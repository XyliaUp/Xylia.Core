//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Xml;
//using System.Xml.Linq;

//using Xylia.Attribute;
//using Xylia.Extension;

//namespace Xylia.Files.XmlEx
//{
//	/// <summary>
//	/// 节点接口
//	/// </summary>
//	public abstract class INode : IDisposable, ICloneable
//	{
//		#region 设置字段
//		/// <summary>
//		/// 指示是否需要加载子节点
//		/// </summary>
//		public virtual bool LoadChidren { get; set; } = true;

//		/// <summary>
//		/// 是否使用公共加载方法（对于数据量较大的情况，公共加载并不友好）
//		/// </summary>
//		public virtual bool PublicSet { get; set; } = true;

//		/// <summary>
//		/// 是否显示调试信息
//		/// </summary>
//		protected virtual bool ShowDebugInfo => false;
//		#endregion





//		#region 字段
//		public XmlNode XmlNode
//		{
//			set
//			{
//				this.Property = value.Property();
//				if (this.LoadChidren && value.HasChildNodes)
//					this.SetChildren(value.ChildNodes.OfType<XmlElement>());
//			}
//		}

//		public XElement XElement;


//		private XmlProperty m_Property;

//		public XmlProperty Property
//		{
//			set => this.SetProperty(this.m_Property = value);
//			get
//			{
//				if (this.m_Property != null) return this.m_Property;

//				if (this.XElement != null)
//				{
//					this.SetProperty(this.m_Property = this.XElement.Property());

//					return this.m_Property;
//				}

//				return null;
//			}
//		}


//		public StringAttributeCollection Attributes => this.Property.Attributes;

//		public bool ContainAttribute(string AttrName, out string AttrValue, bool IgnoreCase = true, bool Extension = false)
//			=> this.Attributes.ContainsName(AttrName, out AttrValue, IgnoreCase, Extension);
//		#endregion



//		#region 方法
//		/// <summary>
//		/// 设置子节点
//		/// </summary>
//		/// <param name="XmlElements"></param>
//		protected virtual void SetChildren(IEnumerable<XmlElement> XmlElements)
//		{

//		}

//		/// <summary>
//		/// 设置属性
//		/// </summary>
//		/// <param name="Property"></param>
//		protected virtual void SetProperty(XmlProperty Property)
//		{
//			if (this.PublicSet && Property != null)
//			{
//				foreach (var Attr in Property.Attributes)
//				{
//					this.SetMember(Attr.Name, Attr.Value, true, ShowDebugInfo ? true : null);
//				}
//			}
//		}
//		#endregion

//		#region IDisposable
//		private bool disposedValue;

//		protected virtual void Dispose(bool disposing)
//		{
//			if (!disposedValue)
//			{
//				if (disposing)
//				{
//					// TODO: 释放托管状态(托管对象)
//					this.XmlNode = null;

//					this.m_Property.Dispose();
//					this.m_Property = null;
//				}

//				// TODO: 释放未托管的资源(未托管的对象)并替代终结器
//				// TODO: 将大型字段设置为 null
//				disposedValue = true;
//			}
//		}

//		// // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
//		// ~INode()
//		// {
//		//     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
//		//     Dispose(disposing: false);
//		// }

//		public void Dispose()
//		{
//			// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
//			Dispose(disposing: true);


//			GC.SuppressFinalize(this);
//			GC.Collect();
//		}
//		#endregion

//		#region ICloneable
//		/// <summary>
//		/// 深拷贝
//		/// </summary>
//		/// <returns></returns>
//		public INode DeepClone()
//		{
//			using (Stream objectStream = new MemoryStream())
//			{
//				IFormatter formatter = new BinaryFormatter();
//				formatter.Serialize(objectStream, this);
//				objectStream.Seek(0, SeekOrigin.Begin);
//				return formatter.Deserialize(objectStream) as INode;
//			}
//		}

//		public object Clone()
//		{
//			return this.MemberwiseClone();
//		}
//		#endregion
//	}
//}