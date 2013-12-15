using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class User
    {
        public User() { }

        public DataTable Login(string identity, string userName, string password)
        {
            string strSql = "Select user_id, userNickName from [user] where cast(email as varchar(50)) = @UserName and password = @UserPassword and identities = @UserIdentity";
            SqlParameter[] parameters = {
                        new SqlParameter("@UserName", SqlDbType.VarChar, 50),
                        new SqlParameter("@UserPassword", SqlDbType.VarChar, 50),
                        new SqlParameter("@UserIdentity", SqlDbType.VarChar, 50),
                                        };
            parameters[0].Value = userName;
            parameters[1].Value = password;
            parameters[2].Value = identity;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
            
        }

        public void registUser(string number, string userName, string password)
        {
            string strSql = "insert into [user] (email, userNickName, password,identities) values (@UserID, @UserName, @Password,'0')";
            SqlParameter[] parameters = { 
                                        new SqlParameter("@UserID", SqlDbType.Text),
                                        new SqlParameter("@UserName", SqlDbType.VarChar, 50),
                                        new SqlParameter("@Password", SqlDbType.VarChar, 50),
                                        };
            parameters[0].Value = number;
            parameters[1].Value = userName;
            parameters[2].Value = password;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public DataTable checkRegist(string email)
        {
            string strSql = "Select * from [user] where cast(email as varchar(50)) = @UserName";
            SqlParameter[] parameters = {
                        new SqlParameter("@UserName", SqlDbType.VarChar, 50),
                                        };
            parameters[0].Value = email;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable getUserInfo(int userID)
        {
            string strSql = "Select * from [user] where user_id = @UserID";
            SqlParameter[] parameters = {
                        new SqlParameter("@UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = userID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public void uploadPic(int userID, byte[] B)
        {
            string strSql = "update [user] set picture = @Image where user_id = @UserID";
            SqlParameter[] parameters = {
                        new SqlParameter("@Image", SqlDbType.Binary),
                        new SqlParameter("UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = B;
            parameters[1].Value = userID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public void updateUserInfo(int userID, string userNickName, string userName, int sex, string birthday, string email, string MSM, string qq)
        {
            string strSql = "update [user] set userNickName = @UserNickName, username=@UserName,sex=@Sex,birthday=@Birthday,qq=@QQ,email=@Email,MSM=@MSM where user_id = @UserID";
            SqlParameter[] parameters = {
                        new SqlParameter("@UserNickName", SqlDbType.VarChar, 50),
                        new SqlParameter("@UserName", SqlDbType.VarChar, 50),
                        new SqlParameter("@Sex", SqlDbType.Int),
                        new SqlParameter("@Birthday", SqlDbType.VarChar, 50),
                        new SqlParameter("@QQ", SqlDbType.VarChar, 50),
                        new SqlParameter("@Email", SqlDbType.VarChar, 50),
                        new SqlParameter("@MSM", SqlDbType.VarChar, 50),
                        new SqlParameter("UserID", SqlDbType.Int),
                                        };
            parameters[0].Value = userNickName;
            parameters[1].Value = userName;
            parameters[2].Value = sex;
            parameters[3].Value = birthday;
            parameters[4].Value = qq;
            parameters[5].Value = email;
            parameters[6].Value = MSM;
            parameters[7].Value = userID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public DataTable getUserByClass(int classID)
        {
            string strSql = "Select * from [user] where class_id = @ClassID";
            SqlParameter[] parameters = {
                        new SqlParameter("@ClassID", SqlDbType.Int),
                                        };
            parameters[0].Value = classID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectClassSex(int selectClass, int sex)
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from stu_class,[user] where stu_class.class_id=@SelectClass and stu_class.student_id=[user].user_id and [user].sex=@Sex";
            SqlParameter[] parameters = {
                        new SqlParameter("@SelectClass", SqlDbType.Int),
                        new SqlParameter("@Sex", SqlDbType.Int),
                                        };
            parameters[0].Value = selectClass;
            parameters[1].Value = sex;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectExamSex(int selectExam, int sex)
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from score,[user] where score.exam_id=@SelectExam and score.student_id=[user].user_id and [user].sex=@Sex";
            SqlParameter[] parameters = {
                        new SqlParameter("@SelectExam", SqlDbType.Int),
                        new SqlParameter("@Sex", SqlDbType.Int),
                                        };
            parameters[0].Value = selectExam;
            parameters[1].Value = sex;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectClass(int selectClass)
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from stu_class,[user] where stu_class.class_id=@SelectClass and stu_class.student_id=[user].user_id";
            SqlParameter[] parameters = {
                        new SqlParameter("@SelectClass", SqlDbType.Int),
                                        };
            parameters[0].Value = selectClass;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectExam(int selectExam)
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from score,[user] where score.exam_id=@SelectExam and score.student_id=[user].user_id";
            SqlParameter[] parameters = {
                        new SqlParameter("@SelectExam", SqlDbType.Int),
                                        };
            parameters[0].Value = selectExam;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectSex(int sex)
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from [user] where sex=@Sex";
            SqlParameter[] parameters = {
                        new SqlParameter("@Sex", SqlDbType.Int),
                                        };
            parameters[0].Value = sex;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text, parameters);
        }

        public DataTable selectAll()
        {
            string strSql = "Select [user].user_id,[user].username,[user].userNickName,[user].sex,[user].birthday,[user].qq,[user].email,[user].MSM from [user]";
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text);
        }

        public DataTable selectByUserName(string username)
        {
            string strSql = "select user_id, username, userNickName, sex, birthday, qq, email, MSM from [user] where username like '%" + username + "%' or userNickName='%" + username + "%'";
            
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text);
        }

        public void changePass(int userID, string old, string newp)
        {
            string strSql = "update [user] set password = @NewPass where user_id = @UserID and password=@OldPass";
            SqlParameter[] parameters = {
                        new SqlParameter("@NewPass", SqlDbType.VarChar, 50),
                        new SqlParameter("@OldPass", SqlDbType.VarChar, 50),
                        new SqlParameter("@UserID",  SqlDbType.Int),
                                        };
            parameters[0].Value = newp;
            parameters[1].Value = old;
            parameters[2].Value = userID;
            SqlDbHelper.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }

        public DataTable checkPass(int userID, string old)
        {
            string strSql = "select * from [user] where user_id=@UserID and password=@OldPass";
            SqlParameter[] parameters = {
                        new SqlParameter("@OldPass", SqlDbType.VarChar, 50),
                        new SqlParameter("@UserID",  SqlDbType.Int),
                                        };
            parameters[0].Value = old;
            parameters[1].Value = userID;
            return SqlDbHelper.ExecuteDataTable(strSql, CommandType.Text,parameters);
        }
    }
}
