using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   class TransactionDao
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
   }
}
