﻿using System.Text;

using Vanara.PInvoke;

namespace Xylia.Configure;
public class Ini
{
	#region Constructor
	public static Ini Instance { get; } = new();

	private string path;

	public Ini(string path = null) => this.path = path ?? PathDefine.DefaultPath;
	#endregion

	#region Methods
	public string ReadValue(object section, object key)
	{
		var tmp = new StringBuilder(0x2000);

		var size = Kernel32.GetPrivateProfileString(section.ToString(), key?.ToString(), null, tmp, 0x2000, path);
		return size == 0 ? null : tmp.ToString();
	}

	public void WriteValue(object section, object Key, object Value)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		Kernel32.WritePrivateProfileString(section.ToString(), Key?.ToString(), Value?.ToString(), path);
	}

	public void RemoveSection(object section) => WriteValue(section, null, null);

	public void RemoveKey(object section, object Key) => WriteValue(section, Key, null);
	#endregion
}