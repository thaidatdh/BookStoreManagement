using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("DEFINITION")]
   public class DefinitionDto : ChangeInformation
   {
      public DefinitionDto() : base() { }
      public DefinitionDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<DefinitionDto>(data, this);
      }
      [DTO(Column = "DEFINITION_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int DefinitionId { get; set; }
      [DTO(Column = "DEFINITION_TYPE", DataType = DATATYPE.INTEGER)]
      public int DefinitionType { get; set; }
      [DTO(Column = "VALUE_1", DataType = DATATYPE.STRING)]
      public string Value1 { get; set; }
      [DTO(Column = "VALUE_2", DataType = DATATYPE.STRING)]
      public string Value2 { get; set; }
   }
}



