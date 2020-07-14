using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BookStoreManagement.Utils;

namespace BookStoreManagement.BUS
{
   public class FeatureAttributeService
   {
      private static Dictionary<int, string> FeaturePropertiesMap = new Dictionary<int, string>();
      private static Dictionary<string, int> FeaturePropertiesNameMap = new Dictionary<string, int>();
      private static Dictionary<string, List<int>> AuthorizationMap = new Dictionary<string, List<int>>();
      private static List<FeatureAttribute> ListFeatureAttribute = new List<FeatureAttribute>();
      public static Dictionary<int, string> GetFeaturePropertiesMap()
      {
         if (FeaturePropertiesMap == null || FeaturePropertiesMap.Count == 0)
            InitFeatureProperties();
         return FeaturePropertiesMap;
      }
      public static Dictionary<string, int> GetFeaturePropertiesNameMap()
      {
         if (FeaturePropertiesNameMap == null || FeaturePropertiesNameMap.Count == 0)
            InitFeatureProperties();
         return FeaturePropertiesNameMap;
      }
      public static List<FeatureAttribute> GetListFeatureAttribute()
      {
         if (ListFeatureAttribute == null || ListFeatureAttribute.Count == 0)
            InitFeatureProperties();
         return ListFeatureAttribute;
      }
      public static Dictionary<string, List<int>> GetAuthorizationMap()
      {
         if (AuthorizationMap == null || AuthorizationMap.Count == 0)
            AuthorizationMap = DefinitionDao.GetUserAuthorizationMap();
         return AuthorizationMap;
      }
      public static void InitFeatureProperties()
      {
         var Features = Assembly.GetExecutingAssembly().GetTypes().Where(n => n.IsClass && n.Namespace == "BookStoreManagement.UI").ToList();
         var list = new List<int>();
         foreach (var f in Features)
         {
            string name = "";
            int id = 0;
            if (f.IsDefined(typeof(FeatureAttribute), true))
            {
               var temp = ((FeatureAttribute[])f.GetCustomAttributes(typeof(FeatureAttribute), true)).FirstOrDefault();
               if (temp != null)
               {
                  id = temp.Id;
                  name = temp.Name;
                  ListFeatureAttribute.Add(temp);
               }
            }
               
            if (id > 0 && !String.IsNullOrEmpty(name))
            {
               FeaturePropertiesMap[id] = name;
               FeaturePropertiesNameMap[name] = id;
            }
         }
      }
      public static bool isAuthorized(string userType, int featureId)
      {
         if (FeaturePropertiesMap == null || FeaturePropertiesMap.Count == 0)
            InitFeatureProperties();
         if (AuthorizationMap == null || AuthorizationMap.Count == 0)
            AuthorizationMap = DefinitionDao.GetUserAuthorizationMap();
         //Check
         if (!AuthorizationMap.ContainsKey(userType))
            return false;
         return AuthorizationMap.GetValue(userType).Contains(featureId);
      }
   }
}
