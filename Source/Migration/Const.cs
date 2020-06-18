using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration
{
   public class Const
   {
      public static string SOURCE_PATH = "";
      public static int DEFAULT_STAFF_ID = 1;
      public static int ADMIN_STAFF_ID = 1;
      public static string DEFAULT_STAFF_VALUE = "";
      public static List<string> RequireFiles = new List<string>()
      {
         "STAFF.CSV",
         "CUSTOMER.CSV",
         "BOOK.CSV",
         "SUBJECT.CSV",
         "AUTHOR.CSV",
      };
      public static List<string> MigrateSection = new List<string>()
      {
         "STAFF",
         "CUSTOMER",
         "CATEGORY",
         "PUBLISHER",
         "AUTHOR",
         "BOOK",
      };
      public static Dictionary<string, string[]> RequireSection = new Dictionary<string, string[]>()
      {
         {"STAFF", new string[] {  } },
         {"CUSTOMER", new string[] {  }},
         {"CATEGORY", new string[] {  }},
         {"PUBLISHER", new string[] {  }},
         {"AUTHOR", new string[] {  }},
         {"BOOK", new string[] { "AUTHOR" , "CATEGORY", "PUBLISHER" } },
      };
      public static Dictionary<string, string[]> RequireFilesSection = new Dictionary<string, string[]>()
      {
         {"STAFF", new string[] {  "STAFF.CSV"} },
         {"CUSTOMER", new string[] {  "CUSTOMER.CSV"}},
         {"CATEGORY", new string[] { "SUBJECT.CSV"  }},
         {"PUBLISHER", new string[] {   "BOOK.CSV"}},
         {"AUTHOR", new string[] {  "AUTHOR.CSV"}},
         {"BOOK", new string[] { "BOOK.CSV" } },
      };
   }
}
