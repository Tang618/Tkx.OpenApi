using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tkx.Common
{
    /// <summary>封装API请求
    /// 
    /// </summary>
    public static class AjaxJson
    {

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Encoding encoding = null)
        {
            HttpClient httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(url);
            var ret = encoding.GetString(data);
            return ret;
        }
        /// <summary>
        /// Http Get 同步方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding = null)
        { 
            HttpClient httpClient = new HttpClient(); 
            var t = httpClient.GetByteArrayAsync(url);
            t.Wait();
            var ret = encoding.GetString(t.Result);
            return ret;
        } 
        /// <summary>
        /// POST 异步
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStream"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string Tokenid, string Platform, string version, string CreateTime, string UserTy, string DeviceID, string DataKey, Dictionary<string, string> formData = null)
        {

            try
            {
                HttpClientHandler handler = new HttpClientHandler();

                HttpClient client = new HttpClient(handler);
                MemoryStream ms = new MemoryStream();
                formData.FillFormDataStream(ms);//填充formData
                HttpContent hc = new StreamContent(ms);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                hc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");//坑呀.就是没设置响应类型
                hc.Headers.Add("Tokenid", Tokenid);
                hc.Headers.Add("Platform", Platform);
                hc.Headers.Add("version", version);
                hc.Headers.Add("CreateTime", CreateTime);
                hc.Headers.Add("UserTy", UserTy);
                hc.Headers.Add("DeviceID", DeviceID);
                hc.Headers.Add("DataKey", DataKey);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
                hc.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");




                var r = await client.PostAsync(url, hc);
                byte[] tmp = await r.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(tmp);
            }
            catch (Exception e)
            {

               
            }

            return "";
        }
         
        /// <summary>
        /// POST 同步
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStream"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpPost(string url,string Tokenid,string Platform,string version,string CreateTime, string UserTy,string DeviceID,string DataKey, Dictionary<string, string> formData = null)
        {

            HttpClientHandler handler = new HttpClientHandler();
      
            HttpClient client = new HttpClient(handler);
            MemoryStream ms = new MemoryStream();
            formData.FillFormDataStream(ms);//填充formData
            HttpContent hc = new StreamContent(ms);


            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));//application/x-www-form-urlencoded; charset=UTF-8

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8)); 

            // hc.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            hc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");//坑呀.就是没设置响应类型
            hc.Headers.Add("Tokenid", Tokenid);
            hc.Headers.Add("Platform", Platform);
            hc.Headers.Add("Version", version);
            hc.Headers.Add("CreateTime", CreateTime);
            hc.Headers.Add("UserTy", UserTy);
            hc.Headers.Add("DeviceID", DeviceID);
            hc.Headers.Add("DataKey", DataKey);




            var t = client.PostAsync(url, hc);
         
            t.Wait();
            var t2 = t.Result.Content.ReadAsByteArrayAsync();

            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(t2.Result);
        } 
        /// <summary>把a=1&b=2&c=3封装成List
        /// /
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryList(string formData)
        {
            Dictionary<string, string> li = new Dictionary<string, string>();
            string[] par = formData.Split('&');
            for (int i = 0; i < par.Length; i++)
            {
                string[] n = par[i].Split('=');
                li.Add(n[0].ToString(), n[1].ToString());
            }
            return li;
        } 
        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }
        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        } 
    }



}
