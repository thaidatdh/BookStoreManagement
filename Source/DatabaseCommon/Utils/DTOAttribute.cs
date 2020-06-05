using DatabaseCommon.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
   public class DTOAttribute : Attribute
   {
      public string Column { get; set; }
      public DATATYPE DataType { get; set; }
      public bool isPrimaryKey { get; set; }
      public string DefaultValue { get; set; }
      public PropertyInfo PropertyInfo { get; set; }

      public DTOAttribute()
      {
         this.Column = "";
         this.isPrimaryKey = false;
         this.DefaultValue = "";
         this.DataType = DATATYPE.STRING;
      }
      public DTOAttribute(string column, string DefaultValue, DATATYPE DataType, bool isPrimaryKey = false)
      {
         this.Column = column;
         this.isPrimaryKey = isPrimaryKey;
         this.DefaultValue = DefaultValue;
         this.DataType = DataType;
      }
   }

}
