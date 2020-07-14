using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections;
using CommonLibrary.Utils;
using DatabaseCommon.Const;

namespace DatabaseCommon
{
   public class DatabaseUtils
   {
      public class Entity
      {
         public string TableName { get; set; }
         public List<PropertyInfo> Properties { get; set; }
         public Dictionary<string, DTOAttribute> AttributeDictionary { get; set; }
         public DTOAttribute PrimaryKeyAttribute { get; set; }
         public string PrimaryKeyPropertyName { get; set; }
      }
      public static Dictionary<string, Entity> EntityMap = new Dictionary<string, Entity>(); // table name -> entity properties
      public static Dictionary<Type, List<PropertyInfo>> DTOProperties;
      private static SqlConnection connection;
      private static String ConnectionString
      {
         get
         {
            //return @"Server=.;Database=BSDB;User Id=sa;Password=hello123;";
            return @"Server=.;Database=BSDB;Integrated Security = True;";
         }
      }
      private static SqlConnection Connection
      {
         get
         {
            if (connection == null || !connection.State.Equals(ConnectionState.Open))
            {
               connection = new SqlConnection(ConnectionString);
               connection.Open();
            }
            return connection;
         }
      }
      public static bool Open()
      {
         SqlConnection cnn = Connection;
         if (cnn != null && cnn.State.Equals(ConnectionState.Open))
            return true;
         return false;
      }
      public static DataTable ExcuteSelectQuery(string sql)
      {
         SqlCommand command = new SqlCommand(sql, Connection);
         DataTable result = ExcuteSelectQuery(command);
         command.Dispose();
         return result;
      }
      public static DataTable ExcuteSelectQuery(SqlCommand command, string TableName = "TABLE")
      {
         SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
         DataTable result = new DataTable(TableName);
         dataAdapter.Fill(result);
         dataAdapter.Dispose();
         return result;
      }
      public static int ExecuteQuery(string sql)
      {
         SqlCommand command = new SqlCommand(sql, Connection);
         int rowAffected = command.ExecuteNonQuery();
         command.Dispose();
         return rowAffected;
      }
      public static int ExecuteQuery(SqlCommand command)
      {
         int rowAffected = command.ExecuteNonQuery();
         command.Dispose();
         return rowAffected;
      }
      public static int ExecuteInsertQuery(SqlCommand command)
      {
         int rowAffected = command.ExecuteScalar().ToInt32();
         command.Dispose();
         return rowAffected;
      }
      public static object ExecuteScalar(string sql)
      {
         SqlCommand command = new SqlCommand(sql, Connection);
         object result = command.ExecuteScalar();
         command.Dispose();
         return result;
      }
      public static void CloseConnection()
      {
         if (connection != null && connection.State.Equals(ConnectionState.Open))
            connection.Close();
      }

