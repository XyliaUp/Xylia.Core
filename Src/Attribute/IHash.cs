namespace Xylia.Attribute
{
	/// <summary>
	/// 键值对应集合接口
	/// </summary>
	public interface IHash
	{
		/// <summary>
		/// 是否包含指定 <see href="Name"/> 键
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="ExtraParam">额外参数</param>
		/// <returns></returns>
		bool Contains(string Name, int? ExtraParam = null);


		/// <summary>
		/// 获取指定 <see href="Name"/> 键的值
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		string GetValue(string Name);

		/// <summary>
		/// 获取指定 <see href="Name"/> 键的值
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		string this[string Name] { get; }
	}
}