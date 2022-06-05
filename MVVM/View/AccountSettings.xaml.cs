using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
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
            getUserID.Text = MainWindow.MWinstance.AccountID.Text;
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
                    ("SELECT `account_ID`,`account_Username`, " +
                    "`account_Email`,`account_Password`, `account_Name`," +
                    "`account_Department` " +
                    "FROM `tbl_account`" +
                    "Where `account_Username` = '"
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
                //Convert string to int to store the User ID
                _ = int.Parse(getUserID.Text);
                int ID = reader.GetInt16(0);
                getUserID.Text = ID.ToString();
                txt_acountUsername.Text = reader.GetString(1);
                txt_accountEmail.Text = reader.GetString(2);
                PasswordTemp.Text = reader.GetString(3);
                txt_accountname.Text = reader.GetString(4);
                cbx_accDepartment.Text = reader.GetString(5);
            }
            else if (count > 0)
            {
                _ = MessageBox.Show("Duplicate Account details");
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
            try
            {
                const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
                if (txt_accountEmail.Text != "" && txt_accountname.Text != "" && txt_acountUsername.Text != "")
                {
                    string query = "UPDATE tbl_account SET account_ID = @id, " +
                        "account_Username= @username, account_Email = @email, account_Password = @password," +
                        "account_Name = @name,account_Department = @department WHERE account_ID = @id";
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    databaseConnection.Open();
                    _ = commandDatabase.Parameters.AddWithValue("@id", Convert.ToInt16(getUserID.Text));
                    _ = commandDatabase.Parameters.AddWithValue("@username", txt_acountUsername.Text);
                    _ = commandDatabase.Parameters.AddWithValue("@email", txt_accountEmail.Text);
                    _ = commandDatabase.Parameters.AddWithValue("@name", txt_accountname.Text);
                    _ = commandDatabase.Parameters.AddWithValue("@department", cbx_accDepartment.Text);
                    //To set new Password
                    if (pbx_acountPassword.Password == "")
                    {
                        _ = commandDatabase.Parameters.AddWithValue("@password", PasswordTemp.Text);
                    }
                    else if (pbx_acountPassword.Password != "")
                    {
                        if (pbx_acountPassword.Password.Length >= 8 && pbx_acountPassword.Password.Length <= 12)
                        {
                            _ = commandDatabase.Parameters.AddWithValue("@password", EncryptPassword.HashString(pbx_acountPassword.Password));
                        }
                        else
                        {
                            MessageBox.Show("Please make sure that the password you inputted was atleast 8 characters.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Close();
                    accountView();
                    _ = MessageBox.Show("Updated successfully!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    addActivityUpdateAccount();
                }
                else
                {
                     MessageBox.Show("One of the fields is empty!");
                }
            }
            catch (Exception)
            {
               
            }


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
            HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }

        public void addActivityUpdateAccount()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @detail)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(getUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Update Account");
            _ = commandDatabase2.Parameters.AddWithValue("@detail", txt_accountname.Text + " updated their account information.");
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

        public void addActivityLogout()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(getUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Logout");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txt_accountname.Text + " logout of the system.");
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
