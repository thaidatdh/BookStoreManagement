﻿using CommonLibrary.Utils;
using DatabaseCommon.Const;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DatabaseCommon.DatabaseUtils;

namespace DatabaseCommon.Services
{
   public class DTOService
   {
      public static Dictionary<string, Entity> EntityMap = new Dictionary<string, Entity>(); // table name -> entity properties
      public static void PassValueByAttribute<T>(Object data, object dto)
      {
         string tableName = DatabaseUtils.GetTableName<T>();
         Entity entity = null;
         if (EntityMap.ContainsKey(tableName))
         {
            entity = EntityMap.GetValue(tableName);
         }
         else
         {
            entity = new Entity();
            entity.TableName = tableName;

            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            DatabaseUtils.GetProperties<T>(ref propertyInfos);

            Type baseType = dto.GetType().BaseType;
            while (baseType != null && !(baseType is Object))
            {
               DatabaseUtils.GetProperties(baseType, ref propertyInfos);
               baseType = baseType.BaseType;
            }

            entity.Properties = propertyInfos;
            entity.AttributeDictionary = new Dictionary<string, DTOAttribute>();
            foreach (PropertyInfo info in propertyInfos)
            {
               DTOAttribute dtoAttr = DatabaseUtils.GetCustomAttribute<T>(info.Name);

               entity.AttributeDictionary[info.Name] = dtoAttr;
            }
            EntityMap[tableName] = entity;
         }
         foreach (PropertyInfo info in entity.Properties)
         {
            DTOAttribute dtoAttr = entity.AttributeDictionary.GetValue(info.Name);
            if (dtoAttr == null) continue;
            object columnValue = GetValue(dtoAttr.Column, dtoAttr.DataType, data);
            if (columnValue == null) continue;
            var columnValueType = columnValue.GetType();
            if (columnValueType == typeof(DBNull)) continue;

            switch (dtoAttr.DataType)
            {
               case DATATYPE.BOOLEAN:
                  info.SetValue(dto, columnValue.ToBoolean()); break;
               case DATATYPE.BIGINT:
                  info.SetValue(dto, columnValue.ToInt64()); break;
               case DATATYPE.GENERATED_ID:
               case DATATYPE.INTEGER:
                  info.SetValue(dto, columnValue.ToInt32()); break;
               case DATATYPE.DATE:
                  info.SetValue(dto, Convert.ToString(columnValue)); break;
               case DATATYPE.DOUBLE:
                  info.SetValue(dto, columnValue.ToDouble()); break;
               case DATATYPE.STRING:
                  if (columnValue != null) info.SetValue(dto, columnValue.ToString());
                  else info.SetValue(dto, columnValue);
                  break;
               case DATATYPE.TIMESTAMP:
                  if (columnValue != null) info.SetValue(dto, TypesUtils.Parse.ToDateTime(columnValue.ToString()));
                  break;
               default:
                  info.SetValue(dto, columnValue); break;
            }
         }
      }
      public static object GetValue(string columnName, DATATYPE dataType, Object data)
      {
         DataRow dr = (DataRow)data;
         if (!dr.Table.Columns.Contains(columnName)) return "";

         switch (dataType)
         {
            case DATATYPE.BOOLEAN:
               return (dr[columnName] == null || dr[columnName].Equals(0)) ? false : dr[columnName];
            case DATATYPE.GENERATED_ID:
            case DATATYPE.INTEGER:
               return dr[columnName];
            case DATATYPE.STRING:
               return dr[columnName];
            case DATATYPE.TIMESTAMP:
               object dataValue = dr[columnName];
               return dataValue;
            case DATATYPE.DOUBLE:
               return dr[columnName];
            case DATATYPE.BIGINT:
               return dr[columnName];
            case DATATYPE.DATE:
               return dr[columnName] == DBNull.Value ? null : dr[columnName];
            default:
               return dr[columnName];
         }
      }
   }
}
