using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("PROVIDER")]
   public class ProviderDto : ChangeInformation
   {
      public ProviderDto() : base() { }
      public ProviderDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<ProviderDto>(data, this);
      }
      [DTO(Column = "PROVIDER_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int ProviderId { get; set; }
      [DTO(Column = "NAME", DataType = DATATYPE.STRING)]
      public string Name { get; set; }
      [DTO(Column = "ADDRESS", DataType = DATATYPE.STRING)]
      public string Address { get; set; }
      [DTO(Column = "CONTACT", DataType = DATATYPE.STRING)]
      public string Contact { get; set; }
      [DTO(Column = "EMAIL", DataType = DATATYPE.STRING)]
      public string Email { get; set; }
      [DTO(Column = "NOTE", DataType = DATATYPE.STRING)]
      public string Note { get; set; }
	   [DTO(Column = "IS_DELETED", DataType = DATATYPE.BOOLEAN)]
      public bool IsDeleted { get; set; }  
   }
}

