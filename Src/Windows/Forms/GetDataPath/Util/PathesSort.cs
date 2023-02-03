using System.Collections.Generic;

using Xylia.bns.Util;

namespace Xylia.bns.Util.Sort
{
	/// <summary>
	/// 输出前排序 (调用请接入ToArrayCustom函数）
	/// </summary>
	public class PathesSort : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			// 1在后，-1在前
			if (x.Contains("备份") || x.Contains("backup")) return 1;
			else if (y.Contains("备份") || y.Contains("backup")) return -1;
			else return 0;
		}
	}
}
