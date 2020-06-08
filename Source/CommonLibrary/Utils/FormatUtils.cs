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
      public static string FormatDate(string date)
      {
         try
         {
            DateTime dt = DateTime.Parse(date);
            return dt.ToString("yyyyMMdd");
         }
         catch
         {
            return "";
         }
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
            if (input.Length > 20)
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
            else
            {
               _phone = input;
            }
         }
         return _phone.Replace("\\", "").Replace(@"\", "");
      }
      public static string FormatDate(DateTime date)
      {
         return date.ToString("yyyyMMdd");
      }
      public static DateTime ParseDate(string formatedDate)
      {
         try
         {
            return DateTime.ParseExact(formatedDate, "yyyyMMdd", null);
         }
         catch
         {
            return DateTime.MinValue;
         }
      }
   }
}
