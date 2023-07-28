namespace Xylia.Configure;

public static class PathDefine
{
	public static string ApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\";

	public static string MyDocuments => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";

	public static string Desktop => Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";




	/// <summary>
	/// 主存储文件夹
	/// </summary>
	public static string MainFolder { get; set; } = MyDocuments + @"Xylia\";

	/// <summary>
	/// 配置文件存储文件夹
	/// </summary>
	public static string CfgFolder { get; set; } = MainFolder + @"Config\";

	/// <summary>
	/// 默认配置文件路径 （\Xylia.Conf)
	/// </summary>
	public static string DefaultPath { get; set; } = CfgFolder + @"\Xylia.Conf";
}
