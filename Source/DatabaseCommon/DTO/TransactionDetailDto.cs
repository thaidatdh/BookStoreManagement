using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("TRANSACTION")]
   public class TransactionDetailDto
   {
      public TransactionDetailDto() { }
      public TransactionDetailDto(Object data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
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
   }
}

