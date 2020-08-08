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
    /// Interaction logic for EditStaff.xaml
    /// </summary>
    public partial class EditStaff : UserControl
    {
        string avatar_path = "Images/bg_default.jpg";
        StaffDto staff = null;
        public EditStaff(StaffDto staff)
        {
            InitializeComponent();
            this.staff = staff;
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            boxName.Text = staff.FirstName + " " + staff.LastName;

            if (staff.Gender == "MALE")
            {
                radioMale.IsChecked = true;
            }
            else if (staff.Gender == "FEMALE")
            {
                radioFemale.IsChecked = true;
            }
            else
            {
                radioOther.IsChecked = true;
            }

            try
            {
                boxDoB.Text = staff.DOB.Substring(6, 2) + "/" + staff.DOB.Substring(4, 2) + "/" + staff.DOB.Substring(0, 4);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                boxDoB.Text = "Empty";
            }

            boxEmail.Text = staff.Email;
            boxPhone.Text = staff.Phone;
            boxAddress.Text = staff.Address;

            try
            {
                boxStartDay.Text = staff.StartDate.Substring(6, 2) + "/" + staff.StartDate.Substring(4, 2) + "/" + staff.StartDate.Substring(0, 4);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                boxStartDay.Text = "Empty";
            }

            try
            {
                boxEndDay.Text = staff.EndDate.Substring(6, 2) + "/" + staff.EndDate.Substring(4, 2) + "/" + staff.EndDate.Substring(0, 4);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                boxEndDay.Text = "Empty";
            }

            boxSalary.Text = staff.Salary.ToString();
            boxNote.Text = staff.Note;
            if (staff.Active)
            {
                radioActive.IsChecked = true;
            }
            else
            {
                radioLock.IsChecked = true;
            }

            avatar_path = staff.PhotoLink;
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

            staff.UpdatedDate = new DateTime();
            staff.UpdatedBy = 1;

            if (radioActive.IsChecked==true)
            {
                staff.Active = true;
            }
            else
            {
                staff.Active = false;
            }

            if (StaffBUS.update(staff))
            {
                MessageBox.Show("Update staff success!");
            }

            MainWindow.MainGrid.Children.Clear();
            UserControl staffInfo = new StaffInfo(staff);
            MainWindow.MainGrid.Children.Add(staffInfo);
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainGrid.Children.Clear();
            UserControl staffInfo = new StaffInfo(staff);
            MainWindow.MainGrid.Children.Add(staffInfo);
        }
    }
}
