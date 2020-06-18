using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class PublisherDao
   {
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
         return DatabaseUtils.GetEntity<PublisherDto>(Id);
      }
   }
}
