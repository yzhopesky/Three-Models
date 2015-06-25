using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BookShop.Model;

namespace BookShop.DAL
{
    /// <summary>
    /// 负责创出EF上下文对象，保证线程内唯一，在一次请求内只创建一次该对象
    /// </summary>
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new book_shop3Entities();
                CallContext.SetData("dbContext", dbContext);
                return dbContext;
            }
            return dbContext;
        }
    }
}

