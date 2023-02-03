using System.IO;

namespace Xylia.Extension
{
	public static class IO
	{
		/// <summary>
		/// 将 byte[] 转成 Stream
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static Stream ToStream(this byte[] bytes) => new MemoryStream(bytes);

		/// <summary>
		/// 将流转为数据
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static byte[] ToBytes(this Stream stream)
		{
			//流回退到起始位置才能开始写入，否则会为空
			stream.Seek(0, SeekOrigin.Begin);

			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);

			// 设置当前流的位置为流的开始
			stream.Seek(0, SeekOrigin.Begin);
			return bytes;
		}


		public static void SaveFile(this Stream stream, string fileName)
		{
			//流回退到起始位置才能开始写入，否则会为空
			stream.Seek(0, SeekOrigin.Begin);

			// 把 Stream 转换成 byte[]
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);

			// 设置当前流的位置为流的开始
			stream.Seek(0, SeekOrigin.Begin);

			// 把 byte[] 写入文件
			FileStream fs = new FileStream(fileName, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);

			bw.Write(bytes);
			bw.Close();
			fs.Close();
		}


		/// <summary>
		/// 当前流的剩余长度
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static long Remain(this Stream stream) => stream.Length - stream.Position;

		public static long Remain(this BinaryReader Reader) => Reader.BaseStream.Remain();

		public static long Remain(this BinaryWriter Writer) => Writer.BaseStream.Remain();

		public static byte[] ReadToEnd(this BinaryReader Reader) => Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position));
	}
}
