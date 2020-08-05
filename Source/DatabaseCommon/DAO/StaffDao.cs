using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class StaffDao : GenericDao<StaffDto>
   {
      public static List<StaffDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<StaffDto>("SELECT S.*, U.* from STAFF S, USERS U WHERE S.USER_ID = U.USER_ID");
      }
      public static int Insert(StaffDto dto)
      {
         dto.UserType = Const.CONST.USERS.USER_TYPE_STAFF;
         int UserId = DatabaseUtils.InsertEntity<UserDto>(dto, true);
         dto.UserId = UserId;
         return DatabaseUtils.InsertEntity<StaffDto>(dto);
      }
      public static bool Update(StaffDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto, true) > 0 && DatabaseUtils.UpdateEntity<StaffDto>(dto) > 0;
      }
      public static bool DeActive(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE STAFF SET ACTIVE = 0 WHERE STAFF_ID=" + Id) > 0;
      }
   }
}
