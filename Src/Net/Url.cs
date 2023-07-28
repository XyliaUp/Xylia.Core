using System.Net;

namespace Xylia.Net;

public static class Url
{
	public static bool ExistsUrl(this string url)
	{
		try
		{
			new WebClient().OpenRead(url);
			return true;
		}
		catch (WebException) { return false; }
		catch { return false; }
	}
}
