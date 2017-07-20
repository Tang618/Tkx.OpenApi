namespace Tkx.Model
{
    /// <summary>统一返回代码定义点
    /// 
    /// </summary>
    public  enum ErrcodeType
    {
        系统繁忙 = -1,/*用户连接超出我设置上限时给的返回值*/
        请求成功 = 0,
        令牌无效 = 7000,/*令牌每15分钟清空一次,如超时或失效时,返回值*/
        终端未识别 = 7001,/*我们约定N个终端代码,超出无效*/
        参数无效 = 7002,
        类型无效 = 7003,/* 简单参数*/
        版本太低 = 7004,
        用户超时 = 7005, /*用户登陆后产生一个票据,如果在别的设备登陆后,这个票据会被清除.就会返回这个代码 */
        安全校验码无效 = 7006, /*校验码无效*/
        设备码 = 7008, /*校验码无效*/
        时间差 = 7010, /*校验码无效*/


        /*8000 ~ 9000 车场*/
        

        数据读写失败 = 8888,/*数据校验通过了.但插入数据库时失败了*/
        资源关闭无效 = 7777,/*资源关闭*/
        业务处理无效 = 9999 /*业务层上的BUG*/
    }

    /// <summary>返回类
    /// 
    /// </summary>
    public class Errcode
    {
        public ErrcodeType errcodeType { get; set; }
        public string Mess { get; set; }
        public bool isShow { get; set; }


        public DataMode Data { get; set; }
    }
    /// <summary>令牌信息
    /// 
    /// </summary>
    public class TokenModel
    {
        /// <summary>令牌
        /// 
        /// </summary>
        public string Tokenid { get; set; }
        /// <summary>平台
        /// 
        /// </summary>
        public string Platform { get; set; }
        /// <summary>版本
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>时间戳
        /// 
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>用户信息预留
        /// 
        /// </summary>
        public string UserTy { get; set; }

        /// <summary>设备码或IP地址
        /// 
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>校验码 
        /// 
        /// </summary>
        public string DataKey { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DataMode
    {
        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
            set
            {
                if (value == 0)
                {
                    pageCount = 0;
                }
                else
                {
                    pageCount = value;

                }
            }
        }
        private object list;
        public object List
        {
            get
            {
                if (list == null)
                { list = new string[0]; }

                return list;

            }
            set
            {
                if (value == null)
                {
                    list = new string[0];

                }
                else
                {
                    list = value;
                }
            }
        }
    }
}