using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.Utils
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public class FeatureAttribute : Attribute
   {
      public string Name { get; set; }
      public int Id { get; set; }
      public string Group { get; set; }
      public PropertyInfo PropertyInfo { get; set; }

      public FeatureAttribute()
      {
         this.Name = "";
         this.Id = 0;
         this.Group = "";
      }
      public FeatureAttribute(int id, string name, string group)
      {
         this.Name = name;
         this.Id = id;
         this.Group = group;
      }
   }
}
