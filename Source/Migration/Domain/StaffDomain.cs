using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Domain
{
   class StaffDomain
   {
      public static List<StaffDomain> Gets() 
      {
         return FileUtils.CSV.GetDataTable(Const.SOURCE_PATH + "/STAFF.CSV").Rows.Cast<DataRow>().Select(n => new StaffDomain(n)).ToList();
      }
      public string StaffNum { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public StaffDomain(DataRow dr)
      {
         StaffNum = dr["StaffNum"].ToString();
         FirstName = dr["FName"].ToString();
         LastName = dr["LName"].ToString();
      }
   }
}
