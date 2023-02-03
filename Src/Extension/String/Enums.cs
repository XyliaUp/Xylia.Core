namespace Xylia.Extension
{
	public static partial class String
	{
		public enum EqualType
		{
			/// <summary>
			/// 忽略大小写
			/// </summary>
			IgnoreCase,


			/// <summary>
			/// 允许为Null
			/// </summary>
			AllowNulls,

			/// <summary>
			/// 无其他要求
			/// </summary>
			None,
		}

		public enum JudegeLineType
		{
			/// <summary>
			/// 非空的
			/// </summary>
			NoEmpty,


			/// <summary>
			/// 需要换行符号
			/// </summary>
			Signature,
		}
	}
}
