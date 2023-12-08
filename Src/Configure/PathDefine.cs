namespace Xylia.Configure;

public static class PathDefine
{
	public static string ApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

	public static string Desktop => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);



	/// <summary>
	/// 主存储文件夹
	/// </summary>
	public static string MainFolder { get; } = Path.Combine(ApplicationData, @"Xylia");

	/// <summary>
	/// 默认配置文件路径
	/// </summary>
	public static string DefaultPath { get; set; } = Path.Combine(MainFolder, @"Settings.config");
}
