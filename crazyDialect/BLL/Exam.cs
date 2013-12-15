using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class Exam
    {
        DAL.Exam exam = new DAL.Exam();
        DAL.Class classes = new DAL.Class();
        private string[] studentOptions = new string[100];
        

        public void createExam(string examName, DataTable dt, int userID, int classID)
        {
            DataRow dr = null;
            int examID = createExamID(examName, userID, classID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                insertQuestion(examID, dr);
            }
        }

        public int createExamID(string examName, int userID, int classID)
        {
            return exam.createExamID(examName, userID, classID);
        }

        public void delExam(int examID)
        {
            exam.delExam(examID);
            exam.delExamQuestions(examID);
        }
        

        public void insertQuestion(int examID, DataRow dr)
        {
            string question = dr["题干"].ToString();
            string option = dr["A"].ToString()+"::"+ dr["B"].ToString()+"::"+dr["C"].ToString()+"::"+ dr["D"].ToString();
            string answer = dr["答案"].ToString();
            exam.insertQuestion(examID, question, answer, option);
        }

        public DataTable getQuestion(int examID)
        {
            return exam.getQuestion(examID);
        }

        public void setStudentOption(string option, int num)
        {
            studentOptions[num] = option;
        }

        public string[] getStudentOption()
        {
            return studentOptions;
        }

        public bool calculateScore(DataTable dt, int exam_id, int student_id)
        {
            float eachQuestion = 100 / dt.Rows.Count;
            float score = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["answer"].ToString() == studentOptions[i])
                {
                    score += eachQuestion;
                }
            }
            string option = string.Join(":",studentOptions);
            if (exam.insertScore(student_id, exam_id, option, score) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public DataTable getExam(int userID, int classID)
        {
            //DataTable dt = classes.getStuClass(userID);
            //DataTable[] datatable1 = new DataTable[100];
            //DataTable newDataTable = new DataTable();
            //int count = 0;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    datatable1[i] = exam.getExam(int.Parse(dt.Rows[i]["class_id"].ToString()));
            //    count++;
            //}
            //if (count > 0)
            //{
            //    newDataTable = datatable1[0].Clone();
            //    object[] obj = new object[newDataTable.Columns.Count];
            //    for (int i = 0; i < count; i++)
            //    {
            //        for (int j = 0; j < datatable1[i].Rows.Count; j++)
            //        {
            //            datatable1[i].Rows[j].ItemArray.CopyTo(obj, 0);
            //            newDataTable.Rows.Add(obj);
            //        }
            //    }
            //}
            DataTable newDataTable = exam.getExam(classID);
            return newDataTable;
        }

        public DataTable getExamByClassID(int classID)
        {
            return exam.getExam(classID);
        }

        public DataTable getTeacherExam(int userID)
        {
            return exam.getTeacherExam(userID);
        }

        public string checkExamHasDone(int student_id, int exam_id)
        {
            DataTable dt = exam.checkExamHasDone(student_id, exam_id);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["score"].ToString();
            }
            else
            {
                return "null";
            }
        }

        public string[] getUserAnswers(int student_id, int exam_id)
        {
            DataTable dt = new DataTable();
            dt = exam.checkExamHasDone(student_id, exam_id);
            string answers = dt.Rows[0]["answers"].ToString();
            string[] studentAnswer = answers.Split(':');
            return studentAnswer;
        }
    }
}
