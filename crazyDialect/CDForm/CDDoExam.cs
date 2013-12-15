using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CDForm
{
    public partial class CDDoExam : Form
    {

        public int _exam_id;
        private bool status;
        BLL.Exam exam = new BLL.Exam();
        DataTable dt = new DataTable();
        Label[] labels = new Label[100];
        RadioButton[] radioButtons = new RadioButton[5];
        string[] studentAnswer;
        public Form _index;

        //刷新父级窗口
        public delegate void updateParentData(object sender);
        public event updateParentData updateIt;


        public CDDoExam(int exam_id, Form index, bool status = true)
        {
            InitializeComponent();
            this.status = status;
            this._exam_id = exam_id;
            this._index = index;

            //将四个单选按钮放入数组中，方便操作
            radioButtons[1] = radioButton1;
            radioButtons[2] = radioButton2;
            radioButtons[3] = radioButton3;
            radioButtons[4] = radioButton4;

            //获取该试卷所有的试题，放入datatable dt中
            dt = exam.getQuestion(exam_id);

            //如果该试卷已做 则获取学生答案
            if (this.status == false)
            {
                studentAnswer = exam.getUserAnswers(UserHelper.userID, this._exam_id);
            }

            //动态创建题目标号 label
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label label = new Label();
                label.BackColor = SystemColors.ActiveCaption;
                label.BorderStyle = BorderStyle.Fixed3D;
                label.Location = new Point(30 * i + 12, 325);
                label.Size = new Size(30, 30);
                label.TabIndex = 4;
                label.Text = (i + 1).ToString();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Click += new EventHandler(label_Click);

                if (this.status == false)
                {
                    if (dt.Rows[i]["answer"].ToString() == studentAnswer[i])
                    {
                        label.BackColor = Color.Green;
                    }
                    else
                    {
                        label.BackColor = Color.Red;
                    }
                }

                flowLayoutPanel1.Controls.Add(label);
                labels[i] = label;
                this.ResumeLayout(false);
            }
        }

        //放置当前题目的题号
        private int num;
        //form load完之后需要加载当前题目的选项
        private void CDDoExam_Load(object sender, EventArgs e)
        {
            textBox1.DataBindings.Add(new Binding("Text", dt, "question"));
            string[] optionArray = Regex.Split(dt.Rows[0]["options"].ToString(), "::", RegexOptions.IgnoreCase);
            radioButton1.Text = "A:" + optionArray[0];
            radioButton2.Text = "B:" + optionArray[1];
            radioButton3.Text = "C:" + optionArray[2];
            radioButton4.Text = "D:" + optionArray[3];
            this.BindingContext[dt].Position = 0;
            num = this.BindingContext[dt].Position;

            if (this.status == false)
            {
                for (int i = 1; i < radioButtons.Count(); i++)
                {
                    radioButtons[i].Enabled = false;
                }
                btnSubmit.Hide();
            }
            

            StudentOption();

            //textBox1.Text = num + ". " + textBox1.Text;
        }

        //题号click操作的公共函数
        private void label_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            this.BindingContext[dt].Position = int.Parse(clickedLabel.Text)-1;
            num = this.BindingContext[dt].Position;
            string[] optionArray = Regex.Split(dt.Rows[num]["options"].ToString(), "::", RegexOptions.IgnoreCase);
            radioButton1.Text = "A:"+optionArray[0];
            radioButton2.Text = "B:" + optionArray[1];
            radioButton3.Text = "C:" + optionArray[2];
            radioButton4.Text = "D:" + optionArray[3];
            StudentOption();
        }
        //radio单选click之后的公共函数
        private void radio_click(object sender, EventArgs e)
        {
            RadioButton clickRadio = sender as RadioButton;
            labels[num].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            string option = clickRadio.Text.Substring(0, 1);
            exam.setStudentOption(option, num);
        }

        private void StudentOption()
        {
            string[] studentOptions;
            if(this.status == false)studentOptions = this.studentAnswer;
            else studentOptions = exam.getStudentOption();
            if (studentOptions[num] == null||(studentOptions[num] == ""))
            {
                for (int i = 1; i < radioButtons.Count(); i++)
                {
                    radioButtons[i].Checked = false;
                }
            }
            else
            {
                string option = studentOptions[num];
                int intoption = option=="A"?1:(option=="B"?2:(option=="C"?3:(option=="D"?4:0)));
                radioButtons[intoption].Checked = true;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (exam.calculateScore(dt, this._exam_id, UserHelper.userID))
            {
                MessageBox.Show("提交考卷成功");
                this.Close();
                if (this.updateIt != null)
                {
                    updateIt(null);
                } 

            }
            else
            {
                MessageBox.Show("提交考卷失败");
            }
        }

    
    }
}
