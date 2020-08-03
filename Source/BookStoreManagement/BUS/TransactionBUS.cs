using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   class TransactionBUS
   {
      private static List<TransactionDto> allTransaction = new List<TransactionDto>();
      public static List<TransactionDto> GetAllTransaction()
      {
         if (allTransaction == null || allTransaction.Count == 0)
         {
            allTransaction = TransactionDao.GetAll();
         }
         return allTransaction;
      }
      public static int Insert(TransactionDto dto)
      {
         int id = TransactionDao.Insert(dto);
         dto.TransactionId = id;
         allTransaction.Add(dto);
         allTransaction.OrderBy(n => n.TransactionId);
         return id;
      }
      public static int Update(TransactionDto dto)
      {
         TransactionDto oldDto = allTransaction.FirstOrDefault(n => n.TransactionId == dto.TransactionId);
         if (oldDto != null)
            allTransaction.Remove(oldDto);
         allTransaction.Add(dto);
         allTransaction.OrderBy(n => n.TransactionId);
         return TransactionDao.Update(dto);
      }
      public static bool Delete(TransactionDto dto)
      {
         TransactionDto oldDto = allTransaction.FirstOrDefault(n => n.TransactionId == dto.TransactionId);
         if (oldDto != null)
            allTransaction.Remove(oldDto);
         return TransactionDao.Delete(dto.TransactionId);
      }
   }
}
