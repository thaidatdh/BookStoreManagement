using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using CommonLibrary;
using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
using PagedList;
using System;
using System.Collections.Generic;
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
   /// Interaction logic for SaleTransactionControl.xaml
   /// </summary>
   public partial class SaleTransactionControl : UserControl
   {
      int pageNumber = 1;
      int pageNumberTransaction = 1;
      int pageSize = 10;
      FormMode mode = FormMode.New;
      IPagedList<BookDto> listBooks;
      IPagedList<TransactionDetailDto> listDetail;
      List<BookDto> allShowedBooks;
      List<BookDto> allBooks;
      List<TransactionDetailDto> allDetails;
      List<CustomerDto> listCustomer = new List<CustomerDto>();
      TransactionDto Transaction;
      readonly List<string> listPaymentType = new List<string>() { "CASH", "DEBIT", "CREDIT", "MOMO" };
      bool isInitial = false;
      public SaleTransactionControl()
      {
         InitializeComponent();
         EnableEditMode();
         Transaction = new TransactionDto();
         mode = FormMode.New;
         dateTransaction.SelectedDate = DateTime.Now;
         allDetails = new List<TransactionDetailDto>();
      }
      private void EnableViewMode()
      {
         btnEdit.Visibility = Visibility.Visible;
         btnAdd.Visibility = Visibility.Collapsed;
         btnDeleteDetail.Visibility = Visibility.Hidden;
         btnEditDetail.Visibility = Visibility.Collapsed;
         btnSave.Visibility = Visibility.Collapsed;
         ListBookContent.Visibility = Visibility.Collapsed;
         ControlUtils.ChangeEnableValue(transactionContent, false);
      }
      private void EnableEditMode()
      {
         btnEdit.Visibility = Visibility.Collapsed;
         btnAdd.Visibility = Visibility.Visible;
         btnDeleteDetail.Visibility = Visibility.Visible;
         btnEditDetail.Visibility = Visibility.Visible;
         btnSave.Visibility = Visibility.Visible;
         ListBookContent.Visibility = Visibility.Visible;
         ControlUtils.ChangeEnableValue(transactionContent, true);
      }
      public SaleTransactionControl(TransactionDto dto, FormMode formMode)
      {
         InitializeComponent();
         mode = formMode;
         if (mode == FormMode.Edit)
         {
            EnableEditMode();
         }
         else
         {
            EnableViewMode();
         }
         Transaction = dto;
         allDetails = new List<TransactionDetailDto>();
         allDetails.AddRange(Transaction.TransactionDetails);
      }
      private void InitComponentValue()
      {
         dateTransaction.SelectedDate = FormatUtils.ParseDate(Transaction.EntryDate);
         if (Transaction.CustomerDto != null)
         {
            string name = (Transaction.CustomerDto.FirstName + " " + Transaction.CustomerDto.LastName).Trim();
            int SelectedIndex = listCustomer.FindIndex(n => (n.FirstName + " " + n.LastName).Trim().Equals(name));
            cbCustomer.SelectedIndex = SelectedIndex < 0 ? 0 : SelectedIndex;
         }
         else
         {
            cbCustomer.SelectedIndex = 0;
         }
         int SelectedTypeIndex = listPaymentType.FindIndex(n => n.Equals(Transaction.Type));
         cbPaymentType.SelectedIndex = SelectedTypeIndex < 0 ? 0 : SelectedTypeIndex;
      }
      private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
      {
         string type = cbSearchType.SelectedValue.ToString().ToKey().ToUpper().Trim();
         string value = txtSearchValue.Text.ToUpper();
         string temp = "";
         switch (type)
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
               allShowedBooks = allBooks.Where(n => (String.IsNullOrEmpty(temp) ? false : n.PublishedDate.ToUpper().Contains(temp)) || n.PublishedDate.ToUpper().Contains(value.ToKey())).ToList();
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
            case "FORMAT":
               allShowedBooks = allBooks.Where(n => n.Format.ToUpper().Contains(value)).ToList();
               break;
            case "PAGE":
               allShowedBooks = allBooks.Where(n => n.Page.ToUpper().Contains(value)).ToList();
               break;
            case "SIZE":
               allShowedBooks = allBooks.Where(n => n.Size.ToUpper().Contains(value)).ToList();
               break;
            case "REMAINING":
               if (!Regex.IsMatch(value, "[^0-9]"))
               {
                  allShowedBooks = allBooks.Where(n => n.Remaining == value.ToInt32()).ToList();
               }
               break;
         }

         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private void btnView_Click(object sender, RoutedEventArgs e)
      {
         if (tableData.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableData.SelectedItem;
         if (selectedItem == null)
            return;
         //Code here
         MainWindow.AddSubChild(new BookInfoControl(selectedItem, FormMode.View));
      }
      private async Task<IPagedList<BookDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allBooks = BookBUS.GetAllNotDeletedBooks();
            if (allShowedBooks == null)
            {
               allShowedBooks = new List<BookDto>();
               allShowedBooks.AddRange(allBooks);
            }

            return allShowedBooks.ToPagedList(pagedNumber, pageSize);
         });
      }
      private async Task<IPagedList<TransactionDetailDto>> GetPagedListDetailAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            if (allDetails == null)
               allDetails = new List<TransactionDetailDto>();
            return allDetails.ToPagedList(pagedNumber, pageSize);
         });
      }
      private async Task reloadTableDetail(int pageNumber)
      {
         listDetail = await GetPagedListDetailAsync(pageNumber);
         btnPrevious.IsEnabled = listDetail.HasPreviousPage;
         btnNext.IsEnabled = listDetail.HasNextPage;
         tableBooks.ItemsSource = listDetail.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allDetails.Count == 0 ? 0 : pageStart + 1;
         int end = allDetails.Count < pageSize ? allDetails.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allDetails.Count);
      }
      private async Task reloadTable(int pageNumber)
      {
         listBooks = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listBooks.HasPreviousPage;
         btnNext.IsEnabled = listBooks.HasNextPage;
         tableData.ItemsSource = listBooks.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedBooks.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedBooks.Count < pageSize ? allShowedBooks.Count : pageStart + pageSize;
         lbPagingBook.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedBooks.Count);
      }
      private async void btnPreviousBook_Click(object sender, RoutedEventArgs e)
      {
         if (listBooks == null || !listBooks.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNextBook_Click(object sender, RoutedEventArgs e)
      {
         if (listBooks == null || !listBooks.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }

      private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         cbSearchType.ItemsSource = new List<string> { "Name", "Author", "Publisher", "Published Date", "ID", "Barcode", "Category", "Format", "Size", "Page", "Remaining" };
         cbSearchType.SelectedIndex = 0;
         cbPaymentType.ItemsSource = listPaymentType;
         cbPaymentType.SelectedIndex = 0;
         listCustomer = CustomerDao.GetAll();
         cbCustomer.ItemsSource = listCustomer.Select(n => (n.FirstName + " " + n.LastName).Trim()).ToList();
         cbCustomer.SelectedIndex = 0;
         InitComponentValue();
         await reloadTable(pageNumber);
         await reloadTableDetail(pageNumberTransaction);
         this.Dispatcher.Invoke(new Action(() => isInitial = true));
      }

      private void btnBack_Click(object sender, RoutedEventArgs e)
      {
         MainWindow.RemoveSubChild(this);
      }

      private async void btnDeleteDetail_Click(object sender, RoutedEventArgs e)
      {
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         TransactionDetailDto selectedItem = (TransactionDetailDto)tableBooks.SelectedItem;
         if (selectedItem == null)
            return;
         var rs = MessageBox.Show("Are you sure you want to delete this book?\n" + selectedItem.BookName, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
         if (rs.Equals(MessageBoxResult.Yes))
         {
            allDetails.Remove(selectedItem);
            await reloadTableDetail(pageNumber);
         }
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listDetail == null || !listDetail.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }

      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listDetail == null || !listDetail.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private void btnSave_Click(object sender, RoutedEventArgs e)
      {
         EnableViewMode();
         if (Transaction == null)
            Transaction = new TransactionDto();
         Transaction.TransactionDetails = allDetails;
         Transaction.Type = cbPaymentType.SelectedItem.ToString();
         Transaction.Amount = 0;
         Transaction.Discount = 0;
         allDetails.ForEach(n => 
         { 
            Transaction.Amount += n.Amount*n.Price;
            Transaction.Discount += n.Discount;
         });
         CustomerDto customer = listCustomer.FirstOrDefault(n => (n.FirstName + " " + n.LastName).Trim().Equals(cbCustomer.SelectedItem.ToString().Trim()));
         Transaction.CustomerId = customer == null ? (Transaction.CustomerId == 0 ? 0 : Transaction.CustomerId) : customer.UserId;
         Transaction.EntryDate = FormatUtils.FormatDate(dateTransaction.SelectedDate.Value);
         if (mode == FormMode.New)
         {
            TransactionBUS.Insert(Transaction);
         }
         else if (mode == FormMode.Edit)
         {
            TransactionBUS.Update(Transaction);
         }
         mode = FormMode.View;
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.EDIT_SALE, FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         mode = FormMode.Edit;
         EnableEditMode();
      }

      private async void btnAdd_Click(object sender, RoutedEventArgs e)
      {
         if (tableData.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         BookDto selectedItem = (BookDto)tableData.SelectedItem;
         if (selectedItem == null)
            return;
         if (Transaction == null)
            Transaction = new TransactionDto();
         TransactionDetailDto existsBook = allDetails.FirstOrDefault(n => n.BookId == selectedItem.BookId);
         if (existsBook == null)
         {
            TransactionDetailDto dto = new TransactionDetailDto();
            dto.TransactionId = Transaction.TransactionId;
            dto.BookId = selectedItem.BookId;
            dto.Price = selectedItem.Price;
            dto.Amount = 1;
            allDetails.Add(dto);
         }
         else
         {
            allDetails.Remove(existsBook);
            existsBook.Amount += 1;
            allDetails.Add(existsBook);
         }
         await reloadTableDetail(pageNumber);
      }

      private async void btnEditDetail_Click(object sender, RoutedEventArgs e)
      {
         if (tableBooks.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         TransactionDetailDto selectedItem = (TransactionDetailDto)tableBooks.SelectedItem;
         if (selectedItem == null)
            return;
         allDetails.Remove(selectedItem);
         MainWindow.AddSubChild(new TransactionDetailEditControl(ref selectedItem, "SALE"));
         allDetails.Add(selectedItem);
         await reloadTableDetail(pageNumber);
      }

      private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
         bool isInit = false;
         this.Dispatcher.Invoke(new Action(() => isInit = isInitial));
         if (this.Visibility == Visibility.Visible && isInit)
         {
            await reloadTableDetail(pageNumber);
         }
      }
   }
}
