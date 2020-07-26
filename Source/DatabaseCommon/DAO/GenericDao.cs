using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
      public static Expression<Func<T, S>> Select<S>(Expression<Func<T, S>> predicate)
      {
         return predicate;
      }

      public static IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
      {
         if (predicate == null)
            return null;

         Type objectType = typeof(T);

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return null;

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = GenericDaoHelper.CreateLinkInheritancePart(objectType, predicate.Parameters[0].Name, alphabetExpressionMap);
         var query = GenericDaoHelper.CreateWherePart(predicate.Body, alphabetExpressionMap);

         return DatabaseUtils.GetEntityList<T>(String.Format("SELECT * FROM {0} WHERE {1}", fromLink, query.Replace("\"", "'")));
      }

      public static IEnumerable<T> Where(string rawQuery)
      {
         Type objectType = typeof(T);

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return null;

         return DatabaseUtils.GetEntityList<T>(String.Format("SELECT * FROM {0} WHERE {1}", tableName, rawQuery));
      }

      public static T First(Expression<Func<T, bool>> predicate)
      {
         if (predicate == null)
            return default(T);

         Type objectType = predicate.Parameters[0].Type;

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return default(T);

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = GenericDaoHelper.CreateLinkInheritancePart(objectType, predicate.Parameters[0].Name, alphabetExpressionMap);
         var query = GenericDaoHelper.CreateWherePart(predicate.Body, alphabetExpressionMap);

         return DatabaseUtils.GetEntity<T>(String.Format("SELECT * FROM {0} WHERE {1} LIMIT 1", fromLink, query.Replace("\"", "'")));
      }

      public static bool Exist(Expression<Func<T, bool>> predicate)
      {
         if (predicate == null)
            return false;

         Type objectType = predicate.Parameters[0].Type;

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return false;

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = GenericDaoHelper.CreateLinkInheritancePart(objectType, predicate.Parameters[0].Name, alphabetExpressionMap);
         var query = GenericDaoHelper.CreateWherePart(predicate.Body, alphabetExpressionMap);

         return (long)DatabaseUtils.GetPropertyValue("SELECT EXISTS(SELECT * FROM "+ fromLink + " WHERE "+ query.Replace("\"", "'") + " LIMIT 1) as RESULT", "RESULT") == 1 ? true : false;
      }
   }
   public static class GenericDaoHelper
   {
      private static Dictionary<ExpressionType, string> actionType = new Dictionary<ExpressionType, string>()
      {
         { ExpressionType.Equal, "=" },
         {  ExpressionType.AndAlso, "and" },
         {  ExpressionType.OrElse, "or" },
         {  ExpressionType.GreaterThan, ">" },
         {  ExpressionType.GreaterThanOrEqual, ">=" },
         {  ExpressionType.LessThan, "<" },
         {  ExpressionType.LessThanOrEqual, "<=" },
         {  ExpressionType.NotEqual, "<>" }
      };
      private static Dictionary<string, string> reservedActionType = new Dictionary<string, string>()
      {
         {  "=", "<>" },
         {  "<>", "=" },
         {  ">", "<=" },
         {  ">=", "<" },
         {  "<", ">=" },
         {  "<=", ">" },
         {  "and", "or" },
         {  "or", "and" },
         { "like \"{0}%\"", "not like \"{0}%\""},
         { "like \"%{0}\"", "not like \"%{0}\""},
         { "like \"%{0}%\"", "not like \"%{0}%\""},
      };
      private static Dictionary<string, string> methodType = new Dictionary<string, string>()
      {
         { "Equals",  "=" },
         { "StartsWith",  "like \"{0}%\""},
         { "EndsWith",  "like \"%{0}\""},
         { "Contains",  "like \"%{0}%\""},
      };

      internal static string CreateLinkInheritancePart(Type type, string alphabet = "n", Dictionary<Type, string> alphabetExpressionMap = null)
      {
         var tableName = DatabaseUtils.GetTableName(type);
         var inheritanceColumn = DatabaseUtils.GetInheritanceColumn(type);

         alphabetExpressionMap[type] = alphabet;

         if (type.BaseType != null && type.BaseType != typeof(Object) && type.BaseType != typeof(ChangeInformation))
         {
            string nextAlphabet = alphabet.Remove(alphabet.Length - 1, 1) + Convert.ToChar(Convert.ToUInt16(alphabet[alphabet.Length - 1]) + 1);

            var baseTableName = DatabaseUtils.GetTableName(type.BaseType);
            return CreateLinkInheritancePart(type.BaseType, nextAlphabet, alphabetExpressionMap) + " " + String.Format("JOIN {0} AS {1} ON {2}.{3} = {1}.{3}", tableName, alphabet, nextAlphabet, inheritanceColumn);
         }

         return String.Format("{0} AS {1}", tableName, alphabet);
      }
      internal static string CreateWherePart(Expression body, Dictionary<Type, string> alphabetExpressionMap)
      {
         string result = "";

         Expression mainBody = body;

         bool isReserved = false;

         string formatExpressionString = mainBody.ToString().StartsWith("(") ? "({0} {1} {2})" : "{0} {1} {2}";
         if (body is UnaryExpression)
         {
            mainBody = (mainBody as UnaryExpression).Operand;
            formatExpressionString = "not ({0} {1} {2})";
            isReserved = true;
         }

         if (mainBody is BinaryExpression)
         {
            var action = mainBody.NodeType;

            var bodyExpression = (mainBody as BinaryExpression);
            if (bodyExpression.Left is MemberExpression && bodyExpression.Right is ConstantExpression)
            {
               var leftExpresson = bodyExpression.Left as MemberExpression;
               var rightExpression = bodyExpression.Right as ConstantExpression;

               var valueName = leftExpresson.Member.Name;
               var columnName = String.Format("{0}.{1}", alphabetExpressionMap[leftExpresson.Member.DeclaringType], DatabaseUtils.GetCustomAttribute(leftExpresson.Member.ReflectedType, valueName).Column);
               var value = HandleQueryValue(rightExpression.Value);

               string ActionType = string.Empty;
               if (value == null || value.ToLower().Equals("null"))
                  ActionType = "is";
               else
               {
                  if (!isReserved) ActionType = actionType.GetValue(action);
                  else ActionType = reservedActionType[actionType[action]];
               }

               result = String.Format(formatExpressionString, columnName, ActionType, value);
            }
            else if (bodyExpression.Left is MemberExpression && bodyExpression.Right is MemberExpression)
            {
               var leftExpresson = bodyExpression.Left as MemberExpression;
               var rightExpression = bodyExpression.Right as MemberExpression;

               var valueName = leftExpresson.Member.Name;
               var columnName = String.Format("{0}.{1}", alphabetExpressionMap[leftExpresson.Member.DeclaringType], DatabaseUtils.GetCustomAttribute(leftExpresson.Member.ReflectedType, valueName).Column);
               var value = HandleQueryValue(Expression.Lambda(rightExpression).Compile().DynamicInvoke());

               string ActionType = string.Empty;
               if (value == null || value.ToLower().Equals("null"))
                  ActionType = "is";
               else
               {
                  if (!isReserved) ActionType = actionType.GetValue(action);
                  else ActionType = reservedActionType.GetValue(actionType.GetValue(action));
               }

               result = String.Format(formatExpressionString, columnName, ActionType, value);
            }
            else
            {
               result += String.Format(formatExpressionString, CreateWherePart(bodyExpression.Left, alphabetExpressionMap), actionType[action], CreateWherePart(bodyExpression.Right, alphabetExpressionMap));
            }
         }
         else if (mainBody is MethodCallExpression)
         {
            var bodyExpression = (mainBody as MethodCallExpression);
            if (methodType.ContainsKey(bodyExpression.Method.Name))
            {
               var methodExpressionAction = methodType[bodyExpression.Method.Name];

               var valueName = (bodyExpression.Object as MemberExpression).Member.Name;
               var columnName = String.Format("{0}.{1}", alphabetExpressionMap[(bodyExpression.Object as MemberExpression).Member.ReflectedType], DatabaseUtils.GetCustomAttribute((bodyExpression.Object as MemberExpression).Member.ReflectedType, valueName).Column);
               string value = (bodyExpression.Arguments[0] is ConstantExpression) ? HandleQueryValue((bodyExpression.Arguments[0] as ConstantExpression).Value, methodExpressionAction) : HandleQueryValue(Expression.Lambda(bodyExpression.Arguments[0] as MemberExpression).Compile().DynamicInvoke(), methodExpressionAction);

               if (methodExpressionAction.Contains("like"))
               {
                  if (!isReserved)
                     result = String.Format("{0} {1}", columnName, String.Format(methodExpressionAction, value));
                  else result = String.Format("{0} {1}", columnName, String.Format(reservedActionType[methodExpressionAction], value));
               }
               else
               {
                  if (!isReserved)
                     result = String.Format("{0} {1} {2}", columnName, methodExpressionAction, value);
                  else result = String.Format("{0} {1} {2}", columnName, reservedActionType[methodExpressionAction], value);
               }
            }
         }
         return result;
      }
      public static string HandleQueryValue(object objectValue, string methodAction = "")
      {
         if (objectValue != null)
         {
            var type = objectValue.GetType();
            if (type == typeof(string) && !methodAction.StartsWith("like"))
               return "'" + objectValue + "'";
         }
         else
         {
            return "null";
         }

         return objectValue.ToString();
      }

      internal static string CreateSelectPart<T, S>(Expression<Func<T, S>> selectPredicate, Dictionary<Type, string> alphabetExpressionMap, out List<string> valueNames)
      {
         List<string> selectList = new List<string>();
         valueNames = new List<string>();

         if (selectPredicate.Body.GetType().GetProperties().Any(n => n.Name.Equals("Arguments")))
         {
            IEnumerable<Expression> value = ((dynamic)selectPredicate.Body).Arguments;
            foreach (MemberExpression memEx in value)
            {
               var attribute = DatabaseUtils.GetCustomAttribute(memEx.Member.DeclaringType, memEx.Member.Name, true);
               if (attribute == null)
                  continue;

               if (alphabetExpressionMap.Count == 0 && memEx.Member.DeclaringType != typeof(T))
                  throw new Exception("Column " + attribute.Column + " not exist on table " + DatabaseUtils.GetTableName(typeof(T)) + ".");

               string columnName = (alphabetExpressionMap.Count != 0) ? String.Format("{0}.{1}", alphabetExpressionMap[memEx.Member.ReflectedType], attribute.Column) : attribute.Column;
               selectList.Add(columnName);

               valueNames.Add(memEx.Member.Name);
            }
         }
         else
         {
            MemberExpression memEx = (MemberExpression)selectPredicate.Body;

            var attribute = DatabaseUtils.GetCustomAttribute(memEx.Member.DeclaringType, memEx.Member.Name, true);
            if (attribute != null)
            {
               if (alphabetExpressionMap.Count == 0 && memEx.Member.DeclaringType != typeof(T))
                  throw new Exception("Column "+ attribute.Column + " not exist on table "+ DatabaseUtils.GetTableName(typeof(T)) + ".");

               string columnName = (alphabetExpressionMap.Count != 0) ? String.Format("{0}.{1}", alphabetExpressionMap[memEx.Member.ReflectedType], attribute.Column) : attribute.Column;
               selectList.Add(columnName);

               valueNames.Add(memEx.Member.Name);
            }
         }

         return String.Join(",", selectList);
      }
   }
   public static class GenericDaoExtension
   {
      private static List<dynamic> PredicateIgnoreList = new List<dynamic>();

      public static Expression<Func<T, dynamic>> IgnoreLink<T>(this Expression<Func<T, dynamic>> predicate)
      {
         PredicateIgnoreList.Add(predicate);
         return predicate;
      }

      public static List<S> Where<T, S>(this Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>> predicate)
      {
         if (selectPredicate == null)
            return null;

         if (predicate == null)
            return null;

         Type objectType = predicate.Parameters[0].Type;

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return null;

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = (!PredicateIgnoreList.Contains(selectPredicate)) ? GenericDaoHelper.CreateLinkInheritancePart(objectType, predicate.Parameters[0].Name, alphabetExpressionMap) : tableName;
         var whereLink = GenericDaoHelper.CreateWherePart(predicate.Body, alphabetExpressionMap);
         string selectLink = GenericDaoHelper.CreateSelectPart(selectPredicate, alphabetExpressionMap, out List<string> valuesName);
         string query = String.Format("SELECT {0} FROM {1} WHERE {2}", selectLink, fromLink, whereLink.Replace("\"", "'"));

         if (PredicateIgnoreList.Contains(selectPredicate))
            PredicateIgnoreList.Remove(selectPredicate);

         return DatabaseUtils.GetPropertyValueList<S>(query, valuesName);
      }

      public static S First<T, S>(this Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>> predicate)
      {
         if (selectPredicate == null)
            return default(S);

         if (predicate == null)
            return default(S);

         Type objectType = predicate.Parameters[0].Type;

         var tableName = DatabaseUtils.GetTableName(objectType);
         if (String.IsNullOrEmpty(tableName))
            return default(S);

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = (!PredicateIgnoreList.Contains(selectPredicate)) ? GenericDaoHelper.CreateLinkInheritancePart(objectType, predicate.Parameters[0].Name, alphabetExpressionMap) : tableName;
         var whereLink = GenericDaoHelper.CreateWherePart(predicate.Body, alphabetExpressionMap);
         string selectLink = GenericDaoHelper.CreateSelectPart(selectPredicate, alphabetExpressionMap, out List<string> valuesName);
         string query = String.Format("SELECT {0} FROM {1} WHERE {2} LIMIT 1", selectLink, fromLink, whereLink.Replace("\"", "'"));

         if (PredicateIgnoreList.Contains(selectPredicate))
            PredicateIgnoreList.Remove(selectPredicate);

         return DatabaseUtils.GetPropertyValueList<S>(query, valuesName).FirstOrDefault();
      }

      public static List<S> All<T, S>(this Expression<Func<T, S>> selectPredicate)
      {
         if (selectPredicate == null)
            return null;

         var tableName = DatabaseUtils.GetTableName(typeof(T));
         if (String.IsNullOrEmpty(tableName))
            return null;

         Dictionary<Type, string> alphabetExpressionMap = new Dictionary<Type, string>();
         var fromLink = (!PredicateIgnoreList.Contains(selectPredicate)) ? GenericDaoHelper.CreateLinkInheritancePart(typeof(T), "n", alphabetExpressionMap) : tableName;
         string selectLink = GenericDaoHelper.CreateSelectPart(selectPredicate, alphabetExpressionMap, out List<string> valuesName);
         string query = String.Format("SELECT {0} FROM {1}", selectLink, fromLink);

         if (PredicateIgnoreList.Contains(selectPredicate))
            PredicateIgnoreList.Remove(selectPredicate);

         return DatabaseUtils.GetPropertyValueList<S>(query, valuesName);
      }
   }
}