      //
      public static List<T> GetEntityList<T>(string sql)
      {
         DataTable dt = DatabaseUtils.ExcuteSelectQuery(sql);
         if (dt != null)
         {
            return dt.Rows.Cast<DataRow>().Select(n => GetObject<T>(n)).ToList();
         }
         return new List<T>();
      }
      public static int InsertEntity<T>(T dto, bool insertIncludeParentAttribute = false, bool insertIncludeID = false, bool isIncludeIdentityId = true)
      {
         SqlCommand command = GenerateInsertQuery<T>(dto, insertIncludeParentAttribute, insertIncludeID, isIncludeIdentityId);
         return DatabaseUtils.ExecuteInsertQuery(command);
      }
      public static T GetEntity<T>(string sql)
      {
         Object data = DatabaseUtils.ExcuteSelectQuery(sql);
         if (data != null)
         {
            DataTable dt = (DataTable)data;
            foreach (DataRow dr in dt.Rows)
            {
               return GetObject<T>(dr);
            }
         }
         return default(T);
      }
      public static T GetEntity<T>(int id)
      {
         string tableName = GetTableName<T>();
         string PrimaryKey = GetEntityProperties<T>(tableName).PrimaryKeyPropertyName;
         string sql = "SELECT * FROM " + tableName + " where " + PrimaryKey + " = " + id;
         if (String.IsNullOrEmpty(PrimaryKey)) sql = "SELECT * FROM " + tableName;
         Object data = DatabaseUtils.ExcuteSelectQuery(sql);
         if (data != null)
         {
            DataTable dt = (DataTable)data;
            foreach (DataRow dr in dt.Rows)
            {
               return GetObject<T>(dr);
            }
         }
         return default(T);
      }
      public static int UpdateEntity<T>(T dto, bool includeParentAttribute = false)
      {
         SqlCommand command = GenerateUpdateQuery<T>(dto, includeParentAttribute);
         return ExecuteQuery(command);
      }
      public static long DeleteEntity<T>(long ID)
      {
         SqlCommand command = GenerateDeleteQuery<T>(ID);
         return ExecuteQuery(command);
      }
      public static List<T> GetEntityList<T>()
      {
         List<T> result = new List<T>();
         SqlCommand command = GenerateSelectQuery<T>();
         DataTable dt = DatabaseUtils.ExcuteSelectQuery(command, GetTableName<T>());
         if (dt != null)
         {
            result = dt.Rows.Cast<DataRow>().Select(n => GetObject<T>(n)).ToList();
         }
         return result;
      }
      private static T GetObject<T>(params object[] lstArgument)
      {
         return (T)Activator.CreateInstance(typeof(T), lstArgument);
      }
      public static string GetTableName<T>()
      {
         string str = typeof(T).Name;
         if (typeof(T).IsDefined(typeof(TableAttribute), true))
            str = ((TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), true))[0].Name;
         return str;
      }
      public static void InitDTOProperties()
      {
         DTOProperties = Assembly.GetExecutingAssembly().GetTypes().Where(n => n.IsClass && n.Namespace == "DatabaseCommon.DTO").ToDictionary(k => k, v => v.GetProperties(System.Reflection.BindingFlags.Public
             | System.Reflection.BindingFlags.Instance
             | System.Reflection.BindingFlags.DeclaredOnly).ToList());
      }
      public static void GetProperties<T>(ref List<PropertyInfo> list)
      {
         if (DTOProperties == null || DTOProperties.Count == 0)
            InitDTOProperties();

         list = DTOProperties[typeof(T)];
      }
      public static void GetProperties(Type type, ref List<PropertyInfo> list)
      {
         if (DTOProperties == null || DTOProperties.Count == 0)
            InitDTOProperties();

         list = DTOProperties[type];
      }
      public static DTOAttribute GetCustomAttribute<T>(string propertyName)
      {
         PropertyInfo propertyInfo = typeof(T).GetProperties().FirstOrDefault(n => n.Name.Equals(propertyName));
         if (propertyInfo == null) return null;

         Object[] attribute = propertyInfo.GetCustomAttributes(typeof(DTOAttribute), true);

         if (attribute.Length == 0)
         {
            return null;
         }
         List<DTOAttribute> lst = (from n in attribute select (DTOAttribute)n).ToList();
         DTOAttribute attr = lst.FirstOrDefault();
         if (attr != null)
         {
            attr.PropertyInfo = propertyInfo;
            return attr;
         }

         return null;
      }
      private static Entity GetEntityProperties<T>(string tableName)
      {
         Entity result = new Entity();
         List<PropertyInfo> m_propertyInfos = new List<PropertyInfo>();
         GetProperties<T>(ref m_propertyInfos);

         if (tableName.Equals("") || m_propertyInfos.Count == 0) { return null; }

         result.TableName = tableName;
         result.Properties = m_propertyInfos;
         result.AttributeDictionary = new Dictionary<string, DTOAttribute>();
         if (m_propertyInfos.Count > 0)
         {
            foreach (PropertyInfo info in m_propertyInfos)
            {
               DTOAttribute attr = GetCustomAttribute<T>(info.Name);
               result.AttributeDictionary[info.Name] = attr;
               if (attr.isPrimaryKey && result.PrimaryKeyAttribute == null)
               {
                  result.PrimaryKeyAttribute = attr;
                  result.PrimaryKeyPropertyName = info.Name;
               }
            }
         }
         return result;
      }
      private static Entity GetEntityProperties<T>(string tableName, bool includeParentAttribute = false)
      {
         Entity result = new Entity();
         List<PropertyInfo> m_propertyInfos = new List<PropertyInfo>();
         GetProperties<T>(ref m_propertyInfos);

         if (tableName.Equals("") || m_propertyInfos.Count == 0) { return null; }

         result.TableName = tableName;
         result.Properties = m_propertyInfos;
         result.AttributeDictionary = new Dictionary<string, DTOAttribute>();

         if (includeParentAttribute)
         {
            var m_parentPropertyInfos = new List<PropertyInfo>();
            GetProperties(typeof(T).BaseType, ref m_parentPropertyInfos);
            if (m_parentPropertyInfos != null)
               m_propertyInfos.AddRange(m_parentPropertyInfos);
         }

         if (m_propertyInfos.Count > 0)
         {
            foreach (PropertyInfo info in m_propertyInfos)
            {
               DTOAttribute attr = GetCustomAttribute<T>(info.Name);
               result.AttributeDictionary[info.Name] = attr;
               if (attr.isPrimaryKey && result.PrimaryKeyAttribute == null)
               {
                  result.PrimaryKeyAttribute = attr;
                  result.PrimaryKeyPropertyName = info.Name;
               }
            }
         }
         return result;
      }
      private static SqlCommand GenerateDeleteQuery<T>(long ID = 0)
      {
         string result = "";

         Entity entity = null;
         string tableName = GetTableName<T>();
         if (EntityMap.ContainsKey(tableName))
         {
            entity = EntityMap.GetValue(tableName);
         }
         else
         {
            entity = GetEntityProperties<T>(tableName);
            EntityMap[tableName] = entity;
         }
         if (entity == null) { return null; }
         if (entity.Properties.Count > 0)
         {
            string whereClause = "";

            if (ID != 0)
            {
               if (entity.PrimaryKeyAttribute != null)
               {
                  whereClause += entity.PrimaryKeyAttribute.Column + " = " + ID;
               }
               if (whereClause.Equals("") || ID == 0) { return null; }
               result += String.Format("DELETE FROM {0} WHERE {1}", tableName, whereClause);
            }
            else
            {
               result += String.Format("DELETE FROM {0}", tableName);
            }
         }

         if (result == "")
            return null;

         SqlCommand command = new SqlCommand(result, Connection);
         return command;
      }
      private static SqlCommand GenerateSelectQuery<T>(long ID = -1)
      {
         Hashtable paraMap = new Hashtable();
         string result = "";

         string tableName = GetTableName<T>();
         if (ID > 0)
         {
            Entity entity = GetEntityProperties<T>(tableName);
            if (entity == null) { return null; }
            if (entity.Properties.Count > 0)
            {
               string whereClause = "";

               if (entity.PrimaryKeyAttribute != null)
               {
                  whereClause += entity.PrimaryKeyAttribute.Column + " = " + ID;
               }
               if (whereClause.Equals("") || ID == 0) { return null; }
               result += String.Format("SELECT * FROM {0} WHERE {1}", tableName, whereClause);
            }
         }
         else
         {
            result += String.Format("SELECT * FROM {0}", tableName);
         }

         if (result == "")
            return null;

         SqlCommand command = new SqlCommand(result, Connection);
         return command;
      }
      private static SqlCommand GenerateUpdateQuery<T>(T dto, bool includeParentAttribute = false)
      {
         Hashtable paraMap = new Hashtable();
         string result = "";

         Entity entity = null;
         string tableName = GetTableName<T>();
         if (EntityMap.ContainsKey(tableName))
         {
            entity = EntityMap.GetValue(tableName);
         }
         else
         {
            entity = GetEntityProperties<T>(tableName, includeParentAttribute);
            EntityMap[tableName] = entity;
         }
         if (entity == null) { return null; }
         int index = 1;
         if (entity.Properties.Count > 0)
         {
            string columns = "";
            string whereClause = "";

            foreach (PropertyInfo info in entity.Properties)
            {
               DTOAttribute attr = entity.AttributeDictionary.GetValue(info.Name);
               if (attr.isPrimaryKey || attr.Column.Equals("CREATE_DATE")) continue;
               if (!String.IsNullOrEmpty(columns))
                  columns += ", ";

               columns += attr.Column;
               columns += " = @" + attr.Column;
               paraMap[info.Name] = "@" + attr.Column;
               index++;
            }

            DTOAttribute attrKey = entity.PrimaryKeyAttribute;
            if (attrKey != null)
            {
               whereClause += attrKey.Column + " = @" + attrKey.Column;
               paraMap[entity.PrimaryKeyPropertyName] = "@" + attrKey.Column;
            }
            if (whereClause.Equals("")) { return null; }

            result += String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, columns, whereClause);
         }

