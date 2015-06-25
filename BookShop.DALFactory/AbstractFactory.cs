using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.IDAL;
using System.Reflection;

namespace BookShop.DALFactory
{
    public class AbstractFactory
    {
        /// <summary>
        /// 通过反射的方式创建数据操作类的实例
        /// </summary>
        private readonly static string NameSpace = ConfigurationManager.AppSettings["NameSpace"];
        private readonly static string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        public static IUserInfoDAL CreateUserInfoDAL()
        {
            string fullNameSpace = NameSpace + ".UserInfoDAL";
            return CreateInstance(fullNameSpace, AssemblyPath) as IUserInfoDAL;
        }
        private static object CreateInstance(string fullNameSpace, string assemblyPath)
        {
            var assembly = Assembly.Load(assemblyPath);
            return assembly.CreateInstance(fullNameSpace);
        }
    }
}
