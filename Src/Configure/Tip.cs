using System;
using System.Windows.Forms;

using HZH_Controls.Forms;

namespace Xylia
{
	public static class Tip
	{
		public static void Message(string Content = null, string Title = null, MessageBoxButtons Btn = MessageBoxButtons.OK)
		{
			Content = Default(Content, StrType.Content);
			Title = Default(Title, StrType.Message);

			Xylia.Windows.Forms.Announcement.Show(Content, Title);
			//MessageBox.Show(Content, Title, Btn, MessageBoxIcon.Information);
		}

		public static void Warning(string Content = null, string Title = null)
		{
			Content = Default(Content, StrType.Content);
			Title = Default(Title, StrType.Warning);

			Xylia.Windows.Forms.Announcement.Show(Content, Title);
			//MessageBox.Show(Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void Stop(string Content = null, string Title = null)
		{
			Content = Default(Content, StrType.Content);
			Title = Default(Title, StrType.Warning);

			Xylia.Windows.Forms.Announcement.Show(Content, Title);
			//MessageBox.Show(Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		public enum StrType
		{
			Message = -1,
			Warning,
			Stop,
			Content,
		}

		private static string Default(string Str, StrType Type)
		{
			if (Str == null || Str == "" || Str == " ")
			{
				switch (Type)
				{
					case StrType.Message:
						return "提示信息";

					case StrType.Warning:
						return "警告信息";

					case StrType.Stop:
						return "错误信息";

					default:
						return "未定义内容的提示信息";
				}
			}
			else
			{
				return Str;
			}
		}

		 /// <summary>
		 /// 发送提示消息
		 /// </summary>
		 /// <param name="Msg"></param>
		 /// <param name="IsError"></param>
		 /// <param name="Duration"></param>
		public static void SendMessage(object Msg, bool IsError = false, int Duration = 3000)
		{
			string MsgInfo = Msg.ToString();

			if (Msg is Exception)
			{
				IsError = true;
				MsgInfo = (Msg as Exception).Message;

				Console.WriteLine(MsgInfo);
			}


			if (IsError) FrmTips.ShowTipsError(MsgInfo, Duration);
			else FrmTips.ShowTipsSuccess(MsgInfo, Duration);
		}
	}
}