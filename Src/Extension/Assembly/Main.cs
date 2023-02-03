using System;
using System.Diagnostics;
using System.Reflection;

namespace Xylia.Extension
{
	public static class AssemblyEx
	{
		/// <summary>
		/// 获得编译时间
		/// </summary>
		public static DateTime BuildTime => Assembly.GetEntryAssembly().GetBuildTime();

		public static string Product => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;

		public static string Title => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

		public static Version Version => Assembly.GetEntryAssembly().GetName().Version;



		public static DateTime GetBuildTime(this Assembly Assembly)
		{
			Version version = Assembly.GetName().Version;

			return 946656000L.GetDateTime()
				.AddDays(version.Build)
				.AddSeconds(version.Revision * 2);
		}
	}
}