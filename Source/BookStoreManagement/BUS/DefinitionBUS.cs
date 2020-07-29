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
      private static List<DefinitionDto> ListCategories = new List<DefinitionDto>();
      public static List<DefinitionDto> GetAllCategory()
      {
         if (ListCategories == null || ListCategories.Count == 0)
         {
            ListCategories = DefinitionDao.GetCategoryList();
         }
         return ListCategories;
      }
      public static String GetListCategoryId(String categoryName)
      {
         List<string> CategoryNameList = categoryName.Split(new char[] { ',' }).Select(n => n.ToKey()).ToList();
         List<int> CategoryIdList = GetAllCategory().Where(n => CategoryNameList.Contains(n.Value1.ToKey())).Select(n => n.DefinitionId).ToList();
         return String.Join(",", CategoryIdList);
      }
      public static String GetListCategoryName(String categoryId)
      {
         List<int> CategoryIdList = categoryId.Split(new char[] { ',' }).Select(n => n.ToInt32()).ToList();
         List<string> CategoryNameList = GetAllCategory().Where(n => CategoryIdList.Contains(n.DefinitionId)).Select(n => n.Value1).ToList();
         return String.Join(", ", CategoryNameList);
      }
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
