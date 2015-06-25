using BookShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.WebApp.Controllers
{
    public class UserInfoController : Controller
    {
        //
        // GET: /UserInfo/
        IBLL.IUserInfoService userInfoService = new BLL.UserInfoService();
        public ActionResult Index()
        {
            //IBLL.IUserInfoService userInfoService = new BLL.UserInfoService();//Spring.net进行解耦
            //var users = userInfoService.LoadEntities(c => true);
            //ViewData.Model = users;
            return View();
        }
        /// <summary>
        /// 获取用户的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfo()
        {
            int pageIndex;
            int pageSize;
            if (!int.TryParse(Request["page"], out pageIndex))
            {
                pageIndex = 1;
            }
            if (!int.TryParse(Request["rows"], out pageSize))
            {
                pageSize = 5;
            }
            int totalCount;
            var users = userInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, c => true, c => c.ID, true);
            var rows = from u in users
                       select new { ID = u.ID, UserName = u.UserName, UserPass = u.UserPass, Email = u.Email, RegTime = u.RegTime };

            return Json(new { total = totalCount, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除用户的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            foreach (string id in strIds)
            {
                list.Add(Convert.ToInt32(id));
            }
            if (userInfoService.DeleteEntities(list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AddInfo(UserInfo userInfo)
        {
            userInfo.RegTime = DateTime.Now;
            userInfoService.AddEntity(userInfo);
            return Content("ok");
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ActionResult EditInfo(UserInfo userInfo)
        {
            userInfoService.UpdateEntity(userInfo);
            return Content("ok");
        }
    }
}
