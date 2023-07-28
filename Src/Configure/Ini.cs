using System.Text;

using Vanara.PInvoke;

namespace Xylia.Configure;
public static class Ini
{
	#region Read
	public static string ReadValue(object section, object key, string path = null)
	{
		path ??= PathDefine.DefaultPath;

		var tmp = new StringBuilder();
		var size = Kernel32.GetPrivateProfileString(section.ToString(), key?.ToString(), null, tmp, 2000, path);
		return size == 0 ? null : tmp.ToString();
	}
	#endregion

	#region Write
	public static void WriteValue(object Section, object Key, object Value, string path = null)
	{
		path ??= PathDefine.DefaultPath;
		Directory.CreateDirectory(Path.GetDirectoryName(path));

		Kernel32.WritePrivateProfileString(Section.ToString(), Key.ToString(), Value?.ToString(), path);
	}

	public static void RemoveSection(object Section, string path = null) => WriteValue(Section, null, null, path);

	public static void RemoveKey(object Section, object Key, string path = null) => WriteValue(Section, Key, null, path);
	#endregion
}