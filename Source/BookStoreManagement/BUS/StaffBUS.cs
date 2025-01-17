﻿using DatabaseCommon.DAO;
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
      private static List<StaffDto> allStaffs = new List<StaffDto>();
      public static int Login(string username, string encryptedPassword)
      {
         StaffDto dto = StaffDao.Where(n => n.Username.Equals(username) && n.Password.Equals(encryptedPassword)).FirstOrDefault();
         if (dto == null) 
            return 0;
         Config.Manager.CURRENT_USER = dto;
         DatabaseCommon.DatabaseUtils.CurrentUserId = dto.UserId;
         return dto.Active ? 1 : -1;
      }
      public static void Logout()
      {
         Config.Manager.CURRENT_USER = null;
         DatabaseCommon.DatabaseUtils.CurrentUserId = 0;
      }

        
      public static List<StaffDto> GetAllStaffs()
      {
          if (allStaffs == null || allStaffs.Count == 0)
          {
                allStaffs = StaffDao.GetAll().ToList();
          }
          return allStaffs;
      }

      public static StaffDto GetStaffByID(int staffID)
      {
          foreach(StaffDto e in allStaffs)
          {
              if (e.StaffId == staffID)
              {
                  return e;
              }
          }
          return null;
      }

      public static int Insert(StaffDto staff)
      {
          int id = StaffDao.Insert(staff);
          staff.UserId = id;
          allStaffs.Add(staff);
          allStaffs.OrderBy(n => n.UserId);
          return id;
      }

      public static bool update(StaffDto staff)
      {
          bool result = StaffDao.Update(staff);
          if (result)
          {
              StaffDto oldDto = allStaffs.FirstOrDefault(n => n.UserId == staff.UserId);
              if (oldDto != null)
                  allStaffs.Remove(oldDto);
              allStaffs.Add(staff);
              allStaffs.OrderBy(n => n.UserId);
          }
          return result;
        }

      public static bool delete(int UserId)
      {
          foreach (StaffDto staff in allStaffs)
            {
                if (staff.UserId == UserId)
                {
                    allStaffs.Remove(staff);
                    break;
                }
            }
          return StaffDao.delete(UserId);
      }

      public static bool checkPassword(int staffID, string password)
      {
          StaffDto dto = null;
          dto = StaffDao.Where(n => n.StaffId==staffID && n.Password.Equals(password)).FirstOrDefault();
          return dto != null;
      }

      public static bool changePassword(int staffID, string password)
      {
            return StaffDao.updatePassword(staffID, password);
      }
    }
}
