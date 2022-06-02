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
        private void exitApp(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Windows[0].Hide();
                //addActivityLogout();
                Login load = new Login();
                load.Show();

            }
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        //public void addActivityLogout()
        //{
        //    string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
        //    MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
        //    MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
        //    commandDatabase2.Parameters.AddWithValue("@ID", 0);
        //    commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
        //    commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
        //    commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
        //    commandDatabase2.Parameters.AddWithValue("@activity", "Logout");
        //    commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " logout of the system.");
        //    commandDatabase2.CommandTimeout = 60;
        //    MySqlDataReader reader2;
        //    try
        //    {
        //        databaseConnection2.Open();
        //        reader2 = commandDatabase2.ExecuteReader();
        //        databaseConnection2.Close();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
    }
}
