using DatabaseCommon.DTO;
using Migration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
using CommonLibrary.Utils;
using DatabaseCommon.DAO;

namespace Migration.Conversion
{
   class Staff
   {
      public static Dictionary<string, int> StaffMap = new Dictionary<string, int>();
      public static void Migrate() 
      {
         LoggerMigration.StartMigrate("STAFF");
         List<StaffDomain> listDomain = StaffDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (StaffDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.FirstName + domain.LastName)) continue;
            StaffDto dto = new StaffDto();
            dto.FirstName = domain.FirstName;
            dto.LastName = domain.LastName;
            dto.Username = domain.StaffNum;
            dto.CreateBy = Const.ADMIN_STAFF_ID;
            dto.UpdatedBy = Const.ADMIN_STAFF_ID;
            dto.Password = CryptoUtils.encryptSHA256(CONST.STAFF.DEFAULT_PASSWORD);
            int id = StaffDao.Insert(dto);
            StaffMap[domain.StaffNum] = id;
            if (!String.IsNullOrEmpty(Const.DEFAULT_STAFF_VALUE) && domain.StaffNum.Equals(Const.DEFAULT_STAFF_VALUE))
            {
               Const.DEFAULT_STAFF_ID = id;
            }
            LoggerMigration.log(++count, total, "Insert staff " + dto.FirstName + " " + dto.LastName);
         }
         LoggerMigration.EndMigrate();
      }
   }
}
