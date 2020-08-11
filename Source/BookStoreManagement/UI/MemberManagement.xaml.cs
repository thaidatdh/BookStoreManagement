using BookStoreManagement.BUS;
using CommonLibrary.Utils;
using DatabaseCommon.DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using BookStoreManagement.Utils;

namespace BookStoreManagement.UI
{
    /// <summary>
    /// Interaction logic for MemberManagement.xaml
    /// </summary>
    [Feature(Id = 18, Name = FeatureNameUtils.Member.MANAGEMENT, Group = FeatureNameUtils.FeatureGroup.MEMBER_MANAGEMENT)]
    [Feature(Id = 19, Name = FeatureNameUtils.Member.IMPORT, Group = FeatureNameUtils.FeatureGroup.MEMBER_MANAGEMENT)]
    [Feature(Id = 20, Name = FeatureNameUtils.Member.NEW, Group = FeatureNameUtils.FeatureGroup.MEMBER_MANAGEMENT)]
    [Feature(Id = 21, Name = FeatureNameUtils.Member.EDIT, Group = FeatureNameUtils.FeatureGroup.MEMBER_MANAGEMENT)]
    [Feature(Id = 22, Name = FeatureNameUtils.Member.DELETE, Group = FeatureNameUtils.FeatureGroup.MEMBER_MANAGEMENT)]
    public partial class MemberManagement : UserControl
    {
        /// <summary>
        /// loadType=1 if load from Database, loadType=2 if load from excel file
        /// </summary>
        int loadType = 1;
        int pageNumber = 1;
        int pageSize = 10;
        IPagedList<CustomerDto> listMembers;
        List<CustomerDto> allShowedMembers;
        List<CustomerDto> allMembers;

        /// <summary>
        /// members is loaded from excel file
        /// </summary>
        List<CustomerDto> members;

        public MemberManagement()
        {
            InitializeComponent();
        }

        private async void loaded(object sender, RoutedEventArgs e)
        {
            cbType.SelectedIndex = 0;

            await reloadTable(pageNumber);
        }

        private async Task<IPagedList<CustomerDto>> GetPagedListAsync(int pagedNumber = 1)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (loadType==1 && allShowedMembers == null)
                {
                    allMembers = CustomerBUS.GetAllNotDeletedMembers();
                    allShowedMembers = new List<CustomerDto>();
                    allShowedMembers.AddRange(allMembers);
                }else if (loadType==2)
                {
                    allShowedMembers.Clear();
                    allShowedMembers.AddRange(members);
                }

