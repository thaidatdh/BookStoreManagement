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
   [Table("TRANSACTIONS")]
   public class TransactionDto : ChangeInformation
   {
      public TransactionDto() : base() { }
      public TransactionDto(Object data) : base(data)
      {
         DTOService.PassValueByAttribute<TransactionDto>(data, this);
      }
      [DTO(Column = "TRANSACTION_ID", DataType = DATATYPE.GENERATED_ID, isPrimaryKey = true)]
      public int TransactionId { get; set; }
      [DTO(Column = "CUSTOMER_ID", DataType = DATATYPE.INTEGER)]
      public int? CustomerId { get; set; }
      [DTO(Column = "PROVIDER_ID", DataType = DATATYPE.INTEGER)]
      public int? ProviderId { get; set; }
      [DTO(Column = "STAFF_ID", DataType = DATATYPE.INTEGER)]
      public int? StaffId { get; set; }
      [DTO(Column = "AMOUNT", DataType = DATATYPE.BIGINT)]
      public long Amount { get; set; }
      [DTO(Column = "DISCOUNT", DataType = DATATYPE.BIGINT)]
      public long Discount { get; set; }
      [DTO(Column = "ENTRY_DATE", DataType = DATATYPE.STRING)]
      public string EntryDate { get; set; }
      [DTO(Column = "TYPE", DataType = DATATYPE.STRING)]
      public string Type { get; set; }
      [DTO(Column = "IS_DELETED", DataType = DATATYPE.BOOLEAN)]
      public bool IsDeleted { get; set; }

      private List<TransactionDetailDto> listDetail;
      public List<TransactionDetailDto> TransactionDetails 
      { 
         get {
            if (listDetail == null && TransactionId != default(int))
            {
               listDetail = TransactionDetailDao.GetByTransaction(TransactionId); //Get transaction detail
            }
            if (listDetail == null || TransactionId == default(int))
            {
               listDetail = new List<TransactionDetailDto>();
            }
            return listDetail;
         }
         set
         {
            listDetail = value;
         }
      }
      public string Description
      {
         get
         {
            if (CustomerId != null && CustomerId.Value != 0)
               return "Sale Transaction " + ReceiverName;
            else if (ProviderId != null && ProviderId.Value != 0)
               return "Import Transaction " + ReceiverName;
            else if (StaffId != null && StaffId.Value != 0)
               return "Transaction for Staff " + ReceiverName;
            else
               return "Uncategorized";
         }
      }
      public ProviderDto ProviderDto
      {
         get
         {
            if (ProviderId == null || ProviderId.Value < 0)
               return null;
            return ProviderDao.GetById(ProviderId.Value);
         }
      }
      public StaffDto StaffDto
      {
         get
         {
            if (StaffId == null || StaffId.Value < 0)
               return null;
            return StaffDao.GetById(StaffId.Value);
         }
      }
      public CustomerDto CustomerDto
      {
         get
         {
            if (CustomerId == null || CustomerId.Value < 0)
               return null;
            return CustomerDao.GetById(CustomerId.Value);
         }
      }
      public string ReceiverName
      {
         get
         {
            if (CustomerId != null && CustomerId.Value != 0)
               return CustomerDto == null ? "" : (CustomerDto.FirstName + " " + CustomerDto.LastName).Trim();
            else if (ProviderId != null && ProviderId.Value != 0)
               return ProviderDto == null ? "" : ProviderDto.Name.Trim();
            else if (StaffId != null && StaffId.Value != 0)
               return StaffDto == null ? "" : (StaffDto.FirstName + " " + StaffDto.LastName).Trim();
            else
               return "";
         }
      }

   }
}

