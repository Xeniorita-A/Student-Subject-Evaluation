using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public Student()
        {
            InitializeComponent();
        }


        private void ImportGrade_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateReport_click(object sender, RoutedEventArgs e)
        {

        }

        //Ths is the method for the search field in the report tab
        private void TxtSearchStudReport_Changed(object sender, TextChangedEventArgs e)
        {

        }

        //Ths is the method for the search field in the Student List tab
        private void TextSearchStuds_Changed(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        //Code for exiting the app
        private void exitApp(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
