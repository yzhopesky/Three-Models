using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;


namespace BookShop.IDAL
{
    public interface IDBSession
    {
        IUserInfoDAL userInfoDAL { get; set; }
        DbContext Db { get; }
        int ExecuteSql(string sql, params SqlParameter[] paras);
        int SaveChanges();
    }
}
