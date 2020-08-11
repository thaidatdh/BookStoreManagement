using BookStoreManagement.BUS;
using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStoreManagement.UI
{
   /// <summary>
   /// Interaction logic for CreateStaff.xaml
   /// </summary>
   public partial class CreateStaff : UserControl
   {
      string avatar_path = "Images/bg_default.jpg";
      StaffDto staff = null;

      public CreateStaff()
      {
         InitializeComponent();
         this.staff = new StaffDto();
      }

      private void loaded(object sender, RoutedEventArgs e)
      {
         string path = AppDomain.CurrentDomain.BaseDirectory;
         string image_path = path + avatar_path;
         try
         {
            BitmapImage image = new BitmapImage(new Uri(image_path, UriKind.Absolute));
            avatar.Source = image;
         }
         catch (Exception ex)
         {

         }
      }

      private void choose_avatar_click(object sender, RoutedEventArgs e)
      {
         // Create OpenFileDialog 
         Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

         // Set filter for file extension and default file extension 
         dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

         // Display OpenFileDialog by calling ShowDialog method 
         Nullable<bool> result = dlg.ShowDialog();


         // Get the selected file name and display in a TextBox 
         if (result == true)
         {
            // Open document 
            string filename = dlg.FileName;

            string path = AppDomain.CurrentDomain.BaseDirectory;
            int lastIndex = filename.LastIndexOf('.');
            string extension = filename.Substring(lastIndex, filename.Length - lastIndex);
            Guid guid = Guid.NewGuid();
            avatar_path = "Images/" + guid.ToString() + extension;
            string image_path = path + avatar_path;
            File.Copy(filename, image_path);

            BitmapImage image = new BitmapImage(new Uri(image_path, UriKind.Absolute));
            avatar.Source = image;
         }
      }

      private void confirm_click(object sender, RoutedEventArgs e)
      {
         if (boxName.Text.Length == 0)
         {
            MessageBox.Show("Name Box is empty");
            return;
         }

         string[] tokenName = boxName.Text.Split(' ');

         if (tokenName.Length == 1)
         {
            staff.FirstName = "";
            staff.LastName = tokenName[0];
         }
         else
         {
            staff.FirstName = tokenName[0];
            staff.LastName = "";
            for (int i = 1; i < tokenName.Length; i++)
            {
               staff.LastName += tokenName[i];
               if (i < tokenName.Length - 1)
               {
                  staff.LastName += " ";
               }
            }
         }

         if (radioMale.IsChecked == true)
         {
            staff.Gender = "MALE";
         }
         else if (radioFemale.IsChecked == true)
         {
            staff.Gender = "FEMALE";
         }
         else
         {
            staff.Gender = "OTHER";
         }

         string[] tokenDOB = boxDoB.Text.Split('/');
         if (tokenDOB.Length == 3)
         {
            staff.DOB = tokenDOB[2] + tokenDOB[1] + tokenDOB[0];
         }
         else
         {
            MessageBox.Show("Day of Birth is wrong format");
            return;
         }

         try
         {
            MailAddress email = new MailAddress(boxEmail.Text);
            staff.Email = email.ToString();
         }
         catch (ArgumentException ex)
         {
            MessageBox.Show("Email is empty");
            return;
         }
         catch (FormatException ex)
         {
            MessageBox.Show("Email is wrong format");
            return;
         }

         if (boxPhone.Text.Length > 0)
         {
            staff.Phone = boxPhone.Text;
         }
         else
         {
            MessageBox.Show("Phone Number box is empty");
            return;
         }

         if (boxAddress.Text.Length > 0)
         {
            staff.Address = boxAddress.Text;
         }
         else
         {
            MessageBox.Show("Address box is empty");
            return;
         }

         staff.PhotoLink = avatar_path;
         staff.Note = boxNote.Text;
         try
         {
            staff.Salary = long.Parse(boxSalary.Text);
         }
         catch (FormatException ex)
         {
            MessageBox.Show("Salary box is wrong format");
            return;
         }

         string[] tokenStartDay = boxStartDay.Text.Split('/');
         if (tokenStartDay.Length == 3)
         {
            staff.StartDate = tokenStartDay[2] + tokenStartDay[1] + tokenStartDay[0];
         }
         else
         {
            MessageBox.Show("Start day is wrong format");
            return;
         }

         string[] tokenEndDay = boxEndDay.Text.Split('/');
         if (tokenEndDay.Length == 3)
         {
            staff.EndDate = tokenEndDay[2] + tokenEndDay[1] + tokenEndDay[0];
         }
         else
         {
            MessageBox.Show("End day is wrong format");
            return;
         }

         staff.UserType = "STAFF";
         staff.CreateDate = new DateTime();
         staff.CreateBy = 1;
         staff.UpdatedDate = new DateTime();
         staff.UpdatedBy = 1;
         staff.Active = true;

         if (boxUsername.Text.Length == 0)
         {
            MessageBox.Show("Username is empty");
            return;
         }
         else
         {
            staff.Username = boxUsername.Text;
         }

         if (!boxPassword.Password.Equals(boxRePassword.Password))
         {
            MessageBox.Show("Password is not match");
            return;
         }

         if (boxPassword.Password.Length < 6)
         {
            MessageBox.Show("Password must be greater than 6 charaters");
            return;
         }
         else
         {
            staff.Password = CryptoUtils.encryptSHA256(boxPassword.Password);
         }

         StaffBUS.Insert(staff);
         MessageBox.Show("Create staff success!");

         MainWindow.MainGrid.Children.Clear();
         UserControl staffManagement = new StaffManagement();
         MainWindow.MainGrid.Children.Add(staffManagement);
      }

      private void back_click(object sender, RoutedEventArgs e)
      {
         MainWindow.MainGrid.Children.Clear();
         UserControl staffManagement = new StaffManagement();
         MainWindow.MainGrid.Children.Add(staffManagement);
      }
   }
}
