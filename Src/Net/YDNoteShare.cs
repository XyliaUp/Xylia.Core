using System.Web;

using Newtonsoft.Json;

using Xylia.Net.Http;

namespace Xylia.Net;
public class YDNoteShare
{
	public static string Combine(string key, string p) => $"http://note.youdao.com/yws/api/personal/file/{p}?method=download&inline=true&shareKey={key}";

	public static Uri Resolve(string shareUrl, out FileInfo info)
	{
		if (!Url.ExistsUrl(shareUrl))
			throw new InvalidDataException();


		var shareUri = new Uri(shareUrl);
		string key = HttpUtility.ParseQueryString(shareUri.Query)["id"];

		info = JsonConvert.DeserializeObject<FileInfo>($"https://note.youdao.com/yws/public/note/{key}".GetHtml());
		info.key = key;

		return new Uri(Combine(key, info.p));
	}


	public struct FileInfo
	{
		public string key;

		public string p;
		public int ct;
		public object su;
		public int pr;
		public object au;

		/// <summary>
		/// 浏览次数
		/// </summary>
		public int pv;
		public int mt;
		public int size;
		public int domain;
		public string tl;
		public bool isFinanceNote;
	}
}