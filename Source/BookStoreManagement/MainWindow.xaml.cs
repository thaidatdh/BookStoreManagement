﻿using BookStoreManagement.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
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
using BookStoreManagement.UI;
using DatabaseCommon.Const;
using BookStoreManagement.BUS;
using System.Diagnostics;
using System.IO;

namespace BookStoreManagement
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public static Grid MainGrid { get; set; }
      private static StackPanel PanelLogin { get; set; }
      private static Button ProfileButton { get; set; }
      private static Button AuthorizationButton { get; set; }
      public MainWindow()
      {
         DatabaseCommon.DatabaseUtils.Open();
         if (!Directory.Exists(CONST.APPLICATION_PATH + "\\persistent\\images\\"))
         {
            Directory.CreateDirectory(CONST.APPLICATION_PATH + "\\persistent\\images\\");
         }
         InitializeComponent();
         this.Title = "Book Store Management";
         GridMain.Children.Clear();
         UserControl userControl = new LoginControl();
         GridMain.Children.Add(userControl);
         MainGrid = GridMain;
         PanelLogin = panelLogin;
         ProfileButton = btnProfile;
         AuthorizationButton = btnAuthorization;
         HideLoginMenu();
        }
      public static void AddSubChild(UserControl subControl)
      {
         if (subControl == null || MainGrid == null)
            return;
         foreach (UserControl control in MainGrid.Children)
         {
            control.Visibility = Visibility.Collapsed;
         }
         MainGrid.Children.Add(subControl);
         subControl.Visibility = Visibility.Visible;
      }
      public static void ReplaceMainControl(UserControl Control)
      {
         if (Control == null || MainGrid == null)
            return;
         MainGrid.Children.Clear();
         MainGrid.Children.Add(Control);
         Control.Visibility = Visibility.Visible;
      }
      public static void RemoveSubChild(UserControl subControl)
      {
         if (subControl == null || MainGrid == null)
            return;
         MainGrid.Children.Remove(subControl);
         subControl.Visibility = Visibility.Collapsed;
         UserControl LastControl = null;
         foreach (UserControl control in MainGrid.Children)
         {
            LastControl = control;
         }
         if (LastControl != null)
            LastControl.Visibility = Visibility.Visible;
      }
      private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
      {
         ButtonCloseMenu.Visibility = Visibility.Visible;
         ButtonOpenMenu.Visibility = Visibility.Collapsed;
      }

      private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
      {
         ButtonCloseMenu.Visibility = Visibility.Collapsed;
         ButtonOpenMenu.Visibility = Visibility.Visible;
      }
      public static void HideLoginMenu()
      {
         if (PanelLogin != null)
         {
            PanelLogin.Visibility = Visibility.Collapsed;
         }
         if (ProfileButton != null)
         {
            ProfileButton.Visibility = Visibility.Collapsed;
         }
         if (AuthorizationButton != null)
         {
            AuthorizationButton.Visibility = Visibility.Collapsed;
         }
      }
      public static void ShowLoginedMenu(string Username)
      {
         if (PanelLogin != null)
         {
            PanelLogin.Visibility = Visibility.Visible;
         }
         if (ProfileButton != null)
         {
            ProfileButton.Visibility = Visibility.Visible;
            ProfileButton.Content = Username;
         }
         if (AuthorizationButton != null && Config.Manager.CURRENT_USER != null)
         {
            if (FeatureAttributeService.isAuthorized(FeatureNameUtils.Authorization.MANAGEMENT, FeatureNameUtils.FeatureGroup.AUTHORIZATION))
               AuthorizationButton.Visibility = Visibility.Visible;
            else
               AuthorizationButton.Visibility = Visibility.Collapsed;
         }
      }
      private bool isShowed(Type controlType)
      {
         foreach (UIElement element in GridMain.Children)
         {
            if (element.GetType().ToString().Equals(controlType.ToString()))
            {
               return true;
            }
         }
         return false;
      }
      private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         UserControl usc = null;
         bool isAuthorized = false;
         if (ListViewMenu.SelectedIndex == -1) return;
         switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
         {
            case "Book":
               if (FeatureAttributeService.isAuthorized(typeof(BookManagementControl)))
               {
                  if (!isShowed(typeof(BookManagementControl)))
                  {
                     usc = new BookManagementControl();
                     GridMain.Children.Clear();
                     GridMain.Children.Add(usc);
                  }
                  isAuthorized = true;
               }
               break;
            case "Customer":
               if (FeatureAttributeService.isAuthorized(typeof(MemberManagement)))
               {
                   if (!isShowed(typeof(MemberManagement)))
                   {
                       usc = new MemberManagement();
                       GridMain.Children.Clear();
                       GridMain.Children.Add(usc);
                   }
                   isAuthorized = true;
               }
               break;
            case "Staff":
               if (FeatureAttributeService.isAuthorized(typeof(StaffManagement)))
               {
                   if (!isShowed(typeof(StaffManagement)))
                   {
                       usc = new StaffManagement();
                       GridMain.Children.Clear();
                       GridMain.Children.Add(usc);
                   }
                   isAuthorized = true;
               }
               break;
            case "Provider":
                if (FeatureAttributeService.isAuthorized(typeof(ProviderManagementControl)))
                {
                    if (!isShowed(typeof(ProviderManagementControl)))
                    {
                        usc = new ProviderManagementControl();
                        GridMain.Children.Clear();
                        GridMain.Children.Add(usc);
                    }
                    isAuthorized = true;
                }
                    break;
            case "Publisher":
                if (FeatureAttributeService.isAuthorized(typeof(PublisherManagementControl)))
                {
                    if (!isShowed(typeof(PublisherManagementControl)))
                    {
                        usc = new PublisherManagementControl();
                        GridMain.Children.Clear();
                        GridMain.Children.Add(usc);
                    }
                    isAuthorized = true;
                }
                    break;
            case "Category":
               if (FeatureAttributeService.isAuthorized(typeof(CategoryManagementControl)))
               {
                  if (!isShowed(typeof(CategoryManagementControl)))
                  {
                     usc = new CategoryManagementControl();
                     GridMain.Children.Clear();
                     GridMain.Children.Add(usc);
                  }
                  isAuthorized = true;
               }
               break;
            case "Author":
                if (FeatureAttributeService.isAuthorized(typeof(AuthorManagementControl)))
                {
                    if (!isShowed(typeof(AuthorManagementControl)))
                    {
                        usc = new AuthorManagementControl();
                        GridMain.Children.Clear();
                        GridMain.Children.Add(usc);
                    }
                    isAuthorized = true;
                }
                    break;
            case "Transaction":
               if (FeatureAttributeService.isAuthorized(typeof(TransactionManagementControl)))
               {
                  if (!isShowed(typeof(TransactionManagementControl)))
                  {
                     usc = new TransactionManagementControl();
                     GridMain.Children.Clear();
                     GridMain.Children.Add(usc);
                  }
                  isAuthorized = true;
               }
               break;
            default:
               break;
         }
         if (!isAuthorized)
         {
            MessageBox.Show("You are not authorized for this feature!");
         }
         ListViewMenu.SelectedIndex = -1;
      }

      private void btnProfile_Click(object sender, RoutedEventArgs e)
      {
         MainWindow.AddSubChild(new StaffInfo(Config.Manager.CURRENT_USER)); 
      }

      private void btnCloseApplication_Click(object sender, RoutedEventArgs e)
      {
         Application.Current.Shutdown();
      }

      private void btnLogOut_Click(object sender, RoutedEventArgs e)
      {
         HideLoginMenu();
         StaffBUS.Logout();
         UserControl userControl = new LoginControl();
         ReplaceMainControl(userControl);
      }

      private void btnAuthorization_Click(object sender, RoutedEventArgs e)
      {
         UserControl usc = null;
         GridMain.Children.Clear();
         usc = new UserAuthorizationControl();
         GridMain.Children.Add(usc);
      }

      private void Window_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
      }
   }
}
