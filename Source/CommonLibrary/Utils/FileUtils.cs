using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace CommonLibrary.Utils
{
   public class FileUtils
   {
      public class CSV
      {
         public static DataTable GetDataTable(string csvFilePath)
         {
            DataTable csvData = new DataTable();

            try
            {
               using (TextFieldParser csvReader = new TextFieldParser(csvFilePath))
               {
                  csvReader.SetDelimiters(new string[] { "," });
                  csvReader.HasFieldsEnclosedInQuotes = true;
                  string[] colFields = csvReader.ReadFields();
                  foreach (string column in colFields)
                  {
                     DataColumn datecolumn = new DataColumn(column);
                     datecolumn.AllowDBNull = true;
                     csvData.Columns.Add(datecolumn);
                  }

                  while (!csvReader.EndOfData)
                  {
                     string[] fieldData = csvReader.ReadFields();
                     //Making empty value as null
                     for (int i = 0; i < fieldData.Length; i++)
                     {
                        if (fieldData[i] == "")
                        {
                           fieldData[i] = null;
                        }
                     }
                     csvData.Rows.Add(fieldData);
                  }
               }
            }
            catch (Exception ex)
            {
            }
            return csvData;
         }
      }
   }
}
