using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings : UserControl
    {

        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
        public AccountSettings()
        {
            InitializeComponent();
            txtUserDetails.Text = MainWindow.MWinstance.User.Text;
            accountView();

        }

        //Populate the fields in Account Settings
        public void accountView()
        {
            //Nakadisabled so hindi pa maaedit unless pindutin ang change button
            txt_accountname.IsEnabled = false;
            txt_acountUsername.IsEnabled = false;
            txt_accountEmail.IsEnabled = false;
            pbx_acountPassword.IsEnabled = false;
            cbx_accDepartment.IsEnabled = true;

            //Lets get the user from the username textfield that we store on the mainwindow user
            const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            databaseConnection.Open();

            MySqlCommand commandDatabase = new MySqlCommand
                    ("SELECT `tbl_account`.`account_Username`, " +
                    "`tbl_account`.`account_Email`, " +
                    "`tbl_account`.`account_Password`, " +
                    "`tbl_department`.`dept_Code`, " +
                    "`tbl_department`.`dept_Chairperson` " +
                    "FROM `tbl_account` LEFT JOIN `tbl_department` " +
                    "ON `tbl_account`.`account_Department` = " +
                    "`tbl_department`.`dept_ID` " +
                    "Where `tbl_account`.`account_Username` = '"
                    + txtUserDetails.Text + "' OR `account_Email` = '"
                    + txtUserDetails.Text + "' ", databaseConnection);

            MySqlDataReader reader;
            reader = commandDatabase.ExecuteReader();

            //count so bibilangin yung ilan yung tugma
            int count = 0;
            while (reader.Read())
            {
                count++;
            }
            if (count == 1)
            {
                txt_accountname.Text = reader.GetString(4);
                txt_accountEmail.Text = reader.GetString(1);
                txt_acountUsername.Text = reader.GetString(0);
                pbx_acountPassword.Password= reader.GetString(2);
                cbx_accDepartment.Text = reader.GetString(3);
            }
            else if (count > 0)
            {
                MessageBox.Show("Duplicate Account details");
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            databaseConnection.Close();
        }

        //Coce for the button change in "Name"
        private void changeName_click(object sender, RoutedEventArgs e)
        {
            txt_accountname.IsEnabled = true;
        }

        //Coce for the button change in "Username"
        private void changeUsername_click(object sender, RoutedEventArgs e)
        {
            txt_acountUsername.IsEnabled = true;
        }

        //Coce for the button change in "Email"
        private void changeEmail_click(object sender, RoutedEventArgs e)
        {
            txt_accountEmail.IsEnabled = true;
        }

        //Coce for the button change in "Password"
        private void changePassword_click(object sender, RoutedEventArgs e)
        {
            pbx_acountPassword.IsEnabled = true;
        }

        //This is to update the Account details of the user
        private void updateAccount_click(object sender, RoutedEventArgs e)
        {

        }

        //If the user changes his/her mind and decided to cancel 
        private void cancelChanges(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discard the changes?", "CANCEL",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                accountView();
            }
        }

        //Help
        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        //Code to exit the app
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
