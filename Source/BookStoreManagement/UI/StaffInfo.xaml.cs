﻿using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using DatabaseCommon.Const;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.IO;
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
   /// Interaction logic for StaffInfo.xaml
   /// </summary>
   public partial class StaffInfo : UserControl
   {
      StaffDto staff = null;
      public StaffInfo(StaffDto staff)
      {
         InitializeComponent();
         this.staff = staff;
      }

      private void loaded(object sender, RoutedEventArgs e)
      {
         textName.Text = staff.FirstName + " " + staff.LastName;
         textGender.Text = staff.Gender;

         try
         {
            textDoB.Text = staff.DOB.Substring(6, 2) + "/" + staff.DOB.Substring(4, 2) + "/" + staff.DOB.Substring(0, 4);
         }
         catch (Exception ex)
         {
            textDoB.Text = "Empty";
         }

         textEmail.Text = staff.Email;
         textPhone.Text = staff.Phone;
         textAddress.Text = staff.Address;
         textUsername.Text = staff.Username;
         textSalary.Text = staff.Salary.ToString();

         try
         {
            textStartDay.Text = staff.StartDate.Substring(6, 2) + "/" + staff.StartDate.Substring(4, 2) + "/" + staff.StartDate.Substring(0, 4);
         }
         catch (Exception ex)
         {
            textStartDay.Text = "Empty";
         }

         try
         {
            textEndDay.Text = staff.EndDate.Substring(6, 2) + "/" + staff.EndDate.Substring(4, 2) + "/" + staff.EndDate.Substring(0, 4);
         }
         catch (Exception ex)
         {
            textEndDay.Text = "Empty";
         }

         textNote.Text = staff.Note;

         if (staff.Active == true)
         {
            textActive.Text = "Active";
            textActive.Foreground = new SolidColorBrush(Colors.Green);
         }
         else
         {
            textActive.Text = "Lock";
            textActive.Foreground = new SolidColorBrush(Colors.Red);
         }

         string path = CONST.APPLICATION_PATH;
         string avatar_path = path + staff.PhotoLink;

         if (!File.Exists(avatar_path))
         {
            avatar_path = path + CONST.USERS.DEFAULT_PHOTO_LINK;
         }
         try
         {
            BitmapImage image = new BitmapImage(new Uri(avatar_path, UriKind.Absolute));
            avatar.Source = image;
         }
         catch (Exception ex) { }
      }

      private void Edit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Staff.EDIT, FeatureNameUtils.FeatureGroup.STAFF_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         MainWindow.MainGrid.Children.Clear();
         UserControl editStaff = new EditStaff(staff);
         MainWindow.MainGrid.Children.Add(editStaff);
      }

      private void btn_change_password_click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Staff.EDIT, FeatureNameUtils.FeatureGroup.STAFF_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         ChangePassword changePassword = new ChangePassword(staff.StaffId);
         changePassword.Show();
      }

      private void back_click(object sender, RoutedEventArgs e)
      {
         MainWindow.MainGrid.Children.Clear();
         UserControl staffManagement = new StaffManagement();
         MainWindow.MainGrid.Children.Add(staffManagement);
      }
   }
}
