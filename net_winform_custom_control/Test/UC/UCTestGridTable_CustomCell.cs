﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test.UC
{
    public partial class UCTestGridTable_CustomCell : UserControl, HZH_Controls.Controls.IDataGridViewCustomCell
    {
        private TestGridModel m_object = null;
        public UCTestGridTable_CustomCell()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is TestGridModel)
                m_object = (TestGridModel)obj;
        }

        private void ucBtnExt1_Click(object sender, EventArgs e)
        {
            if (m_object != null)
            {
                HZH_Controls.Forms.FrmTips.ShowTipsSuccess("修改："+m_object.Name);
            }
        }

        private void ucBtnExt2_Click(object sender, EventArgs e)
        {
            if (m_object != null)
            {
                HZH_Controls.Forms.FrmTips.ShowTipsSuccess("删除：" + m_object.Name);
            }
        }
    }
}