using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class CustomerDao
   {
      public static List<CustomerDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<CustomerDto>("SELECT C.*, U.* from CUSTOMER C, USERS U WHERE C.USER_ID = U.USER_ID");
      }
      public static int Insert(CustomerDto dto)
      {
         int UserId = DatabaseUtils.InsertEntity<UserDto>(dto);
         dto.UserId = UserId;
         return DatabaseUtils.InsertEntity<CustomerDto>(dto, false, true);
      }
      public static bool Update(CustomerDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto) > 0 && DatabaseUtils.UpdateEntity<CustomerDto>(dto) > 0;
      }
   }
}
