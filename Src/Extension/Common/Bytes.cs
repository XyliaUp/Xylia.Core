using System.Text;

namespace Xylia.Extension;
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
		if (string.IsNullOrWhiteSpace(Hex)) 
			return Array.Empty<byte>();


		var inputByteArray = new byte[Hex.Length / 2];
		for (var x = 0; x < inputByteArray.Length; x++)
			inputByteArray[x] = (byte)Convert.ToInt32(Hex.Substring(x * 2, 2), 16);

		return inputByteArray;
	}



	public static string StringZip(this string Text)
	{
		if (string.IsNullOrWhiteSpace(Text))
			return Text;


		StringBuilder builder = new StringBuilder();

		char firstChar = Text[0];  //字符串中第一个字符                
		int count = 1;            //字符数量默认为1

		for (int i = 1; i < Text.Length; i++)
		{
			char s = Text[i];      //字符串中第2个字符

			if (firstChar == '0' && firstChar == s) count++; //存在多个0时
			else
			{
				if (count > 1)  //数量 >1 时
				{
					builder.Append($"[{count}]");
					count = 1;                //初始化
				}
				else builder.Append(firstChar);  //数量 ≤1 时
			}
			firstChar = s; //把第2个字符赋值给第一个字符
		}

		if (count > 1) builder.Append($"[{count}]");  

		if (count <= 1 || firstChar != '0') builder.Append(firstChar);

		return builder.ToString();
	}

	public static string StringUnzip(this string Cipher)
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
			else if (s == ']') throw new Exception("无效的压缩文本后标，请确认前标是否遗漏。");
			else builder.Append(s);
		}

		return builder.ToString().Replace(" ", null);
	}
}