                return allShowedMembers.ToPagedList(pagedNumber, pageSize);
            });
        }

        private async Task reloadTable(int pageNumber)
        {
            listMembers = await GetPagedListAsync(pageNumber);
            btnPrevious.IsEnabled = listMembers.HasPreviousPage;
            btnNext.IsEnabled = listMembers.HasNextPage;
            List<Member> data = new List<Member>();
            int number = 1;

            foreach(CustomerDto e in listMembers.ToList())
            {
                Member member = new Member();
                member.Number = number++;
                member.BarCode = e.UserId;               
                member.Email = e.Email;
                member.Gender = e.Gender;
                member.Name = e.FirstName + " " + e.LastName;
                member.Phone = e.Phone;
                try
                {
                    member.DoB = e.DOB.Substring(6, 2) + "/" + e.DOB.Substring(4, 2) + "/" + e.DOB.Substring(0, 4);
                }
                catch(Exception ex)
                {
                    member.DoB = "";
                }

                data.Add(member);
            }

            tableMembers.ItemsSource = data;

            int pageStart = (pageNumber - 1) * pageSize;
            int start = allShowedMembers.Count == 0 ? 0 : pageStart + 1;
            int end = allShowedMembers.Count < pageSize ? allShowedMembers.Count : pageStart + pageSize;
            lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedMembers.Count);
        }

        private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typeString = cbType.SelectedItem.ToString().ToUpper();
            string type = typeString.Substring(38, typeString.Length - 38);
            string value = txtSearchValue.Text.ToUpper();

            switch (type)
            {
                case "NAME":
                    allShowedMembers = allMembers.Where(n => (n.FirstName+" "+n.LastName).ToUpper().Contains(value)).ToList();
                    break;
                case "EMAIL":
                    allShowedMembers = allMembers.Where(n => n.Email.ToUpper().Contains(value)).ToList();
                    break;
                case "PHONE NUMBER":
                    allShowedMembers = allMembers.Where(n => n.Phone.ToUpper().Contains(value)).ToList();
                    break;
            }

            pageNumber = 1;
            await reloadTable(pageNumber);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainGrid.Children.Clear();
            UserControl createMember = new CreateMember();
            MainWindow.MainGrid.Children.Add(createMember);
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Filter = "Excel files (*.xlsx, *.xls, *.csv) | *.xlsx; *.xls; *.csv";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                members = importExcelFile(dlg.FileName);

                loadType = 2;
                btnConfirm.Visibility = Visibility.Visible;
                pageNumber = 1;
                await reloadTable(pageNumber);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Member member = (Member)tableMembers.SelectedItem;
            CustomerBUS.Delete(member.BarCode);

            allShowedMembers = null;
            await reloadTable(pageNumber);

            MessageBox.Show("Delete member success!");
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerDto member_transfer = null;
            if (loadType == 1)
            {
                Member member = (Member)tableMembers.SelectedItem;
                member_transfer = CustomerBUS.GetMemberByID(member.BarCode);

                MainWindow.MainGrid.Children.Clear();
                UserControl memberInfo = new MemberInfo(member_transfer);
                MainWindow.MainGrid.Children.Add(memberInfo);
            }
            //else if (loadType==2)
            //{
            //    int selected = tableMembers.SelectedIndex;
            //    member_transfer = members[selected];
            //}
        }

        private async void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (listMembers == null || !listMembers.HasPreviousPage)
                return;
            await reloadTable(--pageNumber);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (listMembers == null || !listMembers.HasNextPage)
                return;
            await reloadTable(++pageNumber);
        }

        private List<CustomerDto> importExcelFile(string path)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            int row = range.Rows.Count;
            List<CustomerDto> members = new List<CustomerDto>();
            DateTime current = new DateTime();

            for (int i = 2; i <= row; i++)
            {
                //string number= (string)(range.Cells[i, 1] as Excel.Range).Value2;
                string firstname= (string)(range.Cells[i, 2] as Excel.Range).Value2;
                string lastname= (string)(range.Cells[i, 3] as Excel.Range).Value2;
                string dob= (string)(range.Cells[i, 4] as Excel.Range).Value2;
                string address= (string)(range.Cells[i, 5] as Excel.Range).Value2;
                string phone= (string)(range.Cells[i, 6] as Excel.Range).Value2;
                string gender= (string)(range.Cells[i, 7] as Excel.Range).Value2;
                string email= (string)(range.Cells[i, 8] as Excel.Range).Value2;
                string note= (string)(range.Cells[i, 9] as Excel.Range).Value2;
                string credit_card= (string)(range.Cells[i, 10] as Excel.Range).Value2;
                string momo= (string)(range.Cells[i, 11] as Excel.Range).Value2;
                string bank_number= (string)(range.Cells[i, 12] as Excel.Range).Value2;
                string bank_name= (string)(range.Cells[i, 13] as Excel.Range).Value2;
                string point= (string)(range.Cells[i, 14] as Excel.Range).Value2;

                CustomerDto member = new CustomerDto();
                member.FirstName = firstname;
                member.LastName = lastname;
                member.DOB = dob;
                member.Address = address;
                member.Phone = phone;
                member.Gender = gender;
                member.Email = email;
                member.Note = note;
                member.PhotoLink = "Images/bg_default.jpg";
                member.UserType = "CUSTOMER";
                member.CreateDate = current;
                member.CreateBy = 1;
                member.UpdatedDate = current;
                member.UpdatedBy = 1;
                member.CreditCard = credit_card;
                member.Momo = momo;
                member.BankNumber = bank_number;
                member.BankName = bank_name;
                member.Point = int.Parse(point);
                member.IsDeleted = false;

                members.Add(member);
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return members;
        }

        private async void confirm_click(object sender, RoutedEventArgs e)
        {
            foreach(CustomerDto member in members)
            {
                CustomerBUS.Insert(member);
            }

            MessageBox.Show("Save list members success!");
            allShowedMembers = null;
            loadType = 1;
            btnConfirm.Visibility = Visibility.Hidden;
            pageNumber = 1;
            await reloadTable(pageNumber);
        }
    }

    class Member
    {
        public int BarCode { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DoB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
