using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Utils;
using DatabaseCommon.Const;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("BOOK")]
   public class BookDto : ChangeInformation
   {
      public BookDto() : base() { }
      public BookDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
      }
      [DTO(Column = "BOOK_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int BookId { get; set; }
      [DTO(Column = "NAME", DataType = DATATYPE.STRING)]
      public int Name { get; set; }
      [DTO(Column = "BARCODE", DataType = DATATYPE.STRING)]
      public string Barcode { get; set; }
      [DTO(Column = "FORMAT", DataType = DATATYPE.STRING)]
      public string Format { get; set; }
      [DTO(Column = "SIZE", DataType = DATATYPE.STRING)]
      public string Size { get; set; }
      [DTO(Column = "PAGE", DataType = DATATYPE.STRING)]
      public string Page { get; set; }
      [DTO(Column = "DESCRIPTION", DataType = DATATYPE.STRING)]
      public string Description { get; set; }
      [DTO(Column = "PRICE", DataType = DATATYPE.BIGINT)]
      public string Price { get; set; }
      [DTO(Column = "REMAINING", DataType = DATATYPE.INTEGER)]
      public string Remaining { get; set; }
      [DTO(Column = "LOCATION", DataType = DATATYPE.STRING)]
      public string Location { get; set; }
      [DTO(Column = "PHOTO_LINK", DataType = DATATYPE.STRING)]
      public string PhotoLink { get; set; }
      [DTO(Column = "CATEGORY_ID", DataType = DATATYPE.STRING)]
      public string CategoryId { get; set; }
      [DTO(Column = "AUTHOR_ID", DataType = DATATYPE.STRING)]
      public string AuthorId { get; set; }
      [DTO(Column = "PUBLISHER_ID", DataType = DATATYPE.INTEGER)]
      public string PublisherId { get; set; }
      [DTO(Column = "PUBLISHED_DATE", DataType = DATATYPE.STRING)]
      public string PublishedDate { get; set; }
      [DTO(Column = "PROVIDER_ID", DataType = DATATYPE.INTEGER)]
      public string ProviderId { get; set; }
   }
}



