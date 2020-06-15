using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using DatabaseCommon.DAO;
using DatabaseCommon.Services;

namespace DatabaseCommon.DTO
{
   [Table("TRANSACTION")]
   public class TransactionDto : ChangeInformation
   {
      public TransactionDto() : base() { }
      public TransactionDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<UserDto>(data, this);
      }
      [DTO(Column = "TRANSACTION_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int TransactionId { get; set; }
      [DTO(Column = "CUSTOMER_ID", DataType = DATATYPE.INTEGER)]
      public int CustomerId { get; set; }
      [DTO(Column = "PROVIDER_ID", DataType = DATATYPE.INTEGER)]
      public int ProviderId { get; set; }
      [DTO(Column = "STAFF_ID", DataType = DATATYPE.INTEGER)]
      public int StaffId { get; set; }
      [DTO(Column = "AMOUNT", DataType = DATATYPE.BIGINT)]
      public long Amount { get; set; }
      [DTO(Column = "DISCOUNT", DataType = DATATYPE.BIGINT)]
      public long Discount { get; set; }
      [DTO(Column = "ENTRY_DATE", DataType = DATATYPE.STRING)]
      public long EntryDate { get; set; }
      [DTO(Column = "TYPE", DataType = DATATYPE.STRING)]
      public string Type { get; set; }

      private List<TransactionDetailDto> listDetail;
      public List<TransactionDetailDto> TransactionDetails 
      { 
         get {
            if (listDetail == null && TransactionId != default(int))
            {
               listDetail = TransactionDetailDao.GetByTransaction(TransactionId); //Get transaction detail
               return listDetail;
            }
            else
               return listDetail;
         }
         set
         {
            listDetail = value;
         }
      }
   }
}

