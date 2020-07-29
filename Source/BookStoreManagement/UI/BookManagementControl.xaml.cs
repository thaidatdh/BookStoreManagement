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
   /// Interaction logic for BookManagementControl.xaml
   /// Paging: https://www.youtube.com/watch?v=L1wpQ_fKjVw
   /// </summary>
   /// 
   [Feature(Id = 2, Name = "Book Management",   Group = "Book Management")]
   [Feature(Id = 3, Name = "Import Books",      Group = "Book Management")]
   [Feature(Id = 4, Name = "Add New Book",      Group = "Book Management")]
   [Feature(Id = 5, Name = "Edit Book",         Group = "Book Management")]
   [Feature(Id = 6, Name = "Delete Book",       Group = "Book Management")]
   public partial class BookManagementControl : UserControl
   {
      int pageNumber = 1;
      int pageSize = 10;
      IPagedList<BookDto> listBooks;
      List<BookDto> allShowedBooks;
      List<BookDto> allBooks;
      public BookManagementControl()
      {
         InitializeComponent();
         cbType.ItemsSource = new List<string> { "Name", "Author", "Publisher", "Published Date", "ID", "Barcode", "Category" };
         cbType.SelectedIndex = 0;
      }
      private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
      }
      private async Task<IPagedList<BookDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            if (allBooks == null || allBooks.Count == 0)
               allBooks = BookDao.Where(n => n.IsDeleted == false).ToList();
            if (allShowedBooks == null)
            {
               allShowedBooks = new List<BookDto>();
               allShowedBooks.AddRange(allBooks);
            }
               
            return allShowedBooks.ToPagedList(pagedNumber, pageSize);
         });
      }

      private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
      {
         string type = cbType.SelectedValue.ToString().ToKey().ToUpper().Trim();
         string value = txtSearchValue.Text.ToUpper();
         string temp = "";
         switch(type)
         {
            case "NAME":
               allShowedBooks = allBooks.Where(n => n.Name.ToUpper().Contains(value)).ToList();
               break;
            case "AUTHOR":
               allShowedBooks = allBooks.Where(n => n.ListAuthorDto.Select(a => a.Name).Where(a => a.ToUpper().Contains(value)).Any()).ToList();
               break;
            case "PUBLISHER":
               allShowedBooks = allBooks.Where(n => n.PublisherDto != null && n.PublisherDto.Name.ToUpper().Contains(value)).ToList();
               break;
            case "PUBLISHEDDATE":
               temp = FormatUtils.FormatDate(value);
               allShowedBooks = allBooks.Where(n => (String.IsNullOrEmpty(temp) ? false : n.PublishedDate.ToUpper().Contains(temp)) || n.PublishedDate.ToUpper().Contains(value)).ToList();
               break;
            case "ID":
               allShowedBooks = allBooks.Where(n => n.BookId.ToString().ToUpper().Contains(value)).ToList();
               break;
            case "BARCODE":
               allShowedBooks = allBooks.Where(n => n.Barcode.ToUpper().Contains(value)).ToList();
               break;
            case "CATEGORY":
               allShowedBooks = allBooks.Where(n => n.ListCategoryDto.Select(a => a.Value1).Where(a => a.ToUpper().Contains(value)).Any()).ToList();
               break;
         }
         
         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private void btnImport_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Import Books", "Book Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
      }

      private void btnView_Click(object sender, RoutedEventArgs e)
      {
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableBooks.SelectedItem;
         if (selectedItem == null)
            return;
         //Code here
      }

      private void btnAdd_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Add New Book", "Book Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableBooks.SelectedItem;
         if (selectedItem == null)
            return;
         //Code here
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Edit Book", "Book Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableBooks.SelectedItem;
         if (selectedItem == null)
            return;
         //Code here
      }
      
      private async void btnDelete_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Delete Book", "Book Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableBooks.SelectedItem;
         if (selectedItem == null) 
            return;
         string bookInfo = selectedItem.Name + (selectedItem.ListAuthorDto.Count > 0 ? "\nAuthor: " + selectedItem.ListAuthorDto.First().Name : "");
         var rs = MessageBox.Show("Are you sure you want to delete this book?\n" + bookInfo, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
         if (rs.Equals(MessageBoxResult.Yes))
         {
            allShowedBooks.Remove(selectedItem);
            allBooks.Remove(selectedItem);
            BookDao.Delete(selectedItem.BookId);
            await reloadTable(pageNumber);
         }
      }
      private async Task reloadTable(int pageNumber)
      {
         listBooks = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listBooks.HasPreviousPage;
         btnNext.IsEnabled = listBooks.HasNextPage;
         tableBooks.ItemsSource = listBooks.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedBooks.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedBooks.Count < pageSize ? allShowedBooks.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedBooks.Count);
      }
      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (!listBooks.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (!listBooks.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }
   }
}
