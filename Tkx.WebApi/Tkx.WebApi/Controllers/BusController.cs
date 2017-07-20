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
        //����Ժ��װ��Redis 

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
                errcode = ResultSet(ErrcodeType.����ɹ�, _mess, null);
                return errcode;
            } 
            errcode = ResultSet(ErrcodeType.������Ч, _mess, null);

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
                errcode = ResultSet(ErrcodeType.����ɹ�, _mess, null);
                return _mess;
            }
            
            return "������Ч";
        }
        [Route("LogApiList")]
        [HttpPost]
        public Errcode LogApiList(string area, string ApiName,int? ApiState,int pageIndex, int pageSize)
        {
            if (base.Tokenmodel.Version == "2.0")
            { }
            if (pageSize > 50)
            {
                return ResultSet(ErrcodeType.������Ч, _mess, null);
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
                errcode = ResultSet(ErrcodeType.����ɹ�, ErrcodeType.����ɹ�.ToString(), d); 
                return errcode;
            }


            errcode = ResultSet(ErrcodeType.ҵ������Ч, _mess, null);

            return errcode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Time">ʱ���ʽ2017-01-01 ���ձ�ʾҪ��ȫ������</param>
        /// <param name="ApiName">Ҫ��ѯ�ӿ�����,  Ĭ��:���� ��ʾҪ��ȫ��</param>
        /// <param name="pageIndex"> ҳ��</param>
        /// <param name="pageSize">��Ŀ</param>
        /// <returns></returns>
        [Route("LogApiCountList")] 
        [HttpPost]
        public Errcode LogApiCountList(string Time,string ApiName, int pageIndex, int pageSize)
        {

            if (pageSize > 50)
            {
                return ResultSet(ErrcodeType.������Ч, _mess, null);
            }
            string whereString = "1=1";

            if (!Time.IsDBNull())
            {
                //��ѯ����ӿڷ������
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
                errcode = ResultSet(ErrcodeType.����ɹ�, ErrcodeType.����ɹ�.ToString(), d);
                return errcode;
            }


            errcode = ResultSet(ErrcodeType.ҵ������Ч, _mess, null);

            return errcode;
        }

        /// <summary>��ѯ������Ϣ
        /// 
        /// </summary>
        /// <param name="ParentId">�ϼ��򱾼�ID(����Ϊ������0)</param>
        /// <param name="Nmae">����(����ʡ,��,��������)</param>
        /// <param name="Leve">1 ��ȡ������Ϣ.2 ��ȡ�¼���Ϣ </param>
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