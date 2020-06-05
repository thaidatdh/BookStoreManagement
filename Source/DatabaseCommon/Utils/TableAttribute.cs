using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCommon
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
   internal class TableAttribute : Attribute
   {
      public string Name { get; set; }
      public TableAttribute(string name)
      {
         this.Name = name;
      }
   }
}
