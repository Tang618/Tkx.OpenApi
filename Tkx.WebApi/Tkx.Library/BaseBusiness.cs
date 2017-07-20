using System;
using System.Collections.Generic;
using System.Text;

using Tkx.Model;

namespace Tkx.Library
{
    /// <summary>业务层基类
    /// 主要用于封装业务层经常要使用的东西.(如果你发现这个函数有不少地方要用.建议就放这里面)
    /// </summary>
  public  class BaseBusiness
    {
        public Errcode errcode;
        //返回提示语
        protected string _mess = "";


        public DAL.ParKingDAL parKingDal = new DAL.ParKingDAL();//数据访问层默认约定用DAL结尾
        public DAL.BusDAL busDAL = new DAL.BusDAL();//公共模块的
        public Tkx.DAL.ApiLogDAL apiLog = new DAL.ApiLogDAL();

        protected string LogNetAdds = "\\Error\\Business\\";//Error//pay/

        /// <summary>统一返回集合 
        /// </summary>
        /// <returns></returns>.
        protected Errcode ResultSet(ErrcodeType er, string Mess, object da,int PageCount)
        {
        
           
            DataMode d = new DataMode();
            d.List = da;
            d.PageCount = PageCount;
            return ResultSet(er, true, Mess, d);
        }




        /// <summary>统一返回集合 
        /// </summary>
        /// <returns></returns>.        
        protected Errcode ResultSet(ErrcodeType er, string Mess, DataMode da)
        {

            return ResultSet(er, true, Mess, da);
        }
        /// <summary>统一返回集合 
        /// </summary>
        /// <returns></returns>
        protected Errcode ResultSet(ErrcodeType er, bool isShow, string Mess, DataMode da)
        {

            if (da == null)
            {
                da = new DataMode();
            }
            Errcode e = new Errcode();
            e.errcodeType = er;
            e.isShow = isShow;
            e.Mess = Mess;
            e.Data = da;
            return e;
        }
    }
}
