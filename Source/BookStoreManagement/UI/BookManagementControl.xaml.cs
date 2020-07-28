using BookStoreManagement.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
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
   /// </summary>
   /// 
   [Feature(Id = 2, Name = "Book Management", Group = "Book Management")]
   public partial class BookManagementControl : UserControl
   {
      public BookManagementControl()
      {
         InitializeComponent();
         cbType.ItemsSource = new List<string> { "Name", "Author", "Publisher", "Published Date", "ID", "Barcode" };
         cbType.SelectedIndex = 0;
      }
      private void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         List<BookDto> listBook = BookDao.GetAll();
         tableBooks.ItemsSource = listBook;
      }

      private void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
      {

      }
      [Feature(Id = 3, Name = "Import Books", Group = "Book Management")]
      public void btnImport_Click(object sender, RoutedEventArgs e)
      {
         //
      }

      private void btnView_Click(object sender, RoutedEventArgs e)
      {

      }
      [Feature(Id = 4, Name = "Add New Book", Group = "Book Management")]
      public void btnAdd_Click(object sender, RoutedEventArgs e)
      {

      }
      [Feature(Id = 5, Name = "Edit Book", Group = "Book Management")]
      public void btnEdit_Click(object sender, RoutedEventArgs e)
      {

      }
      [Feature(Id = 6, Name = "Delete Book", Group = "Book Management")]
      public void btnDelete_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}
