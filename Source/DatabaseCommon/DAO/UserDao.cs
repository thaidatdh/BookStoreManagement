﻿using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.DAO
{
   public class UserDao : GenericDao<UserDto>
   {
      public static List<UserDto> GetAll()
      {
         return DatabaseUtils.GetEntityList<UserDto>();
      }
      public static int Insert(UserDto dto)
      {
         return DatabaseUtils.InsertEntity<UserDto>(dto, true);
      }
      public static bool Update(UserDto dto)
      {
         return DatabaseUtils.UpdateEntity<UserDto>(dto, true) > 0;
      }
      public static UserDto GetById(int Id)
      {
         return DatabaseUtils.GetEntity<UserDto>(Id);
      }

   }
}
