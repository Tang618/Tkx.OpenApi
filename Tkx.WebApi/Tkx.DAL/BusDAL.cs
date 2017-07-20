using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Tkx.Model;
namespace Tkx.DAL
{
   public class BusDAL : BaseDal
    {

        /// <summary>获取单个省信息
        /// 
        /// </summary>
        /// <param name="Id">主键</param> 
        /// <param name="Name">简称</param>   
        /// <returns></returns> 
        public IList<AreaModel> GetProvinceSingle(Int32 Id, String Name)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.Int,4),
         new SqlParameter("@Name",SqlDbType.NVarChar,200)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = Name;


            string cmdText = "SELECT [Id],[ParentId],[ShortName],[Name],[Level],[Letter],[Code],[ZipCode],[Initial],[Lng],[Lat]  FROM Tkx_Region WHERE [Id]=@Id OR [Name]=@Name ";
            return db.ExecuteToList<AreaModel>(cmdText, CommandType.Text, commandParameters);
        }
        /// <summary>获取全国省分信息
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<AreaModel> GetProvince()
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Level",SqlDbType.Int,4)
        };
            commandParameters[0].Value = 1;
            string cmdText = "SELECT [Id],[ParentId],[ShortName],[Name],[Level],[Letter],[Code],[ZipCode],[Initial],[Lng],[Lat]  FROM Tkx_Region WHERE Level=@Level";
            return db.ExecuteToList<AreaModel>(cmdText, CommandType.Text, commandParameters);
        }
        /// <summary>根据上级ID查询关联下级地市级数据
        /// 
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public IList<AreaModel> GetArea(int ParentId)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@ParentId",SqlDbType.Int,4)
        };
            commandParameters[0].Value = ParentId;
            string cmdText = "SELECT [Id],[ParentId],[ShortName],[Name],[Level],[Letter],[Code],[ZipCode],[Initial],[Lng],[Lat]  FROM Tkx_Region WHERE ParentId=@ParentId";
            return db.ExecuteToList<AreaModel>(cmdText, CommandType.Text, commandParameters);
        }
        /// <summary>根据上级名称,查询关联下级地市级数据
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IList<AreaModel> GetArea(string Name)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Name",SqlDbType.NVarChar,200)
        };
            commandParameters[0].Value = Name;

            string cmdText = "SELECT [Id],[ParentId],[ShortName],[Name],[Level],[Letter],[Code],[ZipCode],[Initial],[Lng],[Lat]  FROM Tkx_Region WHERE  [ParentId]=(SELECT [Id] FROM Tkx_Region WHERE [Name]=@Name )";
            return db.ExecuteToList<AreaModel>(cmdText, CommandType.Text, commandParameters);
        }
    }
    
}