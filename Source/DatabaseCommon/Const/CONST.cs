using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon.Const
{
   public class CONST
   {
      public class USERS
      {
         public const string USER_TYPE_CUSTOMER = "CUSTOMER";
         public const string USER_TYPE_ADMIN = "ADMIN";
         public const string USER_TYPE_STAFF = "STAFF";
         public const string DEFAULT_USER_TYPE = USER_TYPE_CUSTOMER;

         public const string GENDER_MALE = "MALE";
         public const string GENDER_FEMALE = "FEMALE";
         public const string GENDER_NOT_SPECIFY = "NOT_SPECIFY";

         public const string DEFAULT_PHOTO_LINK = "\\persistent\\images\\default_user_photo.png";
      }
      public class STAFF
      {
         public const string DEFAULT_PASSWORD = "hello123";
         public const string DEFAULT_ENCRYPTED_PASSWORD = "27cc6994fc1c01ce6659c6bddca9b69c4c6a9418065e612c69d110b3f7b11f8a";
      }
      public class DEFINITION
      {
         public const string STORE_NAME = "STORE";
         public const string STORE_ADDRESS = "ADDRESS";
         public const string STORE_CONTACT = "CONTACT";

         public const int DEFINITION_TYPE_STORE = 1;
         public const int DEFINITION_TYPE_AUTHORIZATION = 2;
         public const int DEFINITION_TYPE_CATEGORY = 3;
      }
      public class BOOK
      {
         public const string DEFAULT_PHOTO_LINK = "\\persistent\\images\\default_book_photo.png";
      }
   }
}
