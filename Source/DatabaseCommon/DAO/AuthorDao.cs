using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class AuthorDao : GenericDao<AuthorDto>
   {
      public static List<AuthorDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<AuthorDto>();
      }
      public static int Insert(AuthorDto dto)
      {
         return DatabaseUtils.InsertEntity<AuthorDto>(dto, true);
      }
      public static bool Update(AuthorDto dto)
      {
         return DatabaseUtils.UpdateEntity<AuthorDto>(dto, true) > 0;
      }
      public static AuthorDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<AuthorDto>(Id);
      }
   }
}
