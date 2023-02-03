namespace Xylia.Attribute
{
	/// <summary>
	/// 属性结构
	/// </summary>
	public class MyAttribute
	{
		#region 构造
		public MyAttribute()
		{

		}

		public MyAttribute(string Name, object Value)
		{
			this.Name = Name;
			this.Value = Value;
		}
		#endregion


		#region 字段
		/// <summary>
		/// 属性名
		/// </summary>
		public string Name;

		/// <summary>
		/// 属性值
		/// </summary>
		public object Value;
		#endregion



		/// <summary>
		/// 是否拥有有效值
		/// </summary>
		public bool HasValue => this.Value != null;
	}
}
