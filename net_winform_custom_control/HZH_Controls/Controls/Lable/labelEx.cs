//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System;
//using System.Runtime.InteropServices;
//using System.Drawing;

//using System.Windows.Forms;

//namespace HZH_Controls.Controls.Lable
//{
//	public class LabelEx : System.Windows.Forms.TextBox
//	{
//		[DllImport("user32", EntryPoint = "HideCaret")]
//		private static extern bool HideCaret(IntPtr hWnd);

//		[DllImport("user32", EntryPoint = "ShowCaret")]
//		private static extern bool ShowCaret(IntPtr hWnd);

//		public LabelEx() : base()
//		{
//			this.TabStop = false;
//			this.SetStyle(ControlStyles.Selectable, false);
//			this.Cursor = Cursors.Default;
//			this.ReadOnly = true;
//			this.ShortcutsEnabled = false;
//			this.HideSelection = true;
//			this.GotFocus += new EventHandler(TextBoxLabel_GotFocus);
//			this.MouseMove += new MouseEventHandler(TextBoxLabel_MouseMove);
//		}

//		private void TextBoxLabel_GotFocus(object sender, System.EventArgs e)
//		{
//			if (ShowCaret(((TextBox)sender).Handle))
//			{
//				HideCaret(((TextBox)sender).Handle);
//			}
//		}

//		private void TextBoxLabel_MouseMove(object sender, MouseEventArgs e)
//		{
//			if (((TextBox)sender).SelectedText.Length > 0)
//			{
//				((TextBox)sender).SelectionLength = 0;
//			}
//		}
//	}
//}
