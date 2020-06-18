using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Domain
{
   class CustomerDomain
   {
      public static List<CustomerDomain> Gets() 
      {
         return FileUtils.CSV.GetDataTable(Const.SOURCE_PATH + "/CUSTOMER.CSV").Rows.Cast<DataRow>().Select(n => new CustomerDomain(n)).ToList();
      }
      public string PersonID { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Address { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string Phone { get; set; }
      public string Email { get; set; }
      public string Birthdate { get; set; }
      public string Sex { get; set; }
      public CustomerDomain(DataRow dr)
      {
         PersonID = dr["PersonID"].ToString();
         FirstName = dr["FirstName"].ToString();
         LastName = dr["LastName"].ToString();
         Address = dr["Address"].ToString();
         City = dr["City"].ToString();
         State = dr["State"].ToString();
         Phone = dr["phone"].ToString();
         Email = dr["Email"].ToString();
         Birthdate = dr["Birthdate"].ToString();
         Sex = dr["Sex"].ToString();
      }
   }
}
