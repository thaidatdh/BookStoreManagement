using BookStoreManagement.BUS;
using CommonLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManagement.UI
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private int staffID = 0;
        public ChangePassword(int staffID)
        {
            InitializeComponent();
            this.staffID = staffID;
        }

        private void change_password_click(object sender, RoutedEventArgs e)
        {
            //kiem tra trung
            string oldPassword = boxOldPassword.Password;
            string newPassword = boxNewPassword.Password;
            string rePassword = boxReNewPassword.Password;

            if (!newPassword.Equals(rePassword))
            {
                MessageBox.Show("Password is not match");
                return;
            }

            string oldPasswordSHA = CryptoUtils.encryptSHA256(oldPassword);
            string newPasswordSHA = CryptoUtils.encryptSHA256(newPassword);

            if (!StaffBUS.checkPassword(staffID, oldPasswordSHA))
            {
                MessageBox.Show("Password is incorrect");
                return;
            }

            if (StaffBUS.changePassword(staffID, newPasswordSHA))
            {
                MessageBox.Show("Change Password is success!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Change Password is fail!");
            }
        }
    }
}