         if (result == "")
            return null;

         SqlCommand command = new SqlCommand(result, Connection);
         FillValues<T>(dto, command, entity, paraMap);
         return command;
      }
      private static SqlCommand GenerateInsertQuery<T>(T dto, bool insertIncludeParentAttribute = false,bool insertIncludeID = false, bool isIdentityId = true)
      {
         Hashtable paraMap = new Hashtable();
         string result = "";

         Entity entity = null;
         string tableName = GetTableName<T>();
         if (EntityMap.ContainsKey(tableName))
         {
            entity = EntityMap.GetValue(tableName);
         }
         else
         {
            entity = GetEntityProperties<T>(tableName, insertIncludeParentAttribute); //entity = GetEntityProperties<T>(tableName);
            EntityMap[tableName] = entity;
         }
         if (entity == null) { return null; }
         int index = 1;
         if (entity.Properties.Count > 0)
         {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();

            foreach (PropertyInfo info in entity.Properties)
            {
               DTOAttribute attr = entity.AttributeDictionary.GetValue(info.Name);
               if (attr.isPrimaryKey)
               {
                  if (!insertIncludeID)
                  {
                     continue;
                  }
               }

               if (index > 1)
               {
                  columns.Append(", ");
                  values.Append(", ");
               }

               columns.Append(attr.Column);

               values.Append("@" + attr.Column);
               paraMap[info.Name] = "@" + attr.Column;

               index++;
            }

            result += String.Format("INSERT INTO {0} ({1}) OUTPUT INSERTED.{2} VALUES ({3})", tableName, columns.ToString(), entity.PrimaryKeyAttribute.Column, values.ToString());
         }

         if (result == "")
            return null;
         if (insertIncludeID && isIdentityId)
         {
            string IdentityInsertOn = "SET IDENTITY_INSERT " + tableName + " ON;\n";
            string IdentityInsertOff = "\nSET IDENTITY_INSERT " + tableName + " OFF;";
            result = IdentityInsertOn + result + IdentityInsertOff;
         }
         SqlCommand command = new SqlCommand(result, Connection);
         FillValues<T>(dto, command, entity, paraMap);
         return command;// (Object)command;
      }
      public static void FillValues<T>(T dto, SqlCommand command, Entity entity, Hashtable ParameterMap)
      {
         foreach (DictionaryEntry entry in ParameterMap)
         {
            string propertyName = entry.Key.ToString();
            object value = typeof(T).GetProperty(propertyName).GetValue(dto, (object[])null);
            DTOAttribute attribute = entity.AttributeDictionary.GetValue(propertyName);

            if (attribute.Column.Equals("CREATE_DATE"))
            {
               command.Parameters.Add(entry.Value.ToString(), SqlDbType.NVarChar).Value = DateTime.Now;
               continue;
            }

            if (attribute.Column.Equals("UPDATED_DATE"))
            {
               command.Parameters.Add(entry.Value.ToString(), SqlDbType.NVarChar).Value = DateTime.Now;
               continue;
            }
            //--------------------------------------------------

            var valueType = attribute.DataType;
            var defaultValue = attribute.DefaultValue;

            string paraName = entry.Value.ToString();
            SqlCommand cmd = (SqlCommand)command;
            switch (valueType)
            {
               case DATATYPE.STRING:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.NVarChar).Value = ParseDataSQL(value, valueType, defaultValue).ToString();
                     break;
                  }
               case DATATYPE.INTEGER:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.Int).Value = ParseDataSQL(value, valueType, defaultValue).ToInt32();
                     break;
                  }
               case DATATYPE.BIGINT:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.BigInt).Value = (long)ParseDataSQL(value, valueType, defaultValue);
                     break;
                  }
               case DATATYPE.GENERATED_ID:
                  {
                     int Value = ParseDataSQL(value, valueType, defaultValue).ToInt32();
                     if (Value != 0)
                        cmd.Parameters.Add(paraName, SqlDbType.Int).Value = Value;
                     else
                        cmd.Parameters.Add(paraName, SqlDbType.Int).Value = null;
                     break;
                  }
               case DATATYPE.BOOLEAN:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.Bit).Value = ParseDataSQL(value, valueType, defaultValue).ToBoolean();
                     break;
                  }
               case DATATYPE.TIMESTAMP:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.Timestamp).Value = ParseDataSQL(value, valueType, defaultValue).ToString();

                     break;
                  }
               case DATATYPE.DOUBLE:
                  {
                     int digit = 2;
                     string key = attribute.Column.ToUpper();
                     cmd.Parameters.Add(paraName, SqlDbType.Float).Value = ParseDataSQL(value, valueType, defaultValue).ToDouble(digit);
                     break;
                  }
               case DATATYPE.DATE:
                  {
                     cmd.Parameters.Add(paraName, SqlDbType.NVarChar).Value = ParseDataSQL(value, valueType, defaultValue).ToString();
                     break;
                  }
               default:
                  cmd.Parameters.Add(paraName, SqlDbType.Int).Value = null;
                  break;
            }
         }
      }
      public static object ParseDataSQL(object data, DATATYPE dataType, object defaultValue = null)
      {
         if (data == null)
         {
            if (defaultValue != null)
            {
               data = defaultValue;
            }
         }

         switch (dataType)
         {
            case DATATYPE.STRING:
               if ((data == null || (string)data == "") && defaultValue != null)
               {
                  data = defaultValue;
               }
               return data.ToNotNullString();
            case DATATYPE.INTEGER:
            case DATATYPE.BIGINT:
               if (data == null && defaultValue != null)
               {
                  data = defaultValue;
               }
               return data == null ? 0 : data;
            case DATATYPE.DOUBLE:
               if (data == null && defaultValue != null)
               {
                  data = defaultValue;
               }
               return data == null ? 0.00 : data;
            case DATATYPE.GENERATED_ID:
               if ((data == null || Convert.ToInt32(data) == 0))
               {
                  if (defaultValue != null)
                     return defaultValue;
                  return null;
               }
               return data;
            case DATATYPE.BOOLEAN:
               return data == null ? false : data;
            case DATATYPE.TIMESTAMP:
               if (data != null && !data.ToString().Equals(""))
                  return data.ToString();
               return DateTime.Now;
            case DATATYPE.DATE:
               return data;
            default:
               return null;
         }
      }
   }
}
