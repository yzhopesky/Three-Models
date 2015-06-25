using BookShop.IBLL;
using BookShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDAL()
        {
            CurrentDAL = this.DbSession.userInfoDAL;
        }
        /// <summary>
        /// 批量删除用户的数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var users = this.DbSession.userInfoDAL.LoadEntities(c => list.Contains(c.ID));
            foreach (UserInfo userInfo in users)
            {
                this.DbSession.userInfoDAL.DeleteEntity(userInfo);
            }
            return this.DbSession.SaveChanges() > 0;
        }
    }
}
