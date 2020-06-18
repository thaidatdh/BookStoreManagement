using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Domain
{
   class CategoryDomain
   {
      public static List<CategoryDomain> Gets() 
      {
         return FileUtils.CSV.GetDataTable(Const.SOURCE_PATH + "/SUBJECT.CSV").Rows.Cast<DataRow>().Select(n => new CategoryDomain(n)).ToList();
      }
      public string ID { get; set; }
      public string Name { get; set; }
      public string Biography { get; set; }
      public CategoryDomain(DataRow dr)
      {
         ID = dr["id"].ToString();
         Name = dr["title"].ToString();
      }
   }
}
