using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Tkx.DAL
{
   public class ParKingDAL: BaseDal
    {






        /// <summary>创建车场信息 
        /// 原:6.1.	停车场信息模块
        /// </summary> 
        /// <param name="Id">停车场ID</param> 
        /// <param name="Name">名称</param> 
        /// <param name="Address">地址</param> 
        /// <param name="Phone">联系电话</param> 
        /// <param name="ImageUrl">停车场图片url</param> 
        /// <param name="Lng">经度</param> 
        /// <param name="Lat">纬度</param> 
        /// <param name="Letter">停车场拼音</param> 
        /// <param name="Initial">停车场首字母</param> 
        /// <param name="IsOnline">在线状态</param> 
        /// <param name="Floors">层数</param> 
        /// <param name="CarportNumber">总车位数</param> 
        /// <param name="RemCarportNum">剩余空车位</param> 
        /// <param name="FixedCarportNumber">固定车位数</param> 
        /// <param name="TempCarportNumber">临时车位数</param> 
        /// <param name="CurrentId">当前编号</param> 
        /// <param name="Path">路径地址</param> 
        /// <param name="Type">停车场类型</param> 
        /// <param name="IsDelete">是否可用</param>
        /// <param name="CreateTime">创建时间</param> 
        /// <param name="ParentId">父级编号</param> 
        /// <param name="LocalParkId">父级本地停车场id</param> 
        /// <param name="ParentLocalParkId">本地停车场id</param> 
        /// <param name="CityId">城市Id</param> 
        /// <param name="CityName">城市名称</param> 
        /// <param name="AreaId">区域Id</param> 
        /// <param name="AreaName">区域名称</param> 
        /// <param name="Platform">平台,0.无，1.停开心，</param>  
        /// <returns></returns> 
        public string Create(Guid Id, String Name, String Address, String Phone, String ImageUrl, Decimal Lng, Decimal Lat, String Letter, String Initial, Int32 IsOnline, Int32 Floors, Int32 CarportNumber, Int32 RemCarportNum, Int32 FixedCarportNumber, Int32 TempCarportNumber, Int32 CurrentId, String Path, String Type,bool IsDelete, DateTime CreateTime, Guid ParentId, String LocalParkId, String ParentLocalParkId, Int32 CityId, String CityName, Int32 AreaId, String AreaName, Int32 Platform)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@Name",SqlDbType.VarChar,255),
         new SqlParameter("@Address",SqlDbType.VarChar,255),
         new SqlParameter("@Phone",SqlDbType.VarChar,255),
         new SqlParameter("@ImageUrl",SqlDbType.VarChar,255),
         new SqlParameter("@Lng",SqlDbType.Decimal,20),
         new SqlParameter("@Lat",SqlDbType.Decimal,20),
         new SqlParameter("@Letter",SqlDbType.VarChar,255),
         new SqlParameter("@Initial",SqlDbType.Char,1),
         new SqlParameter("@IsOnline",SqlDbType.Int,4),
         new SqlParameter("@Floors",SqlDbType.Int,4),
         new SqlParameter("@CarportNumber",SqlDbType.Int,4),
         new SqlParameter("@RemCarportNum",SqlDbType.Int,4),
         new SqlParameter("@FixedCarportNumber",SqlDbType.Int,4),
         new SqlParameter("@TempCarportNumber",SqlDbType.Int,4),
         new SqlParameter("@CurrentId",SqlDbType.Int,4),
         new SqlParameter("@Path",SqlDbType.VarChar,255),
         new SqlParameter("@Type",SqlDbType.Int,4),
         new SqlParameter("@IsDelete",SqlDbType.Bit,1),
         new SqlParameter("@CreateTime",SqlDbType.DateTime,16),
         new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@LocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@ParentLocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@CityId",SqlDbType.Int,4),
         new SqlParameter("@CityName",SqlDbType.NVarChar,100),
         new SqlParameter("@AreaId",SqlDbType.Int,4),
         new SqlParameter("@AreaName",SqlDbType.NVarChar,100),
         new SqlParameter("@Platform",SqlDbType.Int,4)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = Name;
            commandParameters[2].Value = Address;
            commandParameters[3].Value = Phone;
            commandParameters[4].Value = ImageUrl;
            commandParameters[5].Value = Lng;
            commandParameters[6].Value = Lat;
            commandParameters[7].Value = Letter;
            commandParameters[8].Value = Initial;
            commandParameters[9].Value = IsOnline;
            commandParameters[10].Value = Floors;
            commandParameters[11].Value = CarportNumber;
            commandParameters[12].Value = RemCarportNum;
            commandParameters[13].Value = FixedCarportNumber;
            commandParameters[14].Value = TempCarportNumber;
            commandParameters[15].Value = CurrentId;
            commandParameters[16].Value = Path;
            commandParameters[17].Value = Type;
            commandParameters[18].Value = IsDelete;
            commandParameters[19].Value = CreateTime;
            commandParameters[20].Value = ParentId;
            commandParameters[21].Value = LocalParkId;
            commandParameters[22].Value = ParentLocalParkId;
            commandParameters[23].Value = CityId;
            commandParameters[24].Value = CityName;
            commandParameters[25].Value = AreaId;
            commandParameters[26].Value = AreaName;
            commandParameters[27].Value = Platform;
            string cmdText = "INSERT INTO [ParkInfo]([Id],[Name],[Address],[Phone],[ImageUrl],[Lng],[Lat],[Letter],[Initial],[IsOnline],[Floors],[CarportNumber],[RemCarportNum],[FixedCarportNumber],[TempCarportNumber],[CurrentId],[Path],[Type],[IsDelete],[CreateTime],[ParentId],[LocalParkId],[ParentLocalParkId],[CityId],[CityName],[AreaId],[AreaName],[Platform])VALUES(@Id,@Name,@Address,@Phone,@ImageUrl,@Lng,@Lat,@Letter,@Initial,@IsOnline,@Floors,@CarportNumber,@RemCarportNum,@FixedCarportNumber,@TempCarportNumber,@CurrentId,@Path,@Type,@IsDelete,@CreateTime,@ParentId,@LocalParkId,@ParentLocalParkId,@CityId,@CityName,@AreaId,@AreaName,@Platform)";

            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();

        }


        /// <summary>修改车场信息
        /// 
        /// </summary>
        /// <param name="Id">停车场ID</param> 
        /// <param name="Name">名称</param> 
        /// <param name="Address">地址</param> 
        /// <param name="Phone">联系电话</param> 
        /// <param name="ImageUrl">停车场图片url</param> 
        /// <param name="Lng">经度</param> 
        /// <param name="Lat">纬度</param> 
        /// <param name="Letter">停车场拼音</param> 
        /// <param name="Initial">停车场首字母</param> 
        /// <param name="IsOnline">在线状态</param> 
        /// <param name="Floors">层数</param> 
        /// <param name="CarportNumber">总车位数</param> 
        /// <param name="RemCarportNum">剩余空车位</param> 
        /// <param name="FixedCarportNumber">固定车位数</param> 
        /// <param name="TempCarportNumber">临时车位数</param> 
        /// <param name="CurrentId">当前编号</param> 
        /// <param name="Path">路径地址</param> 
        /// <param name="Type">停车场类型</param> 
        /// <param name="CreateTime">创建时间</param> 
        /// <param name="ParentId">父级编号</param> 
        /// <param name="LocalParkId">父级本地停车场id</param> 
        /// <param name="ParentLocalParkId">本地停车场id</param> 
        /// <param name="CityId">城市Id</param> 
        /// <param name="CityName">城市名称</param> 
        /// <param name="AreaId">区域Id</param> 
        /// <param name="AreaName">区域名称</param> 
        /// <param name="Platform">平台,0.无，1.停开心，</param>  
        /// <returns></returns> 
        public string Update(Guid Id, String Name, String Address, String Phone, String ImageUrl, Decimal Lng, Decimal Lat, String Letter, String Initial, Int32 IsOnline, Int32 Floors, Int32 CarportNumber, Int32 RemCarportNum, Int32 FixedCarportNumber, Int32 TempCarportNumber, Int32 CurrentId, String Path, Int32 Type, DateTime CreateTime, Guid ParentId, String LocalParkId, String ParentLocalParkId, Int32 CityId, String CityName, Int32 AreaId, String AreaName, Int32 Platform)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@Name",SqlDbType.VarChar,255),
         new SqlParameter("@Address",SqlDbType.VarChar,255),
         new SqlParameter("@Phone",SqlDbType.VarChar,255),
         new SqlParameter("@ImageUrl",SqlDbType.VarChar,255),
         new SqlParameter("@Lng",SqlDbType.Decimal,20),
         new SqlParameter("@Lat",SqlDbType.Decimal,20),
         new SqlParameter("@Letter",SqlDbType.VarChar,255),
         new SqlParameter("@Initial",SqlDbType.Char,1),
         new SqlParameter("@IsOnline",SqlDbType.Int,4),
         new SqlParameter("@Floors",SqlDbType.Int,4),
         new SqlParameter("@CarportNumber",SqlDbType.Int,4),
         new SqlParameter("@RemCarportNum",SqlDbType.Int,4),
         new SqlParameter("@FixedCarportNumber",SqlDbType.Int,4),
         new SqlParameter("@TempCarportNumber",SqlDbType.Int,4),
         new SqlParameter("@CurrentId",SqlDbType.Int,4),
         new SqlParameter("@Path",SqlDbType.VarChar,255),
         new SqlParameter("@Type",SqlDbType.VarChar,50),
         new SqlParameter("@CreateTime",SqlDbType.DateTime,16),
         new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@LocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@ParentLocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@CityId",SqlDbType.Int,4),
         new SqlParameter("@CityName",SqlDbType.NVarChar,100),
         new SqlParameter("@AreaId",SqlDbType.Int,4),
         new SqlParameter("@AreaName",SqlDbType.NVarChar,100),
         new SqlParameter("@Platform",SqlDbType.Int,4)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = Name;
            commandParameters[2].Value = Address;
            commandParameters[3].Value = Phone;
            commandParameters[4].Value = ImageUrl;
            commandParameters[5].Value = Lng;
            commandParameters[6].Value = Lat;
            commandParameters[7].Value = Letter;
            commandParameters[8].Value = Initial;
            commandParameters[9].Value = IsOnline;
            commandParameters[10].Value = Floors;
            commandParameters[11].Value = CarportNumber;
            commandParameters[12].Value = RemCarportNum;
            commandParameters[13].Value = FixedCarportNumber;
            commandParameters[14].Value = TempCarportNumber;
            commandParameters[15].Value = CurrentId;
            commandParameters[16].Value = Path;
            commandParameters[17].Value = Type;
            commandParameters[18].Value = CreateTime;
            commandParameters[19].Value = ParentId;
            commandParameters[20].Value = LocalParkId;
            commandParameters[21].Value = ParentLocalParkId;
            commandParameters[22].Value = CityId;
            commandParameters[23].Value = CityName;
            commandParameters[24].Value = AreaId;
            commandParameters[25].Value = AreaName;
            commandParameters[26].Value = Platform;
            string cmdText = "UPDATE [ParkInfo] SET [Name]=@Name,[Address]=@Address,[Phone]=@Phone,[ImageUrl]=@ImageUrl,[Lng]=@Lng,[Lat]=@Lat,[Letter]=@Letter,[Initial]=@Initial,[IsOnline]=@IsOnline,[Floors]=@Floors,[CarportNumber]=@CarportNumber,[RemCarportNum]=@RemCarportNum,[FixedCarportNumber]=@FixedCarportNumber,[TempCarportNumber]=@TempCarportNumber,[CurrentId]=@CurrentId,[Path]=@Path,[Type]=@Type,[CreateTime]=@CreateTime,[ParentId]=@ParentId,[LocalParkId]=@LocalParkId,[ParentLocalParkId]=@ParentLocalParkId,[CityId]=@CityId,[CityName]=@CityName,[AreaId]=@AreaId,[AreaName]=@AreaName,[Platform]=@Platform WHERE [Id]=@Id ";
            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();

        }


        /// <summary>删除车场信息
        /// 
        /// </summary>
        /// <param name="Id">停车场ID</param> 
        /// <param name="IsDelete">是否可用</param>  
        /// <returns></returns> 
        public string Delete(Guid Id, bool IsDelete)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@IsDelete",SqlDbType.Bit,1)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = IsDelete;
            string cmdText = "UPDATE [ParkInfo] SET [IsDelete]=@IsDelete WHERE [Id]=@Id ";
            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">停车场ID</param>  
        /// <returns></returns> 
        public Guid Sel(Guid Id)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16)
        };
            commandParameters[0].Value = Id;

            string cmdText = "SELECT [Id] FROM ParkInfo WHERE [Id]=@Id ";
            return db.QueryOneValue<Guid>(cmdText, commandParameters);

        }
        /// <summary>根据父级本地停车场id获取平台的停车场ID
        /// 
        /// </summary>
        /// <param name="LocalParkId">父级本地停车场id</param>
        /// <returns></returns>
        public Guid GetLocalParkId(String LocalParkId)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@LocalParkId",SqlDbType.VarChar,50)
        };
            commandParameters[0].Value = LocalParkId;

            string cmdText = "SELECT [Id] FROM ParkInfo WHERE [LocalParkId]=@Id ";
            return db.QueryOneValue<Guid>(cmdText, commandParameters);
        }




        #region 进出场信息
        /// <summary>进场信息 
        /// </summary>
        /// <param name="Id">进场记录ID</param> 
        /// <param name="LocalParkId">本地停车场id</param> 
        /// <param name="CarNo">车牌号</param> 
        /// <param name="CarType">车辆类型</param> 
        /// <param name="EnterParkTime">进场时间</param> 
        /// <param name="EnterParkImage">进场图片</param> 
        /// <param name="EnterParkType">进场方式</param> 
        /// <param name="BoxId">岗亭id</param> 
        /// <param name="ChannelId">通道id</param> 
        /// <param name="DutyPersonal">值班人</param> 
        /// <param name="Remark">备注</param> 
        /// <param name="IsDelete">是否可用</param> 
        /// <param name="CreateTime">创建时间</param> 
        /// <param name="ParkId">停车场ID</param>  
        /// <returns></returns> 
        public string IntoThe(Guid Id, String LocalParkId, String CarNo, Int32 CarType, DateTime EnterParkTime, String EnterParkImage, Int32 EnterParkType, Guid BoxId, Guid ChannelId, String DutyPersonal, String Remark, bool IsDelete, DateTime CreateTime, Guid ParkId)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@LocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@CarNo",SqlDbType.VarChar,50),
         new SqlParameter("@CarType",SqlDbType.Int,4),
         new SqlParameter("@EnterParkTime",SqlDbType.DateTime,16),
         new SqlParameter("@EnterParkImage",SqlDbType.VarChar,50),
         new SqlParameter("@EnterParkType",SqlDbType.Int,4),
         new SqlParameter("@BoxId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ChannelId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@DutyPersonal",SqlDbType.VarChar,50),
         new SqlParameter("@Remark",SqlDbType.VarChar,255),
         new SqlParameter("@IsDelete",SqlDbType.Bit,1),
         new SqlParameter("@CreateTime",SqlDbType.DateTime,16),
         new SqlParameter("@ParkId",SqlDbType.UniqueIdentifier,16)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = LocalParkId;
            commandParameters[2].Value = CarNo;
            commandParameters[3].Value = CarType;
            commandParameters[4].Value = EnterParkTime;
            commandParameters[5].Value = EnterParkImage;
            commandParameters[6].Value = EnterParkType;
            commandParameters[7].Value = BoxId;
            commandParameters[8].Value = ChannelId;
            commandParameters[9].Value = DutyPersonal;
            commandParameters[10].Value = Remark;
            commandParameters[11].Value = IsDelete;
            commandParameters[12].Value = CreateTime;
            commandParameters[13].Value = ParkId;
            string cmdText = "INSERT INTO [EnterParkRecord]([Id],[LocalParkId],[CarNo],[CarType],[EnterParkTime],[EnterParkImage],[EnterParkType],[BoxId],[ChannelId],[DutyPersonal],[Remark],[IsDelete],[CreateTime],[ParkId])VALUES(@Id,@LocalParkId,@CarNo,@CarType,@EnterParkTime,@EnterParkImage,@EnterParkType,@BoxId,@ChannelId,@DutyPersonal,@Remark,@IsDelete,@CreateTime,@ParkId)";
            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();
        }


        #endregion



        #region 车主模块
        /// <summary>添加车主
        /// 
        /// </summary>

        /// <param name="ow_Id">信息ID</param> 
        /// <param name="ow_ParkId">停车场ID</param> 
        /// <param name="ow_LocalParkId">本地停车场ID</param> 
        /// <param name="ow_OwnerName">车主名称</param> 
        /// <param name="ow_SectionId">所属部门id</param> 
        /// <param name="ow_Phone">手机号</param> 
        /// <param name="ow_CardNo">身份证号</param> 
        /// <param name="ow_Sex">性别</param> 
        /// <param name="ow_Nation">民族</param> 
        /// <param name="ow_Address">家庭住址</param> 
        /// <param name="ow_Email">邮箱</param> 
        /// <param name="ow_OwnerStatus">车主状态</param> 
        /// <param name="ow_CarportNum">车位数</param> 
        /// <param name="ow_CarNum">车辆数</param> 
        /// <param name="ow_Remark">备注信息</param> 
        /// <param name="ow_IsDelete">是否可用</param> 
        /// <param name="ow_ConfirmStatus">是否同步</param> 
        /// <param name="ow_CreateTime">创建时间</param>  
        /// <returns></returns> 
        public string OwnerAdd(Guid ow_Id, Guid ow_ParkId, String ow_LocalParkId, String ow_OwnerName, Guid ow_SectionId, String ow_Phone, String ow_CardNo, Int32 ow_Sex, String ow_Nation, String ow_Address, String ow_Email, Int32 ow_OwnerStatus, Int32 ow_CarportNum, Int32 ow_CarNum, String ow_Remark, bool ow_IsDelete, bool ow_ConfirmStatus, DateTime ow_CreateTime)
        {

            SqlParameter[] commandParameters ={
         new SqlParameter("@ow_Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ow_ParkId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ow_LocalParkId",SqlDbType.VarChar,50),
         new SqlParameter("@ow_OwnerName",SqlDbType.VarChar,50),
         new SqlParameter("@ow_SectionId",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ow_Phone",SqlDbType.VarChar,50),
         new SqlParameter("@ow_CardNo",SqlDbType.VarChar,30),
         new SqlParameter("@ow_Sex",SqlDbType.Int,4),
         new SqlParameter("@ow_Nation",SqlDbType.VarChar,50),
         new SqlParameter("@ow_Address",SqlDbType.VarChar,100),
         new SqlParameter("@ow_Email",SqlDbType.VarChar,50),
         new SqlParameter("@ow_OwnerStatus",SqlDbType.Int,4),
         new SqlParameter("@ow_CarportNum",SqlDbType.Int,4),
         new SqlParameter("@ow_CarNum",SqlDbType.Int,4),
         new SqlParameter("@ow_Remark",SqlDbType.VarChar,255),
         new SqlParameter("@ow_IsDelete",SqlDbType.Bit,1),
         new SqlParameter("@ow_ConfirmStatus",SqlDbType.Bit,1),
         new SqlParameter("@ow_CreateTime",SqlDbType.DateTime,16)
        };
            commandParameters[0].Value = ow_Id;
            commandParameters[1].Value = ow_ParkId;
            commandParameters[2].Value = ow_LocalParkId;
            commandParameters[3].Value = ow_OwnerName;
            commandParameters[4].Value = ow_SectionId;
            commandParameters[5].Value = ow_Phone;
            commandParameters[6].Value = ow_CardNo;
            commandParameters[7].Value = ow_Sex;
            commandParameters[8].Value = ow_Nation;
            commandParameters[9].Value = ow_Address;
            commandParameters[10].Value = ow_Email;
            commandParameters[11].Value = ow_OwnerStatus;
            commandParameters[12].Value = ow_CarportNum;
            commandParameters[13].Value = ow_CarNum;
            commandParameters[14].Value = ow_Remark;
            commandParameters[15].Value = ow_IsDelete;
            commandParameters[16].Value = ow_ConfirmStatus;
            commandParameters[17].Value = ow_CreateTime;

            string cmdText = "INSERT INTO [Tkx_Owner]([ow_Id],[ow_ParkId],[ow_LocalParkId],[ow_OwnerName],[ow_SectionId],[ow_Phone],[ow_CardNo],[ow_Sex],[ow_Nation],[ow_Address],[ow_Email],[ow_OwnerStatus],[ow_CarportNum],[ow_CarNum],[ow_Remark],[ow_IsDelete],[ow_ConfirmStatus],[ow_CreateTime])VALUES(@ow_Id,@ow_ParkId,@ow_LocalParkId,@ow_OwnerName,@ow_SectionId,@ow_Phone,@ow_CardNo,@ow_Sex,@ow_Nation,@ow_Address,@ow_Email,@ow_OwnerStatus,@ow_CarportNum,@ow_CarNum,@ow_Remark,@ow_IsDelete,@ow_ConfirmStatus,@ow_CreateTime)";

            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();
        }

        /// <summary>车主删除设置
        /// 
        /// </summary>
        /// <param name="Id">信息ID</param> 
        /// <param name="IsDelete">是否可用</param>  
        /// <returns></returns> 
        public string OwnerDelete(Guid ow_Id, bool ow_IsDelete)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@ow_Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ow_IsDelete",SqlDbType.Bit,1)
        };
            commandParameters[0].Value = ow_Id;
            commandParameters[1].Value = ow_IsDelete;

            string cmdText = "UPDATE [Tkx_Owner] SET [ow_IsDelete]=@ow_IsDelete WHERE [ow_Id]=@ow_Id ";

            if (db.ExecuteNonQuery(cmdText, commandParameters) > 0)
            {
                return FunReturn.成功.ToString();
            }
            return FunReturn.失败.ToString();
        }




        #endregion





        #region 辅助方法 

        /// <summary>判断停车场 是否存在
        /// 
        /// </summary>
        /// <param name="Id">停车场ID</param>  
        /// <returns></returns> 
        public Guid ParkInfoSel(Guid Id)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16)
        };
            commandParameters[0].Value = Id;
            string cmdText = "SELECT [Id] FROM ParkInfo WHERE [Id]=@Id ";
            return db.QueryOneValue<Guid>(cmdText, commandParameters);
        }

        /// <summary>判断停车场 是否存在 
        /// </summary>
        /// <param name="Id">停车场ID</param> 
        /// <param name="ParentLocalParkId">本地停车场id</param>  
        /// <returns></returns> 
        public Guid ParkInfoSel(Guid Id, String ParentLocalParkId)
        {

            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ParentLocalParkId",SqlDbType.VarChar,50)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = ParentLocalParkId;
            string cmdText = "SELECT [Id] FROM ParkInfo WHERE [Id]=@Id  AND [ParentLocalParkId]=@ParentLocalParkId";
           return db.QueryOneValue<Guid>(cmdText, commandParameters);

        }


        /// <summary>查询该岗亭是否属于
        /// 
        /// </summary>
        /// <param name="Id">岗亭ID</param> 
        /// <param name="ParkId">停车场ID</param>  
        /// <returns></returns> 
        public Guid SentryBoxSel(Guid Id, Guid ParkId)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ParkId",SqlDbType.UniqueIdentifier,16)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = ParkId;
            string cmdText = "SELECT [Id] FROM SentryBox WHERE [Id]=@Id AND [ParkId] =@ParkId";
            return db.QueryOneValue<Guid>(cmdText, commandParameters);

        }
        /// <summary>查询通道是否属于该车场的
        /// 
        /// </summary>
        /// <param name="Id">通道ID</param> 
        /// <param name="ParkId">停车场id</param>  
        /// <returns></returns> 
        public Guid ChannelSel(Guid Id, Guid ParkId)
        {
            SqlParameter[] commandParameters ={
         new SqlParameter("@Id",SqlDbType.UniqueIdentifier,16),
         new SqlParameter("@ParkId",SqlDbType.UniqueIdentifier,16)
        };
            commandParameters[0].Value = Id;
            commandParameters[1].Value = ParkId;

            string cmdText = "SELECT [Id] FROM Channel WHERE [Id]=@Id AND [ParkId]=@ParkId";
            return db.QueryOneValue<Guid>(cmdText, commandParameters);
        }

        #endregion
    }
}
