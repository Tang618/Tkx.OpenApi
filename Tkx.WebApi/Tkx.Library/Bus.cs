using System;
using System.Collections.Generic;
using System.Text;
using Tkx.Common;
using Tkx.Model;

namespace Tkx.Library
{
   public  class Bus: BaseBusiness
    {
        // public Tkx.DbBase.ApiLogDAL apiLog = new DbBase.ApiLog();
     

        /// <summary>查询地区信息
        /// 
        /// </summary>
        /// <param name="pid">上级或本级ID(允许为空输入0)</param>
        /// <param name="Nmae">名称(输入省,市,区县名称)</param>
        /// <param name="leveid">1 获取本级信息.2 获取下级信息 3 获取全国省份名称 </param>
        /// <returns></returns>
        public Errcode GetArea(int pid, string Name, int Leve)
        {
            IList<AreaModel> li = new List<AreaModel>();
            if (Leve == 1)
            {
                li = busDAL.GetProvinceSingle(pid, Name);
            }
            else if (Leve == 2)
            {
                if (pid > 0)//如果有传ID就用ID查询.否则就用他的名称来查询.
                {
                    li = busDAL.GetArea(pid);
                }
                else
                {
                    li = busDAL.GetArea(Name);
                }
            }
            else if (Leve == 3)
            {
                li = busDAL.GetProvince();
            }
            else
            {
               return ResultSet(ErrcodeType.参数无效, _mess, li, 0);
            }
            errcode = ResultSet(ErrcodeType.请求成功, _mess, li, 0);
            return errcode;

        }
         

        /// <summary>判断接口的访问上限
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法名</param>
        /// <returns></returns>
        public  bool InterfaceCount(string controller, string action)
        {

            string cName = string.Format("{0}/{1}", controller, action);
Tkx.Model.ApiLogModel ap= apiLog.ApiCountSel(cName, DateTime.Now); 
            if (ap.ap_Id == 0)
            {
                apiLog.ApiCountAdd(cName, 1, DateTime.Now);
                return true; 
            }
            else
            {
                if (ap.ap_maxcount > ap.ap_count) {
                    apiLog.ApiCountUpdate(ap.ap_Id); return true;
                }
                return false;
               
            } 
        }
         
        /// <summary>查询令牌是否有效
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsTokeID(string id)
        {
            return true;
        }

        /// <summary>生成加密字符串 
        /// 主要是给前端a=1&b=2&c=3&c=4的使用.返回加密后的结果,还有明密文的对比信息
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="TokenID">临时令牌</param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public string IsKey(string par, string TokenID,out string dataKey)
        {
            string apkey = GetKey(TokenID);
            string[] pp = par.Split('&');
            string _key = "";

            //当时这么写是dataKey不放在参数组里面
            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i] == "")
                {
                    continue;
                }
                _key += pp[i].Split('=')[1] + ",";
            }
   
            string KK = Safety.Md5(_key.ToLower() + apkey).ToLower();// 一般这里的作法是把参数按字母重排.在组合.但我这边反常规做.直接串在一起作小写,加私有KEY.
            dataKey = string.Format("明文:{0},密文:{1}", _key + apkey, KK);
             Tools.MessBox(string.Format("明文:{0},密文:{1},来自的Key{2}", _key + apkey, KK, dataKey), "//Error//DataKey//");

            return KK;
        }

        /// <summary>封装参数加密
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <param name="TokenID"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public string IsKey(IDictionary<string, object> par, string TokenID, string dataKey)
        {
            string apkey = GetKey(TokenID);
            string _key = "";
            StringBuilder sb = new StringBuilder();
            foreach (var item in par)
            {
                if (item.Key != "dataKey")
                {
                    sb.AppendFormat("{0},", item.Value);
                }
            }
            _key = sb.ToString();
            string KK = Safety.Md5(_key.ToLower() + apkey).ToLower();// 一般这里的作法是把参数按字母重排.在组合.但我这边反常规做.直接串在一起作小写,加私有KEY.
            Tools.MessBox(string.Format("明文:{0},密文:{1},来自的Key{2}", _key + apkey, KK, dataKey), "//Error//DataKey//");
            return KK;
        }

        /// <summary>这地方修改成 获取redis缓存得到key来加密
        /// 
        /// </summary>
        /// <param name="TokenID">临时令牌</param>
        /// <returns></returns>
        private string GetKey(string TokenID)
        {
            return "3ead8725bc574447ad6e441e5a80e140";
        }



      

    }
}
