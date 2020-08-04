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
   public class DiscountBUS
   {
      private static Dictionary<string, DiscountDto> DiscountCodeMap = new Dictionary<string, DiscountDto>();
      public static DiscountDto GetDiscountDto(string discountCode)
      {
         if (DiscountCodeMap.ContainsKey(discountCode))
         {
            return DiscountCodeMap.GetValue(discountCode);
         }
         DiscountDto dto = DiscountDao.First(n => n.Code.Equals(discountCode));
         DiscountCodeMap[discountCode] = dto;
         return dto;
      }
   }
}
