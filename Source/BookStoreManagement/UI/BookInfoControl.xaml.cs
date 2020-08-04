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
using BookStoreManagement.Utils;
using CommonLibrary;
using CommonLibrary.Utils;
using DatabaseCommon.DTO;

namespace BookStoreManagement.UI
{
   /// <summary>
   /// Interaction logic for BookInfoControl.xaml
   /// </summary>
   public partial class BookInfoControl : UserControl
   {
      FormMode mode = FormMode.New;
      BookDto BookDto;
      private static List<string> providerList;
      private static List<string> publisherList;
      private string photoLink = "";
      private static readonly List<string> formatList = new List<string>() 
      { 
         "Paperback","Compact Disc", 
         "Hardcover", 
         "Library Binding", 
         "Mass Market Paperback", 
         "Multimedia", 
         "Other Format" 
      };
      public BookInfoControl()
      {
         InitializeComponent();
         btnEdit.Visibility = Visibility.Collapsed;
      }
      
      public BookInfoControl(BookDto dto, FormMode formMode = FormMode.View)
      {
         InitializeComponent();
         this.mode = formMode;
         BookDto = dto;
         if (mode.Equals(FormMode.View))
         {
            ControlUtils.ChangeEnableValue(GridBookMain, false);
            btnEdit.Visibility = Visibility.Visible;
            btnBrowse.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Collapsed;
         }
         else if (mode.Equals(FormMode.Edit))
         {
            ControlUtils.ChangeEnableValue(GridBookMain, true);
            btnEdit.Visibility = Visibility.Collapsed;
            btnBrowse.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
         }
      }
      private async Task<bool> InitializeComponentValue(BookDto dto)
      {
         return await Task.Factory.StartNew(() =>
         {
            if (dto == null)
               return true;
            var p = dto.PublisherDto;
            this.Dispatcher.Invoke(new Action(() => {
               txtId.Text = dto.Barcode;
               txtPrice.Text = FormatUtils.FormatMoney(dto.Price);
               txtRemain.Text = dto.Remaining.ToString();
               txtPage.Text = dto.Page;
               txtSize.Text = dto.Size;
               txtName.Text = dto.Name;
               txtLocation.UpdateContent(dto.Location);
               txtDescription.UpdateContent(dto.Description);
               txtPublishedDate.SelectedDate = FormatUtils.ParseDate(dto.PublishedDate);
               cbFormat.SelectedItem = dto.Format;
               txtCategory.UpdateContent(DefinitionBUS.GetListCategoryName(dto.CategoryId));
               txtAuthors.UpdateContent(AuthorBUS.GetListAuthorName(dto.AuthorId));
               if (dto.ProviderDto != null)
               {
                  cbProvider.SelectedItem = dto.ProviderDto.Name;
               }
               if (dto.PublisherDto != null)
               {
                  cbPublisher.SelectedItem = dto.PublisherDto.Name;
               }
            }));
            return false;
         });
         
      }
      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         ControlUtils.ChangeEnableValue(GridBookMain, true);
         btnEdit.Visibility = Visibility.Collapsed;
         btnBrowse.Visibility = Visibility.Visible;
         btnSave.Visibility = Visibility.Visible;
         mode = FormMode.Edit;
      }
      private async Task<Dictionary<String, Object>> GetData()
      {
         return await Task.Factory.StartNew(() =>
         {
            String authors = "";
            String categories = "";
            String provider = "";
            String publisher = "";
            BookDto dto = new BookDto();
            this.Dispatcher.Invoke(new Action(() => {
               if (BookDto != null && this.mode.Equals(FormMode.Edit))
                  dto = BookDto;
               provider = cbProvider.SelectedItem.ToString();
               publisher = cbPublisher.SelectedItem.ToString();
               authors = txtAuthors.GetContent();
               categories = txtCategory.GetContent();
               dto.Format = cbFormat.SelectedItem.ToString();
               dto.Description = txtDescription.GetContent().Trim();
               dto.Location = txtLocation.GetContent().Trim();
               dto.Barcode = txtId.Text;
               dto.Price = FormatUtils.FormatMoney(txtPrice.Text.Trim());
               dto.PhotoLink = photoLink;
               dto.Page = txtPage.Text;
               dto.Remaining = txtPage.Text.ToInt32();
               dto.Size = txtSize.Text;
            }));
            return new Dictionary<String, Object>() 
            { 
               { "OBJECT" ,      dto }, 
               { "PROVIDER",     provider }, 
               { "PUBLISHER",    publisher }, 
               { "AUTHOR",       authors }, 
               { "CATEGORY",     categories }
            };
         });
      }
      private async Task<int> Insert(Dictionary<String, Object> mapData)
      {
         return await Task.Factory.StartNew(() =>
         {
            return BookBUS.Insert(mapData);
         });
      }
      private async Task<bool> Update(Dictionary<String, Object> mapData)
      {
         return await Task.Factory.StartNew(() =>
         {
            return BookBUS.Update(mapData);
         });
      }
      private async void btnSave_Click(object sender, RoutedEventArgs e)
      {
         ControlUtils.ChangeEnableValue(GridBookMain, false);
         btnEdit.Visibility = Visibility.Visible;
         btnBrowse.Visibility = Visibility.Hidden;
         btnSave.Visibility = Visibility.Collapsed;
         Dictionary<String, Object> mapData = await GetData();
         if (mode.Equals(FormMode.New))
         {
            await Insert(mapData);
         }
         else
         {
            await Update(mapData);
         }
         mode = FormMode.View;
      }

