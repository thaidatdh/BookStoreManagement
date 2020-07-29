using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Utils
{
   public class TypesUtils
   {
      public class Parse
      {
         public static DateTime ToDateTime(object obj)
         {
            try
            {
               return DateTime.Parse(obj.ToString());
            }
            catch
            {
               return DateTime.Now;
            }
         }
         public static int ToInt32(object obj)
         {
            try
            {
               return Convert.ToInt32(obj);
            }
            catch
            {
               return 0;
            }
         }
         public static long ToInt64(object obj)
         {
            try
            {
               return Convert.ToInt64(obj);
            }
            catch
            {
               return 0;
            }
         }
      }
   }
}
