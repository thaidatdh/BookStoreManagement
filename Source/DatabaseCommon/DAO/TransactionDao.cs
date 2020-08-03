using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class TransactionDao : GenericDao<TransactionDto>
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
               int detailId = DatabaseUtils.InsertEntity<TransactionDetailDto>(detailDto);
               detailDto.TransactionDetailId = detailId;
            }
         }
         return TransactionId;
      }
      public static int Update(TransactionDto dto)
      {
         if (dto.TransactionId == 0)
         {
            return Insert(dto);
         }
         DatabaseUtils.UpdateEntity<TransactionDto>(dto, true);
         if (dto.TransactionDetails == null || dto.TransactionDetails.Count == 0)
         {
            TransactionDetailDao.DeleteAllTransactionDetail(dto.TransactionId);
         }
         if (dto.TransactionDetails != null && dto.TransactionDetails.Count > 0)
         {
            List<TransactionDetailDto> oldDetailList = TransactionDetailDao.Where(n => n.TransactionId == dto.TransactionId).ToList();
            foreach (TransactionDetailDto old in oldDetailList)
            {
               TransactionDetailDto newDto = dto.TransactionDetails.FirstOrDefault(n => n.TransactionDetailId == old.TransactionDetailId);
               if (newDto == null)
               {
                  TransactionDetailDao.Delete(old);
               }
               else
               {
                  TransactionDetailDao.Update(newDto);
               }
            }
            foreach (TransactionDetailDto detailDto in dto.TransactionDetails.Where(n => n.TransactionDetailId == 0))
            {
               detailDto.TransactionId = dto.TransactionId;
               int id = DatabaseUtils.InsertEntity<TransactionDetailDto>(detailDto);
               detailDto.TransactionDetailId = id;
            }
         }
         return dto.TransactionId;
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
