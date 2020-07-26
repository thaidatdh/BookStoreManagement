using DatabaseCommon.Const;
using DatabaseCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DTO
{
   [Table("STAFF", "USER_ID")]
   public class StaffDto : UserDto
   {
      public StaffDto() : base() { }
      public StaffDto(Object data) : base((Object)data)
      {
         DTOService.PassValueByAttribute<StaffDto>(data, this);
      }
      [DTO(Column = "STAFF_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int StaffId { get; set; }
      [DTO(Column = "USER_ID", DataType = DATATYPE.INTEGER)]
      public int UserId { get; set; }
      [DTO(Column = "USERNAME", DataType = DATATYPE.STRING)]
      public string Username { get; set; }
      [DTO(Column = "PASSWORD", DataType = DATATYPE.STRING)]
      public string Password { get; set; }
      [DTO(Column = "SALARY", DataType = DATATYPE.BIGINT)]
      public long Salary { get; set; }
      [DTO(Column = "START_DATE", DataType = DATATYPE.STRING)]
      public string StartDate { get; set; }
      [DTO(Column = "END_DATE", DataType = DATATYPE.STRING)]
      public string EndDate { get; set; }
      [DTO(Column = "ACTIVE", DataType = DATATYPE.BOOLEAN)]
      public bool Active { get; set; }
   }
}
