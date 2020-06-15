using DatabaseCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DTO
{
   public class ChangeInformation
   {
      [DTO(Column = "CREATE_DATE", DataType = Const.DATATYPE.TIMESTAMP)]
      public DateTime CreateDate { get; set; }
      [DTO(Column = "CREATED_BY", DataType = Const.DATATYPE.INTEGER)]
      public int CreateBy { get; set; }
      [DTO(Column = "UPDATED_DATE", DataType = Const.DATATYPE.TIMESTAMP)]
      public DateTime UpdatedDate { get { return DateTime.Now; } set { } }
      [DTO(Column = "UPDATED_BY", DataType = Const.DATATYPE.INTEGER)]
      public int UpdatedBy { get; set; }

      public ChangeInformation() { }
      public ChangeInformation(Object data)
      {
         DTOService.PassValueByAttribute<ChangeInformation>(data, this);
      }
   }
}
