using DatabaseCommon.Const;
using DatabaseCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DTO
{
   [Table("STAFF")]
   public class StaffDto : UserDto
   {
      public StaffDto() { }
      public StaffDto(Object data) : base((Object)data)
      {
         DTOService.PassValueByAttribute<StaffDto>(data, this);
      }
      [DTO(Column = "STAFF_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int StaffId { get; set; }
      [DTO(Column = "USER_ID", DataType = DATATYPE.INTEGER)]
      public int UserId { get; set; }
      [DTO(Column = "SALARY", DataType = DATATYPE.BIGINT)]
      public long Salary { get; set; }
   }
}
