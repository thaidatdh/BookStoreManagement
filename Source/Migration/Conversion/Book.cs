using CommonLibrary;
using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using Migration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Migration.Conversion
{
   class Book
   {
      public static void Migrate() 
      {
         LoggerMigration.StartMigrate("BOOK");
         List<BookDomain> listDomain = BookDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (BookDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.Publisher)) continue;
            BookDto dto = new BookDto();
            dto.Name = domain.Title;
            dto.Format = domain.Format;
            dto.Size = domain.Dimensions;
            dto.Page = domain.Pages;
            dto.PublisherId = Publisher.PublisherNameMap.GetValue(domain.Publisher.ToKey());
            dto.PublishedDate = FormatUtils.FormatDate(domain.Pubdate);
            dto.Price = Price(domain.Price);
            dto.AuthorId = ProcessAuthor(domain);
            dto.CategoryId = ProcessCategory(domain.Subjects);
            dto.CreateBy = Const.DEFAULT_STAFF_ID;
            dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
            dto.Description = "";
            if (!String.IsNullOrEmpty(domain.Overview))
            {
               dto.Description += String.IsNullOrEmpty(dto.Description) ? "" : "<br>";
               dto.Description += "Overview:<br>" + domain.Overview;
            }
            if (!String.IsNullOrEmpty(domain.Excerpt))
            {
               dto.Description += String.IsNullOrEmpty(dto.Description) ? "" : "<br>";
               dto.Description += "Excerpt:<br>" + domain.Excerpt;
            }
            if (!String.IsNullOrEmpty(domain.Synopsis))
            {
               dto.Description += String.IsNullOrEmpty(dto.Description) ? "" : "<br>";
               dto.Description += "Synopsis:<br>" + domain.Synopsis;
            }
            if (!String.IsNullOrEmpty(domain.TOC))
            {
               dto.Description += String.IsNullOrEmpty(dto.Description) ? "" : "<br>";
               dto.Description += "TOC:<br>" + domain.TOC;
            }
            if (!String.IsNullOrEmpty(domain.EditorialReviews))
            {
               dto.Description += String.IsNullOrEmpty(dto.Description) ? "" : "<br>";
               dto.Description += "Editorial Reviews:<br>" + domain.TOC;
            }
            int id = BookDao.Insert(dto);
            LoggerMigration.log(++count, total, "Insert book " + dto.Name);
         }
         LoggerMigration.EndMigrate();
      }
      private static long Price(string price)
      {
         double originalPrice = Regex.Replace(price, "[^0-9]", "").ToDouble();
         return Math.Round(originalPrice * 23000, 0).ToInt64();
      }
      private static string ProcessAuthor(BookDomain domain)
      {
         List<int> resultList = new List<int>();
         if (Author.AuthorNameMap.ContainsKey(domain.Author.ToKey()))
         {
            resultList.Add(Author.AuthorNameMap.GetValue(domain.Author.ToKey()));
         }
         else
         {
            resultList.Add(Author.InsertNewAuthor(domain.Author, domain.AuthorBio));
         }
         
         List<string> authorsList = domain.Authors.Split(new char[] { ',' }).Select(n => n.Trim()).Where(n => !String.IsNullOrEmpty(n)).ToList();
         foreach (string author in authorsList)
         {
            if (author.ToKey().Contains(domain.Author.ToKey())) continue;
            if (Author.AuthorNameMap.ContainsKey(author.ToKey()))
            {
               resultList.Add(Author.AuthorNameMap.GetValue(author.ToKey()));
            }
            else
            {
               resultList.Add(Author.InsertNewAuthor(author));
            }
         }
         return String.Join(",", resultList.ToArray());
      }
      private static string ProcessCategory(string categories)
      {
         List<int> resultList = new List<int>();
         List<string> categoriesList = categories.Split(new char[] { ',', '-' }).Select(n => n.Trim()).Where(n => !String.IsNullOrEmpty(n)).ToList();
         foreach (string category in categoriesList)
         {
            if (String.IsNullOrEmpty(category)) continue;
            if (Category.CategoryNameMap.ContainsKey(category.ToKey()))
            {
               resultList.Add(Category.CategoryNameMap.GetValue(category.ToKey()));
            }
            else
            {
               resultList.Add(Category.InsertNewCategory(category));
            }
         }
         return String.Join(",", resultList.ToArray());
      }
   }
}
