using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Tkx.Model;

namespace Tkx.DAL
{
    public class ApiLogDAL
    {
        //这个有可能会跟主库分离
        public Tkx.DbBase.SqlDbHelper db = new DbBase.SqlDbHelper("Data Source=120.76.26.199;User ID=sa;Password=runmingdb6688,./;Initial Catalog=parking;Pooling=true");


        #region 停开心 接口跟踪方案 还有接口统计 2017-6-30
        /// <summary>停开心接口日记
        /// 
        /// </summary>
        /// <param name="Lg_Url">来访URL</param> 
        /// <param name="Lg_Type">日记类型</param> 
        /// <param name="Lg_Title">接口名称</param> 
        /// <param name="Lg_Parameter">传入参数</param> 
        /// <param name="Lg_Text">返回值</param> 
        /// <param name="Lg_Time">添加时间</param> 
        /// <param name="Lg_Token">令牌</param> 
        /// <param name="Lg_ip">操作IP</param> 
        /// <param name="Lg_Bak">备注</param> 
        /// <param name="Lg_state">状态 1 正常 2 异常</param> 
        /// <param name="Lg_Version">版本号</param> 
        /// <param name="Lg_Platform">平台标识,and,ios,mobile</param> 
        /// <param name="Lg_Area">区域</param> 
        /// <param name="Lg_TimeOut">加载时长</param>  
        /// <returns></returns> 
        public void Add(String Lg_Url, String Lg_Type, String Lg_Title, String Lg_Text, DateTime Lg_Time, String Lg_Token, String Lg_Parameter, String Lg_ip, String Lg_Bak, Int32 Lg_state, String Lg_Version, String Lg_Platform, String Lg_Area, double Lg_TimeOut)
        {


            SqlParameter[] commandParameters ={
         new SqlParameter("@Lg_Url",SqlDbType.VarChar,500),
         new SqlParameter("@Lg_Type",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Title",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Text",SqlDbType.Text,2147483647),
         new SqlParameter("@Lg_Time",SqlDbType.DateTime,16),
         new SqlParameter("@Lg_Token",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Parameter",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_ip",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Bak",SqlDbType.VarChar,1000),
         new SqlParameter("@Lg_state",SqlDbType.Int,4),
         new SqlParameter("@Lg_Version",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Platform",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_Area",SqlDbType.VarChar,50),
         new SqlParameter("@Lg_TimeOut",SqlDbType.Float,6)
        };
            commandParameters[0].Value = Lg_Url;
            commandParameters[1].Value = Lg_Type;
            commandParameters[2].Value = Lg_Title;
            commandParameters[3].Value = Lg_Text;
            commandParameters[4].Value = Lg_Time;
            commandParameters[5].Value = Lg_Token;
            commandParameters[6].Value = Lg_Parameter;
            commandParameters[7].Value = Lg_ip;
            commandParameters[8].Value = Lg_Bak;
            commandParameters[9].Value = Lg_state;
            commandParameters[10].Value = Lg_Version;
            commandParameters[11].Value = Lg_Platform;
            commandParameters[12].Value = Lg_Area;
            commandParameters[13].Value = Lg_TimeOut;

            string cmdText = "INSERT INTO [Tb_ApiLog]([Lg_Url],[Lg_Type],[Lg_Title],[Lg_Text],[Lg_Time],[Lg_Token],[Lg_Parameter],[Lg_ip],[Lg_Bak],[Lg_state],[Lg_Version],[Lg_Platform],[Lg_Area],[Lg_TimeOut])VALUES(@Lg_Url,@Lg_Type,@Lg_Title,@Lg_Text,@Lg_Time,@Lg_Token,@Lg_Parameter,@Lg_ip,@Lg_Bak,@Lg_state,@Lg_Version,@Lg_Platform,@Lg_Area,@Lg_TimeOut)";

            db.ExecuteNonQuery(cmdText, commandParameters);
        }


        /// <summary> 添加接口
        /// </summary>
        /// <param name="ap_Name">接口名</param> 
        /// <param name="ap_Count">统计</param> 
        /// <param name="ap_Time">日期</param>  
        /// <returns></returns> 
        public int ApiCountAdd(String ap_Name, Int32 ap_Count, DateTime ap_Time)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@ap_Name",SqlDbType.VarChar,50),
         new SqlParameter("@ap_Count",SqlDbType.Int,4),
         new SqlParameter("@ap_Time",SqlDbType.DateTime,16)
        };
            commandParameters[0].Value = ap_Name;
            commandParameters[1].Value = ap_Count;
            commandParameters[2].Value = ap_Time;
            string cmdText = "INSERT INTO [Tb_ApiCount]([ap_Name],[ap_Count],[ap_Time])VALUES(@ap_Name,@ap_Count,@ap_Time)";
            return db.ExecuteNonQuery(cmdText, commandParameters);

        }


