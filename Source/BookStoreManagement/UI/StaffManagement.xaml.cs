using BookStoreManagement.BUS;
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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace BookStoreManagement.UI
{
    /// <summary>
    /// Interaction logic for StaffManagement.xaml
    /// </summary>
    public partial class StaffManagement : UserControl
    {
        /// <summary>
        /// loadType=1 if load from Database, loadType=2 if load from excel file
        /// </summary>
        int loadType = 1;
        int pageNumber = 1;
        int pageSize = 10;
        IPagedList<StaffDto> listStaffs;
        List<StaffDto> allShowedStaffs;
        List<StaffDto> allStaffs;

        /// <summary>
        /// members is loaded from excel file
        /// </summary>
        List<StaffDto> staffs;

        public StaffManagement()
        {
            InitializeComponent();
        }

        private async void loaded(object sender, RoutedEventArgs e)
        {
            cbType.SelectedIndex = 0;

            await reloadTable(pageNumber);
        }

        private async Task<IPagedList<StaffDto>> GetPagedListAsync(int pagedNumber = 1)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (loadType == 1 && allShowedStaffs == null)
                {
                    allStaffs = StaffBUS.GetAllStaffs();
                    allShowedStaffs = new List<StaffDto>();
                    allShowedStaffs.AddRange(allStaffs);
                }
                else if (loadType == 2)
                {
                    allShowedStaffs.Clear();
                    allShowedStaffs.AddRange(staffs);
                }

                return allShowedStaffs.ToPagedList(pagedNumber, pageSize);
            });
        }

        private async Task reloadTable(int pageNumber)
        {
            listStaffs = await GetPagedListAsync(pageNumber);
            btnPrevious.IsEnabled = listStaffs.HasPreviousPage;
            btnNext.IsEnabled = listStaffs.HasNextPage;
            List<Staff> data = new List<Staff>();
            int number = 1;

            foreach (StaffDto e in listStaffs.ToList())
            {
                Staff staff = new Staff();
                staff.Number = number++;
                staff.userId = e.UserId;
                staff.staffId = e.StaffId;
                staff.Gender = e.Gender;
                staff.Name = e.FirstName + " " + e.LastName;
                staff.Username = e.Username;
                staff.Salary = e.Salary;
                try
                {
                    staff.DoB = e.DOB.Substring(6, 2) + "/" + e.DOB.Substring(4, 2) + "/" + e.DOB.Substring(0, 4);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    staff.DoB = "Empty";
                }

                data.Add(staff);
            }

            tableStaffs.ItemsSource = data;

            int pageStart = (pageNumber - 1) * pageSize;
            int start = allShowedStaffs.Count == 0 ? 0 : pageStart + 1;
            int end = allShowedStaffs.Count < pageSize ? allShowedStaffs.Count : pageStart + pageSize;
            lbPaging.Text = String.Format("{0} - {1} out of {2}", start, end, allShowedStaffs.Count);
        }

        private async void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typeString = cbType.SelectedItem.ToString().ToUpper();
            string type = typeString.Substring(38, typeString.Length - 38);
            string value = txtSearchValue.Text.ToUpper();

            switch (type)
            {
                case "NAME":
                    allShowedStaffs = allStaffs.Where(n => (n.FirstName + " " + n.LastName).ToUpper().Contains(value)).ToList();
                    break;
                case "USERNAME":
                    allShowedStaffs = allStaffs.Where(n => n.Username.ToUpper().Contains(value)).ToList();
                    break;
            }

            pageNumber = 1;
            await reloadTable(pageNumber);
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
                staffs = importExcelFile(dlg.FileName);

                loadType = 2;
                btnConfirm.Visibility = Visibility.Visible;
                pageNumber = 1;
                await reloadTable(pageNumber);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Staff selection = (Staff)tableStaffs.SelectedItem;
            if (StaffBUS.delete(selection.userId))
            {
                allShowedStaffs = null;
                await reloadTable(pageNumber);

                MessageBox.Show("Delete Staff Success");
            }
            else
            {
                MessageBox.Show("Delete Staff Fail");
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            StaffDto staff_transfer = null;
            if (loadType == 1)
            {
                Staff staff = (Staff)tableStaffs.SelectedItem;
                staff_transfer = StaffBUS.GetStaffByID(staff.staffId);

                MainWindow.MainGrid.Children.Clear();
                UserControl staffInfo = new StaffInfo(staff_transfer);
                MainWindow.MainGrid.Children.Add(staffInfo);
            }
            //else if (loadType == 2)
            //{
            //    int selected = tableStaffs.SelectedIndex;
            //    staff_transfer = staffs[selected];
            //}
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void confirm_click(object sender, RoutedEventArgs e)
        {
            foreach (StaffDto staff in staffs)
            {
                StaffBUS.Insert(staff);
            }

            MessageBox.Show("Save list staffs success!");
            allShowedStaffs = null;
            loadType = 1;
            btnConfirm.Visibility = Visibility.Hidden;
            pageNumber = 1;
            await reloadTable(pageNumber);
        }

        private List<StaffDto> importExcelFile(string path)
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
            List<StaffDto> staffs = new List<StaffDto>();
            DateTime current = new DateTime();

            for (int i = 2; i <= row; i++)
            {
                //string number= (string)(range.Cells[i, 1] as Excel.Range).Value2;
                string firstname = (string)(range.Cells[i, 2] as Excel.Range).Value2;
                string lastname = (string)(range.Cells[i, 3] as Excel.Range).Value2;
                string dob = (string)(range.Cells[i, 4] as Excel.Range).Value2;
                string address = (string)(range.Cells[i, 5] as Excel.Range).Value2;
                string phone = (string)(range.Cells[i, 6] as Excel.Range).Value2;
                string gender = (string)(range.Cells[i, 7] as Excel.Range).Value2;
                string email = (string)(range.Cells[i, 8] as Excel.Range).Value2;
                string note = (string)(range.Cells[i, 9] as Excel.Range).Value2;
                string username = (string)(range.Cells[i, 10] as Excel.Range).Value2;
                string password = (string)(range.Cells[i, 11] as Excel.Range).Value2;
                string salary = (string)(range.Cells[i, 12] as Excel.Range).Value2;
                string startDay = (string)(range.Cells[i, 13] as Excel.Range).Value2;
                string endDay = (string)(range.Cells[i, 14] as Excel.Range).Value2;

                StaffDto staff = new StaffDto();
                staff.FirstName = firstname;
                staff.LastName = lastname;
                staff.DOB = dob;
                staff.Address = address;
                staff.Phone = phone;
                staff.Gender = gender;
                staff.Email = email;
                staff.Note = note;
                staff.PhotoLink = "Images/bg_default.jpg";
                staff.UserType = "STAFF";
                staff.CreateDate = current;
                staff.CreateBy = 1;
                staff.UpdatedDate = current;
                staff.UpdatedBy = 1;
                staff.Username = username;
                staff.Password = password;
                staff.Salary = long.Parse(salary);
                staff.StartDate = startDay;
                staff.EndDate = endDay;
                staff.Active = true;

                staffs.Add(staff);
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return staffs;
        }
    }

    class Staff
    {
        public int userId { get; set; }
        public int staffId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DoB { get; set; }
        public string Username { get; set; }
        public long Salary { get; set; }
    }
}
