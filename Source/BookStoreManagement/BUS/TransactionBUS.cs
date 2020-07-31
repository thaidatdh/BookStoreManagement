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
      public static int Insert(Dictionary<String, Object> map)
      {
         if (map == null)
            return 0;
         try
         {
            TransactionDto dto = (TransactionDto)map.GetValue("OBJECT");
            int id = TransactionDao.Insert(dto);
            dto.TransactionId = id;
            allTransaction.Add(dto);
            return id;
         }
         catch (Exception ex)
         {
            return 0;
         }
      }
      public static bool Update(Dictionary<String, Object> map)
      {
         if (map == null)
            return false;
         try
         {
            TransactionDto dto = (TransactionDto)map.GetValue("OBJECT");
            TransactionDto oldDto = allTransaction.FirstOrDefault(n => n.TransactionId == dto.TransactionId);
            int result = TransactionDao.Update(dto);
            if (result > 0)
            {
               if (oldDto != null)
                  allTransaction.Remove(oldDto);
               allTransaction.Add(dto);
               allTransaction.OrderBy(n => n.TransactionId);
            }
            return result > 0;
         }
         catch (Exception ex)
         {
            return false;
         }
      }
      public static int Insert(TransactionDto dto)
      {
         int id = TransactionDao.Insert(dto);
         dto.TransactionId = id;
         allTransaction.Add(dto);
         allTransaction.OrderBy(n => n.TransactionId);
         return id;
      }
      public static bool Delete(TransactionDto dto)
      {
         TransactionDto oldDto = allTransaction.FirstOrDefault(n => n.TransactionId == dto.TransactionId);
         if (oldDto != null)
            allTransaction.Remove(oldDto);
         allTransaction.Add(dto);
         allTransaction.OrderBy(n => n.TransactionId);
         return TransactionDao.Delete(dto.TransactionId);
      }
   }
}
