using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CDForm
{
    public partial class index : Form
    {

        BLL.Exam bllExam = new BLL.Exam();
        BLL.Class classes = new BLL.Class();
        BLL.User user = new BLL.User();
        private bool update = false;
        private int updateClassID;
        private DataRow userInfo;
        public string path = Application.StartupPath + @"song.mp3";

        
        public index()
        {
            InitializeComponent();
            this.axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(path));
            if (UserHelper.password == "" && UserHelper.identity == "")
            {
                panelLogin.Show();
                panelIndex.Hide();
                panelUserInfo.Hide();
                panelExam.Hide();
                panelStuClass.Hide();
                panelClass.Hide();
                panelCheckUser.Hide();
                panelManageIndex.Hide();
            }
            else
            {
                if (UserHelper.identity == "0") panelIndex.Show();
                else { panelManageIndex.Show(); panelManageIndex.Controls.Add(menuStrip1); }
                showMenu();
                panelLogin.Hide();
                panelUserInfo.Hide();
                panelExam.Hide();
                panelStuClass.Hide();
                panelClass.Hide();
                panelCheckUser.Hide();
            }
        }
        //点击登录按钮
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool identity = (radioStu.Checked || radioTea.Checked);
            string table = radioStu.Checked ? "0" : "1";
            if (username == "" || password == "" || !identity)
            {
                MessageBox.Show("用户名,密码以及身份不能为空！");
                txtUserName.Focus();
                return;
            }
            else
            {
                BLL.User user = new BLL.User();
                DataTable dt = user.Login(username, password, table);
                if (dt.Rows.Count > 0)
                {

                    UserHelper.userID = int.Parse(dt.Rows[0]["user_id"].ToString());
                    UserHelper.userNickName = (dt.Rows[0]["userNickName"].ToString());
                    UserHelper.password = password;
                    UserHelper.identity = table;
                    MessageBox.Show("登陆成功！");

                    showIndex();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误，请重新输入！", "错误");
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    txtUserName.Focus();
                }
            }
        }
        //显示首页
        private void showIndex()
        {
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelStuClass.Hide();
            panelExam.Hide();
            panelClass.Hide();
            panelCheckUser.Hide();
            if (UserHelper.identity == "0")
            {
                panelManageIndex.Hide();
                panelIndex.Show();
                panelIndex.Controls.Add(menuStrip1);
            }
            else
            {
                panelManageIndex.Show();
                panelManageIndex.Controls.Add(menuStrip1);
                panelIndex.Hide();
            }
            showMenu();
            labelHello.Text = "Hello," + UserHelper.userNickName;
        }

        private void btnStuSubmit_Click(object sender, EventArgs e)
        {
            string userNickName = txtStuNickName.Text;
            string userName = txtStuName.Text;
            int sex = txtStuSex.Text == "男" ? 0 : 1;
            string birthday = txtStuBirth.Text;
            string email = txtStuEmail.Text;
            string MSM = txtStuMSM.Text;
            string qq = txtStuQQ.Text;
            user.updateUserInfo(UserHelper.userID, userNickName, userName, sex, birthday, email, MSM, qq);
            MessageBox.Show("资料更新成功");
        }

        private void menuChangeInfo_Click(object sender, EventArgs e)
        {
            panelIndex.Hide();
            panelManageIndex.Hide();
            panelLogin.Hide();
            panelExam.Hide();
            panelStuClass.Hide();
            panelUserInfo.Show();
            panelClass.Hide();
            panelCheckUser.Hide();
            showMenu();

            DataRow dt = user.getUserInfo(UserHelper.userID);
            this.userInfo = dt;

            txtStuNickName.Text = dt["username"].ToString();
            txtStuName.Text = dt["userNickName"].ToString();
            txtStuSex.Text = dt["sex"].ToString() == "0" ? "男" : "女";
            txtStuBirth.Text = dt["birthday"].ToString();
            txtStuEmail.Text = dt["email"].ToString();
            txtStuMSM.Text = dt["MSM"].ToString();
            txtStuQQ.Text = dt["qq"].ToString();
            ShowPhoto(dt);
        }

        public void ShowPhoto(DataRow info)
        {
            //=====================显示图片======================== 
            if ((picStu.Image != null))
            {
                picStu.Image.Dispose();
                //先将图片框清一下 
                picStu.Image = null;
            }

            if (!(info["picture"] == System.DBNull.Value))
            {
                //①将photo字段变成字节型数组 
                byte[] BBytes = null;
                BBytes = (byte[])info["picture"];
                if (BBytes.LongLength > 0)
                {
                    //②将字节型数组内容变成IO内存数据流。 
                    System.IO.MemoryStream Mystrm = new System.IO.MemoryStream(BBytes);
                    //③将IO内存数据流对象再变成照片对象 
                    Bitmap Bmp = new Bitmap(Mystrm);
                    //④将照片对象的值赋给图片框的Image属性显示出图像 
                    picStu.Image = Bmp;
                }
            }
            //=====================显示完毕=============================== 
        }

        private void btnUploadPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                byte[] B = user.uploadPic(UserHelper.userID, fd.FileName);
                this.userInfo["picture"] = B;
                ShowPhoto(this.userInfo);
                MessageBox.Show("图片写入成功");
            }
        }

        private void btnInfoBack_Click(object sender, EventArgs e)
        {
            showIndex();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            refreshPanelStuClass();
        }
        //查看加入了哪个方言集
        private void refreshPanelStuClass()
        {
            panelIndex.Hide();
            panelManageIndex.Hide();
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelExam.Hide();
            panelStuClass.Show();
            panelClass.Hide();
            panelCheckUser.Hide();


            for (int i = flowLayoutPanel2.Controls.Count - 1; i >= 0; i--)
            {
                Control con = flowLayoutPanel2.Controls[i];
                if (con is Label || con is LinkLabel)
                {
                    flowLayoutPanel2.Controls.RemoveAt(i);
                }
            }
            for (int i = flowLayoutPanel3.Controls.Count - 1; i >= 0; i--)
            {
                Control con = flowLayoutPanel3.Controls[i];
                if (con is Label || con is LinkLabel)
                {
                    flowLayoutPanel3.Controls.RemoveAt(i);
                }
            }
            DataTable dt = classes.getOwnClass(UserHelper.userID);
            DataTable dtall = classes.getAllClass(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label label1 = new Label();
                label1.Location = new Point(3, 0);
                label1.Size = new Size(200, 20);
                label1.Text = dt.Rows[i]["class"].ToString();
                flowLayoutPanel3.Controls.Add(label1);

                LinkLabel linklabel1 = new LinkLabel();
                linklabel1.AutoSize = true;
                linklabel1.Location = new Point(172, 0);
                linklabel1.Size = new Size(170, 20);
                linklabel1.Name = dt.Rows[i]["class_id"].ToString();
                linklabel1.Click += new EventHandler(linklabelCancel_Click);
                linklabel1.Text = "退出";
                flowLayoutPanel3.Controls.Add(linklabel1);

                LinkLabel linklabel2 = new LinkLabel();
                linklabel2.AutoSize = true;
                linklabel2.Location = new Point(272, 0);
                linklabel2.Size = new Size(170, 20);
                linklabel2.Name = dt.Rows[i]["class_id"].ToString();
                linklabel2.Click += delegate(Object o, EventArgs ee) { doSomething(linklabel2); };
                linklabel2.Text = "查看考卷";
                flowLayoutPanel3.Controls.Add(linklabel2);

            }

            for (int i = 0; i < dtall.Rows.Count; i++)
            {
                Label label2 = new Label();
                label2.Location = new Point(3, 0);
                label2.Size = new Size(250, 20);
                label2.Text = dtall.Rows[i]["class"].ToString();
                flowLayoutPanel2.Controls.Add(label2);

                LinkLabel linklabel2 = new LinkLabel();
                linklabel2.AutoSize = true;
                linklabel2.Location = new Point(175, 0);
                linklabel2.Size = new Size(77, 20);
                linklabel2.Text = "加入";
                linklabel2.Name = dtall.Rows[i]["class_id"].ToString();
                linklabel2.Click += new EventHandler(linklabelEnter_Click);
                flowLayoutPanel2.Controls.Add(linklabel2);
            }
        }

        private void linklabelEnter_Click(object sender, EventArgs e)
        {
            LinkLabel linklabelClicked = sender as LinkLabel;
            DialogResult result = MessageBox.Show("确定加入该方言？", "加入方言对话框", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {

                int classID = int.Parse(linklabelClicked.Name);
                classes.enterClass(classID, UserHelper.userID);
            }
            refreshPanelStuClass();
        }

        private void linklabelCancel_Click(object sender, EventArgs e)
        {
            LinkLabel linklabelClicked = sender as LinkLabel;
            DialogResult result = MessageBox.Show("确定退出该方言？", "退出方言对话框", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int classID = int.Parse(linklabelClicked.Name);
                classes.cancelClass(classID, UserHelper.userID);
            }
            refreshPanelStuClass();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showIndex();
        }
        public int classNum = 0;
        //加载该方言集的所有试卷
        public void doSomething(object sender)
        {
            panelExam.Show();
            panelIndex.Hide();
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelStuClass.Hide();
            panelClass.Hide();
            panelCheckUser.Hide();
            panelManageIndex.Hide();
            if (UserHelper.identity == "1")
            {
                panelExam.Controls.Add(menuStrip1);
                linkLabel5.Hide();
            }
            else
            {
                linkLabel5.Show();
            }
            LinkLabel clickLabel = sender as LinkLabel;
            if (clickLabel != null)
            {
                this.classNum = int.Parse(clickLabel.Name);
            }
            
            for (int i = flowLayoutPanel1.Controls.Count - 1; i >= 0; i--)
            {
                flowLayoutPanel1.Controls.RemoveAt(i);
            }
            
            DataTable bt = new DataTable();
            if (UserHelper.identity == "0")
            {
                bt = bllExam.getExam(UserHelper.userID, this.classNum);
            }
            else
            {
                bt = bllExam.getTeacherExam(UserHelper.userID);
            }
            for (int i = 0; i < bt.Rows.Count; i++)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(index));
                if (UserHelper.identity == "0")
                {

                    string score = bllExam.checkExamHasDone(UserHelper.userID, int.Parse(bt.Rows[i]["exam_id"].ToString()));
                    Button button1 = new Button();
                    button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
                    button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    button1.Location = new System.Drawing.Point(10, 3);
                    button1.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
                    button1.Name = bt.Rows[i]["exam_id"].ToString();
                    button1.Size = new System.Drawing.Size(183, 53);
                    button1.TabIndex = 0;
                    
                    button1.UseVisualStyleBackColor = true;
                    flowLayoutPanel1.Controls.Add(button1);
                    if (score != "null")
                    {
                        button1.Text = bt.Rows[i]["exam"].ToString() + " (" + score + ")"; 
                        button1.Click += new EventHandler(label2_Click);
                    }
                    else
                    {
                        button1.Text = bt.Rows[i]["exam"].ToString();
                        button1.Click += new EventHandler(labelButton_Click);
                        //flowLayoutPanel2.Controls.Add(label4);
                    }
                }
                else
                {
                    Label label1 = new Label();
                    label1.Location = new Point(3, 0);
                    label1.Size = new System.Drawing.Size(486, 29);
                    label1.Text = bt.Rows[i]["exam"].ToString();

                    LinkLabel label2 = new LinkLabel();
                    label2.Location = new System.Drawing.Point(495, 0);
                    label2.Size = new System.Drawing.Size(89, 29);
                    label2.Name = bt.Rows[i]["exam_id"].ToString();
                    label2.Text = "删除";

                    LinkLabel label3 = new LinkLabel();
                    label3.Location = new System.Drawing.Point(590, 0);
                    label3.Size = new System.Drawing.Size(89, 29);
                    label3.TabStop = true;
                    label3.Text = "查看考卷";
                    label3.Name = bt.Rows[i]["exam_id"].ToString();

                    label3.Click += new EventHandler(label1_Click);
                    label2.Click += new EventHandler(labelDelExam_Click);
                    flowLayoutPanel1.Controls.Add(label1);
                    flowLayoutPanel1.Controls.Add(label2);
                    flowLayoutPanel1.Controls.Add(label3);

                }
            }
        }

        public void label1_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            int exam_id = int.Parse(clickedLabel.Name);
            CDDoExam doexam = new CDDoExam(exam_id, this);
            doexam.updateIt += new CDDoExam.updateParentData(doSomething);
            doexam.Show();
        }

        public void labelButton_Click(object sender, EventArgs e)
        {
            Button clickedLabel = sender as Button;
            int exam_id = int.Parse(clickedLabel.Name);
            CDDoExam doexam = new CDDoExam(exam_id, this);
            doexam.updateIt += new CDDoExam.updateParentData(doSomething);
            doexam.Show();
        }

        public void label2_Click(object sender, EventArgs e)
        {
            Button clickedLabel = sender as Button;
            int exam_id = int.Parse(clickedLabel.Name);
            CDDoExam doexam = new CDDoExam(exam_id, this, false);
            doexam.Show();
        }

        public void labelDelExam_Click(object sender, EventArgs e)
        {
            LinkLabel clickLabel = sender as LinkLabel;
            DialogResult result = MessageBox.Show("确定删除该考试卷？", "删除对话框", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int examID = int.Parse(clickLabel.Name);
                bllExam.delExam(examID);
                this.doSomething(sender);
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            refreshPanelStuClass();
        }

        private void menuClass_Click(object sender, EventArgs e)
        {
            panelClass.Show();
            showMenu();
            panelExam.Hide();
            panelIndex.Hide();
            panelManageIndex.Hide();
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelStuClass.Hide();
            panelCheckUser.Hide();
            panelClass.Controls.Add(menuStrip1);
            loadClass();
        }

        private void loadClass()
        {
            for (int i = flowLayoutPanel4.Controls.Count - 1; i >= 0; i--)
            {
                Control con = flowLayoutPanel4.Controls[i];
                if ((con is Label) || (con is LinkLabel))
                {
                    flowLayoutPanel4.Controls.RemoveAt(i);
                }
            }

            DataTable dt = classes.getClass(UserHelper.userID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label label = new Label();
                label.Location = new Point(3, 0);
                label.Size = new Size(394, 25);
                label.Text = dt.Rows[i]["class"].ToString();

                LinkLabel linkLabel1 = new LinkLabel();
                linkLabel1.Location = new Point(474, 0);
                linkLabel1.Size = new Size(65, 25);
                linkLabel1.Name = dt.Rows[i]["class_id"].ToString();
                linkLabel1.Click += new EventHandler(labelUpdate_Click);
                linkLabel1.Text = "编辑";

                LinkLabel linkLabel2 = new LinkLabel();
                linkLabel2.Location = new Point(403, 0);
                linkLabel2.Size = new Size(65, 25);
                linkLabel2.Name = dt.Rows[i]["class_id"].ToString();
                linkLabel2.Click += new EventHandler(labelDel_Click);
                linkLabel2.Text = "删除";

                flowLayoutPanel4.Controls.Add(label);
                flowLayoutPanel4.Controls.Add(linkLabel1);
                flowLayoutPanel4.Controls.Add(linkLabel2);

                txtClassName.Text = "";
            }
        }

        public void labelUpdate_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            int classID = int.Parse(clickedLabel.Name);
            DataTable dt = classes.getClassName(classID);
            txtClassName.Text = dt.Rows[0]["class"].ToString();
            this.update = true;
            this.updateClassID = classID;
        }

        public void labelDel_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            DialogResult result = MessageBox.Show("确定删除该方言？", "删除对话框", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int classID = int.Parse(clickedLabel.Name);
                classes.delClass(classID);
            }
            loadClass();
        }

        private void menuTest_Click(object sender, EventArgs e)
        {
            panelExam.Show();
            panelExam.Hide();
            panelIndex.Hide();
            panelManageIndex.Hide();
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelStuClass.Hide();
            panelCheckUser.Hide();
            panelExam.Controls.Add(menuStrip1); showMenu();
            if(UserHelper.identity == "1")linkLabel5.Hide();
            this.doSomething(sender);
        }
        private DataTable cbClassDataTabel, cbExamDataTabel;
        private void menuUser_Click(object sender, EventArgs e)
        {
            panelExam.Hide();
            panelExam.Hide();
            panelIndex.Hide();
            panelManageIndex.Hide();
            panelLogin.Hide();
            panelUserInfo.Hide();
            panelStuClass.Hide();
            panelCheckUser.Show();
            panelCheckUser.Controls.Add(menuStrip1);
            showMenu();

            for (int i = cbClass.Items.Count - 1; i >= 0; i--)
            {
                cbClass.Items.RemoveAt(i);
            }

            DataTable dt = classes.getClass(UserHelper.userID);
            cbClassDataTabel = dt;

            cbMouseLeave(cbClass, dt, Convert.ToInt32(cbClass.SelectedValue));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbClass.Items.Add(dt.Rows[i]["class"].ToString());
            }
            cbClass.TextChanged += delegate(Object o, EventArgs ee) { cbMouseLeave(cbClass, dt, Convert.ToInt32(cbClass.SelectedValue)); };
            cbClass.SelectedIndexChanged += delegate(Object o, EventArgs ee) { cbMouseLeave(cbClass, dt, Convert.ToInt32(cbClass.SelectedValue)); };
            btnSelectByClass.Click += delegate(Object o, EventArgs ee) { btnSelectByClass_Click(cbClass, Convert.ToInt32(cbClass.SelectedValue), Convert.ToInt32(cbTest.SelectedValue)); };
        }

        public void cbMouseLeave(object sender, DataTable cbDT, int selectNum)
        {
            ComboBox cb = sender as ComboBox;
            DataTable dt;
            for (int i = cbTest.Items.Count - 1; i >= 0; i--)
            {
                cbTest.Items.RemoveAt(i);
            }
            if (cb.Text == "")
            {
                dt = bllExam.getTeacherExam(UserHelper.userID);
                cbExamDataTabel = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cbTest.Items.Add(dt.Rows[i]["exam"].ToString());
                }
            }
            else
            {
                int index = cbClass.FindString(cbClass.Text);
                dt = bllExam.getExamByClassID(int.Parse(cbDT.Rows[index]["class_id"].ToString()));
                cbExamDataTabel = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cbTest.Items.Add(dt.Rows[i]["exam"].ToString());
                }
            }

        }

        private void btnSelectByClass_Click(object sender, int selectClass, int selectExam)
        {
            bool selectClassBool = false, selectExamBool = false;
            if (cbClass.Text != "") { selectClassBool = true; }
            if (cbTest.Text != "") { selectExamBool = true; }

            bool selectGirl = radioGirl.Checked;
            bool selectBoy = radioBoy.Checked;
            bool selectAll = radioAll.Checked;
            int selectClassInt = 0, selectExamInt = 0;
            if (selectClassBool)
            {
                selectClassInt = int.Parse(cbClassDataTabel.Rows[selectClass]["class_id"].ToString());
            }
            if (selectExamBool)
            {
                selectExamInt = int.Parse(cbExamDataTabel.Rows[selectExam]["exam_id"].ToString());
            }
            DataTable dt = user.selectByClass(selectClassInt, selectExamInt, selectGirl, selectBoy, selectAll, selectClassBool, selectExamBool);
            dataGridUser.DataSource = dt;
        }

        private void btnSelectByName_Click(object sender, EventArgs e)
        {
            string username = txtUserName1.Text;
            dataGridUser.DataSource = user.selectByUserName(username);

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string className = txtClassName.Text;
            if (this.update == false)
            {
                classes.createClass(className, UserHelper.userID);
            }
            else
            {
                classes.updateClassName(className, this.updateClassID);
                this.update = false;
            }
            loadClass();
        }

        private void createExam_Click(object sender, EventArgs e)
        {
            CDCreateExam createExam = new CDCreateExam();
            createExam.updateIt += new CDCreateExam.updateParentData(doSomething);
            createExam.ShowDialog();
        }

        private void menuManageIndex_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelIndex.Hide();
            panelUserInfo.Hide();
            panelExam.Hide();
            panelStuClass.Hide();
            panelClass.Hide();
            panelCheckUser.Hide();
            panelManageIndex.Show();
            panelManageIndex.Controls.Add(menuStrip1);
            showMenu();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            UserHelper.userID = 0;
            UserHelper.userNickName = "";
            UserHelper.identity = "";
            UserHelper.password = "";
            panelLogin.Show();
            panelIndex.Hide();
            panelUserInfo.Hide();
            panelExam.Hide();
            panelStuClass.Hide();
            panelClass.Hide();
            panelCheckUser.Hide();
            panelManageIndex.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CDRegist regist = new CDRegist();
            regist.Show();
        }
        //菜单显示控制
        private void showMenu()
        {
            if (UserHelper.identity == "0")
            {
                menuChange.Visible = true;
                menuLogout.Visible = true;
                menuManageIndex.Visible = false;
                createExam.Visible = false;
                menuCheck.Visible = false;
            }
            else
            {
                menuChange.Visible = true;
                menuLogout.Visible = true;
                menuManageIndex.Visible = true;
                createExam.Visible = true;
                menuCheck.Visible = true;
            }
        }
        //修改密码
        private void menuChangePass_Click(object sender, EventArgs e)
        {
            CDChangePass changePass = new CDChangePass();
            changePass.Show();
        }


        //多线程播放背景音乐

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                Thread thread = new Thread(new ThreadStart(PlayThread));
                thread.Start();
            }
        }
        private void PlayThread()
        {
            axWindowsMediaPlayer1.URL = "song.mp3";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }







    }
}
