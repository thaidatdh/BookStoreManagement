using BookStoreManagement.BUS;
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
   /// Interaction logic for CreateMember.xaml
   /// </summary>
   public partial class CreateMember : UserControl
   {
      string avatar_path = "Images/bg_default.jpg";
      CustomerDto member = null;
      bool update = false;
      public CreateMember()
      {
         InitializeComponent();
         this.member = new CustomerDto();
      }

      public CreateMember(CustomerDto member)
      {
         InitializeComponent();
         this.member = member;
         this.update = true;
      }

      private void loaded(object sender, RoutedEventArgs e)
      {
         cbBankName.SelectedIndex = 0;

         if (update == false)
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
         else
         {
            boxName.Text = member.FirstName + " " + member.LastName;

            if (member.Gender == "MALE")
            {
               radioMale.IsChecked = true;
            }
            else if (member.Gender == "FEMALE")
            {
               radioFemale.IsChecked = true;
            }
            else
            {
               radioOther.IsChecked = true;
            }

            try
            {
               boxDoB.Text = member.DOB.Substring(6, 2) + "/" + member.DOB.Substring(4, 2) + "/" + member.DOB.Substring(0, 4);
            }
            catch (ArgumentOutOfRangeException ex)
            {
               boxDoB.Text = "Empty";
            }

            boxEmail.Text = member.Email;
            boxPhone.Text = member.Phone;
            boxAddress.Text = member.Address;
            boxCreditCard.Text = member.CreditCard;
            boxMoMo.Text = member.Momo;
            boxBankNumber.Text = member.BankNumber;
            boxNote.Text = member.Note;

            for (int i = 0; i < cbBankName.Items.Count; i++)
            {
               string bank = cbBankName.Items.GetItemAt(i).ToString();
               string bank_name = bank.Substring(38, bank.Length - 38);
               if (bank_name == member.BankName)
               {
                  cbBankName.SelectedIndex = i;
                  break;
               }
            }

            avatar_path = member.PhotoLink;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string image_path = path + avatar_path;
            if (!File.Exists(image_path))
            {
               image_path = path + "Images/bg_default.jpg";
            }

            BitmapImage image = new BitmapImage(new Uri(image_path, UriKind.Absolute));
            avatar.Source = image;

            btnConfirm.Content = "Update";
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
            member.FirstName = "";
            member.LastName = tokenName[0];
         }
         else
         {
            member.FirstName = tokenName[0];
            member.LastName = "";
            for (int i = 1; i < tokenName.Length; i++)
            {
               member.LastName += tokenName[i];
               if (i < tokenName.Length - 1)
               {
                  member.LastName += " ";
               }
            }
         }

         if (radioMale.IsChecked == true)
         {
            member.Gender = "MALE";
         }
         else if (radioFemale.IsChecked == true)
         {
            member.Gender = "FEMALE";
         }
         else
         {
            member.Gender = "OTHER";
         }

         string[] tokenDOB = boxDoB.Text.Split('/');
         if (tokenDOB.Length == 3)
         {
            member.DOB = tokenDOB[2] + tokenDOB[1] + tokenDOB[0];
         }
         else
         {
            MessageBox.Show("Day of Birth is wrong format");
            return;
         }

         try
         {
            MailAddress email = new MailAddress(boxEmail.Text);
            member.Email = email.ToString();
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
            member.Phone = boxPhone.Text;
         }
         else
         {
            MessageBox.Show("Phone Number box is empty");
            return;
         }

         if (boxAddress.Text.Length > 0)
         {
            member.Address = boxAddress.Text;
         }
         else
         {
            MessageBox.Show("Address box is empty");
            return;
         }

         if (boxCreditCard.Text.Length > 0)
         {
            member.CreditCard = boxCreditCard.Text;
         }
         else
         {
            MessageBox.Show("Credit card box is empty");
            return;
         }

         if (boxMoMo.Text.Length > 0)
         {
            member.Momo = boxMoMo.Text;
         }
         else
         {
            MessageBox.Show("MoMo box is empty");
            return;
         }

         if (boxBankNumber.Text.Length > 0)
         {
            member.BankNumber = boxBankNumber.Text;
         }
         else
         {
            MessageBox.Show("Bank Number box is empty");
            return;
         }

         string bank = cbBankName.SelectedItem.ToString();
         string bankName = bank.Substring(38, bank.Length - 38);
         member.BankName = bankName;

         member.PhotoLink = avatar_path;
         member.Note = boxNote.Text;

         if (update)
         {
            member.UpdatedDate = new DateTime();
            member.UpdatedBy = 1;

            if (CustomerBUS.Update(member))
            {
               MessageBox.Show("Update member success!");
            }

            MainWindow.MainGrid.Children.Clear();
            UserControl memberInfo = new MemberInfo(member);
            MainWindow.MainGrid.Children.Add(memberInfo);
         }
         else
         {
            member.UserType = "CUSTOMER";
            member.CreateDate = new DateTime();
            member.CreateBy = 1;
            member.UpdatedDate = new DateTime();
            member.UpdatedBy = 1;
            member.Point = 0;
            member.IsDeleted = false;

            CustomerBUS.Insert(member);
            MessageBox.Show("Create member success!");

            MainWindow.MainGrid.Children.Clear();
            UserControl memberManagement = new MemberManagement();
            MainWindow.MainGrid.Children.Add(memberManagement);
         }
      }

      private void back_click(object sender, RoutedEventArgs e)
      {
         MainWindow.MainGrid.Children.Clear();
         if (update)
         {
            UserControl memberInfo = new MemberInfo(member);
            MainWindow.MainGrid.Children.Add(memberInfo);
         }
         else
         {
            UserControl memberManagement = new MemberManagement();
            MainWindow.MainGrid.Children.Add(memberManagement);
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
   }
}
