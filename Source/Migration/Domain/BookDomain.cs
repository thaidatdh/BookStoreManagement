using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Domain
{
   class BookDomain
   {
      private static List<BookDomain> ListDomain = new List<BookDomain>();
      public static List<BookDomain> Gets() 
      {
         if (ListDomain.Count == 0)
            ListDomain = FileUtils.CSV.GetDataTable(Const.SOURCE_PATH + "/BOOK.CSV").Rows.Cast<DataRow>().Where(n => TypesUtils.Parse.ToInt32(n.ItemArray.FirstOrDefault().ToString()) != 0).Select(n => new BookDomain(n)).ToList();
         return ListDomain;
      }
      public string Id { get; set; }
      public string Title { get; set; }
      public string Author { get; set; }
      public string AuthorBio { get; set; }
      public string Authors { get; set; }
      public string Price { get; set; }
      public string Format { get; set; }
      public string Publisher { get; set; }
      public string Pubdate { get; set; }
      public string Edition { get; set; }
      public string Subjects { get; set; }
      public string Pages { get; set; }
      public string Dimensions { get; set; }
      public string Overview { get; set; }
      public string Synopsis { get; set; }
      public string Excerpt { get; set; }
      public string TOC { get; set; }
      public string EditorialReviews { get; set; }
      public BookDomain(DataRow dr)
      {
         Id = dr["id"].ToString();
         Title = dr["title"].ToString();
         Author = dr["author"].ToString();
         AuthorBio = dr["author_bio"].ToString();
         Authors = dr["authors"].ToString();
         Price = dr["price"].ToString();
         Format = dr["format"].ToString();
         Publisher = dr["publisher"].ToString();
         Pubdate = dr["pubdate"].ToString();
         Edition = dr["edition"].ToString();
         Subjects = dr["subjects"].ToString();
         Pages = dr["pages"].ToString();
         Dimensions = dr["dimensions"].ToString();
         Overview = dr["overview"].ToString();
         Synopsis = dr["synopsis"].ToString();
         Excerpt = dr["excerpt"].ToString();
         TOC = dr["toc"].ToString();
         EditorialReviews = dr["editorial_reviews"].ToString();
      }
   }
}
