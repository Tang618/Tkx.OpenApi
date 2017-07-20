using System;
using System.Collections.Generic;
using System.Text;

namespace Tkx.Model
{
    public class ApiLogModel
    {
        /// <summary>   编号 
     /// </summary>
        public Int32 ap_Id
        { get; set; }


        /// <summary>   接口名 
        /// </summary>
        public String ap_name
        { get; set; }


        /// <summary>   统计 
        /// </summary>
        public Int32 ap_count
        { get; set; }


        /// <summary>   日期 
        /// </summary>
        public DateTime ap_time
        { get; set; }


        /// <summary>   接口访问当天最大访问量 
        /// </summary>
        public Int32 ap_maxcount
        { get; set; }
    }

   
}
