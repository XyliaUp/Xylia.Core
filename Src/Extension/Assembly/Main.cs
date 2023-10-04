using System.Reflection;

namespace Xylia.Extension;

public static class AssemblyEx
{
	public static DateTime BuildTime => Assembly.GetEntryAssembly().GetBuildTime();

	public static string Product => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
	public static string Title => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;



	public static AssemblyName AssemblyName => Assembly.GetEntryAssembly().GetName();

	public static Version Version => AssemblyName.Version;
	public static string Name => AssemblyName.Name;






	public static DateTime GetBuildTime(this Assembly Assembly)
	{
		Version version = Assembly.GetName().Version;

		return new DateTime(2000, 1, 1)
			.AddDays(version.Build)
			.AddSeconds(version.Revision * 2);
	}
}