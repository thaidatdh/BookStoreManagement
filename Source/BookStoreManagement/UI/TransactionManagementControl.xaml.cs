using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using CommonLibrary;
using CommonLibrary.Utils;
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
   /// Interaction logic for TransactionManagementControl.xaml
   /// </summary>
   /// 
   [Feature(Id = 11, Name = "Transaction Management",       Group = "Transaction Management")]
   [Feature(Id = 12, Name = "Import Transaction",           Group = "Transaction Management")]
   [Feature(Id = 13, Name = "Edit Transaction",             Group = "Transaction Management")]
   [Feature(Id = 14, Name = "Delete Transaction",           Group = "Transaction Management")]
   [Feature(Id = 15, Name = "Add New Staff Transaction",    Group = "Transaction Management")]
   [Feature(Id = 16, Name = "Add New Sale Transaction",     Group = "Transaction Management")]
   [Feature(Id = 17, Name = "Add New Provider Transaction", Group = "Transaction Management")]
   public partial class TransactionManagementControl : UserControl
   {
      int pageNumber = 1;
      int pageSize = 10;
      IPagedList<TransactionDto> listTransaction;
      List<TransactionDto> allShowedTransaction;
      List<TransactionDto> allTransaction;
      bool isInitial = false;
      public TransactionManagementControl()
      {
         InitializeComponent();
         cbType.ItemsSource = new List<string> { "Date", "Receiver", "Discount", "Amount" };
         if (FeatureAttributeService.isAuthorized("Add New Staff Transaction", "Transaction Management"))
         {
            btnNewStaff.Visibility = Visibility.Visible;
         }
         else
         {
            btnNewStaff.Visibility = Visibility.Collapsed;
         }
         if (FeatureAttributeService.isAuthorized("Add New Sale Transaction", "Transaction Management"))
         {
            btnNewCustomer.Visibility = Visibility.Visible;
         }
         else
         {
            btnNewCustomer.Visibility = Visibility.Collapsed;
         }
         if (FeatureAttributeService.isAuthorized("Add New Provider Transaction", "Transaction Management"))
         {
            btnNewProvider.Visibility = Visibility.Visible;
         }
         else
         {
            btnNewProvider.Visibility = Visibility.Collapsed;
         }
      }
      private async Task reloadTable(int pageNumber)
      {
         listTransaction = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listTransaction.HasPreviousPage;
         btnNext.IsEnabled = listTransaction.HasNextPage;
         tableTransaction.ItemsSource = listTransaction.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedTransaction.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedTransaction.Count < pageSize ? allShowedTransaction.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedTransaction.Count);
      }
      private async Task<IPagedList<TransactionDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allTransaction = TransactionBUS.GetAllTransaction();
            if (allShowedTransaction == null)
            {
               allShowedTransaction = new List<TransactionDto>();
               allShowedTransaction.AddRange(allTransaction);
            }

            return allShowedTransaction.ToPagedList(pagedNumber, pageSize);
         });
      }
      private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
         this.Dispatcher.Invoke(new Action(() => isInitial = true));
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

      private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
      {
         string type = cbType.SelectedValue.ToString().ToKey().ToUpper().Trim();
         string value = txtSearchValue.Text.ToUpper();
         string temp = "";
         switch (type)
         {
            case "RECEIVER":
               allShowedTransaction = allTransaction.Where(n => n.ReceiverName.ToUpper().Contains(value)).ToList();
               break;
            case "DATE":
               temp = FormatUtils.FormatDate(value);
               allShowedTransaction = allTransaction.Where(n => (String.IsNullOrEmpty(temp) ? false : n.EntryDate.ToUpper().Contains(temp)) || n.EntryDate.ToUpper().Contains(value.ToKey())).ToList();
               break;
            case "AMOUNT":
               if (!Regex.IsMatch(value, "[^0-9]"))
               {
                  allShowedTransaction = allTransaction.Where(n => n.Amount == value.ToInt64()).ToList();
               }
               break;
            case "DISCOUNT":
               if (!Regex.IsMatch(value, "[^0-9]"))
               {
                  allShowedTransaction = allTransaction.Where(n => n.Discount == value.ToInt64()).ToList();
               }
               break;
         }

         pageNumber = 1;
         await reloadTable(pageNumber);
      }
      private async void cbProvider_Checked(object sender, RoutedEventArgs e)
      {
         if (!allShowedTransaction.Any(n => n.Description.StartsWith("Import Transaction")))
         {
            allShowedTransaction.AddRange(allTransaction.Where(n => n.Description.StartsWith("Import Transaction")));
            allShowedTransaction.OrderBy(n => n.TransactionId);
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private async void cbProvider_Unchecked(object sender, RoutedEventArgs e)
      {
         if (allShowedTransaction.Any(n => n.Description.StartsWith("Import Transaction")))
         {
            allShowedTransaction.RemoveAll(n => n.Description.StartsWith("Import Transaction"));
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private async void cbCustomer_Checked(object sender, RoutedEventArgs e)
      {
         if (!allShowedTransaction.Any(n => n.Description.StartsWith("Sale Transaction")))
         {
            allShowedTransaction.AddRange(allTransaction.Where(n => n.Description.StartsWith("Sale Transaction")));
            allShowedTransaction.OrderBy(n => n.TransactionId);
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private async void cbCustomer_Unchecked(object sender, RoutedEventArgs e)
      {
         if (allShowedTransaction.Any(n => n.Description.StartsWith("Sale Transaction")))
         {
            allShowedTransaction.RemoveAll(n => n.Description.StartsWith("Sale Transaction"));
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private async void cbStaff_Checked(object sender, RoutedEventArgs e)
      {
         if (!allShowedTransaction.Any(n => n.Description.StartsWith("Transaction for Staff")))
         {
            allShowedTransaction.AddRange(allTransaction.Where(n => n.Description.StartsWith("Transaction for Staff")));
            allShowedTransaction.OrderBy(n => n.TransactionId);
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private async void cbStaff_Unchecked(object sender, RoutedEventArgs e)
      {
         if (allShowedTransaction.Any(n => n.Description.StartsWith("Transaction for Staff")))
         {
            allShowedTransaction.RemoveAll(n => n.Description.StartsWith("Transaction for Staff"));
            pageNumber = 1;
            await reloadTable(pageNumber);
         }
      }

      private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Add New Sale Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
      }

      private void btnNewStaff_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Add New Staff Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
      }

      private void btnNewProvider_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Add New Provider Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Edit Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableTransaction.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
      }

      private async void btnDelete_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("Delete Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableTransaction.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         TransactionDto selectedItem = (TransactionDto)tableTransaction.SelectedItem;
         if (selectedItem == null || selectedItem.IsDeleted == true)
            return;
         var rs = MessageBox.Show("Are you sure you want to delete this transaction?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
         if (rs.Equals(MessageBoxResult.Yes))
         {
            allShowedTransaction.Remove(selectedItem);
            allTransaction.Remove(selectedItem);
            TransactionBUS.Delete(selectedItem);
            await reloadTable(pageNumber);
         }
      }

      private void btnView_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized("View Transaction", "Transaction Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableTransaction.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listTransaction == null || !listTransaction.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }

      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listTransaction == null || !listTransaction.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }
   }
}
