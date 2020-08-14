using BookStoreManagement.Utils;
using CommonLibrary.Utils;
using DatabaseCommon.DAO;
using DatabaseCommon.DTO;
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
using System.Xml.XPath;
using BookStoreManagement.BUS;
using DatabaseCommon.Const;

namespace BookStoreManagement.UI
{
   /// <summary>
   /// Interaction logic for UserAuthorizationControl.xaml
   /// </summary>
   /// 
   [Feature(Id = 1, Name = FeatureNameUtils.Authorization.MANAGEMENT, Group = FeatureNameUtils.FeatureGroup.AUTHORIZATION)]
   public partial class UserAuthorizationControl : UserControl
   {
      private List<DefinitionDto> ListDto = new List<DefinitionDto>();
      private List<StackPanel> ListPanel = new List<StackPanel>();
      private bool isEditing = false;
      public UserAuthorizationControl()
      {
         InitializeComponent();
         ListDto = DefinitionDao.GetAuthorizationList();
         InitListControlsCheckbox();
         InitComoboxType();
      }
      private List<CheckBox> ListCheckbox = new List<CheckBox>();
      public void InitListControlsCheckbox()
      {
         ListPanel = new List<StackPanel>() { ListControls1, ListControls2, ListControls3 };
         ListPanel.ForEach(n => n.Children.Clear());
         var groups = FeatureAttributeService.GetListFeatureAttribute().GroupBy(n => n.Group).OrderBy(n => n.Key);
         int count = 0, controlIndex = 0;
         foreach (var group in groups)
         {
            if (count > 22 && controlIndex < 2)
            {
               count = 0;
               controlIndex += 1;
            }
            TextBlock textBlock = new TextBlock();
            textBlock.Text = group.Key.ToString();
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            textBlock.FontSize = 20.0;
            textBlock.Margin = new Thickness(0, 10, 0, 10);
            ListPanel[controlIndex].Children.Add(textBlock);
            count += 3;
            var listFeatures = group.ToList();
            int countAttr = 0;
            foreach (var attr in listFeatures)
            {
               CheckBox checkBox = new CheckBox();
               checkBox.Name = "cb_" + attr.Id.ToString();
               checkBox.Content = attr.Name;
               checkBox.Foreground = new SolidColorBrush(Colors.Black);
               checkBox.Visibility = Visibility.Visible;
               ListPanel[controlIndex].Children.Add(checkBox);
               ListCheckbox.Add(checkBox);
               count++;
               countAttr++;
               if ((count > 25 || (count > 22 && countAttr == listFeatures.Count)) && controlIndex < 2)
               {
                  count = 0;
                  controlIndex += 1;
               }
            }
         }
      }
      private void InitComoboxType()
      {
         ComboBoxType.Items.Clear();
         var map = FeatureAttributeService.GetAuthorizationMap();
         foreach (var pair in map)
         {
            ComboBoxType.Items.Add(pair.Key);
            ComboBoxType.SelectedIndex = 0;
         }
      }

      private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         UpdateData(ComboBoxType.SelectedIndex);
      }
      private void UpdateData(int index)
      {
         if (index < 0) 
         { 
            index = 0;
            ComboBoxType.SelectedIndex = index;
            return;
         }
         txtName.Text = ComboBoxType.Items[index].ToString();
         var map = FeatureAttributeService.GetAuthorizationMap();
         var featureMap = FeatureAttributeService.GetFeaturePropertiesMap();
         string v = ComboBoxType.Items[index].ToString();
         List<int> ListSelected = map.GetValue(v);
         ListCheckbox.ForEach(n => ((CheckBox)n).IsChecked = false);
         foreach (var selectedCheckBox in ListSelected)
         {
            if (featureMap.ContainsKey(selectedCheckBox))
            {
               string featureName = featureMap.GetValue(selectedCheckBox);
               CheckBox checkBox = ListCheckbox.FirstOrDefault(n => n.Content.Equals(featureName));
               if (checkBox != null)
               {
                  checkBox.IsChecked = true;
               }
            }
         }
      }
      private void HideEditButtons()
      {
         btnCancelEdit.Visibility = Visibility.Collapsed;
         btnAddNew.Visibility = Visibility.Visible;
         btnEdit.Visibility = Visibility.Visible;
         btnSave.Visibility = Visibility.Collapsed;
         btnDelete.Visibility = Visibility.Visible;
         ContentPanel.IsEnabled = false;
      }
      private void ShowEditButtons()
      {
         btnCancelEdit.Visibility = Visibility.Visible;
         btnAddNew.Visibility = Visibility.Collapsed;
         btnEdit.Visibility = Visibility.Collapsed;
         btnSave.Visibility = Visibility.Visible;
         btnDelete.Visibility = Visibility.Collapsed;
         ContentPanel.IsEnabled = true;
      }
      private void btnEdit_Click(object sender, RoutedEventArgs e)
      {
         if (ComboBoxType.SelectedItem.ToString().Equals(CONST.USERS.USER_TYPE_ADMIN))
         {
            MessageBox.Show("Can not edit ADMIN!");
            return;
         }
         ContentPanel.IsEnabled = true;
         HeaderPanel.IsEnabled = false;
         txtName.IsEnabled = false;
         isEditing = true;
         ShowEditButtons();
      }

