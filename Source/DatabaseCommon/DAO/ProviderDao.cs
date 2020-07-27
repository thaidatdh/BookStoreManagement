using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class ProviderDao : GenericDao<ProviderDto>
   {
      private static Dictionary<int, ProviderDto> ProviderIdMap = new Dictionary<int, ProviderDto>();
      public static List<ProviderDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<ProviderDto>();
      }
      public static int Insert(ProviderDto dto)
      {
         return DatabaseUtils.InsertEntity<ProviderDto>(dto, true);
      }
      public static bool Update(ProviderDto dto)
      {
         return DatabaseUtils.UpdateEntity<ProviderDto>(dto, true) > 0;
      }
      public static ProviderDto GetById(int Id)
      {
         if (ProviderIdMap.ContainsKey(Id))
         {
            return ProviderIdMap.GetValue(Id);
         }
         ProviderDto dto = DatabaseUtils.GetEntity<ProviderDto>(Id);
         ProviderIdMap[Id] = dto;
         return dto;
      }
      public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE PROVIDER SET IS_DELETED = 1 WHERE PROVIDER_ID=" + Id) > 0;
      }
   }
}
