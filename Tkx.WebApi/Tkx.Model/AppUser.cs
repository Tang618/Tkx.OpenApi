using System;
using System.Collections.Generic;
using System.Text;

namespace Tkx.Model
{
    /// <summary>
    /// 第三方厂商ID集合地方
    /// </summary>
   public class AppUser
    {
        /// <summary>登陆ID
        /// 
        /// </summary>
        public string Appid { get; set; }

        /// <summary>登陆密码
        /// 
        /// </summary>
        public string AppKey { get; set; }

        /// <summary> 角色权限组,允许能访问的接口名称
        /// </summary>
        public string[] AppRole { get; set; }
        /// <summary>私钥(用于参数加密使用)
        /// 
        /// </summary>
        public string Key { get; set; }
    }
}
