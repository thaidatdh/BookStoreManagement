using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLibrary
{
   public class FormatUtils
   {
      private const string AES_ENCRYPT_KEY = "BSDB!THCMUSTKPM";
      public static string FormatDate(string date)
      {
         try
         {
            DateTime dt = DateTime.Parse(date);
            return dt.ToString("yyyyMMdd");
         }
         catch
         {
            return Regex.Replace(date, "[^0-9]","");
         }
      }
      public static string FormatMoney(long value)
      {
         return value.ToString("#,###");
      }
      public static object FormatMoney(string value, bool toString = false)
      {
         long moneyLong = FormatMoney(value);
         if (!toString)
         {
            return moneyLong;
         }
         else
         {
            return FormatMoney(moneyLong);
         }
      }
      public static long FormatMoney(string value)
      {
         return TypesUtils.Parse.ToInt64(Regex.Replace(value, "[^0-9]",""));
      }
      private static String trimCharacter(String input)
      {
         input = input.Trim();
         String pattern = @"[-()\\s]";
         Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
         input = regex.Replace(input, "");
         regex = null;
         pattern = null;
         return input;
      }
      private static string getDigits(string input)
      {
         String str = "";
         bool flag = true;
         Regex regex = new Regex(@"\d+");
         Match match = regex.Match(input);
         if (match.Success)
         {
            str += match.Value;
            while (flag)
            {
               match = match.NextMatch();
               if (match.Success)
                  str += match.Value;
               else
                  flag = false;
            }
         }
         return str;
      }
      public static String formatPhone(String input)
      {
         string _phone = input.Replace("_", "");
         if (_phone == null || "".Equals(_phone))
         {
            return "";
         }

         _phone = trimCharacter(_phone);
         if (_phone.Length == 10)
         {
            _phone = _phone.Insert(6, "-");
            _phone = _phone.Insert(3, "-");
         }
         else
         {
            string digits = getDigits(input);
            if (digits.Length >= 10 && digits.Length < 20)
            {
               _phone = digits.Insert(6, "-");
               _phone = digits.Insert(3, "-");
            }
            else if (digits.Length > 20)
            {
               _phone = digits.Substring(0, 10);
               _phone = _phone.Insert(6, "-");
               _phone = _phone.Insert(3, "-");
            }
         }
         return _phone.Replace("\\", "").Replace(@"\", "");
      }
      public static string FormatDate(DateTime date)
      {
         try
         {
            return date.ToString("yyyyMMdd");
         }
         catch
         {
            return "";
         }
      }
      public static DateTime? ParseDate(string formatedDate)
      {
         try
         {
            return DateTime.ParseExact(formatedDate, "yyyyMMdd", null);
         }
         catch
         {
            return ToDate(formatedDate);
         }
      }
      public static DateTime? ToDate(string formatedDate)
      {
         try
         {
            return DateTime.Parse(formatedDate);
         }
         catch
         {
            return null;
         }
      }
   }
}
