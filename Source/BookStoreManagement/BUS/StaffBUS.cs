using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   class StaffBUS
   {
      public static bool Login(string username, string encryptedPassword)
      {
         StaffDto dto = StaffDao.Where(n => n.Username.Equals(username) && n.Password.Equals(encryptedPassword) && n.Active == true).FirstOrDefault();
         if (dto == null) 
            return false;
         Config.Manager.CURRENT_USER = dto;
         return true;
      }
   }
}
