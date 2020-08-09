using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
            string sql_user = "Update Users Set first_name='" + dto.FirstName + "', last_name='" + dto.LastName
                  + "', dob='" + dto.DOB + "', address='" + dto.Address + "', phone='" + dto.Phone + "', gender='"
                  + dto.Gender + "', email='" + dto.Email + "', note='" + dto.Note + "', photo_link='" + dto.PhotoLink
                  + "', updated_date='" + dto.UpdatedDate + "', updated_by=" + dto.UpdatedBy + " Where user_id="+dto.UserId;
            string sql_customer = "Update Customer Set credit_card='" + dto.Momo + "', momo='" + dto.Momo + "', bank_number='"
                + dto.BankNumber + "', bank_name='" + dto.BankName + "' Where user_id=" + dto.UserId;

            return (DatabaseUtils.ExecuteQuery(sql_user) > 0) && (DatabaseUtils.ExecuteQuery(sql_customer) > 0);

         //return DatabaseUtils.UpdateEntity<UserDto>(dto, true) > 0 && DatabaseUtils.UpdateEntity<CustomerDto>(dto) > 0;
        }
        public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE CUSTOMER SET IS_DELETED = 1 WHERE USER_ID=" + Id) > 0;
      }
   }
}
