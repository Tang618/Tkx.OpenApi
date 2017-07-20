using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tkx.Model;
using Tkx.Common;
using System.Net;
using System.Text;
using System.IO;

namespace Tkx.WebApi.Controllers
{
    public class HomeController : Controller
    {




       static string url = "";

        public HomeController()
        {
         
        }

        string _mess = "";
        public IActionResult Index()
        {
         var g=   Convert.ToDateTime("2017/07/18 20:40:09");
            return View();
        }


      
        public Tkx.Library.Bus bus = new Library.Bus();


        /// <summary>用于调用使用
        /// </summary>
        /// <returns></returns>
        public Errcode NotTokeid(int id)
        { 
            Errcode err = new Errcode();
            err.errcodeType = (ErrcodeType)id;
            err.isShow = true;
            err.Mess = "";
            err.Data = null;
            return err;
        }


        public IActionResult Error()
        {
            return View();
        }

        /// <summary>获取令牌
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
           _mess = AjaxJson.HttpGet(string.Format("{0}/api/bus/tokenID?appid=and&appkey=cf0cf1b9b5de40c5b7b72d0b349d577d",url), System.Text.Encoding.UTF8);



            Errcode err = Newtonsoft.Json.JsonConvert.DeserializeObject<Errcode>(_mess);
            _mess = err.Mess;

            return _mess;
        }
         
        /// <summary>加密测试
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateKey()
        {
            string Tokenid = Request.Form["Tokenid"].ToString();
            string nPar = Request.Form["parm"].ToString();
            string dataKey = "";
            bus.IsKey(nPar, Tokenid, out dataKey);
            return dataKey;
        }
    
        /// <summary>模拟提交
        /// 
        /// </summary>
        /// <returns></returns>
        public string Tpost()
        {
            url = string.Format("http://{0}", Request.Host.Value);

            string Tokenid = Request.Form["Tokenid"].ToString();
            string nPar = Request.Form["parm"].ToString(); 
            url = url + "/api" + Request.Form["name"];
            string dataKey;
          string _key=  bus.IsKey(nPar, Tokenid, out dataKey);
          //  string parm = nPar + "&dataKey=" + _key;
            string CreateTime = Tools.Time();

            _mess = AjaxJson.HttpPost(url, Tokenid, "and", "2.0", CreateTime, "0", "12.1.2.1", _key, AjaxJson.GetQueryList(nPar)); 
            return _mess; 
        }
         
        /// <summary>后期树的数据
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TreeDataFormatModel> TreeData()
        {


            List<TreeDataFormatModel> li = new List<TreeDataFormatModel>();

            TreeDataFormatModel T = new TreeDataFormatModel();
           


            T = new TreeDataFormatModel();
            T.id = 2;
            T.text = "所有分类";
           // T.state = TreeState.open.ToString();
            T.attributes = " ";

          //  T.children = new List<TreeDataFormatModel>();

            //T.children = new List<TreeDataFormatModel>() {
            //    new TreeDataFormatModel() { id = 20, text = "公共模块Bus", attributes = " " },
            //    new TreeDataFormatModel() { id = 21, text = "会员Account", attributes = " " },
            //    new TreeDataFormatModel() { id = 22, text = "医院Hospital", attributes = " " },
            //};

            li.Add(T);


            //JsonResult js = new JsonResult();
     
            //js.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return li;
        }
    }
}
