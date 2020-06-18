using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class ProviderDao
   {
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
         return DatabaseUtils.GetEntity<ProviderDto>(Id);
      }
   }
}
