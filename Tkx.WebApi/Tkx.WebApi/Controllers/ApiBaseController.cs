using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tkx.Model;

using Tkx.Common;
using System.IO;
using System.Text;

namespace Tkx.WebApi.Controllers
{




    /// <summary>过滤器编写
    /// 1.主要是从AOP角度出发.主要是要对action的执行前,后还有授权进行拦截.
    /// 本身WEBAPI已经考虑过几种情况下的拦截处理. 
    /// </summary>
    public class ApiBaseController : Controller,IActionFilter , IAuthorizationFilter
    {
        public static List<TokenModel> Tokens = null;//令牌组
        protected string _mess = "升级中,请稍后访问.";
        public ApiBaseController()
        {
            if (Tokens == null)
            {
                Tokens = new List<TokenModel>();
            }
            errcode = ResultSet(ErrcodeType.业务处理无效, _mess, null);
        }

        protected Tkx.Library.Bus bus = new Library.Bus();
        protected Tkx.Library.ParKing parKing = new Library.ParKing();//做成依赖注入.

        public Errcode errcode ;
        public TokenModel Tokenmodel;
        /// <summary>记录开始时间 
        /// </summary>
        private DateTime dt_S;
        /// <summary>执行前
        /// 1.当前接口访问数是否超标
        /// 2.判断是否是获取令牌 Y 就绕过后面验证
        /// 3.判断令牌是否有效
        /// 4.判断dataKey加密是否正确
        /// 5.判断时长是否超过要求-+10分钟
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string action = context.ActionDescriptor.RouteValues["action"];//方法
            string controller = context.ActionDescriptor.RouteValues["controller"];//控制名 

