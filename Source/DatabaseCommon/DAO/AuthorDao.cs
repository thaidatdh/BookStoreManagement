using CommonLibrary.Utils;
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
      private static Dictionary<int, AuthorDto> AuthorIdMap = new Dictionary<int, AuthorDto>();
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
         if (AuthorIdMap.ContainsKey(Id))
         {
            return AuthorIdMap.GetValue(Id);
         }
         AuthorDto dto = DatabaseUtils.GetEntity<AuthorDto>(Id);
         AuthorIdMap[Id] = dto;
         return dto;
      }
      public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE AUTHOR SET IS_DELETED = 1 WHERE AUTHOR_ID=" + Id) > 0;
      }
   }
}
