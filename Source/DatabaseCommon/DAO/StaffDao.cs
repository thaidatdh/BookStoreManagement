using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class StaffDao
   {
      public static List<StaffDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<StaffDto>("SELECT S.*, U.* from STAFF S, USERS U WHERE S.USER_ID = U.USER_ID");
      }
      public static int Insert(StaffDto dto)
      {
         int UserId = DatabaseUtils.InsertEntity<UserDto>(dto);
         dto.UserId = UserId;
         return DatabaseUtils.InsertEntity<StaffDto>(dto);
      }
      public static bool Update(StaffDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto) > 0 && DatabaseUtils.UpdateEntity<StaffDto>(dto) > 0;
      }
   }
}
