using System;
using System.Collections.Generic;
using System.Text;

namespace Tkx.DAL
{
   public abstract class BaseDal
    {
        public Tkx.DbBase.SqlDbHelper db = new DbBase.SqlDbHelper("Data Source=120.76.26.199;User ID=sa;Password=runmingdb6688,./;Initial Catalog=parking;Pooling=true");
    }

    public enum FunReturn
    { 成功,失败}
}
