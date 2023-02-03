using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Controls;

namespace Xylia.Windows.Forms
{
	public partial class Announcement : MetroFramework.Forms.MetroForm
    {
        public Announcement(string Content, bool Test = false,Dictionary<string, EventHandler> vs = null)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            Content = Content.Replace("<br>", "\n");


			if (Content.Contains("</div>") || Test) this.metroLabel1.Text = Content;
			else
			{
				this.Height = 120 + (metroLabel1.Text = Content).Split('\n').Length * 20;
				this.metroLabel1.Text = this.metroLabel1.Text.Replace(@"\" + "n", "\n");
			}


			//(this.htmlLabel1.Text == "暂无公告" ? (Control)this.metroLabel1 : this.htmlLabel1).BringToFront();

			if (vs == null)  return;
            foreach(var item in vs)
            {
                MetroButton Btn = new MetroButton();
                Btn.Text = item.Key;
                Btn.Click += item.Value;
                metroPanel1.Controls.Add(Btn);
            }

            SortBtn();
        }


        public static void Show(object Content,string Title = "提示消息")
        {
            Announcement announcement =  new Announcement(Content.ToString());
            announcement.TilteText = Title;

            announcement.Btn_Exit.Visible = false;
            announcement.ShowDialog();        
        }


        /// <summary>
        /// 对按钮排序
        /// </summary>
        private void SortBtn()
        {
            int StartLength = 100;

            if (this.Width > 600) StartLength = 0;


            int BtnCount = 0;
            int BtnIndex = 0;


            foreach (Control ctl in metroPanel1.Controls)
            {
                if (ctl is Button || ctl is MetroButton)
                    BtnCount++;
            }

            int SpaceWidth = (metroPanel1.Width - StartLength - BtnCount * Btn_Exit.Width) / (BtnCount + 1);


            foreach (Control Ctrl in metroPanel1.Controls)
            {
                if ((Ctrl is Button || Ctrl is MetroButton) && Ctrl.Visible == true)
                {
                    BtnIndex++;

                    int X = StartLength + SpaceWidth * BtnIndex + (BtnIndex - 1) * Btn_Exit.Width;

                    Ctrl.Location = new Point(X); 
                    Ctrl.Refresh();
                }
            }
        }


        private void Announcement_MouseEnter(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void Announcement_Load(object sender, EventArgs e)
        {
            if (this.Height > 500)
                this.Height = 500;

            else if(this.Height < 220)
            {
                this.Height = 220;
                this.Width = 400;
            }
        }


        [Category("Set"),Description("页面设置")]
        public string TilteText
        {
            get => this.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                this.Text = value;
            }              
        }

        public string OkText
        {
            get => Btn_OK.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                Btn_OK.Text = value;
            }
        }

        public string ExitText
        {
            get => Btn_Exit.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                Btn_Exit.Text = value;
            }
        }


        public delegate void CancelHandle(object sender, EventArgs e);
        public delegate void OkHandle(object sender, EventArgs e);

        //定义事件
        public event CancelHandle  CancelClicked;
        public event OkHandle      OkClicked;

        private void Announcement_SizeChanged(object sender, EventArgs e)
        {
            int LocationY = metroPanel1.Location.Y + 10;

            metroPanel1.Height = DefaultHeight_Btn;
            panel1.Height = this.Height - 100;

            SortBtn();          
        }

        int DefaultHeight_Btn = 23;

        private void Announcement_TextChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void Btn_Demo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            if (CancelClicked != null) CancelClicked(sender, new EventArgs());//把按钮自身作为参数传递
            this.Close();
        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;


            if (OkClicked != null)  OkClicked(sender, new EventArgs());
            else this.Close();
        }
    }
}