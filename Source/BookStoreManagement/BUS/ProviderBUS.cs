using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   public class ProviderBUS
   {
      private static List<ProviderDto> providerList = new List<ProviderDto>();
      public static List<ProviderDto> GetProviderList()
      {
         if (providerList == null || providerList.Count == 0)
         {
            providerList = ProviderDao.Where(n => n.IsDeleted == false).ToList();
         }
         return providerList;
      }
      public static List<string> GetProviderNameList()
      {
         if (providerList == null || providerList.Count == 0)
         {
            providerList = ProviderDao.Where(n => n.IsDeleted == false).ToList();
         }
         return providerList.Select(n => n.Name).ToList();
      }
      public static int GetPublisherId(string name)
      {
         if (providerList == null || providerList.Count == 0)
         {
            providerList = ProviderDao.Where(n => n.IsDeleted == false).ToList();
         }
         ProviderDto dto = providerList.FirstOrDefault(n => n.Name.Equals(name));
         if (dto == null)
            dto = ProviderDao.Where(n => n.Name.Equals(name)).ToList().FirstOrDefault();
         if (dto == null)
            return 0;
         else
            return dto.ProviderId;
      }
   }
}
