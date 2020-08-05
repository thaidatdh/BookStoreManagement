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
   [Feature(Id = 11, Name = FeatureNameUtils.Transaction.TRANSACTION_MANAGEMENT,       Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 12, Name = FeatureNameUtils.Transaction.EDIT_STAFF,       Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 13, Name = FeatureNameUtils.Transaction.EDIT_PROVIDER,             Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 18, Name = FeatureNameUtils.Transaction.EDIT_SALE, Group = FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 14, Name = FeatureNameUtils.Transaction.DELETE,           Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 15, Name = FeatureNameUtils.Transaction.NEW_STAFF,    Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 16, Name = FeatureNameUtils.Transaction.NEW_SALE,     Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   [Feature(Id = 17, Name = FeatureNameUtils.Transaction.NEW_PROVIDER, Group =  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT)]
   
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
         cbType.SelectedIndex = 0;

         if (FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_STAFF,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            btnNewStaff.Visibility = Visibility.Visible;
         }
         else
         {
            btnNewStaff.Visibility = Visibility.Collapsed;
         }
         if (FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_SALE,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            btnNewCustomer.Visibility = Visibility.Visible;
         }
         else
         {
            btnNewCustomer.Visibility = Visibility.Collapsed;
         }
         if (FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_PROVIDER,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
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
         if (allShowedTransaction == null)
            return;
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
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataLoad("Import Transaction");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private async void cbProvider_Unchecked(object sender, RoutedEventArgs e)
      {
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataUnLoad("Import Transaction");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }
      private async Task<bool> DataLoad(string type)
      {
         return await Task.Factory.StartNew(() =>
         {
            if (allShowedTransaction != null && !allShowedTransaction.Any(n => n.Description.StartsWith(type)))
            {
               allShowedTransaction.AddRange(allTransaction.Where(n => n.Description.StartsWith(type)));
               allShowedTransaction = allShowedTransaction.OrderBy(n => n.TransactionId).ToList();
            }

            return true;
         });
      }
      private async Task<bool> DataUnLoad(string type)
      {
         return await Task.Factory.StartNew(() =>
         {
            if (allShowedTransaction != null)
            {
               allShowedTransaction.RemoveAll(n => n.Description.StartsWith(type));
            }

            return true;
         });
      }
      private async void cbCustomer_Checked(object sender, RoutedEventArgs e)
      {
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataLoad("Sale Transaction");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private async void cbCustomer_Unchecked(object sender, RoutedEventArgs e)
      {
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataUnLoad("Sale Transaction");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }
      private async void cbStaff_Checked(object sender, RoutedEventArgs e)
      {
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataLoad("Transaction for Staff");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }
      
      private async void cbStaff_Unchecked(object sender, RoutedEventArgs e)
      {
         if (!isInitial) return;
         if (lbPaging != null)
            lbPaging.Text = "Loading";
         await DataUnLoad("Transaction for Staff");
         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_SALE,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         MainWindow.AddSubChild(new SaleTransactionControl());
      }

      private void btnNewStaff_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_STAFF,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         MainWindow.AddSubChild(new StaffTransactionControl());
      }

      private void btnNewProvider_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.NEW_PROVIDER,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         MainWindow.AddSubChild(new ProviderTransactionControl());
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         //Code Here if authorized
         if (tableTransaction.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         TransactionDto selectedItem = (TransactionDto)tableTransaction.SelectedItem;
         if (selectedItem == null || selectedItem.IsDeleted == true)
            return;
         if (selectedItem.Description.StartsWith("Sale") && FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.EDIT_SALE, FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MainWindow.AddSubChild(new SaleTransactionControl(selectedItem, FormMode.Edit));
         }
         else if (selectedItem.Description.StartsWith("Transaction") && FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.EDIT_STAFF, FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MainWindow.AddSubChild(new StaffTransactionControl(selectedItem, FormMode.Edit));
         }
         else if (selectedItem.Description.StartsWith("Import") && FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.EDIT_PROVIDER, FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MainWindow.AddSubChild(new ProviderTransactionControl(selectedItem, FormMode.Edit));
         }
         else
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
      }

      private async void btnDelete_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.DELETE,  FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
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
         if (tableTransaction.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         TransactionDto selectedItem = (TransactionDto)tableTransaction.SelectedItem;
         if (selectedItem == null || selectedItem.IsDeleted == true)
            return;
         if (selectedItem.Description.StartsWith("Sale"))
         {
            MainWindow.AddSubChild(new SaleTransactionControl(selectedItem, FormMode.View));
         }
         else if (selectedItem.Description.StartsWith("Transaction"))
         {
            MainWindow.AddSubChild(new StaffTransactionControl(selectedItem, FormMode.View));
         }
         else if (selectedItem.Description.StartsWith("Import"))
         {
            MainWindow.AddSubChild(new ProviderTransactionControl(selectedItem, FormMode.View));
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
