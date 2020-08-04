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
      private static List<ProviderDto> allNotDeletedProviders = new List<ProviderDto>();
      public static List<ProviderDto> GetAllNotDeletedProviders()
      {
         if (allNotDeletedProviders == null || allNotDeletedProviders.Count == 0)
         {
            allNotDeletedProviders = ProviderDao.Where(n => n.IsDeleted == false).ToList();
         }
         return allNotDeletedProviders;
      }

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

      public static bool Update(ProviderDto dto)
      {

         ProviderDto oldDto = allNotDeletedProviders.FirstOrDefault(n => n.ProviderId == dto.ProviderId);
         bool result = ProviderDao.Update(dto);
         if (result)
         {
            if (oldDto != null)
               allNotDeletedProviders.Remove(oldDto);
            allNotDeletedProviders.Add(dto);
            allNotDeletedProviders = allNotDeletedProviders.OrderBy(n => n.ProviderId).ToList();
         }
         return result;

      }
      public static int Insert(ProviderDto dto)
      {
         int id = ProviderDao.Insert(dto);
         dto.ProviderId = id;
         allNotDeletedProviders.Add(dto);
         allNotDeletedProviders = allNotDeletedProviders.OrderByDescending(n => n.ProviderId).ToList();
         return id;
      }
      public static bool Delete(ProviderDto dto)
      {
         allNotDeletedProviders.Remove(dto);
         return ProviderDao.Delete(dto.ProviderId);
      }
   }
}
