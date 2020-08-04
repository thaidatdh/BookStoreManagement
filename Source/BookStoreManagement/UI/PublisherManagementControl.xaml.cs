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
   /// Interaction logic for PublisherManagementControl.xaml
   /// Paging: https://www.youtube.com/watch?v=L1wpQ_fKjVw
   /// </summary>
   /// 
   [Feature(Id = 24, Name = "Publisher Management",   Group = "Publisher Management")]
   [Feature(Id = 25, Name = "Add New Publisher",      Group = "Publisher Management")]
   [Feature(Id = 26, Name = "Edit Publisher",         Group = "Publisher Management")]
   [Feature(Id = 27, Name = "Delete Publisher",       Group = "Publisher Management")]
   //[Feature(Id = 28, Name = "Import Publishers",      Group = "Publisher Management")]

    public partial class PublisherManagementControl : UserControl
   {
        FormMode mode = FormMode.New;
        int pageNumber = 1;
      int pageSize = 6;
      IPagedList<PublisherDto> listPublishers;
      List<PublisherDto> allShowedPublishers;
      List<PublisherDto> allPublishers;
      bool isInitial = false;
        bool btnEditState = true;
        public PublisherManagementControl()
      {

        InitializeComponent();
            if (!FeatureAttributeService.isAuthorized("Add New Publisher", "Publisher Management"))
            {
                buttonSave.IsEnabled = false;
            }

            if (!FeatureAttributeService.isAuthorized("Delete Publisher", "Publisher Management"))
            {
                tablePublishers.IsEnabled = false;
            }

        }


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
         this.Dispatcher.Invoke(new Action(() => isInitial = true));
      }
      private async Task<IPagedList<PublisherDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allPublishers = PublisherBUS.GetAllNotDeletedPublishers();
            if (allShowedPublishers == null)
            {
               allShowedPublishers = new List<PublisherDto>();
               allShowedPublishers.AddRange(allPublishers);
            }
               
            return allShowedPublishers.ToPagedList(pagedNumber, pageSize);
         });
      }

    
      private async Task reloadTable(int pageNumber)
      {
         listPublishers = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listPublishers.HasPreviousPage;
         btnNext.IsEnabled = listPublishers.HasNextPage;
         tablePublishers.ItemsSource = listPublishers.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedPublishers.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedPublishers.Count < pageSize ? allShowedPublishers.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedPublishers.Count);
      }
      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listPublishers == null || !listPublishers.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listPublishers == null || !listPublishers.HasNextPage)
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

            allShowedPublishers = allPublishers.Where(n => n.Name.ToUpper().Contains(name) && n.Contact.ToUpper().Contains(contact) && n.Address.ToUpper().Contains(address) && n.Email.ToUpper().Contains(email)).ToList();

            pageNumber = 1;
            await reloadTable(pageNumber);
        }
        private async Task<int> Insert(PublisherDto Publisher)
        {
            return await Task.Factory.StartNew(() =>
            {
                return PublisherBUS.Insert(Publisher);
            });
        }
        private async Task<bool> Update(PublisherDto Publisher)
        {
            return await Task.Factory.StartNew(() =>
            {
                return PublisherBUS.Update(Publisher);
            });
        }

        private async void btnSave(object sender, RoutedEventArgs e)
        {
            if (mode.Equals(FormMode.New))
            {
                PublisherDto PublisherNew = new PublisherDto();
                PublisherNew.Name = txtName.Text;
                PublisherNew.Contact = txtContact.Text;
                PublisherNew.Address = txtAddress.Text;
                PublisherNew.Email = txtEmail.Text;
                pageNumber = 1;
                await Insert(PublisherNew);
            }
            else
            {
                PublisherDto selectedItem = (PublisherDto)tablePublishers.SelectedItem;
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
            allShowedPublishers = PublisherBUS.GetAllNotDeletedPublishers();
        }

        private void selectRow(object sender, MouseButtonEventArgs e)
        {
            if (!FeatureAttributeService.isAuthorized("Edit Publisher", "Publisher Management"))
            {
                MessageBox.Show("You are not authorized for this feature!");
                return;
            } else {
                mode = FormMode.Edit;
                PublisherDto selectedItem = (PublisherDto)tablePublishers.SelectedItem;
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
            //if (tablePublishers.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a book in table to continue!");
            //    return;
            //}
            PublisherDto selectedItem = (PublisherDto)tablePublishers.SelectedItem;
            if (selectedItem == null)
                return;
            string PublisherInfo = selectedItem.Name;
            var rs = MessageBox.Show("Are you sure you want to delete this Publisher?\n" + PublisherInfo, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs.Equals(MessageBoxResult.Yes))
            {
                allShowedPublishers.Remove(selectedItem);
                allPublishers.Remove(selectedItem);
                PublisherBUS.Delete(selectedItem);
                await reloadTable(pageNumber);
            }
        }
    }
}
