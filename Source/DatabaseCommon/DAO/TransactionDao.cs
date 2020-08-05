using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   class TransactionDao : GenericDao<TransactionDto>
   {
      public static List<TransactionDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<TransactionDto>();
      }
      public static int Insert(TransactionDto dto)
      {
         int TransactionId = DatabaseUtils.InsertEntity<TransactionDto>(dto, true);
         if (dto.TransactionDetails != null && dto.TransactionDetails.Count > 0)
         {
            foreach (TransactionDetailDto detailDto in dto.TransactionDetails)
            {
               detailDto.TransactionId = TransactionId;
               DatabaseUtils.InsertEntity<TransactionDetailDto>(detailDto);
            }
         }
         return TransactionId;
      }
      public static TransactionDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<TransactionDto>(Id);
      }
      public static bool Delete(int Id)
      {
         return DatabaseUtils.ExecuteQuery("UPDATE TRANSACTIONS SET IS_DELETED = 1 WHERE TRANSACTION_ID=" + Id) > 0;
      }
      public static List<TransactionDto> GetByCustomer(int customer_id)
      {
         return TransactionDao.Where(n => n.CustomerId == customer_id).ToList();
      }
      public static List<TransactionDto> GetByStaff(int staff_id)
      {
         return TransactionDao.Where(n => n.StaffId == staff_id).ToList();
      }
      public static List<TransactionDto> GetByProvider(int provider_id)
      {
         return TransactionDao.Where(n => n.ProviderId == provider_id).ToList();
      }
      public static List<TransactionDto> GetProviderTransaction()
      {
         return TransactionDao.Where(n => n.ProviderId != null && n.ProviderId != 0).ToList();
      }
      public static List<TransactionDto> GetStaffTransaction()
      {
         return TransactionDao.Where(n => n.StaffId != null && n.StaffId != 0).ToList();
      }
      public static List<TransactionDto> GetCustomerTransaction()
      {
         return TransactionDao.Where(n => n.CustomerId != null && n.CustomerId != 0).ToList();
      }
   }
}
