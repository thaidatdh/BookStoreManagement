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
   /// Interaction logic for AuthorManagementControl.xaml
   /// Paging: https://www.youtube.com/watch?v=L1wpQ_fKjVw
   /// </summary>
   /// 
   [Feature(Id = 29, Name = FeatureNameUtils.Author.MANAGEMENT,     Group = FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT)]
   [Feature(Id = 30, Name = FeatureNameUtils.Author.NEW,            Group = FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT)]
   [Feature(Id = 31, Name = FeatureNameUtils.Author.EDIT,           Group = FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT)]
   [Feature(Id = 32, Name = FeatureNameUtils.Author.DELETE,         Group = FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT)]
    //[Feature(Id = 33, Name = FeatureNameUtils.Author.IMPORT,      Group = FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT)]

    public partial class AuthorManagementControl : UserControl
   {
        FormMode mode = FormMode.New;
        int pageNumber = 1;
      int pageSize = 6;
      IPagedList<AuthorDto> listAuthors;
      List<AuthorDto> allShowedAuthors;
      List<AuthorDto> allAuthors;
      bool isInitial = false;
        bool btnEditState = true;
        public AuthorManagementControl()
      {

        InitializeComponent();
            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Author.NEW, FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT))
            {
                buttonSave.IsEnabled = false;
            }

            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Author.DELETE, FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT))
            {
                tableAuthors.IsEnabled = false;
            }

        }


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
         this.Dispatcher.Invoke(new Action(() => isInitial = true));
      }
      private async Task<IPagedList<AuthorDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allAuthors = AuthorBUS.GetAllNotDeletedAuthors();
            if (allShowedAuthors == null)
            {
               allShowedAuthors = new List<AuthorDto>();
               allShowedAuthors.AddRange(allAuthors);
            }
               
            return allShowedAuthors.ToPagedList(pagedNumber, pageSize);
         });
      }

    
      private async Task reloadTable(int pageNumber)
      {
         listAuthors = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listAuthors.HasPreviousPage;
         btnNext.IsEnabled = listAuthors.HasNextPage;
         tableAuthors.ItemsSource = listAuthors.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedAuthors.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedAuthors.Count < pageSize ? allShowedAuthors.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedAuthors.Count);
      }
      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listAuthors == null || !listAuthors.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listAuthors == null || !listAuthors.HasNextPage)
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
            string note = txtNote.Text.ToUpper();

            allShowedAuthors = allAuthors.Where(n => n.Name.ToUpper().Contains(name) && n.Note.ToUpper().Contains(note)).ToList();

            pageNumber = 1;
            await reloadTable(pageNumber);
        }
        private async Task<int> Insert(AuthorDto Author)
        {
            return await Task.Factory.StartNew(() =>
            {
                return AuthorBUS.Insert(Author);
            });
        }
        private async Task<bool> Update(AuthorDto Author)
        {
            return await Task.Factory.StartNew(() =>
            {
                return AuthorBUS.Update(Author);
            });
        }

        private async void btnSave(object sender, RoutedEventArgs e)
        {
            if (mode.Equals(FormMode.New))
            {
                AuthorDto AuthorNew = new AuthorDto();
                AuthorNew.Name = txtName.Text;
                AuthorNew.Note = txtNote.Text;
                pageNumber = 1;
                await Insert(AuthorNew);
            }
            else
            {
                AuthorDto selectedItem = (AuthorDto)tableAuthors.SelectedItem;
                selectedItem.Name = txtName.Text;
                selectedItem.Note = txtNote.Text;
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
            txtNote.Text = "";
            allShowedAuthors = AuthorBUS.GetAllNotDeletedAuthors();
        }

        private void selectRow(object sender, MouseButtonEventArgs e)
        {
            if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Author.EDIT, FeatureNameUtils.FeatureGroup.AUTHOR_MANAGEMENT))
            {
                MessageBox.Show("You are not authorized for this feature!");
                return;
            } else {
                mode = FormMode.Edit;
                AuthorDto selectedItem = (AuthorDto)tableAuthors.SelectedItem;
                txtName.Text = selectedItem.Name;
                txtNote.Text = selectedItem.Note;
            }
        }


        private async void btnDelete(object sender, RoutedEventArgs e)
        {
            //if (!FeatureAttributeService.isAuthorized("Delete Book", "Book Management"))
            //{
            //    MessageBox.Show("You are not authorized for this feature!");
            //    return;
            //}
            ////Code Here if authorized
            //if (tableAuthors.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a book in table to continue!");
            //    return;
            //}
            AuthorDto selectedItem = (AuthorDto)tableAuthors.SelectedItem;
            if (selectedItem == null)
                return;
            string AuthorInfo = selectedItem.Name;
            var rs = MessageBox.Show("Are you sure you want to delete this Author?\n" + AuthorInfo, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs.Equals(MessageBoxResult.Yes))
            {
                allShowedAuthors.Remove(selectedItem);
                allAuthors.Remove(selectedItem);
                AuthorBUS.Delete(selectedItem);
                await reloadTable(pageNumber);
            }
        }


    }
}
