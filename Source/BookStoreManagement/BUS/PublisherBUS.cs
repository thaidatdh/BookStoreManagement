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
      public static List<string> GetProviderNameList()
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
   }
}
