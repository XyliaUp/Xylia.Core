using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xylia.Extension;

namespace Xylia.Attribute
{
	/// <summary>
	/// 属性集合
	/// </summary>
	public class AttributeCollection : List<MyAttribute>, IDisposable, IHash
	{
		#region 字段
		/// <summary>
		/// 缓存集合
		/// </summary>
		private readonly Hashtable ht_Name = new(StringComparer.Create(CultureInfo.InvariantCulture, true));
		#endregion

		#region 构造
		public AttributeCollection(List<KeyValuePair<string, object>> AttributeLists)
		{
			AttributeLists.ForEach(alist => this.Add(new MyAttribute(alist.Key, alist.Value)));
		}

		public AttributeCollection()
		{

		}
		#endregion


		#region 获得属性值
		/// <summary>
		/// 获得属性值
		/// </summary>
		/// <param name="AttrOrIndex"></param>
		/// <param name="IngoreCase">是否忽略大小写</param>
		/// <returns>
		/// 如果传入数组，将从前往后返回第一个不为null的值。
		/// 无效时，返回默认值
		/// </returns>
		public virtual object this[object AttrOrIndex, bool IngoreCase = true]
		{
			get
			{
				if (AttrOrIndex is int @int) return this[@int];

				#region Array类型
				else if (AttrOrIndex is Array array)
				{
					foreach (object tmp in array)
					{
						object val = this[tmp.ToString(), IngoreCase];
						if (val != null)
						{
							if (val is string @string)
							{
								if (!string.IsNullOrWhiteSpace(@string)) return @string;
							}
							else return val.ToString();
						}
					}
				}
				#endregion

				//List类型
				else if (AttrOrIndex is List<string> list) return this[list.ToArray(), IngoreCase];

				//文本类型
				else if (this.ContainsName(AttrOrIndex.ToString(), out string Val, IngoreCase)) return Val;

				return null;
			}
		}

		/// <summary>
		/// 多参数方法
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public virtual object this[params string[] param] => this[param.ToList(), true];

		/// <summary>
		/// 多参数方法
		/// </summary>
		/// <param name="IgnoreCase"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public virtual object this[bool IgnoreCase, params string[] param] => this[param.ToList(), IgnoreCase];
		#endregion



		#region 方法
		/// <summary>
		/// 是否含有属性名
		/// </summary>
		/// <param name="AttrName"></param>
		/// <param name="Value"></param>
		/// <param name="IgnoreCase">忽略大小写</param>
		/// <param name="Extension">判断扩展</param>
		/// <returns></returns>
		public bool ContainsName(string AttrName, out object Value, bool IgnoreCase = true, bool Extension = false)
		{
			if (ht_Name.Contains(AttrName))
			{
				//大小写校验
				if (IgnoreCase || ((MyAttribute)ht_Name[AttrName]).Name == AttrName)
				{
					Value = ((MyAttribute)ht_Name[AttrName]).Value;
					return true;
				}
			}

			// 扩展模式
			if (Extension)
			{
				//return ContainsName(AttrName, out Value, IgnoreCase);
			}

			Value = null;
			return false;
		}

		/// <summary>
		/// 是否含有属性名
		/// </summary>
		/// <param name="AttrName"></param>
		/// <param name="IgnoreCase">忽略大小写</param>
		/// <param name="Extension">判断扩展</param>
		/// <returns></returns>
		public bool ContainsName(string AttrName, bool IgnoreCase = true, bool Extension = false) => this.ContainsName(AttrName, out object _, IgnoreCase, Extension);



		public bool ContainsName(string AttrName, out string Value, bool IgnoreCase = true, bool Extension = false)
		{
			bool Flag = this.ContainsName(AttrName, out object @obj, IgnoreCase, Extension);

			Value = @obj?.ToString();
			return Flag;
		}




		/// <summary>
		/// 是否含有属性值
		/// </summary>
		/// <param name="AttrVal"></param>
		/// <returns></returns>
		public bool ContainsValue(object AttrVal)
		{
			return this.Where(a => a.Value == AttrVal).FirstOrDefault() != null;
		}

		public new void Add(MyAttribute attribute)
		{
			base.Add(attribute);
			this.ht_Name.Add(attribute.Name, attribute);
		}

		public new void AddRange(IEnumerable<MyAttribute> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var item in items) Add(item);
		}

		public new bool Remove(MyAttribute item)
		{
			this.ht_Name.Remove(item.Name);

			return base.Remove(item);
		}

		public new void Clear()
		{
			base.Clear();
			this.ht_Name.Clear();
		}
		#endregion



		#region IDispose
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)
				}

				this.Clear();

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


		#region IHash
		public bool Contains(string Name, int? ExtraParam = null) => this.ContainsName(Name.GetExtraParam(ExtraParam));

		public string GetValue(string Name)
		{
			if (this.ContainsName(Name, out object value)) return value?.ToString();

			return null;
		}

		public string this[string Name] => this.GetValue(Name);
		#endregion
	}
}