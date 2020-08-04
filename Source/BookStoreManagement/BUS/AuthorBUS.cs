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
        private static List<AuthorDto> authorList = new List<AuthorDto>();
        private static List<AuthorDto> allNotDeletedAuthors = new List<AuthorDto>();
        public static List<AuthorDto> GetAllNotDeletedAuthors()
        {
            if (allNotDeletedAuthors == null || allNotDeletedAuthors.Count == 0)
            {
                allNotDeletedAuthors = AuthorDao.Where(n => n.IsDeleted == false).ToList();
            }
            return allNotDeletedAuthors;
        }

        public static List<string> GetAuthorNameList()
        {
            if (authorList == null || authorList.Count == 0)
            {
                authorList = AuthorDao.Where(n => n.IsDeleted == false).ToList();
            }
            return authorList.Select(n => n.Name).ToList();
        }
        public static int GetPublisherId(string name)
        {
            if (authorList == null || authorList.Count == 0)
            {
                authorList = AuthorDao.Where(n => n.IsDeleted == false).ToList();
            }
            AuthorDto dto = authorList.FirstOrDefault(n => n.Name.Equals(name));
            if (dto == null)
                dto = AuthorDao.Where(n => n.Name.Equals(name)).ToList().FirstOrDefault();
            if (dto == null)
                return 0;
            else
                return dto.AuthorId;
        }

        public static bool Update(AuthorDto dto)
        {

            AuthorDto oldDto = allNotDeletedAuthors.FirstOrDefault(n => n.AuthorId == dto.AuthorId);
            bool result = AuthorDao.Update(dto);
            if (result)
            {
                if (oldDto != null)
                    allNotDeletedAuthors.Remove(oldDto);
                allNotDeletedAuthors.Add(dto);
                allNotDeletedAuthors = allNotDeletedAuthors.OrderBy(n => n.AuthorId).ToList();
            }
            return result;

        }
        public static int Insert(AuthorDto dto)
        {
            int id = AuthorDao.Insert(dto);
            dto.AuthorId = id;
            allNotDeletedAuthors.Add(dto);
            allNotDeletedAuthors = allNotDeletedAuthors.OrderByDescending(n => n.AuthorId).ToList();
            return id;
        }
        public static bool Delete(AuthorDto dto)
        {
            allNotDeletedAuthors.Remove(dto);
            return AuthorDao.Delete(dto.AuthorId);
        }
        public static List<AuthorDto> GetAllAuthors()
      {
         if (authorList == null || authorList.Count == 0)
         {
                authorList = AuthorDao.GetAll();
         }
         return authorList;
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
