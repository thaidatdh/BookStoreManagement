﻿using System;
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
   }
}