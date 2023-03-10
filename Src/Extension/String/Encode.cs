using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace Xylia.Extension
{
	public static partial class String
	{
		/// <summary>
		/// 编码文本
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string Encode(this string Text) => HttpUtility.HtmlEncode(XmlConvert.EncodeName(Text));
		/// <summary>
		/// 解码文本
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string Decode(this string Text) => HttpUtility.HtmlDecode(XmlConvert.DecodeName(Text));




		public static string ConvertToMD5(this string self, string flag = "x2")
		{
			byte[] sor = Encoding.UTF8.GetBytes(self);

			MD5 md5 = MD5.Create();
			byte[] result = md5.ComputeHash(sor);


			StringBuilder strbul = new();
			for (int i = 0; i < result.Length; i++)
				strbul.Append(result[i].ToString(flag));

			return strbul.ToString();
		}
	}
}