using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using CommonLibrary;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
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
   /// Interaction logic for StaffTransactionControl.xaml
   /// </summary>
   public partial class StaffTransactionControl : UserControl
   {
      FormMode mode = FormMode.New;
      TransactionDto Transaction;
      private List<StaffDto> listStaff = new List<StaffDto>();
      public StaffTransactionControl()
      {
         InitializeComponent();
         btnEdit.Visibility = Visibility.Collapsed;
         btnDiscard.Visibility = Visibility.Collapsed;
         btnSave.Visibility = Visibility.Visible;
         InitStaffCombobox();
         cbStaff.SelectedIndex = 0;
         ControlUtils.ChangeEnableValue(transactionContent, true);
      }
      private void InitStaffCombobox()
      {
         listStaff = StaffDao.GetAll();
         cbStaff.ItemsSource = listStaff.Select(n => (n.FirstName + " " + n.LastName).Trim()).ToList();
      }
      private void InitData()
      {
         if (Transaction == null)
            return;
         txtAmount.Text = FormatUtils.FormatMoney(Transaction.Amount);
         if (Transaction.StaffDto != null)
         {
            string name = (Transaction.StaffDto.FirstName + " " + Transaction.StaffDto.LastName).Trim();
            int SelectedIndex = listStaff.FindIndex(n => (n.FirstName + " " + n.LastName).Trim().Equals(name));
            cbStaff.SelectedIndex = SelectedIndex < 0 ? 0 : SelectedIndex;
         }
         else
         {
            cbStaff.SelectedIndex = 0;
         }
         dateTransaction.SelectedDate = FormatUtils.ParseDate(Transaction.EntryDate);
      }
      public StaffTransactionControl(TransactionDto dto, FormMode formMode)
      {
         InitializeComponent();
         InitStaffCombobox();
         Transaction = dto;
         InitData();
         mode = formMode;
         if (mode == FormMode.View)
         {
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            btnDiscard.Visibility = Visibility.Collapsed;
            ControlUtils.ChangeEnableValue(transactionContent, false);
         }
         else
         {
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            btnDiscard.Visibility = Visibility.Collapsed;
            ControlUtils.ChangeEnableValue(transactionContent, true);
         }
         
      }
      private void UpdateTransaction()
      {
         if (Transaction == null)
         {
            Transaction = new TransactionDto();
         }
         StaffDto selectedStaff = listStaff.FirstOrDefault(n => (n.FirstName + " " + n.LastName).Trim().Equals(cbStaff.SelectedItem.ToString()));
         Transaction.StaffId = selectedStaff == null ? 0 : selectedStaff.StaffId;
         Transaction.Amount = FormatUtils.FormatMoney(txtAmount.Text);
         Transaction.EntryDate = FormatUtils.FormatDate(dateTransaction.SelectedDate == null ? DateTime.Now : dateTransaction.SelectedDate.Value);
      }
      private void btnSave_Click(object sender, RoutedEventArgs e)
      {
         if (mode == FormMode.New)
         {
            UpdateTransaction();
            int id = TransactionBUS.Insert(Transaction);
            Transaction.TransactionId = id;
         }
         else if (mode == FormMode.Edit && Transaction != null)
         {
            UpdateTransaction();
            TransactionBUS.Update(Transaction);
         }
         btnEdit.Visibility = Visibility.Visible;
         btnSave.Visibility = Visibility.Collapsed;
         btnDiscard.Visibility = Visibility.Collapsed;
         ControlUtils.ChangeEnableValue(transactionContent, false);
      }

      private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
      {
         string value = txtAmount.Text.Trim();
         if (String.IsNullOrEmpty(value))
            return;
         txtAmount.Text = FormatUtils.FormatMoney(Regex.Replace(value, "[^0-9]", ""), true).ToString();
         txtAmount.SelectionStart = txtAmount.Text.Length;
         txtAmount.SelectionLength = 0;
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Transaction.EDIT_STAFF, FeatureNameUtils.FeatureGroup.TRANSACTION_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         btnSave.Visibility = Visibility.Visible;
         btnDiscard.Visibility = Visibility.Visible;
         btnEdit.Visibility = Visibility.Collapsed;
         mode = FormMode.Edit;
         ControlUtils.ChangeEnableValue(transactionContent, true);
      }

      private void btnBack_Click(object sender, RoutedEventArgs e)
      {
         MainWindow.RemoveSubChild(this);
      }

      private void btnDiscard_Click(object sender, RoutedEventArgs e)
      {
         if (Transaction == null)
            return;
         btnSave.Visibility = Visibility.Collapsed;
         btnDiscard.Visibility = Visibility.Collapsed;
         btnEdit.Visibility = Visibility.Visible;
         InitData();
         ControlUtils.ChangeEnableValue(transactionContent, false);
      }
   }
}
