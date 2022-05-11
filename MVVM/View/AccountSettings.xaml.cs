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
            accountView();

            //Hashing the data
            string plainData = "Mahesh";
            Console.WriteLine("Raw data: {0}", plainData);
            string hashedData = ComputeSha256Hash(plainData);
            Console.WriteLine("Hash {0}", hashedData);
            Console.WriteLine(ComputeSha256Hash("Mahesh"));
            Console.ReadLine();
        }

        //Hashing 
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
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
                int ID = int.Parse(txtUserID.Text);
                ID = reader.GetInt16(0);
                txtUserID.Text = ID.ToString();
                txt_acountUsername.Text = reader.GetString(1);
                txt_accountEmail.Text = reader.GetString(2);
                PasswordTemp.Text = reader.GetString(3); 
                txt_accountname.Text = reader.GetString(4);
                cbx_accDepartment.Text = reader.GetString(5);
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
            try
            {
                const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
                if (txt_accountEmail.Text != "" && txt_accountname.Text != "" && txt_acountUsername.Text != "")
                {
                    string query = "UPDATE tbl_account SET account_ID = @id, " +
                        "account_Username= @username, account_Email = @email, account_Password = @password," +
                        "account_Name = @name,account_Department = @department WHERE account_ID = @id";
                    String hashedData = ComputeSha256Hash(pbx_acountPassword.Password);
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    databaseConnection.Open();
                    commandDatabase.Parameters.AddWithValue("@id", Convert.ToInt16(txtUserID.Text));
                    commandDatabase.Parameters.AddWithValue("@username", txt_acountUsername.Text);
                    commandDatabase.Parameters.AddWithValue("@email", txt_accountEmail.Text);
                    commandDatabase.Parameters.AddWithValue("@name", txt_accountname.Text);
                    commandDatabase.Parameters.AddWithValue("@department", cbx_accDepartment.Text);
                    //To set new Password
                    if (pbx_acountPassword.Password == "")
                    {
                        commandDatabase.Parameters.AddWithValue("@password", PasswordTemp.Text);
                    }
                    else if (pbx_acountPassword.Password != "")
                    {
                        commandDatabase.Parameters.AddWithValue("@password", hashedData);
                    }
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Close();
                    accountView();
                    MessageBox.Show("Updated successfully!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("One of the fields is empty!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
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
