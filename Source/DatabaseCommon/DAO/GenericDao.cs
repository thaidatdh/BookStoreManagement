using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class GenericDao<T>
   {
      public static List<T> GetAll()
      {
         return DatabaseUtils.GetEntityList<T>();
      }
      public static int Insert(T dto)
      {
         return DatabaseUtils.InsertEntity<T>(dto);
      }
      public static bool Update(T dto)
      {
         return DatabaseUtils.UpdateEntity<T>(dto) > 0;
      }
      public static T GetById(int Id)
      {
         return DatabaseUtils.GetEntity<T>(Id);
      }

   }
}
