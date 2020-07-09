using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Utils;
using DatabaseCommon.Const;

namespace DatabaseCommon.DAO
{
   public class DefinitionDao : GenericDao<DefinitionDto>
   {
      public static List<DefinitionDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<DefinitionDto>();
      }
      public static int Insert(DefinitionDto dto)
      {
         return DatabaseUtils.InsertEntity<DefinitionDto>(dto, true);
      }
      public static bool Update(DefinitionDto dto)
      {
         return DatabaseUtils.UpdateEntity<DefinitionDto>(dto, true) > 0;
      }
      public static DefinitionDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<DefinitionDto>(Id);
      }
      public static Dictionary<string, string> GetStoreInformation()
      {
         return DatabaseUtils.GetEntityList<DefinitionDto>("SELECT * FROM DEFINITION WHERE DEFINITION_TYPE = 1").GroupBy(n => n.Value1).ToDictionary(n => n.Key, n => n.ToList().Select(x => x.Value2).First());
      }
      public static List<DefinitionDto> GetCategoryList()
      {
         return DatabaseUtils.GetEntityList<DefinitionDto>("SELECT * FROM DEFINITION WHERE DEFINITION_TYPE=3");
      }
      public static List<DefinitionDto> GetAuthorizationList()
      {
         return DatabaseUtils.GetEntityList<DefinitionDto>("SELECT * FROM DEFINITION WHERE DEFINITION_TYPE=2");
      }
   }
}
