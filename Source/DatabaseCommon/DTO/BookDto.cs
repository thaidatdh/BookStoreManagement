using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using CommonLibrary.Utils;
using DatabaseCommon.Const;
using DatabaseCommon.DAO;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("BOOK")]
   public class BookDto : ChangeInformation
   {
      public BookDto() : base() { }
      public BookDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<BookDto>(data, this);
      }
      [DTO(Column = "BOOK_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int BookId { get; set; }
      [DTO(Column = "NAME", DataType = DATATYPE.STRING)]
      public string Name { get; set; }
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
      public long Price { get; set; }
      [DTO(Column = "REMAINING", DataType = DATATYPE.INTEGER)]
      public int Remaining { get; set; }
      [DTO(Column = "LOCATION", DataType = DATATYPE.STRING)]
      public string Location { get; set; }
      [DTO(Column = "PHOTO_LINK", DataType = DATATYPE.STRING, DefaultValue = CONST.BOOK.DEFAULT_PHOTO_LINK)]
      public string PhotoLink { get; set; }
      [DTO(Column = "CATEGORY_ID", DataType = DATATYPE.STRING)]
      public string CategoryId { get; set; }
      [DTO(Column = "AUTHOR_ID", DataType = DATATYPE.STRING)]
      public string AuthorId { get; set; }
      [DTO(Column = "PUBLISHER_ID", DataType = DATATYPE.INTEGER)]
      public int PublisherId { get; set; }
      [DTO(Column = "PUBLISHED_DATE", DataType = DATATYPE.STRING)]
      public string PublishedDate { get; set; }
      [DTO(Column = "PROVIDER_ID", DataType = DATATYPE.INTEGER)]
      public int ProviderId { get; set; }
      [DTO(Column = "IS_DELETED", DataType = DATATYPE.BOOLEAN)]
      public bool IsDeleted { get; set; }

      //Get Dto methods

      public ProviderDto ProviderDto {
         get
         {
            if (ProviderId > 0)
            {
               return ProviderDao.GetById(ProviderId);
            }
            else
            {
               return null;
            }
         }
      }
      public PublisherDto PublisherDto
      {
         get
         {
            if (ProviderId > 0)
            {
               return PublisherDao.GetById(ProviderId);
            }
            else
            {
               return null;
            }
         }
      }

      private List<int> _listAuthorId { get; set; }
      public List<int> ListAuthorId
      {
         get
         {
            if (_listAuthorId == null)
            {
               _listAuthorId = AuthorId.Split(new char[] { ',' }).Where(n => !String.IsNullOrEmpty(n.Trim())).Select(n => TypesUtils.Parse.ToInt32(n.Trim())).Distinct().ToList();
            }
            AuthorId = String.Join(",", _listAuthorId.Distinct());
            return _listAuthorId;
         }
         set
         {
            _listAuthorId = value;
            AuthorId = String.Join(",", _listAuthorId.Distinct());
         }
      }
      public List<AuthorDto> ListAuthorDto
      {
         get
         {
            return ListAuthorId.Select(n => AuthorDao.GetById(n)).ToList();
         }
      }
      private List<int> _listCategoryId { get; set; }
      public List<int> ListCategoryId
      {
         get
         {
            if (_listCategoryId == null)
            {
               _listCategoryId = CategoryId.Split(new char[] { ',' }).Where(n => !String.IsNullOrEmpty(n.Trim())).Select(n => TypesUtils.Parse.ToInt32(n.Trim())).Distinct().ToList();
            }
            CategoryId = String.Join(",", _listCategoryId.Distinct());
            return _listCategoryId;
         }
         set
         {
            _listCategoryId = value;
            CategoryId = String.Join(",", _listCategoryId.Distinct());
         }
      }
      public List<DefinitionDto> ListCategoryDto
      {
         get
         {
            return ListCategoryId.Select(n => DefinitionDao.GetById(n)).ToList();
         }
      }

   }
}



