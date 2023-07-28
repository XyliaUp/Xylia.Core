using System.Net;
using System.Text;

namespace Xylia.Net.Http;
public static class HttpHelper
{
	public static string GetHtml(this string Url,int CodePage = 936)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		return Encoding.GetEncoding(CodePage).GetString(new WebClient().DownloadData(Url));
	}
}
