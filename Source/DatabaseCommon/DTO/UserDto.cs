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
   public class UserDto : ChangeInformation
   {
      public UserDto() : base() { }
      public UserDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
      }
      [DTO(Column = "USER_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int UserId { get; set; }
      [DTO(Column = "FIRST_NAME", DataType = DATATYPE.STRING)]
      public string FirstName { get; set; }
      [DTO(Column = "LAST_NAME", DataType = DATATYPE.STRING)]
      public string LastName { get; set; }
      [DTO(Column = "DOB", DataType = DATATYPE.STRING)]
      public string DOB { get; set; }
      [DTO(Column = "ADDRESS", DataType = DATATYPE.STRING)]
      public string Address { get; set; }
      [DTO(Column = "PHONE", DataType = DATATYPE.STRING)]
      public string Phone { get; set; }
      [DTO(Column = "EMAIL", DataType = DATATYPE.STRING)]
      public string Email { get; set; }
      [DTO(Column = "GENDER", DataType = DATATYPE.STRING, DefaultValue = CONST.USERS.GENDER_NOT_SPECIFY)]
      public string Gender { get; set; }
      [DTO(Column = "NOTE", DataType = DATATYPE.STRING)]
      public string Note { get; set; }
      [DTO(Column = "USER_TYPE", DataType = DATATYPE.STRING, DefaultValue = CONST.USERS.DEFAULT_USER_TYPE)]
      public string UserType { get; set; }
   }
}
