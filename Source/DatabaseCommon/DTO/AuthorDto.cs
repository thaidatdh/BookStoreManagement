using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("AUTHOR")]
   public class AuthorDto : ChangeInformation
   {
      public AuthorDto() : base() { }
      public AuthorDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
      }
      [DTO(Column = "AUTHOR_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int AuthorId { get; set; }
      [DTO(Column = "NAME", DataType = DATATYPE.STRING)]
      public string Name { get; set; }
      [DTO(Column = "NOTE", DataType = DATATYPE.STRING)]
      public string Note { get; set; }
	  [DTO(Column = "IS_DELETED", DataType = DATATYPE.BOOLEAN)]
      public bool IsDeleted { get; set; }  
   }
}

