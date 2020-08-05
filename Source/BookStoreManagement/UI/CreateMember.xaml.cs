using BookStoreManagement.BUS;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
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
        public CreateMember()
        {
            InitializeComponent();
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            cbBankName.SelectedIndex = 0;

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string image_path = path + avatar_path;
            BitmapImage image = new BitmapImage(new Uri(image_path, UriKind.Absolute));
            avatar.Source = image;
        }

        private void confirm_click(object sender, RoutedEventArgs e)
        {
            CustomerDto member = new CustomerDto();

            string[] tokenName = boxName.Text.Split(' ');
            if (tokenName.Length==0)
            {
                MessageBox.Show("Name Box is empty");
                return;
            }else if (tokenName.Length == 1)
            {
                member.FirstName = "";
                member.LastName = tokenName[0];
            }else
            {
                member.FirstName = tokenName[0];
                for (int i=1;i<tokenName.Length;i++)
                {
                    member.LastName += tokenName[i];
                    if (i<tokenName.Length-1)
                    {
                        member.LastName += " ";
                    }
                }
            }

            if (radioMale.IsChecked == true)
            {
                member.Gender = "MALE";
            }else if (radioFemale.IsChecked==true)
            {
                member.Gender = "FEMALE";
            }else
            {
                member.Gender = "OTHER";
            }

            string[] tokenDOB = boxDoB.Text.Split('/');
            if (tokenDOB.Length==3)
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
            catch(FormatException ex)
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

            if (boxAddress.Text.Length>0)
            {
                member.Address = boxAddress.Text;
            }
            else
            {
                MessageBox.Show("Address box is empty");
                return;
            }

            if (boxCreditCard.Text.Length>0)
            {
                member.CreditCard = boxCreditCard.Text;
            }
            else
            {
                MessageBox.Show("Credit card box is empty");
                return;
            }

            if (boxMoMo.Text.Length>0)
            {
                member.Momo = boxMoMo.Text;
            }
            else
            {
                MessageBox.Show("MoMo box is empty");
                return;
            }

            if (boxBankNumber.Text.Length>0)
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

        private void back_click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainGrid.Children.Clear();
            UserControl memberManagement = new MemberManagement();
            MainWindow.MainGrid.Children.Add(memberManagement);
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
                avatar_path ="Images/" + guid.ToString() + extension;
                string image_path = path + avatar_path;
                File.Copy(filename, image_path);

                BitmapImage image = new BitmapImage(new Uri(image_path, UriKind.Absolute));
                avatar.Source = image;
            }
        }
    }
}
