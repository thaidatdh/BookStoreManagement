using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("USERS")]
   public class UserDto
   {
      public UserDto() { }
      public UserDto(Object data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
      }
      [DTO(Column = "USER_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public long UserId { get; set; }
      [DTO(Column = "FIRST_NAME", DataType = DATATYPE.STRING)]
      public string FirstName { get; set; }
      [DTO(Column = "LAST_NAME", DataType = DATATYPE.STRING)]
      public string LastName { get; set; }
   }
}
