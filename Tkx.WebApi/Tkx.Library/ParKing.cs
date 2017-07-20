using System;
using System.Collections.Generic;
using System.Text;
using Tkx.Common;
using Tkx.Model;

namespace Tkx.Library

{
    /// <summary>停车场管理模块
    /// 主要负责车场管理,添加,修改,删除.查询等.
    /// 
    /// 1.创建车场
    /// 2.修改车场
    /// 3.删除车场
    /// 4.车场挂靠集团
    /// 5.车场解除挂靠集团
    /// 
    /// </summary>
    public class ParKing : BaseBusiness
    {

        #region 车场信息

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
        /// <param name="IsOnline">在线状态</param> 
        /// <param name="Floors">层数</param> 
        /// <param name="CarportNumber">总车位数</param> 
        /// <param name="FixedCarportNumber">固定车位数</param> 
        /// <param name="TempCarportNumber">临时车位数</param> 
        /// <param name="Type">停车场类型</param>  
        /// <param name="LocalParkId">父级本地停车场id</param> 
        /// <param name="ParentLocalParkId">本地停车场id</param> 
        /// <param name="CityId">城市Id</param> 
        /// <param name="CityName">城市名称</param> 
        /// <param name="AreaId">区域Id</param> 
        /// <param name="AreaName">区域名称</param> 
        /// <param name="Platform">平台,0.无，1.停开心，</param>  
        /// <returns></returns> 
        public Errcode Create(Guid Id, String Name, String Address, String Phone, String ImageUrl, Decimal Lng, Decimal Lat, Int32 IsOnline, Int32 Floors, Int32 CarportNumber, Int32 FixedCarportNumber, Int32 TempCarportNumber, String Type, String LocalParkId, String ParentLocalParkId, Int32 CityId, String CityName, Int32 AreaId, String AreaName, Int32 Platform)
        {   
            #region 业务校验

            if (Name.IsDBNull())
            {
                _mess = "车场名称不能为空";
                errcode = ResultSet(ErrcodeType.参数无效, _mess, null);
                return errcode;
            }
            if (!Phone.IsValidateMobile())
            {
                _mess = "车场联系电话格式不正确";
                errcode = ResultSet(ErrcodeType.参数无效, _mess, null);
            //    return errcode;
            }
            if (Address.IsDBNull())
            {
                _mess = "车场地址不能为空";
                errcode = ResultSet(ErrcodeType.参数无效, _mess, null);
                return errcode;
            }


            #endregion

 
            Guid _idd= parKingDal.Sel(Id);
            if (_idd != Guid.Empty)
            {
                _mess = "该停车场已存在配置信息，请勿重复添加";
                errcode = ResultSet(ErrcodeType.参数无效, _mess, null);
                return errcode;
            } 
            int RemCarportNum = 0;//剩余空位 
            int CurrentId = 0;//当前编号 
            string Path = "";//路径地址
            string Letter = Tools.GetAllPYLetters(Name);//拼音
            string Initial = Letter.Substring(0, 1);//首字母
            DateTime CreateTime = DateTime.Now;//创建时间

            Guid ParentId = parKingDal.GetLocalParkId(LocalParkId);//父级编号 主要用于做上下级车场关联作用


            
            if (LocalParkId.IsDBNull())
            {
                LocalParkId = "";
            }

            _mess = parKingDal.Create(Id, Name, Address, Phone, ImageUrl, Lng, Lat, Letter, Initial, IsOnline, Floors, CarportNumber, RemCarportNum, FixedCarportNumber, TempCarportNumber, CurrentId, Path, Type,true, CreateTime, ParentId, LocalParkId, ParentLocalParkId, CityId, CityName, AreaId, AreaName, Platform);

            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            }
            return errcode;
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
        public Errcode Update(Guid Id, String Name, String Address, String Phone, String ImageUrl, Decimal Lng, Decimal Lat, String Letter, String Initial, Int32 IsOnline, Int32 Floors, Int32 CarportNumber, Int32 RemCarportNum, Int32 FixedCarportNumber, Int32 TempCarportNumber, Int32 CurrentId, String Path, Int32 Type, DateTime CreateTime, Guid ParentId, String LocalParkId, String ParentLocalParkId, Int32 CityId, String CityName, Int32 AreaId, String AreaName, Int32 Platform)
        {
            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            }
            return errcode;
        }

