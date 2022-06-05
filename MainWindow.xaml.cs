using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? MWinstance;
        public TextBox User;
        public TextBox Username;
        public TextBox AccountID;
        public TextBox AccountName;

        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
        public MainWindow()
        {
            InitializeComponent();
            MWinstance = this;
            User = UserPass;
            AccountID = _ID;
            AccountName = _Name;
            date.Text = DateTime.Now.ToString("dddddd , yyyy/MM/dd");

        }

        private void btn_logout_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Login load = new Login();
                load.Show();
                Close();
            }
        }

        public void addActivityLogout()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(AccountID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Logout");
            _ = commandDatabase2.Parameters.AddWithValue("@details", AccountName.Text + " (" + User.Text + ") Logout of the system.");
            commandDatabase2.CommandTimeout = 60;
            MySqlDataReader reader2;
            try
            {
                databaseConnection2.Open();
                reader2 = commandDatabase2.ExecuteReader();
                databaseConnection2.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
