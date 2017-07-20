 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tkx.Model
{
    /// <summary>地理信息
    /// 
    /// </summary>
    public class AreaModel
    {
        /// <summary>   主键 
        /// </summary> 
        public Int32 Id
        { get; set; }


        /// <summary>   父id 
        /// </summary> 
        public Int32 ParentId
        { get; set; }


        /// <summary>  简称  
        /// </summary> 
        public String ShortName
        { get; set; }


        /// <summary>    全称
        /// </summary> 
        public String Name
        { get; set; }
        /// <summary>   层级 0 1 2 省市区县 
        /// </summary> 
        public Byte Level
        { get; set; }

        /// <summary>   拼音 
        /// </summary> 
        public String Letter
        { get; set; }


        /// <summary>   长途区号 
        /// </summary> 
        public String Code
        { get; set; }


        /// <summary>   邮编 
        /// </summary> 
        public String ZipCode
        { get; set; }


        /// <summary>   首字母 
        /// </summary> 
        public String Initial
        { get; set; }


        /// <summary>   经度 
        /// </summary> 
        public Decimal Lng
        { get; set; }


        /// <summary>   纬度 
        /// </summary> 
        public Decimal Lat
        { get; set; }

    }
}