using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CDForm
{
    public partial class CDCreateExam : Form
    {
        public CDCreateExam()
        {
            InitializeComponent();
        }

        //刷新父级窗口
        public delegate void updateParentData(object sender);
        public event updateParentData updateIt;
        BLL.Exam exam = new BLL.Exam();
        BLL.Class classes = new BLL.Class();
        private DataTable teacherClass;

        private int selectedClassID;

        DataTable dt = new DataTable();

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                string fileName = fd.FileName;
                bind(fileName);
            }
        }

        private void bind(string fileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + ";" + "Extended Properties='Excel 8.0;HDR=Yes; IMEX=1'";
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From [Sheet1$]", strConn);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "exam");
                dt = ds.Tables["exam"];
                dataGridView1.DataSource = dt;
            }
            catch (Exception err)
            {
                MessageBox.Show("操作失败！" + err.ToString());
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string examName = txtExamName.Text;
                if (examName == "" || cbClass.Text== "")
                {
                    MessageBox.Show("以上空格不可为空");
                }
                else
                {
                    exam.createExam(examName, dt, UserHelper.userID, this.selectedClassID);
                    MessageBox.Show("创建试卷成功");
                    if (this.updateIt != null)
                    {
                        updateIt(null);
                    }
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("没有数据");
                this.Close();
            }

        }

        private void CDCreateExam_Load(object sender, EventArgs e)
        {
            teacherClass = classes.getClass(UserHelper.userID);
            for (int i = 0; i < teacherClass.Rows.Count; i++)
            {
                cbClass.Items.Add(teacherClass.Rows[i]["class"].ToString());
            }
        }

        private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedClassID = int.Parse(teacherClass.Rows[Convert.ToInt32(this.cbClass.SelectedIndex)]["class_id"].ToString());
        }

    }
}
