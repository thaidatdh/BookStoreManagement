using BookStoreManagement.BUS;
using DatabaseCommon.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for MemberInfo.xaml
    /// </summary>
    public partial class MemberInfo : UserControl
    {
        CustomerDto member = null;
        public MemberInfo(CustomerDto member)
        {
            this.member = member;
            InitializeComponent();
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            textName.Text = member.FirstName + " " + member.LastName;
            textGender.Text = member.Gender;
            
            try
            {
                textDoB.Text = member.DOB.Substring(6, 2) + "/" + member.DOB.Substring(4, 2) + "/" + member.DOB.Substring(0, 4);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                textDoB.Text = "Empty";
            }

            textEmail.Text = member.Email;
            textPhone.Text = member.Phone;
            textAddress.Text = member.Address;
            textCreditCard.Text = member.CreditCard;
            textMoMo.Text = member.Momo;
            textBankNumber.Text = member.BankNumber;
            textBankName.Text = member.BankName;
            textNote.Text = member.Note;

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string avatar_path = path + member.PhotoLink;
            Debug.WriteLine(avatar_path);
            if (!File.Exists(avatar_path))
            {
                avatar_path = path + "Images/bg_default.jpg";
            }

            BitmapImage image = new BitmapImage(new Uri(avatar_path, UriKind.Absolute));
            avatar.Source = image;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainGrid.Children.Clear();
            UserControl createMember = new CreateMember(member);
            MainWindow.MainGrid.Children.Add(createMember);
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainGrid.Children.Clear();
            UserControl memberManagement = new MemberManagement();
            MainWindow.MainGrid.Children.Add(memberManagement);
        }
    }
}
