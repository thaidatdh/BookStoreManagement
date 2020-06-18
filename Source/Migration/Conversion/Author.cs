using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using Migration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Conversion
{
   class Author
   {
      public static Dictionary<string, int> AuthorNameMap = new Dictionary<string, int>();
      public static void Migrate() 
      {
         LoggerMigration.StartMigrate("AUTHOR");
         List<AuthorDomain> listDomain = AuthorDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (AuthorDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.Name)) continue;
            AuthorDto dto = new AuthorDto();
            dto.Name = domain.Name;
            dto.Note = domain.Biography;
            dto.CreateBy = Const.DEFAULT_STAFF_ID;
            dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
            int id = AuthorDao.Insert(dto);
            AuthorNameMap[dto.Name.ToKey()] = id;
            LoggerMigration.log(++count, total, "Insert author " + dto.Name);
         }
         LoggerMigration.EndMigrate();
      }
      public static int InsertNewAuthor(string Name, string Bio = "")
      {
         string AdditionalInfo = "";
         if (Name.Contains("("))
         {
            AdditionalInfo = Name.Substring(Name.IndexOf("(")).Trim();
            Name = Name.Substring(0, Name.IndexOf("("));
         }
         string NameKey = Name.ToKey();
         if (AuthorNameMap.ContainsKey(NameKey)) 
            return AuthorNameMap.GetValue(NameKey);
         AuthorDto dto = new AuthorDto();
         dto.Name = Name;
         dto.Note = (AdditionalInfo + "\n" + Bio).Trim(new char[] { '\n', ' ' });
         dto.CreateBy = Const.DEFAULT_STAFF_ID;
         dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
         int id = AuthorDao.Insert(dto);
         AuthorNameMap[NameKey] = id;
         LoggerMigration.log("--> Insert new author: " + dto.Name);
         return id;
      }
   }
}