            /// 判断接口是否超出调用
            if (!bus.InterfaceCount(controller, action))
            {
                context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.系统繁忙, ErrcodeType.系统繁忙.ToString(), null)));
                ErrroLog(action, controller, ResultSet(ErrcodeType.系统繁忙, ErrcodeType.系统繁忙.ToString(), null), context);
                context.Result = new EmptyResult();
            } 
            IDictionary<string, object> li = context.ActionArguments;//获取参数组  
            Tokenmodel = new TokenModel();//创建令牌  
            ///这个要绕过校验.其它的要进来校验.后期这块要抽到IAuthorizationFilter成
            if (!(controller.ToLower() == "bus" && action.ToLower() == "tokenid" || action.ToLower() == "gettokenid"))
            { 
                var hreader = Request.Headers;
                foreach (var item in hreader)
                {
                    if (item.Key == "Tokenid")
                    {
                        ///如果令牌有误就短路.
                        if (!bus.IsTokeID(item.Value))
                        {
                            context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.令牌无效, ErrcodeType.令牌无效.ToString(), null)));
                            ErrroLog(action, controller, ResultSet(ErrcodeType.令牌无效, ErrcodeType.令牌无效.ToString(), null), context);
                        }
                        //后面在函数中会使用具体的版本号之类使用. 
                        Tokenmodel.Tokenid = item.Value;
                    }
                    if (item.Key == "Platform")
                    {
                        Tokenmodel.Platform = item.Value;
                    }
                    if (item.Key == "Version")
                    {
                        Tokenmodel.Version = item.Value;
                    }
                    if (item.Key == "CreateTime")
                    {
                        if (!Tools.TimeOut(item.Value, 10))
                        {
                            context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.时间差, ErrcodeType.时间差.ToString(), null)));

                            ErrroLog(action, controller, ResultSet(ErrcodeType.时间差, ErrcodeType.时间差.ToString(), null), context);
                        }
                        Tokenmodel.CreateTime = item.Value;
                    }
                    if (item.Key == "UserTy")
                    {
                        Tokenmodel.UserTy = item.Value;
                    }
                    if (item.Key == "DeviceID")
                    {
                        Tokenmodel.DeviceID = item.Value;
                    }
                    if (item.Key == "DataKey")
                    {
                        Tokenmodel.DataKey = item.Value;
                    }
                }

                ///安全校验码无效
                if (bus.IsKey(li, Tokenmodel.Tokenid, Tokenmodel.DataKey) != Tokenmodel.DataKey)
                {
                   context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.安全校验码无效, ErrcodeType.安全校验码无效.ToString(), null)));
                    ErrroLog(action, controller, ResultSet(ErrcodeType.安全校验码无效, ErrcodeType.安全校验码无效.ToString(), null), context);

                    //  Response.Redirect("/Home/NotTokeid/7006");//
                    //  context.Result = new EmptyResult();
                }

            } 
            dt_S = DateTime.Now;
        }

        private void ErrroLog(string action,string controller, Errcode errcode, ActionExecutingContext context)
        {


            string _par = context.HttpContext.Request.QueryString.Value;//GET的方法

          

            if ((Request.Method == "POST"))
            {
                if (Request.ContentType.Contains("charset=UTF-8"))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var item in Request.Form)
                    {
                        sb.Append(item + ",");
                    }
                    _par = sb.ToString();
                }
            }
            //string action =  context.ActionDescriptor.RouteValues["action"];
            //string controller =  context.ActionDescriptor.RouteValues["controller"];
            String Lg_Url = Request.Host.ToString();
            String Lg_Type = "页面加载";
            String Lg_Title = string.Format("{0}/{1}", controller, action);
             String Lg_Text = Newtonsoft.Json.JsonConvert.SerializeObject(errcode);
            String Lg_Token = Tokenmodel.Tokenid;
            String Lg_Parameter =   _par;// (Request.Method == "POST") ? _par : context.HttpContext.Request.QueryString.Value;
            String Lg_ip = Tokenmodel.DeviceID.IsDBNull() ? "" : Tokenmodel.DeviceID;
            String Lg_Bak = "";
            Int32 Lg_state = Tools.GetEnumInt<ErrcodeType>(errcode.errcodeType.ToString());
            String Lg_Version = "2.0";
            String Lg_Platform = Tokenmodel.Platform.IsDBNull() ? "" : Tokenmodel.Platform;
            String Lg_Area = "厦门";
            double Lg_TimeOut = (DateTime.Now - dt_S).TotalMilliseconds;

            bus.apiLog.Add(Lg_Url, Lg_Type, Lg_Title, Lg_Text, DateTime.Now, Lg_Token, Lg_Parameter, Lg_ip, Lg_Bak, Lg_state, Lg_Version, Lg_Platform, Lg_Area, Lg_TimeOut);

        }
      
        
        /// <summary>执行后
        /// 主要把执行的结果集,输入的参数,该接口访问的耗时.记录到接口日记中.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        { 
            string _par = context.HttpContext.Request.QueryString.Value;//GET的方法
             
            var hrae = Request.Headers.Values;

            if ((Request.Method == "POST"))
            {
                //    if (Request.ContentType == "application/x-www-form-urlencoded; charset=UTF-8")
                if (Request.ContentType.Contains("charset=UTF-8"))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var item in Request.Form)
                    {
                        sb.Append(item + ",");
                    }
                    _par = sb.ToString();
                }
            } 
            string action = context.ActionDescriptor.RouteValues["action"];
            string controller = context.ActionDescriptor.RouteValues["controller"];
            String Lg_Url = Request.Host.ToString();
            String Lg_Type = "页面加载";
            String Lg_Title = string.Format("{0}/{1}", controller, action);
            String Lg_Text = Newtonsoft.Json.JsonConvert.SerializeObject(errcode);
            String Lg_Token = Tokenmodel.Tokenid;
            String Lg_Parameter = _par;// (Request.Method == "POST") ? _par : context.HttpContext.Request.QueryString.Value;
            String Lg_ip = Tokenmodel.DeviceID.IsDBNull()? "": Tokenmodel.DeviceID;
            String Lg_Bak = "";
            Int32 Lg_state = Tools.GetEnumInt<ErrcodeType>(errcode.errcodeType.ToString());
            String Lg_Version = "2.0";
            String Lg_Platform = Tokenmodel.Platform.IsDBNull() ? "" : Tokenmodel.Platform ;
            String Lg_Area = "厦门";
            double Lg_TimeOut = (DateTime.Now - dt_S).TotalMilliseconds;

             bus.apiLog.Add(Lg_Url, Lg_Type, Lg_Title, Lg_Text, DateTime.Now, Lg_Token, Lg_Parameter, Lg_ip, Lg_Bak, Lg_state, Lg_Version, Lg_Platform, Lg_Area, Lg_TimeOut); 
        }

        #region 迁移到bus里面

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
            Errcode e = new Errcode();
            e.errcodeType = er;
            e.isShow = isShow;
            e.Mess = Mess;
            e.Data = da;
            return e;
        }

        #endregion

        /// <summary>商家授权验证类 
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {


          //  throw new NotImplementedException();
        }
    }


    
   
}