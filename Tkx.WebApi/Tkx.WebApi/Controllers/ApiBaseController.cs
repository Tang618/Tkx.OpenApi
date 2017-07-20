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




    /// <summary>��������д
    /// 1.��Ҫ�Ǵ�AOP�Ƕȳ���.��Ҫ��Ҫ��action��ִ��ǰ,������Ȩ��������.
    /// ����WEBAPI�Ѿ����ǹ���������µ����ش���. 
    /// </summary>
    public class ApiBaseController : Controller,IActionFilter , IAuthorizationFilter
    {
        public static List<TokenModel> Tokens = null;//������
        protected string _mess = "������,���Ժ����.";
        public ApiBaseController()
        {
            if (Tokens == null)
            {
                Tokens = new List<TokenModel>();
            }
            errcode = ResultSet(ErrcodeType.ҵ������Ч, _mess, null);
        }

        protected Tkx.Library.Bus bus = new Library.Bus();
        protected Tkx.Library.ParKing parKing = new Library.ParKing();//��������ע��.

        public Errcode errcode ;
        public TokenModel Tokenmodel;
        /// <summary>��¼��ʼʱ�� 
        /// </summary>
        private DateTime dt_S;
        /// <summary>ִ��ǰ
        /// 1.��ǰ�ӿڷ������Ƿ񳬱�
        /// 2.�ж��Ƿ��ǻ�ȡ���� Y ���ƹ�������֤
        /// 3.�ж������Ƿ���Ч
        /// 4.�ж�dataKey�����Ƿ���ȷ
        /// 5.�ж�ʱ���Ƿ񳬹�Ҫ��-+10����
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string action = context.ActionDescriptor.RouteValues["action"];//����
            string controller = context.ActionDescriptor.RouteValues["controller"];//������ 

            /// �жϽӿ��Ƿ񳬳�����
            if (!bus.InterfaceCount(controller, action))
            {
                context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.ϵͳ��æ, ErrcodeType.ϵͳ��æ.ToString(), null)));
                ErrroLog(action, controller, ResultSet(ErrcodeType.ϵͳ��æ, ErrcodeType.ϵͳ��æ.ToString(), null), context);
                context.Result = new EmptyResult();
            } 
            IDictionary<string, object> li = context.ActionArguments;//��ȡ������  
            Tokenmodel = new TokenModel();//��������  
            ///���Ҫ�ƹ�У��.������Ҫ����У��.�������Ҫ�鵽IAuthorizationFilter��
            if (!(controller.ToLower() == "bus" && action.ToLower() == "tokenid" || action.ToLower() == "gettokenid"))
            { 
                var hreader = Request.Headers;
                foreach (var item in hreader)
                {
                    if (item.Key == "Tokenid")
                    {
                        ///�����������Ͷ�·.
                        if (!bus.IsTokeID(item.Value))
                        {
                            context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.������Ч, ErrcodeType.������Ч.ToString(), null)));
                            ErrroLog(action, controller, ResultSet(ErrcodeType.������Ч, ErrcodeType.������Ч.ToString(), null), context);
                        }
                        //�����ں����л�ʹ�þ���İ汾��֮��ʹ��. 
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
                            context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.ʱ���, ErrcodeType.ʱ���.ToString(), null)));

                            ErrroLog(action, controller, ResultSet(ErrcodeType.ʱ���, ErrcodeType.ʱ���.ToString(), null), context);
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

                ///��ȫУ������Ч
                if (bus.IsKey(li, Tokenmodel.Tokenid, Tokenmodel.DataKey) != Tokenmodel.DataKey)
                {
                   context.Result = Content(Newtonsoft.Json.JsonConvert.SerializeObject(ResultSet(ErrcodeType.��ȫУ������Ч, ErrcodeType.��ȫУ������Ч.ToString(), null)));
                    ErrroLog(action, controller, ResultSet(ErrcodeType.��ȫУ������Ч, ErrcodeType.��ȫУ������Ч.ToString(), null), context);

                    //  Response.Redirect("/Home/NotTokeid/7006");//
                    //  context.Result = new EmptyResult();
                }

            } 
            dt_S = DateTime.Now;
        }

        private void ErrroLog(string action,string controller, Errcode errcode, ActionExecutingContext context)
        {


            string _par = context.HttpContext.Request.QueryString.Value;//GET�ķ���

          

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
            String Lg_Type = "ҳ�����";
            String Lg_Title = string.Format("{0}/{1}", controller, action);
             String Lg_Text = Newtonsoft.Json.JsonConvert.SerializeObject(errcode);
            String Lg_Token = Tokenmodel.Tokenid;
            String Lg_Parameter =   _par;// (Request.Method == "POST") ? _par : context.HttpContext.Request.QueryString.Value;
            String Lg_ip = Tokenmodel.DeviceID.IsDBNull() ? "" : Tokenmodel.DeviceID;
            String Lg_Bak = "";
            Int32 Lg_state = Tools.GetEnumInt<ErrcodeType>(errcode.errcodeType.ToString());
            String Lg_Version = "2.0";
            String Lg_Platform = Tokenmodel.Platform.IsDBNull() ? "" : Tokenmodel.Platform;
            String Lg_Area = "����";
            double Lg_TimeOut = (DateTime.Now - dt_S).TotalMilliseconds;

            bus.apiLog.Add(Lg_Url, Lg_Type, Lg_Title, Lg_Text, DateTime.Now, Lg_Token, Lg_Parameter, Lg_ip, Lg_Bak, Lg_state, Lg_Version, Lg_Platform, Lg_Area, Lg_TimeOut);

        }
      
        
        /// <summary>ִ�к�
        /// ��Ҫ��ִ�еĽ����,����Ĳ���,�ýӿڷ��ʵĺ�ʱ.��¼���ӿ��ռ���.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        { 
            string _par = context.HttpContext.Request.QueryString.Value;//GET�ķ���
             
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
            String Lg_Type = "ҳ�����";
            String Lg_Title = string.Format("{0}/{1}", controller, action);
            String Lg_Text = Newtonsoft.Json.JsonConvert.SerializeObject(errcode);
            String Lg_Token = Tokenmodel.Tokenid;
            String Lg_Parameter = _par;// (Request.Method == "POST") ? _par : context.HttpContext.Request.QueryString.Value;
            String Lg_ip = Tokenmodel.DeviceID.IsDBNull()? "": Tokenmodel.DeviceID;
            String Lg_Bak = "";
            Int32 Lg_state = Tools.GetEnumInt<ErrcodeType>(errcode.errcodeType.ToString());
            String Lg_Version = "2.0";
            String Lg_Platform = Tokenmodel.Platform.IsDBNull() ? "" : Tokenmodel.Platform ;
            String Lg_Area = "����";
            double Lg_TimeOut = (DateTime.Now - dt_S).TotalMilliseconds;

             bus.apiLog.Add(Lg_Url, Lg_Type, Lg_Title, Lg_Text, DateTime.Now, Lg_Token, Lg_Parameter, Lg_ip, Lg_Bak, Lg_state, Lg_Version, Lg_Platform, Lg_Area, Lg_TimeOut); 
        }

        #region Ǩ�Ƶ�bus����

        /// <summary>ͳһ���ؼ��� 
        /// </summary>
        /// <returns></returns>.
        protected Errcode ResultSet(ErrcodeType er, string Mess, DataMode da)
        {

            return ResultSet(er, true, Mess, da);
        }
        /// <summary>ͳһ���ؼ��� 
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

        /// <summary>�̼���Ȩ��֤�� 
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {


          //  throw new NotImplementedException();
        }
    }


    
   
}