using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   public class AuthorBUS
   {
      private static List<AuthorDto> ListAuthors = new List<AuthorDto>();
      public static List<AuthorDto> GetAllAuthors()
      {
         if (ListAuthors == null || ListAuthors.Count == 0)
         {
            ListAuthors = AuthorDao.GetAll();
         }
         return ListAuthors;
      }
      public static String GetListAuthorId(String authorsName)
      {
         List<string> AuthorNameList = authorsName.Split(new char[] { ',' }).Select(n => n.ToKey()).ToList();
         List<int> AuthorIdList = GetAllAuthors().Where(n => AuthorNameList.Contains(n.Name.ToKey())).Select(n => n.AuthorId).ToList();
         return String.Join(",", AuthorIdList);
      }
      public static String GetListAuthorName(String authorsId)
      {
         List<int> AuthorIdList = authorsId.Split(new char[] { ',' }).Select(n => n.ToInt32()).ToList();
         List<string> AuthorNameList = GetAllAuthors().Where(n => AuthorIdList.Contains(n.AuthorId)).Select(n => n.Name).ToList();
         return String.Join(", ", AuthorNameList);
      }
   }
}