        /// <summary>删除停车场
        /// 
        /// </summary>
        /// <param name="Id">停车场ID</param> 
        /// <param name="IsDelete">是否可用</param>  
        /// <returns></returns> 
        public Errcode Delete(Guid Id, bool IsDelete)
        {
            _mess = parKingDal.Delete(Id, IsDelete);
            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            }
            return errcode;
        }

        #endregion

        #region 进出场记录集


        /// <summary>进场记录 
        /// </summary>
        /// <param name="LocalParkId">本地停车场id</param> 
        /// <param name="CarNo">车牌号</param> 
        /// <param name="CarType">车辆类型</param> 
        /// <param name="EnterParkTime">进场时间</param> 
        /// <param name="EnterParkImage">进场图片</param> 
        /// 
        /// <param name="EnterParkType">进场方式</param> 
        /// <param name="BoxId">岗亭id</param> 
        /// <param name="ChannelId">通道id</param> 
        /// <param name="DutyPersonal">值班人</param> 
        /// <param name="Remark">备注</param> 
        /// <param name="ParkId">停车场ID</param>  
        /// <returns></returns>  
        public Errcode IntoThe(String LocalParkId, String CarNo, Int32 CarType, DateTime EnterParkTime, String EnterParkImage, Int32 Color, Int32 EnterParkType, Guid BoxId, Guid ChannelId, String DutyPersonal, String Remark, Guid ParkId)
        {
            #region 数据校验类


            ///1.判断车场是否存在
            if (parKingDal.ParkInfoSel(ParkId) == Guid.Empty)
            {
                return ResultSet(ErrcodeType.业务处理无效, "停车场信息无效", null);
            }
            ///2.判断该通过是否存在.是否属于该车场
            if (parKingDal.ChannelSel(ChannelId, ParkId) == Guid.Empty)
            {
                return ResultSet(ErrcodeType.业务处理无效, "通过信息无效", null);
            }
            ///3.判断岗亭是否存在.是否属于该车场
            if (parKingDal.SentryBoxSel(BoxId, ParkId) == Guid.Empty)
            {
                return ResultSet(ErrcodeType.业务处理无效, "岗亭信息无效", null);
               
            }
            //判断
            #endregion
            _mess = parKingDal.IntoThe(Guid.NewGuid(), LocalParkId, CarNo, CarType, EnterParkTime, EnterParkImage, EnterParkType, BoxId, ChannelId, DutyPersonal, Remark, true, DateTime.Now, ParkId);
             
            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            } 
            return errcode;
        }

        #endregion


        #region 车主信息

        /// <summary>车主信息
        /// 
        /// </summary>
        /// <param name="ParkId">停车场ID</param> 
        /// <param name="LocalParkId">本地停车场ID</param> 
        /// <param name="OwnerName">车主名称</param> 
        /// <param name="SectionId">所属部门id</param> 
        /// <param name="Phone">手机号</param> 
        /// <param name="CardNo">身份证号</param> 
        /// <param name="Sex">性别</param> 
        /// <param name="Nation">民族</param> 
        /// <param name="Address">家庭住址</param> 
        /// <param name="Email">邮箱</param> 
        /// <param name="OwnerStatus">车主状态</param> 
        /// <param name="CarportNum">车位数</param> 
        /// <param name="CarNum">车辆数</param> 
        /// <param name="Remark">备注信息</param>  
        /// <returns></returns> 
        public Errcode OwnerAdd(Guid ParkId, String LocalParkId, String OwnerName, Guid SectionId, String Phone, String CardNo, Int32 Sex, String Nation, String Address, String Email, Int32 OwnerStatus, Int32 CarportNum, Int32 CarNum, String Remark)
        {
            #region 数据校验
            ///1.判断车场是否存在
            if (parKingDal.ParkInfoSel(ParkId, LocalParkId) == Guid.Empty)
            {
                return ResultSet(ErrcodeType.业务处理无效, "停车场信息无效", null);
            }

            #endregion

            Guid Id = Guid.NewGuid();
            bool IsDelete = true;//是否可用
            bool ConfirmStatus = true;//是否同步
            DateTime CreateTime = DateTime.Now;//创建时间
            _mess = parKingDal.OwnerAdd(Id, ParkId, LocalParkId, OwnerName, SectionId, Phone, CardNo, Sex, Nation, Address, Email, OwnerStatus, CarportNum, CarNum, Remark,  IsDelete, ConfirmStatus, CreateTime);
            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            }

            return errcode;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">信息ID</param> 
        /// <param name="IsDelete">是否可用</param>  
        /// <returns></returns> 
        public Errcode OwnerDelete(Guid Id, bool IsDelete)
        {
            _mess = parKingDal.OwnerDelete(Id, IsDelete);
            if (_mess != "成功")
            {
                errcode = ResultSet(ErrcodeType.业务处理无效, ErrcodeType.业务处理无效.ToString(), null);
            }
            else
            {
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
            }

            return errcode;
        }


        #endregion



    }
}