using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   class CustomerBUS
   {
        private static List<CustomerDto> allNotDeletedMembers = new List<CustomerDto>();
        public static List<CustomerDto> GetAllNotDeletedMembers()
        {
            if (allNotDeletedMembers == null || allNotDeletedMembers.Count == 0)
            {
                allNotDeletedMembers = CustomerDao.Where(n => n.IsDeleted == false).ToList();
            }
            return allNotDeletedMembers;
        }

        public static CustomerDto GetMemberByID(int memberID)
        {
            foreach(CustomerDto e in allNotDeletedMembers)
            {
                if (e.UserId==memberID)
                {
                    return e;
                }
            }
            return null;
        }

        public static bool Delete(int memberID)
        {
            foreach(CustomerDto member in allNotDeletedMembers)
            {
                if (member.UserId==memberID)
                {
                    allNotDeletedMembers.Remove(member);
                    break;
                }
            }
            return CustomerDao.Delete(memberID);
        }

        public static int Insert(CustomerDto member)
        {
            int id = CustomerDao.Insert(member);
            member.UserId = id;
            allNotDeletedMembers.Add(member);
            allNotDeletedMembers.OrderBy(n => n.UserId);
            return id;
        }

        public static bool Update(CustomerDto member)
        {
            bool result = CustomerDao.Update(member);
            if (result)
            {
                CustomerDto oldDto = allNotDeletedMembers.FirstOrDefault(n => n.UserId == member.UserId);
                if (oldDto != null)
                    allNotDeletedMembers.Remove(oldDto);
                allNotDeletedMembers.Add(member);
                allNotDeletedMembers.OrderBy(n => n.UserId);
            }
            return result;
        }
    }
}
