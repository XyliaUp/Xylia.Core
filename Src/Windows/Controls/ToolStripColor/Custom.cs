using System.Drawing;
using System.Windows.Forms;

namespace Xylia.Windows.Controls.Style
{
	public class ToolStripCustom : ProfessionalColorTable
	{
		/// <summary>
		/// 主菜单项被点击后，展开的下拉菜单面板的边框
		/// </summary>
		public override Color MenuBorder
		{
			get
			{
				return Color.FromArgb(170, 225, 147);
			}
		}
		/// <summary>
		/// 鼠标移动到菜单项（主菜单及下拉菜单）时，下拉菜单项的边框
		/// </summary>
		public override Color MenuItemBorder
		{
			get
			{
				return Color.Transparent;
			}
		}
		#region 顶级菜单被选中背景颜色
		public override Color MenuItemSelectedGradientBegin
		{
			get
			{
				return Color.FromArgb(170, 225, 147);
			}
		}
		public override Color MenuItemSelectedGradientEnd
		{
			get
			{
				return Color.FromArgb(170, 225, 147);
			}
		}
		#endregion

		#region 顶级菜单被按下是，菜单项背景色
		public override Color MenuItemPressedGradientBegin
		{
			get
			{
				return Color.Black;
			}
		}
		public override Color MenuItemPressedGradientMiddle
		{
			get
			{
				return Color.FromArgb(170, 225, 147);
			}
		}
		public override Color MenuItemPressedGradientEnd
		{
			get
			{
				return Color.Black;
			}
		}
		#endregion
		/// <summary>
		/// 菜单项被选中时的颜色
		/// </summary>
		public override Color MenuItemSelected
		{
			get
			{
				return Color.FromArgb(170, 225, 147);
			}
		}
		#region 下拉菜单面板背景设置（不包括下拉菜单项）
		//下拉菜单面板背景一共分为2个部分，左边为图像区域，右侧为文本区域，需要分别设置
		//ToolStripDropDownBackground设置文本部分的背景色
		public override Color ToolStripDropDownBackground
		{
			get
			{
				return Color.White;
			}
		}
		//以ImageMarginGradient开头的3个设置的是图像部分的背景色，begin->end是从左到右的顺序
		public override Color ImageMarginGradientBegin
		{
			get
			{
				return Color.White;
			}
		}
		public override Color ImageMarginGradientMiddle
		{
			get
			{
				return Color.White;
			}
		}
		public override Color ImageMarginGradientEnd
		{
			get
			{
				return Color.White;
			}
		}
		#endregion
	}

}
