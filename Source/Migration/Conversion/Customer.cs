using CommonLibrary;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using Migration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCommon.Const;
namespace Migration.Conversion
{
   class Customer
   {
      public static void Migrate() 
      {
         LoggerMigration.StartMigrate("CUSTOMER");
         List<CustomerDomain> listDomain = CustomerDomain.Gets();
         int count = 0, total = listDomain.Count;
         foreach (CustomerDomain domain in listDomain)
         {
            if (String.IsNullOrEmpty(domain.FirstName + domain.LastName)) continue;
            CustomerDto dto = new CustomerDto();
            dto.FirstName = domain.FirstName;
            dto.LastName = domain.LastName;
            dto.Email = domain.Email;
            dto.Phone = FormatUtils.formatPhone(domain.Phone);
            dto.DOB = FormatUtils.FormatDate(domain.Birthdate);
            dto.Gender = domain.Sex.Equals("0") ? CONST.USERS.GENDER_MALE : CONST.USERS.GENDER_FEMALE;
            dto.Address = (domain.Address + ", " + domain.City + ", " + domain.State).Trim(new char[] { ' ', ',' });
            dto.CreateBy = Const.DEFAULT_STAFF_ID;
            dto.UpdatedBy = Const.DEFAULT_STAFF_ID;
            int id = CustomerDao.Insert(dto);
            LoggerMigration.log(++count, total, "Insert customer " + dto.FirstName + " " + dto.LastName);
         }
         LoggerMigration.EndMigrate();
      }
   }
}
