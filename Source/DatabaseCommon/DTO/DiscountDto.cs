using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("DISCOUNT")]
   public class DiscountDto : ChangeInformation
   {
      public DiscountDto() : base() { }
      public DiscountDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<DiscountDto>(data, this);
      }
      [DTO(Column = "DISCOUNT_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int DiscountId { get; set; }
      [DTO(Column = "DESCRIPTION", DataType = DATATYPE.STRING)]
      public string Description { get; set; }
      [DTO(Column = "CODE", DataType = DATATYPE.STRING)]
      public string Code { get; set; }
      [DTO(Column = "PERCENTAGE", DataType = DATATYPE.DOUBLE)]
      public string Percentage { get; set; }
      [DTO(Column = "Amount", DataType = DATATYPE.BIGINT)]
      public string Amount { get; set; }
      [DTO(Column = "TYPE", DataType = DATATYPE.STRING)]
      public string Type { get; set; }
      [DTO(Column = "START_DATE", DataType = DATATYPE.STRING)]
      public string StartDate { get; set; }
      [DTO(Column = "END_DATE", DataType = DATATYPE.STRING)]
      public string EndDate { get; set; }
   }
}

