using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class PublisherDao : GenericDao<PublisherDto>
   {
      private static Dictionary<int, PublisherDto> PublisherIdMap = new Dictionary<int, PublisherDto>();
      public static List<PublisherDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<PublisherDto>();
      }
      public static int Insert(PublisherDto dto)
      {
         return DatabaseUtils.InsertEntity<PublisherDto>(dto, true);
      }
      public static bool Update(PublisherDto dto)
      {
         return DatabaseUtils.UpdateEntity<PublisherDto>(dto, true) > 0;
      }
      public static PublisherDto GetById(int Id)
      {
         if (PublisherIdMap.ContainsKey(Id))
         {
            return PublisherIdMap.GetValue(Id);
         }
         PublisherDto dto = DatabaseUtils.GetEntity<PublisherDto>(Id);
         PublisherIdMap[Id] = dto;
         return dto;
      }
      public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE PUBLISHER SET IS_DELETED = 1 WHERE PUBLISHER_ID=" + Id) > 0;
      }
   }
}
