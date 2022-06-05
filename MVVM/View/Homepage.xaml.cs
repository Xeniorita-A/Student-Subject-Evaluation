using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : UserControl
    {

        public Homepage()
        {
            InitializeComponent();
            //txtUserID.Text = MainWindow.MWinstance.AccountID.Text;
            //txtUserName.Text = MainWindow.MWinstance.AccountName.Text;
        }
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
       
        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
            HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }
    }
}
