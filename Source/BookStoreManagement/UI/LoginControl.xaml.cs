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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStoreManagement.UI
{
   /// <summary>
   /// Interaction logic for LoginControl.xaml
   /// </summary>
   public partial class LoginControl : UserControl
   {
      public LoginControl()
      {
         InitializeComponent();
      }

      private void btnLogin_Click(object sender, RoutedEventArgs e)
      {
         if (String.IsNullOrEmpty(txtUsername.Text.Trim()) || String.IsNullOrEmpty(txtPassword.Password))
         {
            lbError.Content = "Username or Password is empty!";
            return;
         }
         string encryptedPassword = CryptoUtils.encryptSHA256(txtPassword.Password);
         if (!StaffBUS.Login(txtUsername.Text.Trim(), encryptedPassword))
         {
            lbError.Content = "Username or Password is incorrect!";
            return;
         }
         lbError.Content = "";
         MainWindow.MainGrid.Children.Clear();
         MainWindow.ShowLoginedMenu(txtUsername.Text.Trim());
      }
   }
}
