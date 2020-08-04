using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class TransactionDetailDao : GenericDao<TransactionDetailDto>
   {
      public static List<TransactionDetailDto> GetByTransaction(int TransactionId)
      {
         string sql = "SELECT * FROM TRANSACTION_DETAIL WHERE TRANSACTION_ID=" + TransactionId;
         return DatabaseUtils.GetEntityList<TransactionDetailDto>(sql);
      }
      public static int Insert(TransactionDetailDto dto)
      {
         return DatabaseUtils.InsertEntity<TransactionDetailDto>(dto);
      }
      public static bool Delete(TransactionDetailDto dto)
      {
         return DatabaseUtils.DeleteEntity<TransactionDetailDto>(dto.TransactionDetailId) > 0;
      }
      public static bool DeleteAllTransactionDetail(long transactionId)
      {
         return DatabaseUtils.ExecuteQuery("DELETE FROM TRANSACTION_DETAIL WHERE TRANSACTION_ID=" + transactionId) > 0;
      }
   }
}
