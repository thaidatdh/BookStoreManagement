using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using CommonLibrary;
using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ProviderManagementControl.xaml
    /// Paging: https://www.youtube.com/watch?v=L1wpQ_fKjVw
    /// </summary>
    /// 
    [Feature(Id = 19, Name = FeatureNameUtils.Provider.MANAGEMENT, Group = FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT)]
    [Feature(Id = 20, Name = FeatureNameUtils.Provider.NEW, Group = FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT)]
    [Feature(Id = 21, Name = FeatureNameUtils.Provider.EDIT, Group = FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT)]
    [Feature(Id = 22, Name = FeatureNameUtils.Provider.DELETE, Group = FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT)]
    //[Feature(Id = 23, Name = FeatureNameUtils.Provider.IMPORT,      Group = FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT)]

    public partial class ProviderManagementControl : UserControl
   {
        FormMode mode = FormMode.New;
        int pageNumber = 1;
      int pageSize = 6;
      IPagedList<ProviderDto> listProviders;
      List<ProviderDto> allShowedProviders;
      List<ProviderDto> allProviders;
      bool isInitial = false;
        bool btnEditState = true;
        public ProviderManagementControl()
      {

        InitializeComponent();
            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Provider.NEW, FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT))
            {
                buttonSave.IsEnabled = false;
            }

            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Provider.DELETE, FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT))
            {
                tableProviders.IsEnabled = false;
            }

        }


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
         this.Dispatcher.Invoke(new Action(() => isInitial = true));
      }
      private async Task<IPagedList<ProviderDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allProviders = ProviderBUS.GetAllNotDeletedProviders();
            if (allShowedProviders == null)
            {
               allShowedProviders = new List<ProviderDto>();
               allShowedProviders.AddRange(allProviders);
            }
               
            return allShowedProviders.ToPagedList(pagedNumber, pageSize);
         });
      }

    
      private async Task reloadTable(int pageNumber)
      {
         listProviders = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listProviders.HasPreviousPage;
         btnNext.IsEnabled = listProviders.HasNextPage;
         tableProviders.ItemsSource = listProviders.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedProviders.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedProviders.Count < pageSize ? allShowedProviders.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedProviders.Count);
      }
      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listProviders == null || !listProviders.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listProviders == null || !listProviders.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }

      private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
         bool isInit = false;
         this.Dispatcher.Invoke(new Action(() => isInit = isInitial));
         if (this.Visibility == Visibility.Visible && isInit)
         {
            await reloadTable(pageNumber);
         }
      }

        private async void btnSearch(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.ToUpper();
            string contact = txtContact.Text.ToUpper();
            string address = txtAddress.Text.ToUpper();
            string email = txtEmail.Text.ToUpper();

            allShowedProviders = allProviders.Where(n => n.Name.ToUpper().Contains(name) && n.Contact.ToUpper().Contains(contact) && n.Address.ToUpper().Contains(address) && n.Email.ToUpper().Contains(email)).ToList();

            pageNumber = 1;
            await reloadTable(pageNumber);
        }
        private async Task<int> Insert(ProviderDto provider)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ProviderBUS.Insert(provider);
            });
        }
        private async Task<bool> Update(ProviderDto provider)
        {
            return await Task.Factory.StartNew(() =>
            {
                return ProviderBUS.Update(provider);
            });
        }

        private async void btnSave(object sender, RoutedEventArgs e)
        {
            if (mode.Equals(FormMode.New))
            {
                ProviderDto providerNew = new ProviderDto();
                providerNew.Name = txtName.Text;
                providerNew.Contact = txtContact.Text;
                providerNew.Address = txtAddress.Text;
                providerNew.Email = txtEmail.Text;
                pageNumber = 1;
                await Insert(providerNew);
            }
            else
            {
                ProviderDto selectedItem = (ProviderDto)tableProviders.SelectedItem;
                selectedItem.Name = txtName.Text;
                selectedItem.Contact = txtContact.Text;
                selectedItem.Address = txtAddress.Text;
                selectedItem.Email = txtEmail.Text;
                await Update(selectedItem);
            }
            clean();
            await reloadTable(pageNumber);
        }

        private async void btnClean(object sender, RoutedEventArgs e)
        {
            clean();
            await reloadTable(1);
        }

        private void clean()
        {
            mode = FormMode.New;
            txtName.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            allShowedProviders = ProviderBUS.GetAllNotDeletedProviders();
        }

        private void selectRow(object sender, MouseButtonEventArgs e)
        {
            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Provider.EDIT, FeatureNameUtils.FeatureGroup.PROVIDER_MANAGEMENT))
            {
                MessageBox.Show("You are not authorized for this feature!");
                return;
            } else {
                mode = FormMode.Edit;
                ProviderDto selectedItem = (ProviderDto)tableProviders.SelectedItem;
                txtName.Text = selectedItem.Name;
                txtContact.Text = selectedItem.Contact;
                txtAddress.Text = selectedItem.Address;
                txtEmail.Text = selectedItem.Email;
            }
        }

        //void ShowHideDetails(object sender, RoutedEventArgs e)
        //{
        //    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        //        if (vis is DataGridRow)
        //        {
        //            var row = (DataGridRow)vis;
        //            row.DetailsVisibility =
        //            row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        //            break;
        //        }
        //}

        private async void btnDelete(object sender, RoutedEventArgs e)
        {
            //if (!FeatureAttributeService.isAuthorized("Delete Book", "Book Management"))
            //{
            //    MessageBox.Show("You are not authorized for this feature!");
            //    return;
            //}
            ////Code Here if authorized
            //if (tableProviders.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a book in table to continue!");
            //    return;
            //}
            ProviderDto selectedItem = (ProviderDto)tableProviders.SelectedItem;
            if (selectedItem == null)
                return;
            string providerInfo = selectedItem.Name;
            var rs = MessageBox.Show("Are you sure you want to delete this provider?\n" + providerInfo, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs.Equals(MessageBoxResult.Yes))
            {
                allShowedProviders.Remove(selectedItem);
                allProviders.Remove(selectedItem);
                ProviderBUS.Delete(selectedItem);
                await reloadTable(pageNumber);
            }
        }

        private void tableProviders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
