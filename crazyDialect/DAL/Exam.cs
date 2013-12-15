using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Exam
    {
        public int createExamID(string examName, int userID, int classID)
        {
            string strSql = "insert into exam (exam,teacher_id,class_id) values (@ExamName,@TeacherID,@ClassID) SELECT CAST(scope_identity() AS int)";
            SqlParameter[] parameters ={
                           new SqlParameter ("@ExamName",SqlDbType.VarChar,100),
                           new SqlParameter ("@TeacherID", SqlDbType.Int),
                           new SqlParameter ("@ClassID", SqlDbType.Int),
                                      };
            parameters[0].Value = examName;
            parameters[1].Value = userID;
            parameters[2].Value = classID;
            int n = Convert.ToInt32(SqlDbHelper.ExecuteScalar(strSql, CommandType.Text, parameters));
            return n;
        }

        public void insertQuestion(int examID, string question, string answer, string option)
        {
            string strSql = "insert into question (question, exam_id, answer, options) values (@Question, @ExamID, @Answer, @Options)";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@Question", SqlDbType.Text),
                                        new SqlParameter("@ExamID", SqlDbType.Int),
                                        new SqlParameter("@Answer", SqlDbType.Text),
                                        new SqlParameter("@Options", SqlDbType.Text),
                                        };
            parameters[0].Value = question;
            parameters[1].Value = examID;
            parameters[2].Value = answer;
            parameters[3].Value = option;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        

        public DataTable getQuestion(int examID)
        {
            string strSql = "select * from question where exam_id=@ExamID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ExamID",SqlDbType.Int),
                                      };
            parameters[0].Value = examID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public int insertScore(int student_id, int exam_id, string option, float score)
        {
            string strSql = "insert into score (student_id, exam_id, score, answers) values (@StudentID, @ExamID, @Score, @Answers)";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@StudentID",SqlDbType.Int),
                           new SqlParameter ("@ExamID",SqlDbType.Int),
                           new SqlParameter ("@Score",SqlDbType.Float),
                           new SqlParameter ("@Answers",SqlDbType.Text),
                                      };
            parameters[0].Value = student_id;
            parameters[1].Value = exam_id;
            parameters[2].Value = score;
            parameters[3].Value = option;

            int n = Convert.ToInt32(SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters));
            return n;
        }

        public DataTable getExam(int class_id)
        {
            string strSql = "select exam_id, exam from exam where class_id=@ClassID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ClassID",SqlDbType.Int),
                                      };
            parameters[0].Value = class_id;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable getTeacherExam(int userID)
        {
            string strSql = "select exam_id, exam from exam where teacher_id=@UserID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@UserID",SqlDbType.Int),
                                      };
            parameters[0].Value = userID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable checkExamHasDone(int student_id, int exam_id)
        {
            string strSql = "select * from score where student_id=@StudentID and exam_id=@ExamID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@StudentID",SqlDbType.Int),
                           new SqlParameter("@ExamID",SqlDbType.Int)
                                      };
            parameters[0].Value = student_id;
            parameters[1].Value = exam_id;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public void delExam(int examID)
        {
            string strSql = "delete from exam where exam_id=@ExamID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ExamID",SqlDbType.Int),
                                      };
            parameters[0].Value = examID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public void delExamQuestions(int examID)
        {
            string strSql = "delete from question where exam_id=@ExamID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ExamID",SqlDbType.Int),
                                      };
            parameters[0].Value = examID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        
    }
}