      private void btnAddNew_Click(object sender, RoutedEventArgs e)
      {
         ListCheckbox.ForEach(n => ((CheckBox)n).IsChecked = false);
         txtName.Text = "";
         ContentPanel.IsEnabled = true;
         HeaderPanel.IsEnabled = true;
         txtName.IsEnabled = true;
         ShowEditButtons();
      }

      private void btnDiscard_Click(object sender, RoutedEventArgs e)
      {
         UpdateData(ComboBoxType.SelectedIndex);
         ContentPanel.IsEnabled = false;
         HeaderPanel.IsEnabled = false;
         HideEditButtons();
         isEditing = false;
      }

      private void btnDelete_Click(object sender, RoutedEventArgs e)
      {
         DefinitionDto dto = ListDto.FirstOrDefault(n => n.Value1.Equals(ComboBoxType.SelectedItem.ToString()));
         if (dto != null) //Delete
         {
            if (dto.Value1.Equals("ADMIN"))
            {
               MessageBox.Show("Can not delete ADMIN!");
               return;
            }
            MessageBoxResult resultConfirm = MessageBox.Show("Would you like to delete "+ dto.Value1 +"?", "Delete", MessageBoxButton.YesNo);
            if (resultConfirm == MessageBoxResult.No)
            {
               return;
            }
            //Delete
            bool result = DefinitionBUS.DeleteAuthorization(dto);
            if (result)
            {
               ListDto.Remove(dto);
               ComboBoxType.Items.Remove(ComboBoxType.SelectedItem);
               UpdateData(ComboBoxType.SelectedIndex);
            }
            else //Not deleted
            {
               MessageBox.Show("Delete " + dto.Value1 + " failed!");
            }
         }
      }

      private void btnSave_Click(object sender, RoutedEventArgs e)
      {
         if (String.IsNullOrEmpty(txtName.Text.Trim()))
         {
            MessageBox.Show("Type name can't be empty!");
            return;
         }
         List<String> AuthorizationList = ListCheckbox.Where(n => n.IsChecked == true).Select(n => n.Content.ToString()).ToList();
         if (isEditing)
         {
            DefinitionDto dto = ListDto.FirstOrDefault(n => n.Value1.Equals(ComboBoxType.SelectedItem.ToString()));
            if (dto != null) //Update
            {
               DefinitionBUS.UpdateAuthorization(dto, txtName.Text.Trim(), AuthorizationList);
               ComboBoxType.Items[ComboBoxType.SelectedIndex] = dto.Value1;
               UpdateData(ComboBoxType.SelectedIndex);
            }
         }
         else //Add new
         {
            DefinitionDto duplicateNameDto = ListDto.FirstOrDefault(n => n.Value1.Equals(txtName.Text.Trim()));
            if (duplicateNameDto != null)
            {
               MessageBox.Show(txtName.Text.Trim() + " type already exists!");
               return;
            }
            DefinitionDto dto = DefinitionBUS.InsertAuthorization(txtName.Text.Trim(), AuthorizationList);
            ListDto.Add(dto);
            ComboBoxType.Items.Add(dto.Value1);
            ComboBoxType.SelectedIndex = ComboBoxType.Items.Count - 1;
         }
         isEditing = false;
         ContentPanel.IsEnabled = false;
         HeaderPanel.IsEnabled = false;
         txtName.IsEnabled = true;
         HideEditButtons();
      }
   }
}
