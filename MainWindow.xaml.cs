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
        public static MainWindow MWinstance;
        public TextBox User;

        public MainWindow()
        {
            InitializeComponent();
            MWinstance = this;
            User = UserPass;
            date.Text = DateTime.Now.ToString("dddddd , MM/dd/yyyy, hh:mm");
            
        }

        private void btn_logout_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Login load = new Login();
                load.Show();
                this.Close();
            }
        }
    }
}
