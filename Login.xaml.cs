using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Student_Subject_Evaluation
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";

        public Login()
        {
            InitializeComponent();

            Passwordtext.Visibility = System.Windows.Visibility.Hidden;

            //This is to encrypt the password
            string plainData = "Mahesh";
            Console.WriteLine("Raw data: {0}", plainData);
            string hashedData = ComputeSha256Hash(plainData);
            Console.WriteLine("Hash {0}", hashedData);
            Console.WriteLine(ComputeSha256Hash("Mahesh"));
            Console.ReadLine();
        }

        //Hash the password
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

        //This is the method for show password (mouseenter = when you hover to the icon it will show the password)
        private void mouseentercover(object sender, MouseEventArgs e)
        {
            String password = pbx_password.Password;
            Passwordtext.Text = password;
            pbx_password.Visibility = Visibility.Hidden;
            Passwordtext.Visibility = Visibility.Visible;

        }

        //Mouse leaving so we can hide the password once we leave the icon
        private void mouseleaving(object sender, MouseEventArgs e)
        {
            pbx_password.Visibility = System.Windows.Visibility.Visible;
            Passwordtext.Visibility = Visibility.Hidden;
        }

        public void login()
        {
            //Tatawagin everytime na maglogin
            String hashedData = ComputeSha256Hash(pbx_password.Password);
            const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            databaseConnection.Open();
            if (txt_username.Text == "" && pbx_password.Password == "")
            {
                MessageBox.Show("Please input your username/email and password to login.");
            }
            else
            {
                //Query so I can find the username/email and password that matches the input
                MySqlCommand commandDatabase = new MySqlCommand
                    ("SELECT * from `tbl_account` WHERE `account_Username` = '"
                    + txt_username.Text + "' OR `account_Email` = '"
                    + txt_username.Text + "' AND `account_Password`= '"
                    + (hashedData) + "' ", databaseConnection);

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

                    MessageBox.Show("SUCCESSFULLY LOGIN!");
                    MainWindow load = new MainWindow();

                    //THis is where I store yung Username ng user
                    MainWindow.MWinstance.User.Text = txt_username.Text;
                    //This is to show the main window (Dashboard)
                    load.Show();
                    this.Close();
                }
                else if (count > 0)
                {
                    MessageBox.Show("Duplicate Username and Password");
                }
                else
                {
                    MessageBox.Show("Username and password did not match.");
                }
                //this is here to clear the fields
                txt_username.Text = "";
                pbx_password.Password = "";
            }
        }

        //Every time we click the loginin button we'll call the login() method
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        //Code for exiting the App
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pbx_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (txt_username.Text == "" && pbx_password.Password == "")
                {
                    MessageBox.Show("Please input your username/email and password.");
                }
                else 
                {
                    login();
                }
            }
        }
    }
}
