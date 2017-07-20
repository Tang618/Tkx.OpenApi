using System;
using System.Collections.Generic;

namespace Tkx.DbBase
{

    /// <summary>目前我发现用List<Dictionary<string, object>>()这个更简洁代替掉tablse
    /// 1.1里面没有tables所以自己封装一个.为了区别以后2.0的版本.我特意叫TangDataTable
    /// 在core下面的datatTable 
    /// </summary>
    public class TangDataTable
    { 
        /// <summary>
      /// 这个相当于dt.Rows.Count
      /// </summary>
        public int TotalCount { get; set; }

        public List<TangDataColumn> Columns { get; set; } = new List<TangDataColumn>();

        public List<TangDataRow> Rows { get; set; } = new List<TangDataRow>();

        public TangDataColumn[] PrimaryKey { get; set; }

        public TangDataRow NewRow()
        {
             
            return new TangDataRow(this.Columns, new object[Columns.Count]);
        }
        /// <summary>为了返回值补充的.不知道为什么我填充后只有列名跟列的类型.
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Return(TangDataTable dt)
        {
            List<Dictionary<string, object>> _list = new List<Dictionary<string, object>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, object> _rows = new Dictionary<string, object>();
                foreach (var item in dt.Columns)
                {
                    var n = item.ColumnName;
                    var v = dt.Rows[i][n];
                    _rows.Add(n, v);
                }
                _list.Add(_rows);

            }
            return _list;
        }



    }
    public class TangDataColumn
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        
    }
    public class TangDataRow
    {
        private object[] _ItemArray;
        public List<TangDataColumn> Columns { get; private set; }
        public TangDataRow(List<TangDataColumn> columns, object[] itemArray)
        {
            this.Columns = columns;
            this._ItemArray = itemArray;
        }
        public object this[int index]
        {
            get { return _ItemArray[index]; }
            set { _ItemArray[index] = value; }
        }
        public object this[string columnName]
        {
            get
            {
                int i = 0;
                foreach (TangDataColumn column in Columns)
                {
                    if (column.ColumnName == columnName)
                        break;
                    i++;
                }
                return _ItemArray[i];
            }
            set
            {
                int i = 0;
                foreach (TangDataColumn column in Columns)
                {
                    if (column.ColumnName == columnName)
                        break;
                    i++;
                }
                _ItemArray[i] = value;
            }
        }
    }
}