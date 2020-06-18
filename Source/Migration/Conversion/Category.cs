using CommonLibrary.Utils;
using DatabaseCommon.Const;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using Migration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Conversion
{
   class Category
   {
      public static Dictionary<string, int> CategoryNameMap = new Dictionary<string, int>();
      public static void Migrate()
      {
         LoggerMigration.StartMigrate("CATEGORY");
         List<CategoryDomain> listDomain = CategoryDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (CategoryDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.Name)) continue;
            DefinitionDto dto = new DefinitionDto();
            dto.DefinitionType = CONST.DEFINITION.DEFINITION_TYPE_CATEGORY;
            dto.Value1 = domain.Name;
            dto.CreateBy = Const.DEFAULT_STAFF_ID;
            dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
            int id = DefinitionDao.Insert(dto);
            CategoryNameMap[dto.Value1.ToKey()] = id;
            LoggerMigration.log(++count, total, "Insert category " + dto.Value1);
         }
         LoggerMigration.EndMigrate();
      }
      public static int InsertNewCategory(string Name)
      {
         string NameKey = Name.ToKey();
         if (CategoryNameMap.ContainsKey(NameKey))
            return CategoryNameMap.GetValue(NameKey);
         DefinitionDto dto = new DefinitionDto();
         dto.DefinitionType = CONST.DEFINITION.DEFINITION_TYPE_CATEGORY;
         dto.Value1 = Name;
         dto.CreateBy = Const.DEFAULT_STAFF_ID;
         dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
         int id = DefinitionDao.Insert(dto);
         CategoryNameMap[NameKey] = id;
         LoggerMigration.log("--> Insert new category: " + dto.Value1);
         return id;
      }
   }
}
