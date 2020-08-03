using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class CustomerDao : GenericDao<CustomerDto>
   {
      public static List<CustomerDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<CustomerDto>("SELECT C.*, U.* from CUSTOMER C, USERS U WHERE C.USER_ID = U.USER_ID");
      }
      public static CustomerDto GetById(int id)
      {
         return DatabaseUtils.GetEntity<CustomerDto>("SELECT C.*, U.* from CUSTOMER C, USERS U WHERE C.USER_ID = U.USER_ID AND U.USER_ID=" + id);
      }
      public static int Insert(CustomerDto dto)
      {
         dto.UserType = Const.CONST.USERS.USER_TYPE_CUSTOMER;
         int UserId = DatabaseUtils.InsertEntity<UserDto>(dto, true);
         dto.UserId = UserId;
         return DatabaseUtils.InsertEntity<CustomerDto>(dto, false, true, false);
      }
      public static bool Update(CustomerDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto, true) > 0 && DatabaseUtils.UpdateEntity<CustomerDto>(dto) > 0;
      }
      public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE CUSTOMER SET IS_DELETED = 1 WHERE CUSTOMER_ID=" + Id) > 0;
      }
   }
}
