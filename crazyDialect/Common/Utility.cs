using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Common
{
    public class Utility
    {
        //通用类，检查数据输入是否规范
        ///////////检查手机号码是否规范
        public static bool CheckMobilePhone(string phone)
        {
            bool check = true;
            //if (phone != "")
            //{
            //    if (phone.Length != 11 || !Regex.IsMatch(phone,
            //        @"13[0123456789]\d{8} |15[0123456789]\d{8} |18{0123456789}\d{8}"))
            //    {
            //        check = false;
            //    }
            //}
            return check;
        }

        ///////检查email格式是否规范
        public static bool CheckEmail(string email)
        {
            bool check = true;
            if (email != "")
            {
                if (!Regex.IsMatch(email,
                    @"(\w)*@(\w)*\.(\w)*"))
                {
                    check = false;
                }
            }
            return check;
        }

        ///////检查qq格式是否规范
        public static bool CheckQQ(string qq)
        {
            bool check = true;
            //if (qq != "")
            //{
            //    if (!Regex.IsMatch(qq,
            //        @"^[1-9])*[1-9][0-9]*$"))
            //    {
            //        check = false;
            //    }
            //}
            return check;
        }

        ///////检查电话号码格式是否规范
        public static bool CheckPhone(string phone)
        {
            bool check = true;
            if (phone != "")
            {
                if (!Regex.IsMatch(phone,
                    @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$"))
                {
                    check = false;
                }
            }
            return check;
        }

        public static bool CheckNumber(string number)
        {
            bool check = true;
            if (number != "")
            {
                if (!Regex.IsMatch(number, @"^\d{11}$"))
                {
                    check = false;
                }
            }
            return check;
        }

        public static string Encrypt(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(password);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2,'0');
            }

            return str;
        }
    }
}
