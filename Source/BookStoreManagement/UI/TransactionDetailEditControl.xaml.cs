using CommonLibrary;
using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using DatabaseCommon.DAO;
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
using BookStoreManagement.BUS;

namespace BookStoreManagement.UI
{
   /// <summary>
   /// Interaction logic for TransactionDetailEditControl.xaml
   /// </summary>
   public partial class TransactionDetailEditControl : UserControl
   {
      TransactionDetailDto DetailDto;
      public TransactionDetailEditControl(ref TransactionDetailDto dto, string mode = "PROVIDER")
      {
         InitializeComponent();
         if (mode.Equals("PROVIDER"))
         {
            txtPrice.IsReadOnly = false;
         }
         else if (mode.Equals("SALE"))
         {
            txtPrice.IsReadOnly = true;
         }
         DetailDto = dto;
         InitCompomentContent();

      }
      private void InitCompomentContent()
      {
         if (DetailDto == null)
            return;
         txtAmount.Text = DetailDto.Amount.ToString();
         txtDiscount.Text = DetailDto.DiscountCode;
         txtPrice.Text = DetailDto.Price.ToString();
         txtBookName.Text = DetailDto.BookName;
      }

      private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
      {
         string value = txtPrice.Text.Trim();
         if (String.IsNullOrEmpty(value))
            return;
         txtPrice.Text = FormatUtils.FormatMoney(Regex.Replace(value, "[^0-9]", ""), true).ToString();
         txtPrice.SelectionStart = txtPrice.Text.Length;
         txtPrice.SelectionLength = 0;
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

      private void btnSave_Click(object sender, RoutedEventArgs e)
      {
         DetailDto.Amount = Regex.Replace(txtAmount.Text,"[^0-9]","").ToInt32();
         DetailDto.Price = FormatUtils.FormatMoney(txtPrice.Text);
         if (!String.IsNullOrEmpty(txtDiscount.Text.Trim()))
         {
            string DiscountCode = txtDiscount.Text.Trim();
            DiscountDto discountDto = DiscountBUS.GetDiscountDto(DiscountCode);
            if (discountDto != null)
            {
               if (discountDto.Amount == 0 && discountDto.Percentage != 0)
               {
                  DetailDto.Discount = Math.Round(DetailDto.Price * discountDto.Percentage, 0).ToInt64();
               }
               else if (discountDto.Amount == 0 && discountDto.Percentage != 0)
               {
                  DetailDto.Discount = discountDto.Amount;
               }
               else
               {
                  DetailDto.Discount = 0;
               }
            }
            else
            {
               DetailDto.Discount = 0;
            }
         }
         else
         {
            DetailDto.Discount = 0;
            DetailDto.DiscountId = 0;
         }
         MainWindow.RemoveSubChild(this);
      }

      private void btnBack_Click(object sender, RoutedEventArgs e)
      {
         MainWindow.RemoveSubChild(this);
      }
   }
}
