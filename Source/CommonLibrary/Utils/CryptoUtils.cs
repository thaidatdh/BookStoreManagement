using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CommonLibrary.Utils
{
   public class CryptoUtils
   {
      public static string Base64Encode(string text)
      {
         var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
         return Convert.ToBase64String(textBytes);
      }
      public static string encryptSHA256(string text)
      {
         using (SHA256 sha = SHA256.Create())
         {
            // ComputeHash - returns byte array  
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Convert byte array to a string   
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
               result.Append(bytes[i].ToString("x2"));
            }
            return result.ToString();
         }
      }
   }
}