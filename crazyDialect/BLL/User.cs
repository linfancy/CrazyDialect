using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace BLL
{
    public class User
    {
        DAL.User user = new DAL.User();
        public DataTable Login(string userName, string password, string identity)
        {
            string pwd = Common.Utility.Encrypt(password);
            return user.Login(identity, userName, pwd);
        }

        public void registUser(string number, string userName, string password)
        {
            string pwd = Common.Utility.Encrypt(password);
            user.registUser(number, userName, pwd);
        }

        public bool checkRegist(string email)
        {
            if (user.checkRegist(email).Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DataRow getUserInfo(int userID)
        {
            return user.getUserInfo(userID).Rows[0];
        }

        public byte[] uploadPic(int userID, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int imgLen = (int)fs.Length;
            byte[] B = new byte[imgLen + 1];
            fs.Read(B, 0, imgLen);
            fs.Dispose();
            user.uploadPic(userID, B);
            return B;
        }

        public void updateUserInfo(int userID, string userNickName, string userName, int sex, string birthday, string email, string MSM, string qq)
        {
            user.updateUserInfo(userID, userNickName, userName, sex, birthday, email, MSM, qq);
        }

        public DataTable selectByClass(int selectClass, int selectExam, bool selectByGirl, bool selectByBoy, bool selectAll,bool selectClassBool = false, bool selectExamBool=false)
        {
            int sex=0;
            if (selectClassBool && (selectByBoy || selectByGirl))
            {
                sex = selectByBoy ? 0 : 1;
                return user.selectClassSex(selectClass, sex);

            }
            else if (selectExamBool && (selectByBoy || selectByGirl))
            {
                sex = selectByBoy ? 0 : 1;
                return user.selectExamSex(selectExam, sex);
            }
            else if (selectClassBool && selectAll)
            {
                return user.selectClass(selectClass);
            }
            else if (selectExamBool && selectAll)
            {
                return user.selectExam(selectExam);
            }
            else if ((selectByBoy || selectByGirl))
            {
                sex = selectByBoy ? 0 : 1;
                return user.selectSex(sex);
            }
            else
            {
                return user.selectAll();
            }

           
        }

        public DataTable selectByUserName(string username)
        {
            return user.selectByUserName(username);
        }

        public void changePass(int userID, string oldPass, string NewPass)
        {
            var old = Common.Utility.Encrypt(oldPass);
            var newp = Common.Utility.Encrypt(NewPass);
            user.changePass(userID, old, newp);
        }


        public bool checkPass(int userID, string pass)
        {
            string password = Common.Utility.Encrypt(pass);
            DataTable dt = user.checkPass(userID, password);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
