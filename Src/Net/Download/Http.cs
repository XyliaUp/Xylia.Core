using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Xylia.Net.Download
{
	public static class Http
	{
		/// <summary>
		/// 通过Http方式下载文件
		/// </summary>
		/// <param name="url"></param>
		/// <param name="filename"></param>
		/// <param name="Act"></param>
		/// <param name="Cover">允许覆盖</param>
		/// <param name="Needtip"></param>
		public static void DownloadFile(this Uri url, string filename, bool Cover = false, bool Needtip = true, Action<int, int> Act = null)
		{
			try
			{
				if (!File.Exists(filename) || Cover)
				{
					HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(url);
					HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();
					long totalBytes = myrp.ContentLength;

					if (Act != null) Act(0, (int)totalBytes);

					Stream st = myrp.GetResponseStream();
					Stream so = new FileStream(filename, FileMode.Create);
					long totalDownloadedByte = 0;
					byte[] by = new byte[1024];
					int osize = st.Read(by, 0, (int)by.Length);
					while (osize > 0)
					{
						totalDownloadedByte = osize + totalDownloadedByte;
						Application.DoEvents();
						so.Write(by, 0, osize);
						osize = st.Read(by, 0, (int)by.Length);

						if (Act != null) Act((int)((float)totalDownloadedByte / (float)totalBytes * 100), (int)totalBytes);
						Application.DoEvents(); //刷新太快无法显示时，去除此条注释
					}


					so.Close();
					st.Close();

					//文件下载完成！
					if (Needtip) Tip.Message("文件下载完成！");
				}
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
