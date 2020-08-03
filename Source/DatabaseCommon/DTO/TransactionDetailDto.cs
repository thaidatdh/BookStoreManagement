using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.DAO;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("TRANSACTION_DETAIL")]
   public class TransactionDetailDto
   {
      public TransactionDetailDto() { }
      public TransactionDetailDto(Object data)
      {
         DTOService.PassValueByAttribute<TransactionDetailDto>(data, this);
      }
      [DTO(Column = "TRANSACTION_DETAIL_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int TransactionDetailId { get; set; }
      [DTO(Column = "TRANSACTION_ID", DataType = DATATYPE.INTEGER)]
      public int TransactionId { get; set; }
      [DTO(Column = "BOOK_ID", DataType = DATATYPE.INTEGER)]
      public int BookId { get; set; }
      [DTO(Column = "PRICE", DataType = DATATYPE.BIGINT)]
      public long Price { get; set; }
      [DTO(Column = "AMOUNT", DataType = DATATYPE.INTEGER)]
      public int Amount { get; set; }
      [DTO(Column = "DISCOUNT", DataType = DATATYPE.BIGINT)]
      public long Discount { get; set; }
      [DTO(Column = "DISCOUNT_ID", DataType = DATATYPE.INTEGER)]
      public int DiscountId { get; set; }

      public string BookName
      {
         get
         {
            if (BookId != 0)
               return BookDao.Select(n => n.Name).First(n => n.BookId == BookId);
            return "";
         }
      }
      public string DiscountCode
      {
         get
         {
            if (DiscountId != 0)
               return DiscountDao.Select(n => n.Code).First(n => n.DiscountId == DiscountId);
            return "";
         }
      }
   }
}

