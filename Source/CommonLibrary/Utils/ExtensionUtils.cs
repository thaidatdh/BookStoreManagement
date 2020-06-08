using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.Utils
{
   public static class ExtensionUtils
   {
      public static TValue GetValue<TKey,TValue>(this Dictionary<TKey,TValue> dictionary, TKey key)
      {
         return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
      }
      public static int ToInt32(this object value)
      {
         try
         {
            return Convert.ToInt32(value);
         }
         catch
         {
            return 0;
         }
      }
      public static long ToInt64(this object value)
      {
         try
         {
            return Convert.ToInt64(value);
         }
         catch
         {
            return 0;
         }
      }
      public static bool ToBoolean(this object value)
      {
         try
         {
            return Convert.ToBoolean(value);
         }
         catch
         {
            return false;
         }
      }
      public static double ToDouble(this object value, int digits = -1)
      {
         try
         {
            if (digits == -1)
               return Convert.ToDouble(value);
            return Math.Round(Convert.ToDouble(value), digits);
         }
         catch
         {
            return 0;
         }
      }
      public static string ToNotNullString(this object value)
      {
         return value == null ? "" : value.ToString();
      }
   }
}
