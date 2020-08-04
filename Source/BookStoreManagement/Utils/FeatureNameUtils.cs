using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.Utils
{
   public class FeatureNameUtils
   {
      public class FeatureGroup
      {
         public const string TRANSACTION_MANAGEMENT = "Transaction Management";
         public const string BOOK_MANAGEMENT = "Book Management";
         public const string AUTHORIZATION = "Authorization";
         public const string CATEGORY_MANAGEMENT = "Category Management";
            public const string PROVIDER_MANAGEMENT = "Provider Management";
            public const string PUBLISHER_MANAGEMENT = "Publisher Management";
            public const string AUTHOR_MANAGEMENT = "Author Management";
        }
      public class Transaction
      {
         public const string TRANSACTION_MANAGEMENT = "Transaction Management";
         public const string EDIT_STAFF = "Edit Staff Transaction";
         public const string EDIT_PROVIDER = "Edit Provider Transaction";
         public const string EDIT_SALE = "Edit Sale Transaction";
         public const string DELETE = "Delete Transaction";
         public const string NEW_STAFF = "Add New Staff Transaction";
         public const string NEW_SALE = "Add New Customer Transaction";
         public const string NEW_PROVIDER = "Add New Provider Transaction";
      }
      public class Book
      {
         public const string MANAGEMENT = "Book Management";
         public const string IMPORT = "Import Books";
         public const string EDIT = "Edit Book";
         public const string DELETE = "Delete Book";
         public const string NEW = "Add New Book";
      }
      public class Category
      {
         public const string MANAGEMENT = "Category Management";
         public const string EDIT = "Edit Category";
         public const string DELETE = "Delete Category";
         public const string NEW = "Add New Category";
      }
      public class Authorization
      {
         public const string MANAGEMENT = "User Authorization";
      }
        public class Provider
        {
            public const string MANAGEMENT = "Provider Management";
            public const string IMPORT = "Import Provider";
            public const string EDIT = "Edit Provider";
            public const string DELETE = "Delete Provider";
            public const string NEW = "Add New Provider";
        }

        public class Publisher
        {
            public const string MANAGEMENT = "Publisher Management";
            public const string IMPORT = "Import Publisher";
            public const string EDIT = "Edit Publisher";
            public const string DELETE = "Delete Publisher";
            public const string NEW = "Add New Publisher";
        }

        public class Author
        {
            public const string MANAGEMENT = "Author Management";
            public const string IMPORT = "Import Author";
            public const string EDIT = "Edit Author";
            public const string DELETE = "Delete Author";
            public const string NEW = "Add New Author";
        }
    }
}
