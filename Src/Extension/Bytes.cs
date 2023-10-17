﻿using System.Text;

namespace Xylia.Extension;
public static class Bytes
{
	public static string ToHex(this byte[] value, bool original = true)
	{
		if (value.Length == 0)
		{
			return string.Empty;
		}

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < value.Length; i++)
		{
			var b = value[i];

			#region compress 
			if (!original && b == 0)
			{
				int j = i;
				for (; j < value.Length; j++)
					if (value[j] != 0) break;

				if (j - i > 1)
				{
					sb.Append($"[{j - i}]");

					i = j - 1;
					continue;
				}
			}
			#endregion

			sb.AppendFormat("{0:X2}", b);
		}

		return sb.ToString();
	}

	public static byte[] ToBytes(this string Hex)
	{
		Hex = Hex.UnCompress();
		if (string.IsNullOrWhiteSpace(Hex))
			return Array.Empty<byte>();


		var inputByteArray = new byte[Hex.Length / 2];
		for (var x = 0; x < inputByteArray.Length; x++)
			inputByteArray[x] = (byte)Convert.ToInt32(Hex.Substring(x * 2, 2), 16);

		return inputByteArray;
	}



	public static string UnCompress(this string Cipher)
	{
		if (string.IsNullOrWhiteSpace(Cipher))
			return Cipher;

		StringBuilder builder = new();
		for (int i = 0; i < Cipher.Length; i++)
		{
			char s = Cipher[i];

			if (i + 1 != Cipher.Length && s == '[')
			{
				StringBuilder InsiderBuilder = new StringBuilder();

				int NextId = i + 1;
				char CurChar = Cipher[NextId];

				while (CurChar != ']')
				{
					InsiderBuilder.Append(CurChar);

					int NewId = ++NextId;

					if (Cipher.Length < NewId + 1) throw new Exception("压缩文本中缺失了后标(即 \"]\" 标识)。");
					CurChar = Cipher[NewId];
				}

				for (int f = 0; f < int.Parse(InsiderBuilder.ToString()); f++) builder.Append('0');
				InsiderBuilder.Clear();
				i = NextId;
			}
			else if (s == ']') throw new Exception("无效的压缩文本后标");
			else builder.Append(s);
		}

		return builder.ToString().Replace(" ", null);
	}
}