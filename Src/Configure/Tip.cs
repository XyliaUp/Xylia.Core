namespace Xylia;

public static class Tip
{
	public static void Message(string Content = null, string Title = null, MessageBoxButtons Btn = MessageBoxButtons.OK)
	{
		MessageBox.Show(Content, Title ?? "提示信息", Btn, MessageBoxIcon.Information);
	}

	public static void Warning(string Content = null, string Title = null)
	{
		MessageBox.Show(Content, Title ?? "警告信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	}

	public static void Stop(string Content = null, string Title = null)
	{
		MessageBox.Show(Content, Title ?? "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
	}
}