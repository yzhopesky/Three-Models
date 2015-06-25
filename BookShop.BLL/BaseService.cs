using BookShop.DALFactory;
using BookShop.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL
{
    public abstract class BaseService<T> where T : class,new()
    {
        public IDBSession DbSession
        {
            get { return DBSessionFactory.CreateDbSession(); }
        }
        public IBaseDAL<T> CurrentDAL { get; set; }//表示当前数据操作类的实例
        public abstract void SetCurrentDAL();//定义一个抽象的方法
        public BaseService()
        {
            SetCurrentDAL();//保证子类重写父类中的抽象方法
        }
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)//查询语句
        {
            return this.CurrentDAL.LoadEntities(whereLambda);
        }
        /// <summary>
        /// 分页语句
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderByLambda, bool isAsc)
        {
            return this.CurrentDAL.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, whereLambda, orderByLambda, isAsc);
        }
        /// <summary>
        /// 添加语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            this.CurrentDAL.AddEntity(entity);
            this.DbSession.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 删除语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            this.CurrentDAL.DeleteEntity(entity);
            return this.DbSession.SaveChanges() > 0;
        }
        /// <summary>
        /// 更新语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity)
        {
            this.CurrentDAL.UpdateEntity(entity);
            return this.DbSession.SaveChanges() > 0;
        }
    }
}
