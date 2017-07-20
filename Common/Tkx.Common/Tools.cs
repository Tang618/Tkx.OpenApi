using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tkx.Common
{
    /// <summary>
    /// 工具类
    /// 12-21:添加枚举操作函数..
    /// 2017-7-7:重新梳理,去掉图片生成.把数据校验迁移到另一个类里面.
    /// </summary>
    /// <author>张炜杰</author>
    ///<lastedit>2010-10-1</lastedit>
    public class Tools
    {
        public static string LogNet = "";
        public static string ExpNet = "";//关于系统日记, 
        public static string LogNet_text = "//error//toole//"; //日记记录路径


        #region 数据校验
        /// <summary>判断是否为数字型格式 </summary>
        /// <param name="strTel">输入项</param>
        /// <returns></returns>
        public bool IsNumber(string str)
        {
            Regex strRegex = new Regex(@"[0-9]", RegexOptions.IgnoreCase);
            if (strRegex.Matches(str).Count == str.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>全角转半角</summary>
        /// <param name="input">输入项</param>
        /// <returns></returns>
        public static string QJToBJ(string input)
        {
            char[] cc = input.ToCharArray();
            for (int i = 0; i < cc.Length; i++)
            {
                if (cc[i] == 12288)
                {
                    // 表示空格  
                    cc[i] = (char)32;
                    continue;
                }
                if (cc[i] > 65280 && cc[i] < 65375)
                {
                    cc[i] = (char)(cc[i] - 65248);
                }

            }
            return new string(cc);
        }
        #endregion

        #region 随机数
        /// <summary>获取32位GUID 
        /// </summary>
        /// <returns></returns>
        public static string CreateGUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

         
        private static long tick = DateTime.Now.Ticks;
        /// <summary>根据当前时间刻度产生随机数</summary>
        /// <param name="str">需要产生字符串原始数据</param>
        /// <param name="num">产生的个数</param>
        /// <returns></returns>
        public static string RandomStr(string str, int num)
        {
            if (1 > num) { return ""; }
            string newstr = "";

            Random rnd = new Random((int)tick++);
            for (int i = 0; i < num; i++)
            {

                newstr += str[rnd.Next(0, str.Length)];
            }
            return newstr.ToString();
        }
        /// <summary>生成随机数</summary>
        /// <param name="num">产生的位数</param>
        /// <param name="state">产生随机数的类型（1：数字；2：大写，3：小写，4：数字+大写，5：数字+小写，6：小写+大写,7:中文,8:数字+大写+小写）</param>
        /// <returns></returns>
        public static string CreateRandom(int num, int state)
        {
            string Int = "1234567890";
            string Str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string str = Str.ToLower();
            switch (state)
            {
                case 1://数字
                    return RandomStr(Int, num);
                case 2://大写
                    return RandomStr(Str, num);
                case 3://小写
                    return RandomStr(str, num);
                case 4://数字+大写
                    return RandomStr(Int + Str, num);
                case 5://数字+小写
                    return RandomStr(Int + str, num);
                case 6://小写+大写
                    return RandomStr(Str + str, num);
                case 7:
                    return CreateRegionCode(num);
                case 8: return RandomStr(Int + Str + str, num);

                default:
                    return RandomStr(Int + Str + str, num);
            }
        }

        /// <summary>生成验证码字符串
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CreateChekCodeString(int strlength)
        {
            char[] allCharArray ={'0','1','2','3','4','5','6','7','8','9','A','B',
                'C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q',
            'R','S','T','U','V','W','X','Y','Z'};
            //定义验证码字符串
            string randomCode = "";
            Random rand = new Random();
            //生成4位验证码字符串
            for (int i = 0; i < strlength; i++)
                randomCode += allCharArray[rand.Next(allCharArray.Length)];
            return randomCode;
        }
        /// <summary>生成中文随机文字
        /// 
        /// </summary>
        /// <param name="strlength">长度多少</param>
        /// <returns></returns>
        public static string CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来
            object[] bytes = new object[strlength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
         每个汉字有四个区位码组成
         区位码第1位和区位码第2位作为字节数组第一个元素
         区位码第3位和区位码第4位作为字节数组第二个元素
        */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值


                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);

            }

            //      return bytes;
            StringBuilder sb = new StringBuilder();
            Encoding gb = Encoding.GetEncoding("gb2312");
            for (int i = 0; i < strlength; i++)
            {
                sb.Append(gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[]))));
            }

            return sb.ToString();

        }

        #endregion

        #region JS特效
        /// <summary>JS弹出警告
        /// 
        /// </summary>
        /// <param name="mess">警告内容</param>
        /// <returns></returns>
        public static string ShowMessageBox(string mess)
        {
            return string.Format("<script language='javascript'>alert('{0}');</script>", mess);
        }
        /// <summary>JS弹出警告
        /// 
        /// </summary>
        /// <param name="mess">提示语:例:您还未登陆? \\n 马上登陆!</param>
        /// <param name="url">确认跳转的页面!"/home/index"</param>
        /// <returns></returns>
        public static string ShowMessageBox(string mess, string url)
        {
            //     return string.Format("<script language='javascript' type='text/javascript'>if(confirm('{0}')){window.location.href='{1}';}</script>", mess, url);

            return "<script language='javascript' type='text/javascript'>if(confirm('" + mess + "')){window.location.href='" + url + "';}</script>";

        }
        #endregion

        #region 事件记录函数
        /// <summary>记录事件(记事本)
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void MessBox(string str)
        {
            MessBox(str, "");
        }

        /// <summary>记录事件
        /// 该函数按天记录事件.用于一些轻量级的日记系统.
        /// 可以记录系统异常,程序异常.如果Paths为空的话,默认会在应用程序的根目录建立一个log的文件夹,并在文件夹中写入日记.
        /// </summary>
        /// <param name="str">事件描述</param>
        /// <param name="path">保存路径,允许为空"" (不建议)</param>
        public static void MessBox(string str, string paths)
        {
            try
            {


                string currentDirectory = Directory.GetCurrentDirectory();// Path.GetDirectoryName("Tkx.Common.dll");


                string filename = DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + ".txt";
                string directory = currentDirectory + "\\Log\\";//AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
                if (paths != "")
                {
                    directory = currentDirectory + paths; //+ "\\Log\\";// AppDomain.CurrentDomain.BaseDirectory + paths;
                }

                string path = directory + filename;
                StreamWriter sr;
                //是否存在文件夹,不存在则创建
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                //追加日志
                if (File.Exists(path))
                {
                    sr = File.AppendText(path);
                }
                //创建日志
                else
                {
                    sr = File.CreateText(path);
                }
                sr.WriteLine(str);
                sr.Write("\r\n");
                //  sr.Close();
                sr.Dispose();

            }
            catch
            { }

        }

        #endregion

        #region 正则匹配后的值 
        //   new System.Text.RegularExpressions.Regex("allowtransparency=\"true\" src=?(.*)\"></iframe>").Match(html).Groups[0].Value.ToString().Replace("allowtransparency=\"true\" src=\"","").Replace("\"></iframe>","")

        /// <summary>过滤一些HTML
        ///
        /// </summary>
        /// <param name="html">HMTL内容</param>
        /// <param name="Regular">正则</param>
        /// <param name="start">前面</param>
        /// <param name="ends">后面</param>
        /// <returns></returns>
        public static object GetObject(string html, string Regular, string start, string ends)
        {
            object obj;
            obj = new System.Text.RegularExpressions.Regex(Regular).Match(html).Groups[0].Value.ToString().Replace(start, "").Replace(ends, "");
            return obj;
        }

        ///// <summary>HTML标签过滤
        ///// 
        ///// </summary>
        ///// <param name="html"></param>
        ///// <returns></returns>
        //public static string checkStr(string html)
        //{
        //    System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    //    System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    //   System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    html = regex1.Replace(html, ""); //过滤<script></script>标记 
        //    html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
        //    html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
        //    html = regex4.Replace(html, ""); //过滤iframe 
        //    html = regex5.Replace(html, ""); //过滤frameset 
        //    html = regex6.Replace(html, ""); //过滤frameset 
        //    html = regex7.Replace(html, ""); //过滤frameset 
        //    html = regex8.Replace(html, ""); //过滤frameset 
        //    html = regex9.Replace(html, "");
        //    html = html.Replace(" ", "");
        //    html = html.Replace("</strong>", "");
        //    html = html.Replace("<strong>", "");
        //    html = html.Replace("&rdquo;", "");
        //    html = html.Replace("&nbsp;", "");
        //    html = html.Replace("&ldquo;", "");
        //    html = html.Replace("&hellip;", "");//&nbsp;&ldquo;&rdquo;
        //    html = html.Replace("&rdquo;", "");
        //    html = html.Replace("&middot;", "");
        //    html = html.Replace("&mdash;", "");
        //    html = html.Replace("&uarr;", "");
        //    return html;
        //}

        ///// <summary> 取得HTML中所有图片的 URL。 
        ///// 
        ///// </summary> 
        ///// <param name="sHtmlText">HTML代码</param> 
        ///// <returns>图片的URL列表</returns> 
        //public static string[] GetHtmlImageUrlList(string sHtmlText)
        //{
        //    // 定义正则表达式用来匹配 img 标签 
        //    Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

        //    // 搜索匹配的字符串 
        //    MatchCollection matches = regImg.Matches(sHtmlText);
        //    int i = 0;
        //    string[] sUrlList = new string[matches.Count];

        //    // 取得匹配项列表 
        //    foreach (Match match in matches)
        //        sUrlList[i++] = match.Groups["imgUrl"].Value;
        //    return sUrlList;
        //}
        #endregion

        #region 枚举操作方法 


        /// <summary> 输入个数值,把枚举的text显示出来 
        /// 一般为显示的时候,数据库存放的是1,2,3根据这些数值把对应的文档显示出来.调用此函数
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="i">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumStr<T>(int i)
        {

            return Enum.GetName(typeof(T), i);
        }
        /// <summary>输入一个文本,查询对应枚举的数值,并把数值返回出来.
        /// 一般做为插入数据时,前台显示的是文本,插入到数据库时为1,2,3的数值.
        ///  ViewControllerType _view = (ViewControllerType)1;
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="na">文本</param>
        /// <returns>返回对应枚举的值,如果是-999表示没找到</returns>
        public static int GetEnumInt<T>(string na)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString() == na.ToString())
                {
                    return Convert.ToInt32(item);
                }
            }
            return -999;
        }

        ///// <summary>特殊函数:绑定枚举已迁移到 Common\Yanyun.Controls\DropDownListControls.cs
        ///// <para>把一个枚举格式化后可以方便绑定到下拉列表.</para>
        ///// </summary>
        ///// <param name="tt">枚举类 注:不能传入非枚举类型否则会出错. 调用例:typeof(TaskType)</param>
        ///// <param name="text">设置选中的项,也可为空.</param>
        ///// <returns></returns>
        //public static List<SelectListItem> BdingEnumTypes(Type tt, string text = null)
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    foreach (var item in Enum.GetValues(tt))
        //    {
        //        if (item.ToString() == text)
        //        { items.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = true }); }
        //        else
        //        {
        //            items.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
        //        }

        //    }

        //    return items;
        //} 
        #endregion

        #region 时间转换 



        ///// <summary> unix时间转换为datetime
        /////
        ///// </summary>
        ///// <param name="timeStamp"></param>
        ///// <returns></returns>
        //public static DateTime Time_UnixTimeToTime(string timeStamp)
        //{

        //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        //    long lTime = long.Parse(timeStamp + "0000000");

        //    TimeSpan toNow = new TimeSpan(lTime);

        //    return dtStart.Add(toNow);

        //}

        /// <summary>datetime转换为unixtime
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns> 
        //public static int Time_ConvertDateTimeInt(System.DateTime time)
        //{

        //    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

        //    return (int)(time - startTime).TotalSeconds;

        //}

        /// <summary>获取时间戳
        /// 
        /// </summary>
        /// <returns></returns>
        public static string Time()
        {
            DateTime oldTime = new DateTime(1970, 1, 1);

            TimeSpan span = DateTime.Now.Subtract(oldTime);

            double milliSecondsTime = span.TotalSeconds;

            return Convert.ToInt64(milliSecondsTime).ToString();

        }
        /// <summary>时间戳对比(防止用历史URL地址来访问) 
        /// </summary>
        /// <param name="Span"></param>
        /// <param name="Minutes">分钟数.(输入正整数)</param>
        /// <returns>true表示正负在控制之内的分钟数,false表示已经超出控制分钟数之外</returns>
        public static bool TimeOut(string Span, int Minutes)
        {
            DateTime d = DateTime.Now;
            DateTime n = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(Span));

            var s = d - n;
            if (s.TotalMinutes <= -Minutes || s.TotalMinutes > Minutes)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 实体操作 无效
        ///// <summary>把表填充到实体中
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public static List<T> EntityList<T>(DataTable dt)
        //{
        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        return new List<T>();
        //    }

        //    List<T> entityList = new List<T>();
        //    T entity = default(T);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        entity = Activator.CreateInstance<T>();
        //        Type type = entity.GetType();
        //        PropertyInfo[] pis = type.GetProperties();
        //        foreach (PropertyInfo pi in pis)
        //        {
        //            string name = pi.Name;

        //            if (dt.Columns.Contains(name))
        //            {
        //                if (!pi.CanWrite)
        //                {
        //                    continue;
        //                }
        //                object value = dr[name];
        //                if (dr[name] != DBNull.Value)
        //                {
        //                    try
        //                    {
        //                        if (Nullable.GetUnderlyingType(pi.PropertyType) != null)
        //                        {
        //                            pi.SetValue(entity, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(pi.PropertyType).ToString())), null);
        //                        }
        //                        else
        //                        {
        //                            pi.SetValue(entity, Convert.ChangeType(value, Type.GetType(pi.PropertyType.ToString())), null);
        //                        }

        //                    }
        //                    catch
        //                    {
        //                    }
        //                }
        //            }
        //        }
        //        entityList.Add(entity);
        //    }
        //    return entityList;
        //}
        #endregion

        #region 经纬度换成成球面计算两个距离
        private const double EARTH_RADIUS = 6378.137; //地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>计算两点距离
        /// 
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            //  int g = Convert.ToInt32(s); 
            return s;
        }
        #endregion

        #region 私有方法

        /// <summary>根据数据类型返回特定的数据.
        /// 返回要绑定的数值.
        /// </summary>
        /// <param name="Ty"></param>
        /// <returns></returns>
        private static string Cyc(string Ty)
        {

            string str = Ty;

            switch (Ty)
            {
                case "long":
                    str = "0";
                    break;
                case "Array":
                    str = " ";
                    break;
                case "Boolean":
                    str = "false";
                    break;
                case "String":
                    str = "";
                    break;
                case "DateTime":
                    str = "1900-01-01";
                    break;
                case "Decimal":
                    str = "0";
                    break;
                case "Double":
                    str = "0";
                    break;
                case "Int32":
                    str = "0";
                    break;
                case "Single":
                    str = " ";
                    break;
                case "Guid":/*GUID 是唯一的二进制数；世界上的任何两台计算机都不会生成重复的 GUID 值*/
                    str = Guid.Empty.ToString();
                    break;
                case "Int16":
                    str = "0";
                    break;
                case "Byte":
                    str = "";
                    break;
                case "Object":
                    str = "";
                    break;
                case "VarChar":
                    str = " ";
                    break;
                case "Bit":
                    str = "0";
                    break;
                default:
                    break;
            }
            return str;
        }
        #endregion

        #region Cookies操作 读取,写入
        ///// <summary>读取一个Cookies
        ///// 
        ///// </summary>
        ///// <param name="Na"></param>
        ///// <param name="key"></param>
        //public object Get_Cookies(string Na, string key)
        //{
        //    return System.Web.HttpContext.Current.Request.Cookies[Na][key];
        //}
        ///// <summary>写入一个Cookies
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        //public void Set_Cookies(string name, string value)
        //{
        //    HttpCookie cookie = new HttpCookie("Info");//定义cookie对象以及名为Info的项

        //    DateTime dt = DateTime.Now;//定义时间对象

        //    TimeSpan ts = new TimeSpan(1, 0, 0, 0);//cookie有效作用时间，具体查msdn

        //    cookie.Expires = dt.Add(ts);//添加作用时间

        //    cookie.Values.Add(name, value);//增加属性

        //    HttpContext.Current.Response.Cookies.Add(cookie);
        //    //

        //}
        #endregion
        #region 电子邮件

        /// <summary>发送电子邮件
        ///  
        /// </summary>
        /// <param name="SmtpClient">发送邮件的服务器地址</param>
        /// <param name="strTitle">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="From">从哪个邮件发送</param>
        /// <param name="pws">从哪个邮箱发送的密码(需要登陆)</param>
        /// <param name="To">发送哪个邮箱地址</param>
        //public static void SendMail(string SmtpClient, string strTitle, string Body, string From, string pws, string To)
        //{
        //    SmtpClient client = new SmtpClient(SmtpClient);   //设置邮件协议
        //    client.UseDefaultCredentials = false;//这一句得写前面
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network; //通过网络发送到Smtp服务器
        //    client.Credentials = new NetworkCredential(From, pws); //通过用户名和密码 认证
        //    MailMessage mmsg = new MailMessage(new MailAddress(From), new MailAddress(To)); //发件人和收件人的邮箱地址
        //    mmsg.Subject = strTitle;      //邮件主题
        //    mmsg.SubjectEncoding = Encoding.UTF8;   //主题编码
        //    mmsg.Body = Body;         //邮件正文
        //    mmsg.BodyEncoding = Encoding.UTF8;      //正文编码
        //    mmsg.IsBodyHtml = true;    //设置为HTML格式           
        //    mmsg.Priority = MailPriority.High;   //优先级
        //    try
        //    {
        //        client.Send(mmsg);
        //    }
        //    catch (Exception exp)
        //    {
        //        Yanyun.Common.Tools.MessBox("来自发送邮件时 发生异常:" + exp.Message);
        //    }
        //}
        #endregion
        #region   简繁汉字转换操作
        ///// <summary>
        ///// 繁体转简体
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string Traditional2Simplified(string str)
        //{
        //    return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 9);
        //}
        ///// <summary>
        ///// 简体转繁体
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string Simplified2Traditional(string str)
        //{
        //    return Strings.StrConv(str, VbStrConv.TraditionalChinese, 9); 
        //}
        #endregion
        #region //生成拼音码


        #region 拼音编码
        private static int[] pyValue = new int[]
    {
    -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
    -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
    -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
    -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
    -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
    -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
    -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
    -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
    -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
    -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
    -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
    -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
    -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
    -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
    -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
    -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
    -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
    -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
    -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
    -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
    -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
    -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
    -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
    -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
    -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
    -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
    -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
    -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
    -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
    -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
    -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
    -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
    -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
    };
        private static string[] pyName = new string[]
    {
    "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
    "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
    "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
    "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
    "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
    "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
    "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
    "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
    "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
    "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
    "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
    "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
    "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
    "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
    "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
    "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
    "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
    "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
    "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
    "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
    "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
    "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
    "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
    "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
    "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
    "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
    "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
    "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
    "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
    "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
    "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
    "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
    "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
    };
        #endregion 拼音编码

        /// <summary> 把汉字转换成拼音(全拼)</summary>
        /// <param name="transName">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string GetAllPYLetters(string transName)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]{1}");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = transName.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = Encoding.Unicode.GetBytes(noWChar[j].ToString());//System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());// 获取操作系统的当前 ANSI 代码页的编码。目前core没有这块.郁闷
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }

        /// <summary>生成拼音简码
        /// 
        /// </summary>
        /// <param name="unicodeString">Unicode编码字符串</param>
        /// <returns>拼音简码:就是汉字的首写字母</returns>
        public static string GetChinesePYCode(string unicodeString)
        {
            int i = 0;
            ushort key = 0;
            string strResult = string.Empty;
            //创建两个不同的encoding对象
            Encoding unicode = Encoding.Unicode;
            //创建GBK码对象
            Encoding gbk = Encoding.GetEncoding(936);
            //将unicode字符串转换为字节
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            //再转化为GBK码
            byte[] gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
            while (i < gbkBytes.Length)
            {
                //如果为数字\字母\其他ASCII符号
                if (gbkBytes[i] <= 127)
                {
                    strResult = strResult + (char)gbkBytes[i];
                    i++;
                }
                #region 否则生成汉字拼音简码,取拼音首字母
                else
                {
                    key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    {
                        strResult = strResult + "A";
                    }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    {
                        strResult = strResult + "B";
                    }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    {
                        strResult = strResult + "C";
                    }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    {
                        strResult = strResult + "D";
                    }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    {
                        strResult = strResult + "E";
                    }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    {
                        strResult = strResult + "F";
                    }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    {
                        strResult = strResult + "G";
                    }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    {
                        strResult = strResult + "H";
                    }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    {
                        strResult = strResult + "J";
                    }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    {
                        strResult = strResult + "K";
                    }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    {
                        strResult = strResult + "L";
                    }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    {
                        strResult = strResult + "M";
                    }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    {
                        strResult = strResult + "N";
                    }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    {
                        strResult = strResult + "O";
                    }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    {
                        strResult = strResult + "P";
                    }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    {
                        strResult = strResult + "Q";
                    }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    {
                        strResult = strResult + "R";
                    }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    {
                        strResult = strResult + "S";
                    }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    {
                        strResult = strResult + "T";
                    }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    {
                        strResult = strResult + "W";
                    }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    {
                        strResult = strResult + "X";
                    }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    {
                        strResult = strResult + "Y";
                    }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    {
                        strResult = strResult + "Z";
                    }
                    else
                    {
                        strResult = strResult + "?";
                    }
                    i = i + 2;
                }
                #endregion
            }//end while
            return strResult;
        }

        /// <summary>生成拼音简码 </summary>
        /// <param name="unicodeString">Unicode编码字符串</param>
        /// <param name="maxLength"></param>
        /// <returns>拼音简码:string</returns>
        public static string GetChinesePYCode(string unicodeString, int? maxLength)
        {
            string strResult = GetChinesePYCode(unicodeString);
            if (maxLength.HasValue)
            {
                if (strResult.Length < maxLength.Value)
                    return strResult.Substring(0, strResult.Length);
                else
                    return strResult.Substring(0, maxLength.Value);
            }
            return null;
        }


        #endregion
        #region url编码

        ///// <summary> 使用GB2312指定的编码对象对 URL 字符串进行编码 </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string UrlEncode(string str)
        //{
        //    return HttpUtility.UrlEncode(str, Encoding.GetEncoding("GB2312"));
        //}

        ///// <summary>使用GB2312指定的编码对象对 URL 字符串进行解码 </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string UrlDecode(string str)
        //{
        //    return HttpUtility.UrlDecode(str, Encoding.GetEncoding("GB2312"));
        //}




        ///// <summary> 使用UTF8指定的编码对象对 URL 字符串进行编码 </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string UrlEncodeUTF8(string str)
        //{
        //    return HttpUtility.UrlEncode(str, Encoding.UTF8);
        //}

        ///// <summary>使用UTF8指定的编码对象对 URL 字符串进行解码 </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string UrlDecodeUTF8(string str)
        //{
        //    return HttpUtility.UrlDecode(str, Encoding.UTF8);
        //}
        #endregion
        #region 获取一些计算硬件的信息
        ///// <summary>  获得MAC地址 </summary>
        ///// <returns></returns>
        //public static string GetMacAddress()
        //{
        //    string s = "", mac = "";
        //    string hostInfo = Dns.GetHostName();
        //    System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
        //    for (int i = 0; i < addressList.Length; i++)
        //    {
        //        s += addressList[i].ToString();
        //    }
        //    ManagementClass mc;
        //    mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //    ManagementObjectCollection moc = mc.GetInstances();
        //    foreach (ManagementObject mo in moc)
        //    {

        //        if (mo["IPEnabled"].ToString() == "True")

        //            mac = mo["MacAddress"].ToString();
        //    }
        //    return mac;
        //}
        ///// <summary>获取IP 地址.</summary> 
        ///// <returns></returns>
        //public static string GetAddsIp()
        //{
        //    string strHostName = Dns.GetHostName(); //得到本机的主机名
        //    IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
        //    string strAddr = ipEntry.AddressList[0].ToString();
        //    return (strAddr);
        //}

        ///// <summary>获取客户端IP地址（无视代理）
        ///// 
        ///// </summary>
        ///// <returns>若失败则返回回送地址</returns>
        //public static string GetHostAddress()
        //{
        //    string userHostAddress = HttpContext.Current.Request.UserHostAddress;

        //    if (string.IsNullOrEmpty(userHostAddress))
        //    {
        //        userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }

        //    //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
        //    if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
        //    {
        //        return userHostAddress;
        //    }
        //    return "127.0.0.1";
        //}

        ///// <summary>
        ///// 检查IP地址格式
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <returns></returns>
        //public static bool IsIP(string ip)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        //}
        //public static string GetLocalIpv4()
        //{
        //    try
        //    {
        //        // 获得网络接口，网卡，拨号器，适配器都会有一个网络接口 
        //        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();


        //        foreach (NetworkInterface network in networkInterfaces)
        //        {
        //            // 获得当前网络接口属性
        //            IPInterfaceProperties properties = network.GetIPProperties();


        //            // 每个网络接口可能会有多个IP地址 
        //            foreach (IPAddressInformation address in properties.UnicastAddresses)
        //            {
        //                // 如果此IP不是ipv4，则进行下一次循环
        //                if (address.Address.AddressFamily != AddressFamily.InterNetwork)
        //                    continue;


        //                // 忽略127.0.0.1
        //                if (IPAddress.IsLoopback(address.Address))
        //                    continue;


        //                return address.Address.ToString();
        //            }
        //        }
        //    }


        //    catch (Exception ex)
        //    {

        //    }


        //    return null;
        //}

        #endregion
    }
}