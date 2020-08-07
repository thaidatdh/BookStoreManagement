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
      public static StaffDto GetById(int id)
      {
         return DatabaseUtils.GetEntity<StaffDto>("SELECT S.*, U.* from STAFF S, USERS U WHERE S.USER_ID = U.USER_ID AND S.STAFF_ID=" + id);
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

      public static bool updatePassword(int id, string password)
      {
            return DatabaseUtils.ExecuteQuery("Update Staff Set password='" + password + "' Where Staff_id=" + id) > 0;
      }

      public static bool delete(int userID)
        {
            bool deleteStaff = DatabaseUtils.ExecuteQuery("Delete Staff Where User_id=" + userID) > 0;
            bool deleteUser = DatabaseUtils.ExecuteQuery("Delete Users Where User_id=" + userID) > 0;
            return deleteStaff && deleteUser;
        }
   }
}
