using System;
using System.Security.Cryptography;
using System.Text;

namespace Tkx.Common
{
    /// <summary>安全类开
    ///  各种加密/解密的地方
    /// </summary>
  public class Safety
    {

        /// <summary>生成MD5的函数
        /// 
        /// </summary>
        /// <param name="value">加密明文</param>
        /// <returns></returns>
        public static string Md5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }




    }
}