      private void btnBack_Click(object sender, RoutedEventArgs e)
      {
         MainWindow.RemoveSubChild(this);
      }
      private async Task<bool> InitializeComponentComboBox()
      {
         return await Task.Factory.StartNew(() =>
         {
            if (providerList != null)
               providerList.Clear();
            providerList = new List<string>() { "" };
            providerList.AddRange(ProviderBUS.GetProviderNameList());
            if (publisherList != null)
               publisherList.Clear();
            publisherList = new List<string>() { "" };
            publisherList.AddRange(PublisherBUS.GetPublisherNameList());
            this.Dispatcher.Invoke(new Action(() => {
               cbProvider.Items.Clear();
               cbProvider.ItemsSource = providerList;
               cbProvider.SelectedIndex = 0;
               cbPublisher.Items.Clear();
               cbPublisher.ItemsSource = publisherList;
               cbPublisher.SelectedIndex = 0;
               cbFormat.Items.Clear();
               cbFormat.ItemsSource = formatList;
               cbFormat.SelectedIndex = 0;
            }));
            return true;
         });
      }
      private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await InitializeComponentComboBox();
         if (!mode.Equals(FormMode.New))
            await InitializeComponentValue(BookDto);
      }

      private void btnBrowse_Click(object sender, RoutedEventArgs e)
      {
         MessageBox.Show("This button not yet implemented!");
      }

      private void txtPage_TextChanged(object sender, TextChangedEventArgs e)
      {
         if (String.IsNullOrEmpty(txtPage.Text.Trim()))
            return;
         if (Regex.IsMatch(txtPage.Text,"[^0-9]"))
         {
            string value = txtPage.Text;
            txtPage.Text = Regex.Replace(value, "[^0-9]", "");
         }
      }

      private void txtRemain_TextChanged(object sender, TextChangedEventArgs e)
      {
         if (String.IsNullOrEmpty(txtRemain.Text.Trim()))
            return;
         if (Regex.IsMatch(txtRemain.Text, "[^0-9]"))
         {
            string value = txtRemain.Text;
            txtRemain.Text = Regex.Replace(value, "[^0-9]", "");
         }
      }

      private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
      {
         if (String.IsNullOrEmpty(txtPrice.Text.Trim()))
            return;
         string value = txtPrice.Text;
         if (Regex.IsMatch(txtPrice.Text, "[^0-9,.]"))
         {
            value = Regex.Replace(value, "[^0-9,.]", "");
         }
         txtPrice.Text = FormatUtils.FormatMoney(value, true).ToString();
         txtPrice.SelectionStart = txtPrice.Text.Length;
         txtPrice.SelectionLength = 0;
      }
   }
}
