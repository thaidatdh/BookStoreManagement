using DatabaseCommon.Const;
using DatabaseCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DTO
{
   [Table("CUSTOMER", "USER_ID")]
   public class CustomerDto : UserDto
   {
      public CustomerDto() : base() { }
      public CustomerDto(Object data) : base((Object)data)
      {
         DTOService.PassValueByAttribute<StaffDto>(data, this);
      }
      [DTO(Column = "USER_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int UserId { get; set; }
      [DTO(Column = "MOMO", DataType = DATATYPE.STRING)]
      public string Momo { get; set; }
      [DTO(Column = "CREDIT_CARD", DataType = DATATYPE.STRING)]
      public string CreditCard { get; set; }
      [DTO(Column = "BANK_NUMBER", DataType = DATATYPE.STRING)]
      public string BankNumber { get; set; }
      [DTO(Column = "BANK_NAME", DataType = DATATYPE.STRING)]
      public string BankName { get; set; }
      [DTO(Column = "POINT", DataType = DATATYPE.INTEGER)]
      public string Point { get; set; }
   }
}
