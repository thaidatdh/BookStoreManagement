using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.BUS
{
   public class PublisherBUS
   {
      private static List<PublisherDto> publisherList = new List<PublisherDto>();
        private static List<PublisherDto> allNotDeletedPublishers = new List<PublisherDto>();
        public static List<PublisherDto> GetAllNotDeletedPublishers()
        {
            if (allNotDeletedPublishers == null || allNotDeletedPublishers.Count == 0)
            {
                allNotDeletedPublishers = PublisherDao.Where(n => n.IsDeleted == false).ToList();
            }
            return allNotDeletedPublishers;
        }

        public static List<string> GetPublisherNameList()
        {
            if (publisherList == null || publisherList.Count == 0)
            {
            publisherList = PublisherDao.Where(n => n.IsDeleted == false).ToList();
            }
            return publisherList.Select(n => n.Name).ToList();
        }
      public static int GetPublisherId(string name)
      {
         if (publisherList == null || publisherList.Count == 0)
         {
            publisherList = PublisherDao.Where(n => n.IsDeleted == false).ToList();
         }
         PublisherDto dto = publisherList.FirstOrDefault(n => n.Name.Equals(name));
         if (dto == null)
            dto = PublisherDao.Where(n => n.Name.Equals(name)).ToList().FirstOrDefault();
         if (dto == null)
            return 0;
         else
            return dto.PublisherId;
      }

        public static bool Update(PublisherDto dto)
        {

            PublisherDto oldDto = allNotDeletedPublishers.FirstOrDefault(n => n.PublisherId == dto.PublisherId);
            bool result = PublisherDao.Update(dto);
            if (result)
            {
                if (oldDto != null)
                allNotDeletedPublishers.Remove(oldDto);
                allNotDeletedPublishers.Add(dto);
                allNotDeletedPublishers = allNotDeletedPublishers.OrderBy(n => n.PublisherId).ToList();
            }
            return result;

        }
        public static int Insert(PublisherDto dto)
        {
            int id = PublisherDao.Insert(dto);
            dto.PublisherId = id;
            allNotDeletedPublishers.Add(dto);
            allNotDeletedPublishers = allNotDeletedPublishers.OrderByDescending(n => n.PublisherId).ToList();
            return id;
        }
        public static bool Delete(PublisherDto dto)
        {
            allNotDeletedPublishers.Remove(dto);
            return PublisherDao.Delete(dto.PublisherId);
        }
   }
}
