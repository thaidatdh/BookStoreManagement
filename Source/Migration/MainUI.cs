using CommonLibrary.Utils;
using Migration.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Migration
{
   public partial class MainUI : Form
   {
      public MainUI()
      {
         InitializeComponent();
      }
      private List<String> Files = new List<string>();
      private void btnBrowse_Click(object sender, EventArgs e)
      {
         using (var fbd = new FolderBrowserDialog())
         {
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
               Const.SOURCE_PATH = fbd.SelectedPath;
               string[] files = Directory.GetFiles(fbd.SelectedPath);
               foreach (string file in files)
               {
                  string fileName = Path.GetFileName(file).ToUpper();
                  if (Const.RequireFiles.Contains(fileName))
                  {
                     Files.Add(fileName);
                     checkedListBox1.Items.Add(fileName);
                  }
               }
            }
         }

      }

      private void MainUI_Load(object sender, EventArgs e)
      {
         checkedListBox1.Enabled = false;
         foreach (string section in Const.MigrateSection)
         {
            checkedListBox2.Items.Add(section);
         }
      }
      private Dictionary<string, int> RequireFiles = new Dictionary<string, int>();
      private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
      {
         if (String.IsNullOrEmpty(Const.SOURCE_PATH))
         {
            MessageBox.Show("Please select source folder");
            return;
         }
         if (checkedListBox1.Items.Count == 0) return;
         int checkIndex = e.Index;
         string item = checkedListBox2.Items[checkIndex].ToString();
         if (e.NewValue == CheckState.Checked)
         {
            #region check require section
            string[] requireSection = Const.RequireSection.GetValue(item);
            if (requireSection != null && requireSection.Length > 0)
            {
               foreach (string section in requireSection)
               {
                  int index = checkedListBox2.Items.IndexOf(section);
                  checkedListBox2.SetItemChecked(index, true);
               }
            }
            #endregion

            #region check require file 
            string[] requireFile = Const.RequireFilesSection.GetValue(item);
            if (requireFile != null && requireFile.Length > 0)
            {
               foreach (string file in requireFile)
               {
                  int index = checkedListBox1.Items.IndexOf(file);
                  checkedListBox1.SetItemChecked(index, true);
                  RequireFiles[item + "_" + file] = 1;
               }
            }
            #endregion
         }
         else
         {
            List<string> UncheckSection = new List<string>() { item };
            #region uncheck require section
            string[] requireSection = Const.RequireSection.GetValue(item);
            List<String> CheckedSections = new List<string>();
            foreach (var section in checkedListBox2.CheckedItems)
            {
               CheckedSections.Add(section.ToString());
            }
            foreach (var selectedSection in CheckedSections)
            {
               requireSection = Const.RequireSection.GetValue(selectedSection.ToString());
               if (requireSection != null && requireSection.Length > 0 && requireSection.Contains(item))
               {
                  int index = checkedListBox2.Items.IndexOf(selectedSection);
                  checkedListBox2.SetItemChecked(index, false);
                  UncheckSection.Add(selectedSection.ToString());
               }
            }
            #endregion
            #region uncheck require file 
            foreach (string uncheck in UncheckSection)
            {
               string[] requireFile = Const.RequireFilesSection.GetValue(uncheck);
               if (requireFile != null && requireFile.Length > 0)
               {
                  foreach (string file in requireFile)
                  {
                     bool required = false;
                     foreach (var section in checkedListBox2.CheckedItems)
                     {
                        if (section.ToString().Equals(uncheck)) continue;
                        if (RequireFiles.GetValue(section.ToString() + "_" + file) == 1)
                        {
                           required = true;
                           break;
                        }
                     }
                     if (!required)
                     {
                        int index = checkedListBox1.Items.IndexOf(file);
                        checkedListBox1.SetItemChecked(index, false);
                        RequireFiles[uncheck + "_" + file] = 0;
                     }
                  }
               }
            }
            #endregion
         }
      }
      private bool isAllRequireFileIncluded(string section, out List<string> lackFiles)
      {
         string[] requireFile = Const.RequireFilesSection.GetValue(section);
         lackFiles = new List<string>();
         if (requireFile == null) return false;
         foreach (string file in requireFile)
         {
            if (!Files.Contains(file))
            {
               lackFiles.Add(file);
            }
         }
         return lackFiles.Count == 0;
      }
      private void btnMigrate_Click(object sender, EventArgs e)
      {
         Const.DEFAULT_STAFF_VALUE = textBox1.Text.Trim();
         foreach (var item in checkedListBox2.CheckedItems)
         {
            List<string> lackFiles = new List<string>();
            if (!isAllRequireFileIncluded(item.ToString(), out lackFiles))
            {
               MessageBox.Show("Lack of file in " + item.ToString() + ". End Migrate.\n" + String.Join("\n", lackFiles.ToArray()));
               return;
            }
            Task task = null;
            switch (item.ToString())
            {
               case "STAFF":
                  task = new Task(new Action(() => Staff.Migrate()));
                  break;
               case "CUSTOMER":
                  task = new Task(new Action(() => Customer.Migrate()));
                  break;
               case "CATEGORY":
                  task = new Task(new Action(() => Category.Migrate()));
                  break;
               case "PUBLISHER":
                  task = new Task(new Action(() => Publisher.Migrate()));
                  break;
               case "AUTHOR":
                  task = new Task(new Action(() => Author.Migrate()));
                  break;
               case "BOOK":
                  task = new Task(new Action(() => Book.Migrate()));
                  break;
               default:
                  break;
            }
            if (task != null)
            {
               task.Start();
               task.Wait();
               task.Dispose();
            }
         }
         MessageBox.Show("Migrate Completed");
      }
   }
}
