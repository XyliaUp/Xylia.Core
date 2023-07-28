namespace Xylia.Extension;

public static class IO
{
	public static Stream ToStream(this byte[] bytes) => new MemoryStream(bytes);

	public static byte[] ToBytes(this Stream stream)
	{
		stream.Seek(0, SeekOrigin.Begin);

		byte[] bytes = new byte[stream.Length];
		stream.Read(bytes, 0, bytes.Length);

		// 设置当前流的位置为流的开始
		stream.Seek(0, SeekOrigin.Begin);
		return bytes;
	}


	public static void SaveFile(this Stream stream, string fileName)
	{
		stream.Seek(0, SeekOrigin.Begin);

		byte[] bytes = new byte[stream.Length];
		stream.Read(bytes, 0, bytes.Length);
		stream.Seek(0, SeekOrigin.Begin);

		FileStream fs = new FileStream(fileName, FileMode.Create);
		BinaryWriter bw = new BinaryWriter(fs);

		bw.Write(bytes);
		bw.Close();
		fs.Close();
	}



	public static long Remain(this Stream stream) => stream.Length - stream.Position;

	public static long Remain(this BinaryReader Reader) => Reader.BaseStream.Remain();

	public static long Remain(this BinaryWriter Writer) => Writer.BaseStream.Remain();

	public static byte[] ReadToEnd(this BinaryReader Reader) => Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position));
}
