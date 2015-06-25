using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.IDAL;
using System.Data.Entity;
using BookShop.DAL;
using BookShop.Model;

namespace BookShop.DALFactory
{
    public class DBSession : IDBSession
    {
        public DbContext Db { get { return DbContextFactory.GetCurrentDbContext(); } }
        private IUserInfoDAL _userInfoDAL;
        public IUserInfoDAL userInfoDAL
        {
            get
            {
                if (_userInfoDAL == null)
                {
                  // _userInfoDAL = new UserInfoDAL();//这里可以使用抽象工厂
                    _userInfoDAL = AbstractFactory.CreateUserInfoDAL();
                }
                return _userInfoDAL;
            }
            set
            {
                _userInfoDAL = value;
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, params System.Data.SqlClient.SqlParameter[] paras)
        {
            return Db.Database.ExecuteSqlCommand(sql, paras);
        }
        /// <summary>
        /// 一次的业务可能有多张表的更改和操作，先将数据保存到上下文中去 最后在调用该方法
        /// 一次性把数据保存到数据库中去 避免多次连接数据库
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Db.SaveChanges();
        }
    }
}
