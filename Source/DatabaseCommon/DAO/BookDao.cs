using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class BookDao
   {
      public static List<BookDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<BookDto>();
      }
      public static int Insert(BookDto dto)
      {
         return DatabaseUtils.InsertEntity<BookDto>(dto);
      }
      public static bool Update(BookDto dto)
      {
         return DatabaseUtils.UpdateEntity<BookDto>(dto) > 0;
      }
      public static BookDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<BookDto>(Id);
      }
      public static BookDto GetByBarCode(string barcode)
      {
         return DatabaseUtils.GetEntity<BookDto>("SELECT * FROM BOOK WHERE BARCODE='" + barcode.Replace("'","''") + "'");
      }
   }
}
