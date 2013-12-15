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
    public partial class CDRegist : Form
    {
        private BLL.User user = new BLL.User();
        public CDRegist()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string number = txtNumber.Text;
            string userName = txtName.Text;
            string password = txtPassword.Text;
            string repassword = txtRePassword.Text;
            if (number == "" || userName == "" || password == "" || repassword == "")
            {
                errortip.Text = "以上为必填项";
            }
            else if (!Common.Utility.CheckEmail(number))
            {
                errortip.Text = "邮箱格式不正确";
            }
            else if (password != repassword)
            {
                errortip.Text = "输入的两次密码不同";
            }
            else if (!user.checkRegist(number))
            {
                errortip.Text = "该邮箱已被注册";
            }
            else
            {
                user.registUser(number, userName, password);
                MessageBox.Show("注册成功!");
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
