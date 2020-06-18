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
   class Publisher
   {
      public static Dictionary<string, int> PublisherNameMap = new Dictionary<string, int>();
      public static void Migrate() 
      {
         LoggerMigration.StartMigrate("PUBLISHER");
         List<BookDomain> listDomain = BookDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (BookDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.Publisher) || PublisherNameMap.ContainsKey(domain.Publisher.ToKey())) continue;
            PublisherDto dto = new PublisherDto();
            dto.Name = domain.Publisher;
            dto.CreateBy = Const.DEFAULT_STAFF_ID;
            dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
            int id = PublisherDao.Insert(dto);
            PublisherNameMap[dto.Name.ToKey()] = id;
            LoggerMigration.log(++count, total, "Insert publishser " + dto.Name);
         }
         LoggerMigration.EndMigrate();
      }
   }
}
