using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class Class
    {
        public DataTable getOwnClass(int studentID)
        {
            string strSql = "select class.class,class.class_id from class,stu_class where stu_class.student_id=@studentID and class.class_id=stu_class.class_id";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@StudentID",SqlDbType.Int),
                                      };
            parameters[0].Value = studentID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable getAllClass()
        {
            string strSql = "select * from class";
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text);
        }

        public void enterClass(int classID, int studentID)
        {
            string strSql = "insert into stu_class (class_id, student_id) values (@ClassID, @UserID)";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@ClassID", SqlDbType.Int),
                                        new SqlParameter("@UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = classID;
            parameters[1].Value = studentID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public void cancelClass(int classID, int studentID)
        {
            string strSql = "delete from stu_class where class_id=@ClassID and student_id=@UserID";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@ClassID", SqlDbType.Int),
                                        new SqlParameter("@UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = classID;
            parameters[1].Value = studentID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public void createClass(string className, int userID)
        {
            string strSql = "insert into class (class, teacher_id) values (@Class, @UserID)";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@Class", SqlDbType.VarChar, 50),
                                        new SqlParameter("@UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = className;
            parameters[1].Value = userID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public DataTable getClass(int userID)
        {
            string strSql = "select * from class where teacher_id=@TeacherID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@TeacherID",SqlDbType.Int),
                                      };
            parameters[0].Value = userID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable getStuClass(int userID)
        {
            string strSql = "select * from stu_class where student_id=@TeacherID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@TeacherID",SqlDbType.Int),
                                      };
            parameters[0].Value = userID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable getClassName(int classID)
        {
            string strSql = "select * from class where class_id=@ClassID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ClassID",SqlDbType.Int),
                                      };
            parameters[0].Value = classID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public void updateClassName(string className, int classID)
        {
            string strSql = "update class set class=@ClassName where class_id=@ClassID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ClassName",SqlDbType.VarChar,50),
                           new SqlParameter ("@ClassID",SqlDbType.Int),
                                      };
            parameters[0].Value = className;
            parameters[1].Value = classID;

            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public void delClass(int classID)
        {
            string strSql = "delete class where class_id=@ClassID";
            SqlParameter[] parameters ={ 
                           new SqlParameter ("@ClassID",SqlDbType.Int),
                                      };
            parameters[0].Value = classID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }


    }
}
