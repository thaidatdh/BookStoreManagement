using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Domain
{
   class AuthorDomain
   {
      public static List<AuthorDomain> Gets() 
      {
         return FileUtils.CSV.GetDataTable(Const.SOURCE_PATH + "/AUTHOR.CSV").Rows.Cast<DataRow>().Select(n => new AuthorDomain(n)).ToList();
      }
      public string ID { get; set; }
      public string Name { get; set; }
      public string Biography { get; set; }
      public AuthorDomain(DataRow dr)
      {
         ID = dr["id"].ToString();
         Name = dr["title"].ToString();
         Biography = dr["biography"].ToString();
      }
   }
}
