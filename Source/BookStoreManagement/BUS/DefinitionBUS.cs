using CommonLibrary.Utils;
using DatabaseCommon.Const;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   public class DefinitionBUS
   {
      public static bool UpdateAuthorization(DefinitionDto dto, String name, List<String> FeaturesName)
      {
         var featureMap = FeatureAttributeService.GetFeaturePropertiesNameMap();
         List<int> values = FeaturesName.Select(n => featureMap.GetValue(n)).Where(n => n != 0).ToList();
         var authorizationMap = FeatureAttributeService.GetAuthorizationMap();
         FeatureAttributeService.UpdateAuthorizationNameMap(name, values);
         authorizationMap.Remove(dto.Value1);
         authorizationMap[name] = values;

         dto.Value1 = name;
         dto.Value2 = String.Join(",", values);
         return DefinitionDao.Update(dto);
      }
      public static DefinitionDto InsertAuthorization(String name, List<String> FeaturesName)
      {
         var featureMap = FeatureAttributeService.GetFeaturePropertiesNameMap();
         List<int> values = FeaturesName.Select(n => featureMap.GetValue(n)).Where(n => n != 0).ToList();

         DefinitionDto dto = new DefinitionDto();
         dto.DefinitionType = CONST.DEFINITION.DEFINITION_TYPE_AUTHORIZATION;
         dto.Value1 = name;
         dto.Value2 = String.Join(",", values);
         int id = DefinitionDao.Insert(dto);
         dto.DefinitionId = id;

         var authorizationMap = FeatureAttributeService.GetAuthorizationMap();
         authorizationMap[name] = values;
         FeatureAttributeService.UpdateAuthorizationNameMap(name, values);
         return dto;
      }
      public static bool DeleteAuthorization(DefinitionDto dto)
      {
         if (dto.Value1.Equals("ADMIN"))
            return false;
         return DefinitionDao.Delete(dto);
      }
   }
}
