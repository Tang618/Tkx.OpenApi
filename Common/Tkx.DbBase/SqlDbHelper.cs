using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Tkx.DbBase
{
    /// <summary>
    /// 微软提供的,原始版(有改装.需要用到2015分页存储过程)
    /// </summary>
    public class SqlDbHelper
    {


        //   public static SqlDbHelper sql = new SqlDbHelper();//2011-7-29 增加的,



        //设置数据访问层的异常保存路径
        //     public DbBaseException DbExc = new DbBaseException(System.Configuration.ConfigurationManager.AppSettings["DbExeption"]);

        //已改进,需要在业务层配置.
        private string connectionString = "";  //System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["conn"]));
                                               // private string connectionString1 = "";// System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["conn1"]));

        private string LogNetAdds = "\\Error\\Db\\";//Error//pay/
        /// <summary>构造函数</summary>
        //public SqlDbHelper()
        //{
        //}
        /// <summary>构造函数</summary>
        /// <param name="conn">数据连接</param>
        public SqlDbHelper(string connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
            {
            Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层SqlDbHelper函数出现异常,构造发生异常,连接字符串为空:{1}", DateTime.Now, connectionstring), LogNetAdds);
                          }
            if(Conn==null)
            { 
            Conn = new SqlConnection(connectionstring);
            }


            connectionString = connectionstring;
        }
        private static SqlConnection Conn=null;
        private static SqlCommand cmd;



        public int ExecuteNonQuery(Dictionary<string, SqlParameter[]> sql)
        {
            foreach (var oo in sql)
            {
                var cmdText = oo.Key;//cmdTexts[i].ToString();//获取SQL指令,如果有异常该指令会被记录
                var par = oo.Value;
                return ExecuteNonQuery(cmdText, par);
            }

            return 0;

        }

        /// <summary> ExecuteNonQuery操作，对数据库进行 增、删、改 操作(（1）  </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns> </returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, CommandType.Text, null);
        }
        /// <summary>ExecuteNonQuery操作，对数据库进行 增、删、改 操作（2）
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sql, CommandType.Text, parameters);
        }
        /// <summary> ExecuteNonQuery操作，对数据库进行 增、删、改 操作（3）
        ///
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public int ExecuteNonQuery(string sql, CommandType commandType)
        {
            return ExecuteNonQuery(sql, commandType, null);
        }
        /// <summary>ExecuteNonQuery操作，对数据库进行 增、删、改 操作（4）
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns> </returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            try
            {
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = commandType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        connection.Open();
                        count = command.ExecuteNonQuery();


                    }
                }
                return count;
            }
            catch (SqlException exp)
            {
                //     //DbBaseException.MessBoxSqlBase("ExecuteNonQuery", sql, exp.Message);
                Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层ExecuteNonQuery函数出现异常,执行的SQL指令是:{1},报告的异常是:{2}", DateTime.Now, sql, exp.Message), LogNetAdds);

            }

            return 0;
        }



        /// <summary>执行多行语句,如果有出错就回滚[此函数只能支持执行数据例,添加,删除,修改.不支持查询]
        /// 
        /// </summary>
        /// <param name="ListCmdText">List[附带参数组][附带参数组]</param>
        /// <returns></returns>
        public int ExecuteNonQueryTrans(List<IDictionary<string, SqlParameter[]>> ListCmdText)
        {
            int icnt = 0;
            SqlParameter[] par;
            string cmdText = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                IDbTransaction _trans = conn.BeginTransaction();
                try
                {
                    foreach (var item in ListCmdText)
                    {

                        foreach (var oo in item)
                        {
                            cmdText = oo.Key;//cmdTexts[i].ToString();//获取SQL指令,如果有异常该指令会被记录
                            par = oo.Value;
                            cmd = new SqlCommand();
                            PrepareCommand(cmd, conn, _trans, CommandType.Text, cmdText, par);
                            icnt = icnt + cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                    }
                    _trans.Commit();

                }
                catch (SqlException exp)
                {
                    _trans.Rollback();
                    icnt = 0;//记数请零
                    conn.Close();

                    Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层ExecuteNonQueryTrans函数出现异常,执行的SQL指令是:{1},报告的异常是:{2}", DateTime.Now, cmdText, exp.Message), LogNetAdds);

                    //DbBaseException.MessBoxSqlBase("ExecuteNonQueryTrans", cmdText,"执行事务出现异常,批量回滚!  "+ exp.Message); 
                }

            }
            return icnt;
        }
        /// <summary>[作废不用了..上面的接口写的比较漂亮.]
        /// 
        /// </summary>
        /// <param name="cmdText">执行指令组</param>
        /// <param name="cmdParms">附带参数组</param>
        /// <returns></returns>
        public int ExecuteNonQueryTrans(string[] cmdTexts, List<SqlParameter[]> cmdParms)
        {
            int icnt = 0;
            SqlParameter[] par;
            string cmdText = "";

            //if (cmdParms.Count != cmdText.Length)//如果发现要执行的事务与参数组不相同时,不执行
            //{
            //    YanYun.Common.DbException.MessBoxSqlBase(string.Format("数据执行与参数组不匹配,无法正常执行事务;执行事务的cmdText总条数{0},参数组共{1}条,\r\n\n  cmdText={2} \n\n {3}",cmdText.Length,cmdParms.Count,cmdText.ToString()), "事务");
            //    return 0;
            //}//注释原因,如果提交过
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                IDbTransaction _trans = conn.BeginTransaction();
                try
                {

                    for (int i = 0; i < cmdTexts.Length; i++)
                    {
                        if (cmdParms.Count > i)//允许seleect * from tab where na='"+na+"'//这样的不带参数的进来
                        {
                            par = cmdParms[i];
                        }
                        else
                        {
                            par = null;
                        }

                        cmdText = cmdTexts[i].ToString();//获取SQL指令,如果有异常该指令会被记录
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, _trans, CommandType.Text, cmdText, par);
                        icnt = icnt + cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    _trans.Commit();

                }
                catch (Exception exp)
                {
                    _trans.Rollback();
                    icnt = 0;//记数请零
                    conn.Close();
                    Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层ExecuteNonQueryTrans函数出现异常,执行的SQL指令是:{1},报告的异常是:{2}", DateTime.Now, cmdText, exp.Message), LogNetAdds);  //DbBaseException.MessBoxSqlBase("ExecuteNonQueryTrans", cmdText,"执行事务出现异常,批量回滚!  "+ exp.Message); 
                }
            }
            return icnt;
        }

        private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        #region 暂时不支持的
        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（1）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <returns> </returns>
        //public DataSet ExecuteDataSet(string sql)
        //{
        //    return ExecuteDataSet(sql, CommandType.Text, null);
        //}
        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（2）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <returns> </returns>
        //public DataSet ExecuteDataSet(string sql, CommandType commandType)
        //{
        //    return ExecuteDataSet(sql, commandType, null);
        //}
        ///// <summary>SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（3）
        ///// 
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <param name="parameters">参数数组 </param>
        ///// <returns> </returns>
        //public DataSet ExecuteDataSet(string sql, CommandType commandType, SqlParameter[] parameters)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {


        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                command.CommandType = commandType;
        //                if (parameters != null)
        //                {
        //                    foreach (SqlParameter parameter in parameters)
        //                    {
        //                        command.Parameters.Add(parameter);
        //                    }
        //                }
        //                SqlDataAdapter adapter = new SqlDataAdapter(command);
        //                adapter.Fill(ds);
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        //DbBaseException.MessBoxSqlBase("ExecuteDataSet", sql, exp.Message);

        //        Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层ExecuteDataSet[242]函数出现异常,执行的SQL指令是:{1},报告的异常是:{2}", DateTime.Now, sql, exp.Message), LogNetAdds);

        //    }
        //    return ds;
        //}
        #endregion

        ///// <summary>SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（1）
        ///// 
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <returns> </returns>
        //public DataTable ExecuteDataTable(string sql)
        //{
        //    return ExecuteDataTable(sql, CommandType.Text, null);
        //}

        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（2）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="parameters">参数数组 </param>
        ///// <returns> </returns>
        //public DataTable ExecuteDataTable(string sql, SqlParameter[] parameters)
        //{
        //    return ExecuteDataTable(sql, CommandType.Text, parameters);
        //}
        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（2）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <returns> </returns>
        //public DataTable ExecuteDataTable(string sql, CommandType commandType)
        //{
        //    return ExecuteDataTable(sql, commandType, null);
        //}
        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（3）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <param name="parameters">参数数组 </param>
        ///// <returns> </returns>
        //public DataTable ExecuteDataTable(string sql, CommandType commandType, SqlParameter[] parameters)
        //{

        //    return ExecuteDataTable(sql, commandType, parameters, connectionString);
        //}
        ///// <summary> SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（3）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <param name="parameters">参数数组 </param>
        ///// <returns> </returns>
        //public DataTable ExecuteDataTable(string sql, CommandType commandType, SqlParameter[] parameters, string connString)
        //{
        //    DataTable data = new DataTable();
        //    try
        //    {


        //        using (SqlConnection connection = new SqlConnection(connString))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                command.CommandType = commandType;
        //                if (parameters != null)
        //                {
        //                    foreach (SqlParameter parameter in parameters)
        //                    {
        //                        command.Parameters.Add(parameter);
        //                    }
        //                }
        //                SqlDataAdapter adapter = new SqlDataAdapter(command);
        //                adapter.Fill(data);



        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        Tkx.Common.Tools.MessBox(string.Format("当前时间:{0},数据访问层ExecuteDataTable[323]函数出现异常,执行的SQL指令是:{1},报告的异常是:{2}", DateTime.Now, sql, exp.Message), LogNetAdds);

        //        //DbBaseException.MessBoxSqlBase("ExecuteDataTable", sql, exp.Message);
        //    }
        //    return data;
        //}

        ///// <summary> ExecuteReader执行一查询，返回一SqlDataReader对象实例（1）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <returns> </returns>
        //public IDataReader ExecuteReader(string sql)
        //{
        //    return ExecuteReader(sql, CommandType.Text, null);
        //}
        ///// <summary>ExecuteReader执行一查询，返回一SqlDataReader对象实例（2）
        ///// 
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //public IDataReader ExecuteReader(string sql, SqlParameter[] parameters)
        //{
        //    return ExecuteReader(sql, CommandType.Text, parameters);
        //}
        ///// <summary> ExecuteReader执行一查询，返回一SqlDataReader对象实例（3）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <returns> </returns>
        //public IDataReader ExecuteReader(string sql, CommandType commandType)
        //{
        //    return ExecuteReader(sql, commandType, null);
        //}
        ///// <summary> ExecuteReader执行一查询，返回一SqlDataReader对象实例（4）
        /////
        ///// </summary>
        ///// <param name="sql">要执行的SQL语句 </param>
        ///// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        ///// <param name="parameters">参数数组 </param>
        ///// <returns> </returns>
        //public IDataReader ExecuteReader(string sql, CommandType commandType, SqlParameter[] parameters)
        //{
        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        SqlCommand command = new SqlCommand(sql, connection);
        //        command.CommandType = commandType;
        //        if (parameters != null)
        //        {
        //            foreach (SqlParameter parameter in parameters)
        //            {
        //                command.Parameters.Add(parameter);
        //            }
        //        }
        //        connection.Open();
        //        return command.ExecuteReader(CommandBehavior.CloseConnection);
        //    }
        //    catch (Exception exp)
        //    {
        //        Tkx.Common.Tools.MessBox(string.Format("{0}函数:{1},代码行:{2},SQL指令:{3},异常信息:{4}", DateTime.Now, "ExecuteScalar", 375, sql, exp.Message), LogNetAdds);

        //        //DbBaseException.MessBoxSqlBase("ExecuteScalar", sql, exp.Message);
        //        throw;

        //    }

        //}


        /// <summary>ExecuteScalar执行一查询，返回查询结果的第一行第一列（1）
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, CommandType.Text, null);
        }
        /// <summary> ExecuteScalar执行一查询，返回查询结果的第一行第一列（2）
        ///
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql, CommandType commandType)
        {
            return ExecuteScalar(sql, commandType, null);
        }
        /// <summary>ExecuteScalar执行一查询，返回查询结果的第一行第一列（3）
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            object result = null;
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = commandType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        connection.Open();
                        result = command.ExecuteScalar();
                        if (cmd != null)
                        {
                            cmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Tkx.Common.Tools.MessBox(string.Format("{0}函数:{1},代码行:{2},SQL指令:{3},异常信息:{4}", DateTime.Now, "ExecuteScalar", 432, sql, exp.Message), LogNetAdds);


            }
            return result;
        }

        /// <summary>读取器
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, CommandType.Text, null);
        }



        /// <summary>读取器(获取单个对象使用,)
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns></returns>
        public T ExecuteReader<T>(string sql, CommandType commandType, SqlParameter[] parameters) where T : new()
        {
            T t = new T();
            string tempName = string.Empty;
            
            try
            { 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = commandType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        connection.Open();
                     


                        if (cmd != null)
                        {
                            cmd.Parameters.Clear();
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PropertyInfo[] propertys = t.GetType().GetProperties();

                                    foreach (PropertyInfo pi in propertys)
                                    {
                                        tempName = pi.Name;

                                        if (readerExists(reader, tempName))
                                        {
                                            if (!pi.CanWrite)
                                            {
                                                continue;
                                            }
                                            var value = reader[tempName];

                                            if (value != DBNull.Value)
                                            {
                                                pi.SetValue(t, value, null);
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception exp)
            {
                Tkx.Common.Tools.MessBox(string.Format("{0}函数:{1},代码行:{2},SQL指令:{3},异常信息:{4}", DateTime.Now, "ExecuteReader<T>", 432, sql, exp.Message), LogNetAdds);

               

            }

            return t;
        }

        /// <summary>读取器
        /// 
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader (string sql, CommandType commandType, SqlParameter[] parameters)
        { 
            return ExecuteReader(sql, commandType, parameters, connectionString);
        }
        /// <summary>封装读取器
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql, CommandType commandType, SqlParameter[] parameters,string connStr)
        {
            SqlDataReader dr;
            ///临时换一个新的数据库时使用
            SqlConnection _conn = new SqlConnection(connStr);
            try
            {
                cmd = new SqlCommand(sql, _conn);
                cmd.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                _conn.Open();
                //在执行该命令时，如果关闭关联的 DataReader 对象，则关联的 Connection 对象也将关闭。
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception exp)
            {
                Tkx.Common.Tools.MessBox(string.Format("{0}函数:{1},代码行:{2},SQL指令:{3},异常信息:{4}", DateTime.Now, "ExecuteReader", 432, sql, exp.Message), LogNetAdds);
            
                _conn.Close();
            }


            return null;
        }


        

        /// <summary>利用反射和泛型将SqlDataReader转换成List模型  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns></returns>
        public IList<T> ExecuteToList<T>(string sql, CommandType commandType, SqlParameter[] parameters) where T : new()

        {
            IList<T> list;

            Type type = typeof(T);

            string tempName = string.Empty;
            SqlConnection connection;
           
            try
            {
                using ( connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = commandType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        connection.Open();

                        using (SqlDataReader reader =command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                list = new List<T>();
                                while (reader.Read())
                                {
                                    T t = new T();

                                    PropertyInfo[] propertys = t.GetType().GetProperties();

                                    foreach (PropertyInfo pi in propertys)
                                    {
                                        tempName = pi.Name;

                                        if (readerExists(reader, tempName))
                                        {
                                            if (!pi.CanWrite)
                                            {
                                                continue;
                                            }
                                            var value = reader[tempName];

                                            if (value != DBNull.Value)
                                            {
                                                pi.SetValue(t, value, null);
                                            }

                                        }

                                    }

                                    list.Add(t);

                                }
                                return list;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
               
                Conn.Close();
            }
            return null;
        }


     

        /// <summary>  
        /// 判断SqlDataReader是否存在某列  
        /// </summary>  
        /// <param name="dr">SqlDataReader</param>  
        /// <param name="columnName">列名</param>  
        /// <returns></returns>  
        private bool readerExists(SqlDataReader dr, string columnName)
        {

            //   dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";



            return true;

          //  return (dr.GetSchemaTable().DefaultView.Count > 0);

        }


        /// <summary>>ExecuteScalar执行一查询，返回查询结果的第一行第一列,并强制类型转换（4）扩展
        /// 获取一字特定字段的返回值.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public T QueryOneValue<T>(string cmdText, SqlParameter[] commandParameters)
        {
            T local;

            try
            {
                object obj2 = ExecuteScalar(cmdText, CommandType.Text, commandParameters);
                if ((obj2 == null) || (obj2 == DBNull.Value))
                {
                    return default(T);
                }
                local = (T)Convert.ChangeType(obj2, typeof(T));
            }
            catch (Exception exp)
            {
                //DbBaseException.MessBoxSqlBase("QueryOneValue<T>", cmdText, exp.Message); 
                string obj1 = "";
                local = (T)Convert.ChangeType(obj1, typeof(T));

            }
            finally
            {

            }
            return local;

        }

        #region 分页通用方法 

        /// <summary>分页函数[因为有些数据需要用到关联表.原本的存储过程无法
        /// 
        /// </summary>
        /// <param name="sqlCmdText">SQL指令</param>
        /// <param name="ReFieldsStr">返回参数</param>
        /// <param name="Orderby">排序条件</param>
        /// <param name="pageNum">当前页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="RowCount">共几行</param>
        /// <returns></returns>
        public TangDataTable PageList_Text(string sqlCmdText, string ReFieldsStr, string Orderby, int pageNum, int pageSize, ref int RowCount)
        {
            RowCount = QueryOneValue<int>(string.Format("SELECT COUNT(*) FROM {0} ", sqlCmdText), null);
            int pgCnt = RowCount - pageSize * (pageNum - 1);
            if (pgCnt <= 0)//返回最后几行数据
            {
                //  return db.ExecuteDataTable(string.Format("SELECT  * FROM {0} ORDER BY {1} DESC", sqlCmdText, Orderby));

                return FillDataTable(ExecuteReader(string.Format("SELECT  * FROM {0} ORDER BY {1} DESC", sqlCmdText, Orderby)), pageNum, pageSize);
            }
            else
            {
                // return db.ExecuteDataTable(string.Format("SELECT TOP {2} * FROM (SELECT TOP {1} {3} FROM {0}  ORDER BY {4} ASC) cmts ORDER BY {4} DESC", sqlCmdText, pgCnt, pageSize, ReFieldsStr, Orderby));

                return FillDataTable(ExecuteReader(string.Format("SELECT TOP {2} * FROM (SELECT TOP {1} {3} FROM {0}  ORDER BY {4} ASC) cmts ORDER BY {4} DESC", sqlCmdText, pgCnt, pageSize, ReFieldsStr, Orderby)), pageNum, pageSize);
            }
        }
        /// <summary>TangDataTable
        /// 
        /// </summary>
        /// <param name="sqlCmdText">SQL指令</param>
        /// <param name="ReFieldsStr">返回参数</param>
        /// <param name="Orderby">排序条件</param>
        /// <param name="pageNum">当前页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="RowCount">共几行</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> PageList_List(string TableName, string ReFieldsStr, string OrderString, string WhereString, int PageSize, int PageIndex, ref int TotalRecord, string ConnstrionString)
        {
            #region 代码生成工具参数组

            SqlParameter[] commandParameters ={
         new SqlParameter("@TableName",SqlDbType.VarChar,50),
         new SqlParameter("@ReFieldsStr",SqlDbType.VarChar,600),
         new SqlParameter("@OrderString",SqlDbType.VarChar,200),
         new SqlParameter("@WhereString",SqlDbType.VarChar,200),
         new SqlParameter("@PageSize",SqlDbType.Int,4),
         new SqlParameter("@PageIndex",SqlDbType.Int,4),
               new SqlParameter("@TotalRecord",SqlDbType.Int,4)

    };
            commandParameters[0].Value = TableName;
            commandParameters[1].Value = ReFieldsStr;
            commandParameters[2].Value = OrderString;
            commandParameters[3].Value = WhereString;
            commandParameters[4].Value = PageSize;
            commandParameters[5].Value = PageIndex;
            commandParameters[6].Direction = ParameterDirection.Output;//注：存储过程中带output输出参数时．

            #endregion
            List<Dictionary<string, object>> dt = new List<Dictionary<string, object>>();
            if (ConnstrionString == "")
            {
                //  dt = db.ExecuteDataTable("procPageList", CommandType.StoredProcedure, commandParameters);
                dt = FillData(ExecuteReader("procPageList", CommandType.StoredProcedure, commandParameters), PageIndex, PageSize);

            }
            else
            {
                dt = FillData(ExecuteReader("procPageList", CommandType.StoredProcedure, commandParameters), PageIndex, PageSize);
                // dt = db.ExecuteDataTable("procPageList", CommandType.StoredProcedure, commandParameters, ConnstrionString);
            }
            // BaseBus.sql.QuerySQL(GetCmdSp("procPageList", commandParameters), 0);//注:procPageList存储过程的名称.
            if (commandParameters[6].Value.ToString() == "")
            { TotalRecord = 0; }
            else
            {
                TotalRecord = (int)commandParameters[6].Value;
            }

            return dt;
        }

        /// <summary>存储过程分页
        /// 
        /// </summary>
        /// <param name="TableName">表名或视图名</param>
        /// <param name="ReFieldsStr">字段名(全部字段为*)</param>
        /// <param name="OrderString">排序字段(必须!支持多字段不用加order by)</param>
        /// <param name="WhereString">条件语句(不用加where)</param>
        /// <param name="PageSize">每页多少条记录</param>
        /// <param name="PageIndex">指定当前为第几页</param>
        /// <param name="TotalRecord">返回总记录数</param>
        /// <param name="ConnstrionString">新的连接地址</param>
        /// <returns></returns>
        public TangDataTable PageList(string TableName, string ReFieldsStr, string OrderString, string WhereString, int PageSize, int PageIndex, ref int TotalRecord, string ConnstrionString)
        {

            #region 代码生成工具参数组

            SqlParameter[] commandParameters ={
         new SqlParameter("@TableName",SqlDbType.VarChar,50),
         new SqlParameter("@ReFieldsStr",SqlDbType.VarChar,600),
         new SqlParameter("@OrderString",SqlDbType.VarChar,200),
         new SqlParameter("@WhereString",SqlDbType.VarChar,200),
         new SqlParameter("@PageSize",SqlDbType.Int,4),
         new SqlParameter("@PageIndex",SqlDbType.Int,4),
               new SqlParameter("@TotalRecord",SqlDbType.Int,4)

    };
            commandParameters[0].Value = TableName;
            commandParameters[1].Value = ReFieldsStr;
            commandParameters[2].Value = OrderString;
            commandParameters[3].Value = WhereString;
            commandParameters[4].Value = PageSize;
            commandParameters[5].Value = PageIndex;
            commandParameters[6].Direction = ParameterDirection.Output;//注：存储过程中带output输出参数时．
            TangDataTable dt;
            #endregion
            if (ConnstrionString == "")
            {
                //  dt = db.ExecuteDataTable("procPageList", CommandType.StoredProcedure, commandParameters);
                dt = FillDataTable(ExecuteReader("procPageList", CommandType.StoredProcedure, commandParameters), PageIndex, PageSize);

            }
            else
            {
                dt = FillDataTable(ExecuteReader("procPageList", CommandType.StoredProcedure, commandParameters), PageIndex, PageSize);
                // dt = db.ExecuteDataTable("procPageList", CommandType.StoredProcedure, commandParameters, ConnstrionString);
            }
            // BaseBus.sql.QuerySQL(GetCmdSp("procPageList", commandParameters), 0);//注:procPageList存储过程的名称.
            if (commandParameters[6].Value.ToString() == "")
            { TotalRecord = 0; }
            else
            {
                TotalRecord = (int)commandParameters[6].Value;
            }

            return dt;
        }


        private List<Dictionary<string, object>> FillData(SqlDataReader reader, int pageIndex, int pageSize)
        {
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
            List<string> _Columns = new List<string>();
            bool defined = false;

            int index = 0;
            int beginIndex = pageSize * pageIndex;//10=1*10
            int endIndex = pageSize * (pageIndex + 1) - 1;//190=10*19
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                if (!defined)
                { 
                    for (int i = 0; i < reader.FieldCount; i++)
                    {

                        _Columns.Add(reader.GetName(i));

                        
                    }
                    defined = true;
                }


                ////这是个BUG   if (  index >= beginIndex  && index <= endIndex)
                if (index <= pageSize)
                {
                   int _c= reader.GetValues(values);
                    Dictionary<string, object> l = new Dictionary<string, object>();
                    for (int i=0;i<_Columns.Count;i++)
                    {
                       
                        l.Add(_Columns[i], values[i]);
                       

                    }

                    table.Add(l);
                    //  table.Rows.Add(new TangDataRow(table.Columns, values));


                }
                index++;



            }
            reader.Dispose();

            

            return table;
        }

        /// <summary>这个是用于内部填充的从读取器到dataTables里面
        /// 此例子是从网上抄取,以后在优化.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private TangDataTable FillDataTable(SqlDataReader reader, int pageIndex, int pageSize)
        {
            bool defined = false;
            TangDataTable table = new TangDataTable();
            int index = 0;
            int beginIndex = pageSize * pageIndex;//10=1*10
            int endIndex = pageSize * (pageIndex + 1) - 1;//190=10*19
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                if (!defined)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        TangDataColumn column = new TangDataColumn()
                        {
                            ColumnName = reader.GetName(i),
                            ColumnType = reader.GetFieldType(i)
                        };
                        table.Columns.Add(column);
                    }
                     defined = true;
                }


                ////这是个BUG   if (  index >= beginIndex  && index <= endIndex)
                if ( index <= pageSize)
                {
                    reader.GetValues(values);
                    table.Rows.Add(new TangDataRow(table.Columns, values));
                }
                index++;



            }
            reader.Dispose();
           
            table.TotalCount = index;
            return table;
        }
        #endregion



        #region 批量插入
        
        #endregion

        ///// <summary> 返回当前连接的数据库中所有由用户创建的数据库</summary>
        ///// <returns> </returns>
        //public DataTable GetTables()
        //{
        //    DataTable data = null;
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        data = connection.GetSchema("Tables");
        //    }
        //    return data;
        //}
    }
}
