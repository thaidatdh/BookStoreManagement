using BookStoreManagement.BUS;
using BookStoreManagement.Utils;
using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using PagedList;
using System;
using System.Collections.Generic;
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
   /// Interaction logic for CategoryManagementControl.xaml
   /// </summary>
   /// 
   [Feature(Id = 7, Name = FeatureNameUtils.Category.MANAGEMENT, Group = FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT)]
   [Feature(Id = 8, Name = FeatureNameUtils.Category.NEW, Group = FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT)]
   [Feature(Id = 9, Name = FeatureNameUtils.Category.EDIT, Group = FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT)]
   [Feature(Id = 10, Name = FeatureNameUtils.Category.DELETE, Group = FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT)]
   public partial class CategoryManagementControl : UserControl
   {
      int pageNumber = 1;
      int pageSize = 10;
      IPagedList<DefinitionDto> listCategory;
      List<DefinitionDto> allShowedCategory;
      List<DefinitionDto> allCategory;
      DefinitionDto CategoryDto;
      FormMode mode = FormMode.Edit;
      public CategoryManagementControl()
      {
         InitializeComponent();
         btnSave.Visibility = Visibility.Hidden;
      }
      private async Task<IPagedList<DefinitionDto>> GetPagedListAsync(int pagedNumber = 1)
      {
         return await Task.Factory.StartNew(() =>
         {
            allCategory = DefinitionBUS.GetAllCategory();
            if (allShowedCategory == null)
            {
               allShowedCategory = new List<DefinitionDto>();
               allShowedCategory.AddRange(allCategory);
            }

            return allShowedCategory.ToPagedList(pagedNumber, pageSize);
         });
      }
      private async Task reloadTable(int pageNumber)
      {
         listCategory = await GetPagedListAsync(pageNumber);
         btnPrevious.IsEnabled = listCategory.HasPreviousPage;
         btnNext.IsEnabled = listCategory.HasNextPage;
         tableCategory.ItemsSource = listCategory.ToList();
         int pageStart = (pageNumber - 1) * pageSize;
         int start = allShowedCategory.Count == 0 ? 0 : pageStart + 1;
         int end = allShowedCategory.Count < pageSize ? allShowedCategory.Count : pageStart + pageSize;
         lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedCategory.Count);
      }

      private async void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         await reloadTable(pageNumber);
      }

      private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
      {
         string value = txtSearchValue.Text.ToUpper().ToKey();
         allShowedCategory = allCategory.Where(n => n.Value1.ToKey().Contains(value)).ToList();
         pageNumber = 1;
         await reloadTable(pageNumber);
      }

      private void btnAdd_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Category.NEW, FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         txtCategory.Text = "";
         txtCategory.IsReadOnly = false;
         btnSave.Visibility = Visibility.Visible;
         mode = FormMode.New;
      }

      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Category.EDIT, FeatureNameUtils.FeatureGroup.CATEGORY_MANAGEMENT))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableCategory.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         DefinitionDto selectedItem = (DefinitionDto)tableCategory.SelectedItem;
         if (selectedItem == null)
            return;
         mode = FormMode.Edit;
         txtCategory.Text = selectedItem.Value1;
         txtCategory.IsReadOnly = false;
         btnSave.Visibility = Visibility.Visible;
      }

      private async void btnDelete_Click(object sender, RoutedEventArgs e)
      {
         if (!FeatureAttributeService.isAuthorized(FeatureNameUtils.Category.DELETE, "Book Management"))
         {
            MessageBox.Show("You are not authorized for this feature!");
            return;
         }
         //Code Here if authorized
         if (tableCategory.SelectedItem == null)
         {
            MessageBox.Show("Please select a book in table to continue!");
            return;
         }
         DefinitionDto selectedItem = (DefinitionDto)tableCategory.SelectedItem;
         if (selectedItem == null)
            return;
         var rs = MessageBox.Show("Are you sure you want to delete category: " + selectedItem.Value1 + "?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
         if (rs.Equals(MessageBoxResult.Yes))
         {
            allShowedCategory.Remove(selectedItem);
            allCategory.Remove(selectedItem);
            DefinitionBUS.DeleteCategory(selectedItem);
            await reloadTable(pageNumber);
         }
      }

      private async void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         if (listCategory == null || !listCategory.HasPreviousPage)
            return;
         await reloadTable(--pageNumber);
      }

      private async void btnNext_Click(object sender, RoutedEventArgs e)
      {
         if (listCategory == null || !listCategory.HasNextPage)
            return;
         await reloadTable(++pageNumber);
      }
      private async Task<int> Insert(string categoryName)
      {
         return await Task.Factory.StartNew(() =>
         {
            return DefinitionBUS.InsertCategory(categoryName);
         });
      }
      private async Task<bool> Update(DefinitionDto dto)
      {
         return await Task.Factory.StartNew(() =>
         {
            return DefinitionBUS.UpdateCategory(dto);
         });
      }
      private async void btnSave_Click(object sender, RoutedEventArgs e)
      {
         if (mode.Equals(FormMode.New))
         {
            await Insert(txtCategory.Text.Trim());
         }
         else if (CategoryDto != null && mode.Equals(FormMode.Edit) && !CategoryDto.Value1.Trim().Equals(txtCategory.Text.Trim()))
         {
            CategoryDto.Value1 = txtCategory.Text;
            await Update(CategoryDto);
         }
         if (!mode.Equals(FormMode.View))
         {
            txtCategory.IsReadOnly = true;
            btnSave.Visibility = Visibility.Hidden;
            await reloadTable(pageNumber);
            mode = FormMode.View;
         }
      }

      private void tableCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         DefinitionDto selectedItem = (DefinitionDto)tableCategory.SelectedItem;
         if (selectedItem == null)
            return;
         CategoryDto = selectedItem;
         txtCategory.Text = selectedItem.Value1;
         btnSave.Visibility = Visibility.Hidden;
         txtCategory.IsReadOnly = true;
         mode = FormMode.View;
      }
   }
}
