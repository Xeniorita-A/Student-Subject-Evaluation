using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Curriculum.xaml
    /// </summary>
    public partial class Curriculum : UserControl
    {

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //your tracing or logging code here (I put a message box as an example)
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        //This is where we bind the column headers of our datagrid
        //This is case sensitive so always check the spelling
        public class CourseCurriculum
        {
            public int CourseNo { get; set; }
            public string? CourseCode { get; set; }
            public string? CourseTitle { get; set; }
            public int CourseUnits { get; set; }
            public string? CoursePrereq { get; set; }
            public string? CourseYearlvl { get; set; }
            public string? CourseSem { get; set; }
            public string? CourseBatch { get; set; }
        }

        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";

        private void Curriculumlist()
        {
            //Course_list is the name of the datagrid where we are showing the course curriculum
            Course_list.Items.Clear();
            txt_searchCurr.Text = "";

            //Query so we can load the data into our datagrid
            String query = "Select * From `tbl_curriculum`";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            databaseConnection.Open();
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt16(8) == 0)
                    {
                        CourseCurriculum _subjects1 = new CourseCurriculum
                        {
                            CourseNo = reader.GetInt16(1),
                            CourseCode = reader.GetString(2),
                            CourseTitle = reader.GetString(3),
                            CourseUnits = reader.GetInt16(4),
                            CoursePrereq = reader.GetString(5),
                            CourseYearlvl = reader.GetString(7),
                            CourseSem = "First Semester",
                            CourseBatch = reader.GetString(9),
                        };
                            Course_list.Items.Add(_subjects1);
                    }

                    else if (reader.GetInt16(8) == 1)
                    {
                        //we will store the data into the variable _subject so we can add it in the datagrid
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseNo = reader.GetInt16(1),
                            CourseCode = reader.GetString(2),
                            CourseTitle = reader.GetString(3),
                            CourseUnits = reader.GetInt16(4),
                            CoursePrereq = reader.GetString(5),
                            CourseYearlvl = reader.GetString(7),
                            CourseSem = "Second Semester",
                            CourseBatch = reader.GetString(9),
                        };
                        //This is the code to add the data stored in _subjects to the datagrid
                            Course_list.Items.Add(_subjects);
                    }
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            //close the connection
            databaseConnection.Close();
        }

        public Curriculum()
        {
            InitializeComponent();
            Curriculumlist();
        }


        public void searchedResult()
        {
            //First we need to clear the datagrid so every time we type it will only show the result of the query
            Course_list.Items.Clear();
            string query = "Select * From `tbl_curriculum` where `curr_Code` LIKE '"
                + txt_searchCurr.Text + "%' OR `curr_Title` LIKE '"
                + txt_searchCurr.Text + "%' ";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            databaseConnection.Open();
            reader = commandDatabase.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CourseCurriculum _subjects = new CourseCurriculum
                    {
                        CourseNo = reader.GetInt16(1),
                        CourseCode = reader.GetString(2),
                        CourseTitle = reader.GetString(3),
                        CourseUnits = reader.GetInt16(4),
                        CoursePrereq = reader.GetString(5),
                    };

                    Course_list.Items.Add(_subjects);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            databaseConnection.Close();
        }

        //This is the method for the search text field it is important so everytime we type we will call the method for search
        private void TextSearchCurr_Changed(object sender, TextChangedEventArgs e)
        {
            //it means that if the search field is empty it will just show the data
            if (txt_searchCurr.Text == "")
            {
                Curriculumlist();
            }
            else
            {
                searchedResult();
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        //Code to exit the Application
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}


