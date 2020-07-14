using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Utils;
using DatabaseCommon.Const;
using System.Windows.Markup;

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
      public static bool Delete(DefinitionDto dto)
      {
         return DatabaseUtils.DeleteEntity<DefinitionDto>(dto.DefinitionId) > 0;
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
      public static Dictionary<string, List<int>> GetUserAuthorizationMap()
      {
         Dictionary<string, List<int>> result = new Dictionary<string, List<int>>();
         List<DefinitionDto> list = GetAuthorizationList();
         foreach (DefinitionDto dto in list)
         {
            string key = dto.Value1;
            List<int> values = dto.Value2.ToNotNullString().Split(new char[] { ',' }).Select(n => TypesUtils.Parse.ToInt32(n)).Where(n => n != 0).ToList();
            result[key] = values;
         }
         return result;
      }
   }
}
