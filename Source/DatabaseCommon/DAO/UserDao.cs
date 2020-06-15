using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class UserDao
   {
      public static List<UserDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<UserDto>();
      }
      public static int Insert(UserDto dto)
      {
         return DatabaseUtils.InsertEntity<UserDto>(dto);
      }
      public static bool Update(UserDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto) > 0;
      }
      public static UserDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<UserDto>(Id);
      }

   }
}
