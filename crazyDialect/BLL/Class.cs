using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class Class
    {
        public DAL.Class classes = new DAL.Class();
        public DataTable getOwnClass(int studentID)
        {
            return classes.getOwnClass(studentID);
        }

        public DataTable getAllClass(DataTable dt)
        {
            DataTable dtall = classes.getAllClass();
            for (int i = 0; i < dtall.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dtall.Rows[i]["class_id"].ToString() == dt.Rows[j]["class_id"].ToString())
                    {
                        dtall.Rows.Remove(dtall.Rows[i]);
                        if(i>0)i--;
                    }
                }
            }
            return dtall;
        }

        public void enterClass(int classID, int studentID)
        {
            classes.enterClass(classID, studentID);
        }

        public void cancelClass(int classID, int studentID)
        {
            classes.cancelClass(classID, studentID);
        }

        public void createClass(string className, int userID)
        {
            classes.createClass(className, userID);
        }

        public DataTable getClass(int userID)
        {
            return classes.getClass(userID);
        }

        public DataTable getClassName(int classID)
        {
            DataTable dt = classes.getClassName(classID);
            return dt;
        }

        public void updateClassName(string className, int classID)
        {
            classes.updateClassName(className, classID);
        }

        public void delClass(int classID)
        {
            classes.delClass(classID);
        }

    }
}
