namespace Xylia;

public interface IFile : IDisposable
{
	/// <summary>
	/// 存储文件
	/// </summary>
	/// <param name="FilePath">文件存储绝对路径</param>
	/// <param name="ShowRelativePath">注释是否显示为相对路径</param>
	/// <param name="RelativePrefix">相对路径前缀</param>
	/// <param name="RetryTime">最大重试次数</param>
	void Save(string FilePath, bool ShowRelativePath = false, string RelativePrefix = null, int RetryTime = 20);

	/// <summary>
	/// 存储文件
	/// </summary>
	/// <param name="Folder">文件存储目录</param>
	/// <param name="RelativePath">相对存储路径</param>
	/// <param name="ShowRelativePath">注释是否显示为相对路径</param>
	/// <param name="RetryTime">最大重试次数</param>
	void Save(string Folder, string RelativePath, bool ShowRelativePath = false, int RetryTime = 20);
}