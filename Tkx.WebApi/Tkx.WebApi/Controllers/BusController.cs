using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tkx.Model;
using Tkx.Common;
using Tkx.DbBase;

namespace Tkx.WebApi.Controllers
{

    // [Produces("application/json")]
    [Route("api/Bus")]
    public class BusController : ApiBaseController
    {
        //这块以后封装到Redis 

        [Route("tokenID")]
        [HttpGet]
        public Errcode tokenID(string Appid, string AppKey)
        {
          
            if (Appid == "and" && AppKey == "cf0cf1b9b5de40c5b7b72d0b349d577d")
            {
                _mess = Tools.CreateGUID();
                Tokenmodel.Tokenid = _mess;
                Tokenmodel.DataKey = "cf0cf1b9b5de40c5b7b72d0b349d577d";
                Tokens.Add(Tokenmodel);
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
                return errcode;
            } 
            errcode = ResultSet(ErrcodeType.令牌无效, _mess, null);

            return errcode;

           
        }
        [Route("GetTokenID")]
        [HttpGet]
        public string GetTokenID(string Appid, string AppKey)
        {
            if (Appid == "and" && AppKey == "cf0cf1b9b5de40c5b7b72d0b349d577d")
            {
                _mess = Tools.CreateGUID();
                Tokenmodel.Tokenid = _mess;
                Tokenmodel.DataKey = "cf0cf1b9b5de40c5b7b72d0b349d577d";
                Tokens.Add(Tokenmodel);
                errcode = ResultSet(ErrcodeType.请求成功, _mess, null);
                return _mess;
            }
            
            return "令牌无效";
        }
        [Route("LogApiList")]
        [HttpPost]
        public Errcode LogApiList(string area, string ApiName,int? ApiState,int pageIndex, int pageSize)
        {
            if (base.Tokenmodel.Version == "2.0")
            { }
            if (pageSize > 50)
            {
                return ResultSet(ErrcodeType.参数无效, _mess, null);
            }
            string whereString = "1=1";

            if (area != "all")
            {
                whereString = string.Format("{0} AND area='{0}'", whereString, area); 
            }

            if (!ApiName.IsDBNull())
            {
                whereString = string.Format("{0} AND Lg_Title='{0}'", whereString, ApiName);
            }
            if (ApiState != null)
            {
                whereString = string.Format("{0} AND Lg_state='{0}'", whereString, ApiState);
            }

            int cnt = 0;
            //TangDataTable dt= bus.apiLog.pageList("[Lg_id],[Lg_Url],[Lg_Type],[Lg_Title],[Lg_Text],[Lg_Time],[Lg_Token],[Lg_Parameter],[Lg_ip],[Lg_Bak],[Lg_state],[Lg_Version],[Lg_Platform],[Lg_Area],[Lg_TimeOut]", "lg_id desc", whereString, pageSize, pageIndex, ref cnt);


            List<Dictionary<string, object>>  datat =bus.apiLog.page_List("[Lg_id],[Lg_Url],[Lg_Type],[Lg_Title],[Lg_Text],[Lg_Time],[Lg_Token],[Lg_Parameter],[Lg_ip],[Lg_Bak],[Lg_state],[Lg_Version],[Lg_Platform],[Lg_Area],[Lg_TimeOut]", "lg_id desc", whereString, pageSize, pageIndex, ref cnt);
            if (cnt > 0) 
            {
                DataMode d = new DataMode();
                d.List = datat;
                d.PageCount = cnt;
                errcode = ResultSet(ErrcodeType.请求成功, ErrcodeType.请求成功.ToString(), d); 
                return errcode;
            }


            errcode = ResultSet(ErrcodeType.业务处理无效, _mess, null);

            return errcode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Time">时间格式2017-01-01 传空表示要查全部日期</param>
        /// <param name="ApiName">要查询接口名称,  默认:传空 表示要查全部</param>
        /// <param name="pageIndex"> 页码</param>
        /// <param name="pageSize">条目</param>
        /// <returns></returns>
        [Route("LogApiCountList")] 
        [HttpPost]
        public Errcode LogApiCountList(string Time,string ApiName, int pageIndex, int pageSize)
        {

            if (pageSize > 50)
            {
                return ResultSet(ErrcodeType.参数无效, _mess, null);
            }
            string whereString = "1=1";

            if (!Time.IsDBNull())
            {
                //查询当天接口访问情况
               // whereString = string.Format("{0} AND ap_name='{0}'", whereString, ApiName);
            }
            if (!ApiName.IsDBNull())
            {
                whereString = string.Format("{0} AND ap_name='{0}'", whereString, ApiName);
            }
            int cnt = 0;
       var dt = bus.apiLog.pageCountList("[ap_Id],[ap_Name],[ap_Count],[ap_Time],[ap_MaxCount]", "ap_Count desc", whereString, pageSize, pageIndex, ref cnt);
            // List<Dictionary<string, object>> datat =
            if (cnt > 0)
            {
                DataMode d = new DataMode();
                d.List = dt;
                d.PageCount = cnt;
                errcode = ResultSet(ErrcodeType.请求成功, ErrcodeType.请求成功.ToString(), d);
                return errcode;
            }


            errcode = ResultSet(ErrcodeType.业务处理无效, _mess, null);

            return errcode;
        }

        /// <summary>查询地区信息
        /// 
        /// </summary>
        /// <param name="ParentId">上级或本级ID(允许为空输入0)</param>
        /// <param name="Nmae">名称(输入省,市,区县名称)</param>
        /// <param name="Leve">1 获取本级信息.2 获取下级信息 </param>
        /// <returns></returns>
        [Route("GetArea")]
        [HttpPost]
        public Errcode GetArea(int ParentId, string Name,int Leve)
        {

            errcode = bus.GetArea(ParentId, Name, Leve);
  
            return errcode;
        }
    }
}