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
   class BookBUS
   {
      private static List<BookDto> allNotDeletedBooks = new List<BookDto>();
      public static List<BookDto> GetAllNotDeletedBooks()
      {
         if (allNotDeletedBooks == null || allNotDeletedBooks.Count == 0)
         {
            allNotDeletedBooks = BookDao.Where(n => n.IsDeleted == false).ToList();
         }
         return allNotDeletedBooks;
      }
      public static int Insert(Dictionary<String, Object> map)
      {
         if (map == null)
            return 0;
         try
         {
            BookDto dto = (BookDto)map.GetValue("OBJECT");
            dto.CategoryId = DefinitionBUS.GetListCategoryId(map.GetValue("CATEGORY").ToString());
            dto.AuthorId = AuthorBUS.GetListAuthorId(map.GetValue("AUTHOR").ToString());
            dto.PublisherId = PublisherBUS.GetPublisherId(map.GetValue("PUBLISHER").ToString());
            dto.ProviderId = ProviderBUS.GetPublisherId(map.GetValue("PROVIDER").ToString());
            int id = BookDao.Insert(dto);
            dto.BookId = id;
            allNotDeletedBooks.Add(dto);
            return id;
         }
         catch (Exception ex)
         {
            return 0;
         }
      }
      public static bool Update(Dictionary<String, Object> map)
      {
         if (map == null)
            return false;
         try
         {
            BookDto dto = (BookDto)map.GetValue("OBJECT");
            dto.CategoryId = DefinitionBUS.GetListCategoryId(map.GetValue("CATEGORY").ToString());
            dto.AuthorId = AuthorBUS.GetListAuthorId(map.GetValue("AUTHOR").ToString());
            dto.PublisherId = PublisherBUS.GetPublisherId(map.GetValue("PUBLISHER").ToString());
            dto.ProviderId = ProviderBUS.GetPublisherId(map.GetValue("PROVIDER").ToString());
            BookDto oldDto = allNotDeletedBooks.FirstOrDefault(n => n.BookId == dto.BookId);
            bool result = BookDao.Update(dto);
            if (result)
            {
               if (oldDto != null)
                  allNotDeletedBooks.Remove(oldDto);
               allNotDeletedBooks.Add(dto);
               allNotDeletedBooks.OrderBy(n => n.BookId);
            }
            return result;
         }
         catch (Exception ex)
         {
            return false;
         }
      }
      public static int Insert(BookDto dto)
      {
         int id = BookDao.Insert(dto);
         dto.BookId = id;
         allNotDeletedBooks.Add(dto);
         allNotDeletedBooks.OrderBy(n => n.BookId);
         return id;
      }
      public static bool Delete(BookDto dto)
      {
         allNotDeletedBooks.Remove(dto);
         return BookDao.Delete(dto.BookId);
      }
   }
}
