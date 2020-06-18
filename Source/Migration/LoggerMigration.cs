using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration
{
   class LoggerMigration
   {
      public static void log(string msg)
      {
         Console.WriteLine("[INFO][" + DateTime.Now + "] " + msg);
      }
      public static void log(int count, int total, string msg)
      {
         Console.WriteLine("[INFO][" + DateTime.Now + "] [" + count + "/" + total + "] " + msg);
      }
      private static string CURRENT_SECTION = "";
      public static void StartMigrate(string section)
      {
         CURRENT_SECTION = section;
         log("-------------------");
         log("Migrate " + CURRENT_SECTION + " start");
         log("-------------------");
      }
      public static void EndMigrate()
      {
         log("-------------------");
         log("Migrate " + CURRENT_SECTION + " end");
         log("-------------------");
         CURRENT_SECTION = "";
      }
   }
}