        /// <summary>接口查询
        /// 
        /// </summary>
        /// <param name="ap_Name">接口名</param> 
        /// <param name="ap_Time">日期</param>  
        /// <returns></returns> 
        public ApiLogModel ApiCountSel(String ap_Name, DateTime ap_Time)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@ap_Name",SqlDbType.VarChar,50),
         new SqlParameter("@ap_Time",SqlDbType.DateTime,16)
        };
            commandParameters[0].Value = ap_Name;
            commandParameters[1].Value = ap_Time;
            string cmdText = "SELECT * FROM Tb_ApiCount WHERE [ap_Name]=@ap_Name AND  datediff(day,[ap_Time],@ap_Time)=0";
            //   return db.ExecuteDataTable(cmdText, commandParameters);

            //  ApiLogModel ap = db.ExecuteReader<ApiLogModel>(cmdText, CommandType.Text, commandParameters);

            ApiLogModel ap = db.ExecuteReader<ApiLogModel>(cmdText, CommandType.Text, commandParameters);



            return ap;


        }


        /// <summary>接口访问量加1
        /// 
        /// </summary>
        /// <param name="ap_Name">接口名</param> 
        /// <param name="ap_Time">日期</param>  
        /// <returns></returns> 
        public void ApiCountUpdate(int ap_Id)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@ap_Id",SqlDbType.Int,4)
        };
            commandParameters[0].Value = ap_Id;

            commandParameters[0].Value = ap_Id;
            string cmdText = "UPDATE [Tb_ApiCount] SET [ap_Count]=[ap_Count]+1 WHERE [ap_Id]=@ap_Id ";
            db.ExecuteNonQuery(cmdText, commandParameters);
        }
        /// <summary>接口访问日记分页
        /// 
        /// </summary>
        /// <param name="ReFieldsStr"></param>
        /// <param name="OrderString"></param>
        /// <param name="WhereString"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> pageList(string ReFieldsStr, string OrderString, string WhereString, int PageSize, int PageIndex, ref int cnt)
        {
            // return db.PageList("tb_apilog", ReFieldsStr, OrderString, WhereString, PageSize, PageIndex, ref cnt, "");
            //改进后直接用 List<Dictionary<string, object>> 
            return db.PageList_List("tb_apilog", ReFieldsStr, OrderString, WhereString, PageSize, PageIndex, ref cnt, "");
        }

        /// <summary>接口访问日记分页
        /// 
        /// </summary>
        /// <param name="ReFieldsStr"></param>
        /// <param name="OrderString"></param>
        /// <param name="WhereString"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> page_List(string ReFieldsStr, string OrderString, string WhereString, int PageSize, int PageIndex, ref int cnt)
        {
            return db.PageList_List("tb_apilog", ReFieldsStr, OrderString, WhereString, PageSize, PageIndex, ref cnt, "");
        }
        /// <summary>接口访问日记分页
        /// 
        /// </summary>
        /// <param name="ReFieldsStr"></param>
        /// <param name="OrderString"></param>
        /// <param name="WhereString"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> pageCountList(string ReFieldsStr, string OrderString, string WhereString, int PageSize, int PageIndex, ref int cnt)
        {
            return db.PageList_List("Tb_ApiCount", ReFieldsStr, OrderString, WhereString, PageSize, PageIndex, ref cnt, "");
            // db.PageList("Tb_ApiCount", ReFieldsStr, OrderString, WhereString, PageSize, PageIndex, ref cnt, "");
        }
        #endregion


    }
}
