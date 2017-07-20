using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tkx.Common
{
    /// <summary>扩展集合 
    /// 这边是整个系统的扩展类地方.划分这块主要是平常开发中可以直接.出扩展方法来使用.
    ///  string n = "1231";//定义变量
    ///   n.CreateMD5();//代替以前toole.MD5(n);的写法
    ///   约定规则.如果是校验类的就用is开头.生成密码的用Create开头
    /// </summary>
    /// <author>张炜杰</author>
    ///<lastedit>2012-2-10</lastedit>
    public static class BaseExpansion
    {
        /// <summary>转成 Int
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this object s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (Exception)
            {

                return 0;
            }
            return 0;
        }
        /// <summary>转成 decimal
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s)
        {
            try
            {
                return Convert.ToDecimal(s);
            }
            catch (Exception)
            {

                return 0m;
            }

        }
        /// <summary>去掉最后一个字符 </summary>
        /// <param name="s">值</param>
        /// <returns></returns>
        public static string SubFinally(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Substring(0, s.Length - 1);
        }
        /// <summary>文字长度的拦截</summary>
        /// <param name="s">值</param>
        /// <param name="length">拦截的长度</param>
        /// <param name="state">如果超出长度,后面要不要加...表示, True要 False不要</param>
        /// <returns></returns>
        public static string Substring(this string s, int length, bool state)
        {
            string _temp = s;
            if (s.Length > length)
            {
                if (state)
                {
                    _temp = s.Substring(0, length) + "...";
                }

            }
            return _temp;
        }
        /// <summary>显示号码138*****0132
        /// 
        /// </summary>
        /// <param name="s"></param> 
        /// <returns></returns>
        public static string baoming(this string s)
        {
            return string.Format("{0}****{1}", s.Substring(0, 3), s.Substring(7, 4));
        }
        #region 验证类
        /// <summary>验证是否为空值
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsDBNull(this string n)
        {
            if (n == "")
            {
                return true;
            }

            if (n == null)
            {
                return true;
            }

            return false;
        }
        /// <summary>输入金额,不能为负数,如果是负数或是不是金额的就直接变0
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal IsValidMoney(this decimal str)
        {
            try
            {
                decimal mo = Convert.ToDecimal(str);
                if (mo > 0)
                {
                    return mo;
                }
                else
                {
                    return Convert.ToDecimal("0");
                }

            }
            catch (Exception)
            {
                return Convert.ToDecimal("0");
            }
        }
        /// <summary>校验IP V4的地址格式是否正确
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsValidIpV4Adds(this string n)
        {
            try
            {


                string[] t = n.Split('.');
                if (t.Length == 4)
                {
                    for (int i = 0; i < t.Length; i++)
                    {
                        int _ip = Convert.ToInt32(t[i]);
                        if (_ip >= 0 && _ip < 256)
                        {
                            continue;
                        }
                        return false;
                    }
                }
                else { return false; }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>检验密码
        /// 长度6到20.不能纯数字或纯字母;
        /// </summary>
        /// <param name="n"></param>
        /// <returns>为True表示符合条件.False不满足校验条件</returns>
        public static bool IsValidPass(this string n)
        {
            if (n.IsDBNull())
            {
                return false;
            }
            if (!(n.Length > 5 || n.Length < 20))
            {
                return false;
            }
            //整数字
            if (n.IsValidNumeric() != 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>验证此字符是否为数字整型
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是就返回整数,不是就返回0</returns>
        public static int IsValidNumeric(this string str)
        {
            int i;
            if (str != null && Regex.IsMatch(str, @"^\d+$"))
                i = int.Parse(str);
            else
                i = 0;
            return i;
        }
        /// <summary>判断是否为email格式.</summary>
        /// <param name="s">字符</param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string s)
        {
            return Regex.IsMatch(s, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
        /// <summary>校验手机号码是否符合标准。</summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidateMobile(this string mobile)
        {
            if (mobile.Length != 11)
            {
                return false;
            }
            if (string.IsNullOrEmpty(mobile))
                return false;
            return Regex.IsMatch(mobile, @"^(13|14|15|16|18|19)\d{9}$");
        }
        /// <summary> 校验网络地址</summary>
        /// <param name="P_str_naddress"></param>
        /// <returns></returns>
        public static bool IsValidateNAddress(this string P_str_naddress)
        {
            if (string.IsNullOrEmpty(P_str_naddress))
                return false;
            return Regex.IsMatch(P_str_naddress, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }
        /// <summary>校验邮政编码</summary>
        /// <param name="P_str_postcode">输入字符串 例:6位</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsValidatePostCode(this string P_str_postcode)
        {
            if (string.IsNullOrEmpty(P_str_postcode))
                return false;
            return Regex.IsMatch(P_str_postcode, @"\d{6}");
        }
        /// <summary>校验电话号码</summary>
        /// <param name="P_str_phone">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsValidatePhone(this string P_str_phone)
        {
            if (string.IsNullOrEmpty(P_str_phone))
                return false;
            return Regex.IsMatch(P_str_phone, @"\d{3,4}-\d{7,8}");
        }
        /// <summary>校验字符串为数字 </summary>
        /// <param name="P_str_num">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsValidateNum(this string P_str_num)
        {
            if (string.IsNullOrEmpty(P_str_num))
                return false;
            return Regex.IsMatch(P_str_num, "^[0-9]*$");
        }
        /// <summary>判断字符串中有没有包含中文
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidateChineseReg(this string words)
        {
            //    bool re = false;
            //    if (Regex.IsMatch(text, @"[\u4e00-\u9fbb]+$"))
            //        re = true; 
            //    return re; 
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>校验时间 格式 yy-MM-dd-HH-mm-ss 
        /// 用于校验提交过来的时间 跟服务器时间差是不是2分钟之内 
        /// </summary>
        /// <param name="time">时间格式 yy-MM-dd-HH-mm-ss   string dTime = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss"); </param>
        /// <returns></returns>
        public static bool IsValidateTimeOut(this string time)
        {
            try
            {
                if (time.Length > 10)
                {
                    TimeSpan tt = DateTime.Now.Subtract(Convert.ToDateTime(string.Format("{0} {1}", time.Substring(0, 8), time.Substring(9).Replace("-", ":"))));
                    if (tt.Minutes <= -2 || tt.Minutes > 2)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception exp)
            {
                string funName = "IsValidateTimeOut";
                string _ex = string.Format("时间:{0},函数:{1},发生异常:{2}", DateTime.Now, funName, exp.Message);
                Tools.MessBox(_ex, "//Error//extension//");
                return false;// throw;
            }
            return false;
        }
        /// <summary>校验身份证号码</summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool IsValidateIDCard(this string Id)
        {
            if (Id.Length == 18)
            {
                return CheckIDCard18(Id);
            }
            else if (Id.Length == 15)
            {

                return CheckIDCard15(Id);
            }
            else
            {
                return false;
            }
        }
        /// <summary>18位的身份证</summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            //  Math.DivRem(sum, 11, out y);
            DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        /// <summary>网上看到反编译的
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int DivRem(int a, int b, out int result)
        {
            result = a % b;
            return (a / b);
        }
        /// <summary>15位的身份证 </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准

        }
        #endregion

        #region 加密类 
        /// <summary> 生成MD5
        /// </summary>
        /// <param name="s">明文</param>
        /// <returns></returns>
        public static string CreateMD5(this string s)
        {
            return Safety.Md5(s);
        }
        #endregion
 

    }
}
