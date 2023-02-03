using System;

namespace Xylia.Extension
{
	public static class Bytes
	{
		public static string ToHex(this byte[] bytes, bool original = false)
		{
			string hex = BitConverter.ToString(bytes, 0).Replace("-", string.Empty).ToLower();
			return original ? hex : hex.StringZip();
		}

		public static byte[] ToBytes(this string Hex)
		{
			Hex = Hex.StringUnzip();

			var inputByteArray = new byte[Hex.Length / 2];
			for (var x = 0; x < inputByteArray.Length; x++)
			{
				var i = Convert.ToInt32(Hex.Substring(x * 2, 2), 16);
				inputByteArray[x] = (byte)i;
			}
			return inputByteArray;
		}
	}
}