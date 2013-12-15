using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CDForm
{
    public partial class CDChangePass : Form
    {
        BLL.User user = new BLL.User();

        public CDChangePass()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtOldPass.Text == "" || txtReNewPass.Text == "" || txtNewPass.Text == "")
            {
                label5.Text = "以上不可为空";
            }
            else if (txtReNewPass.Text != txtNewPass.Text)
            {
                label5.Text = "两次输入新密码不同";
            }
            else if (!user.checkPass(UserHelper.userID, txtOldPass.Text.Trim()))
            {
                label5.Text = "原密码不正确";
            }
            else
            {
                user.changePass(UserHelper.userID, txtOldPass.Text.Trim(), txtReNewPass.Text.Trim());
                MessageBox.Show("修改密码成功");
                this.Hide();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            txtOldPass.Focus();
            this.Hide();
        }
    }
}
