using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Curriculum.xaml
    /// </summary>
    public partial class Curriculum : UserControl
    {
        //string remover 

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //your tracing or logging code here (I put a message box as an example)
            _ = MessageBox.Show(e.ExceptionObject.ToString());
        }

        //This is where we bind the column headers of our datagrid
        //This is case sensitive so always check the spelling
        public class CourseCurriculum
        {
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
                    if (reader.GetString(4).ToString() == " ")
                    {
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = " ",
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7)

                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                    else
                    {
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7)

                        };

                        _ = Course_list.Items.Add(_subjects);
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
            btn_saveCurriculum.IsEnabled = false;
            txtUserID.Text = MainWindow.MWinstance.AccountID.Text;
            txtUserName.Text = MainWindow.MWinstance.AccountName.Text;
        }

        public void searchedResult()
        {
            //First we need to clear the datagrid so every time we type it will only show the result of the query
            Course_list.Items.Clear();
            string query = "Select * From `tbl_curriculum` where `curr_Code` LIKE '"
                + txt_searchCurr.Text + "%' OR `curr_Title` LIKE '"
                + txt_searchCurr.Text + "%'  OR `curr_Batch` LIKE '"
                + txt_searchCurr.Text + "%'";

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
                    if (reader.GetString(4).ToString() == " ")
                    {
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = " ",
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7)

                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                    else
                    {
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7)

                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
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

        //For the list where we load the csv file
        public class ImportSubject
        {
            public string? SubjectCode { get; set; }
            public string? SubjectTitle { get; set; }
            public int SubjectUnits { get; set; }
            public string? SubjectPrereq { get; set; }
            public string? SubjectSem { get; set; }
            public int SubjectYear { get; set; }
        }

        //added for curriculum import: This will display the CSV data into the Datagrid
        //I still have problem regarding this method. When the CSV data has comma "," it will be transfered to another column
        void bindDataCSV(string filePath)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] lines = System.IO.File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    //the first line on the csv file will be our column header on the datagrid
                    string firstline = lines[0];
                    string[] headerLabels = firstline.Split(',');

                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    //for the data
                    for (int r = 1; r < lines.Length; r++)
                    {
                        string[] dataWords = lines[r].Split(',');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }

                    DataTable clone = dt.Clone();
                    string t;
                    IEnumerable<object[]>? qry = from DataRow row in dt.Rows
                                                 let arr = row.ItemArray
                                                 select Array.ConvertAll(arr, s =>
                                                     (t = s as string) != null
                                                     && t.StartsWith("\"")
                                                     && t.EndsWith("\"") ? t.Trim('\"') : s);
                    foreach (object[] arr in qry)
                    {
                        _ = clone.Rows.Add(arr);
                    }
                    if (clone.Rows.Count > 0)
                    {
                        //We will make the datatable the source for the datagrid
                        Import_list.ItemsSource = clone.DefaultView;
                    }
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show("File not found or file was corrupted. Please make sure the file is not open in another program.");
            }
        }

        //Button choose file
        private void btnChoose(object sender, RoutedEventArgs e)
        {
            //Let's clear these fields first.
            txt_Filepath.Text = "";
            try
            {
                btn_saveCurriculum.IsEnabled = true;
                //this will open the windows dialog so we can shoose the file
                Microsoft.Win32.OpenFileDialog lObjFileDlge = new Microsoft.Win32.OpenFileDialog();
                lObjFileDlge.Filter = "CSV Files|*.csv";
                lObjFileDlge.FilterIndex = 1;
                lObjFileDlge.Multiselect = false;
                string fName = "";
                bool? lBlnUserclicked = lObjFileDlge.ShowDialog();
                if (lBlnUserclicked != null || lBlnUserclicked == true)
                {
                    fName = lObjFileDlge.FileName;
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    StreamReader lObjStreamReader = new StreamReader(fName);
                    string[]? lines = File.ReadAllLines(fName);
                    txt_Filepath.Text = fName.ToString();
                    bindDataCSV(txt_Filepath.Text);
                    lObjStreamReader.Close();
                }
                else
                {
                    _ = MessageBox.Show("File not found!");
                }
            }
            catch (Exception)
            {

            }
        }

        //This will let us paste the file path of the file. once enter key was press the system will read the csv
        private void txt_Filepath_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (txt_Filepath.Text == "")
                {
                    _ = MessageBox.Show("Please input the file path or click choose");
                }
                else if (txt_Filepath.Text != "")
                {
                    bindDataCSV(txt_Filepath.Text);
                }
            }
        }

        public void checkCurriculum()
        {
            _ = new DataTable();
            DataTable check = ((DataView)Import_list.ItemsSource).ToTable();
            int count = 0;
            for (int i = 0; i < check.Rows.Count; i++)
            {
                //this is for the columns
                for (int j = 0; j < Import_list.Columns.Count; j++)
                {
                }
                object? code = check.Rows[0][0];
                int batch = int.Parse((string)check.Rows[0][7]);
                //Let's try the query to check if it exist
                string q2 = "SELECT * FROM `tbl_curriculum` WHERE `curr_Code` = '" + code + "' AND `curr_Batch`= " + batch + "";
                MySqlConnection dbc1 = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc1);
                commandDatabase1.CommandTimeout = 60;
                dbc1.Open();
                MySqlDataReader dr1 = commandDatabase1.ExecuteReader();
                while (dr1.Read())
                {
                    count++;
                }
                dbc1.Close();
            }
            if (count >= 1)
            {
                if (MessageBox.Show("This curriculum already existed in the database. Do you want to update this curriculum?" +
                    "?", "Info", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    updateCurriculum();
                }
            }
            else if (count == 0)
            {
                insertCurriculum();
            }
            //dbc.Close();
        }

        public void insertCurriculum()
        {
            try
            {
                if (txt_Filepath.Text != "" && Import_list.Items.IsEmpty == false)
                {
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    dbc.Open();
                    String q = "Insert into `tbl_curriculum` (`curr_ID`,`curr_Code`,`curr_Title`,`curr_Units`, `curr_Pre_Req`, `curr_Department`, `curr_Semester`, `curr_Yearlevel`, `curr_Batch`)" +
                      " Values (@curr_ID, @curr_Code,@curr_Title,@curr_Units, @curr_Pre_Req, @curr_Department,@curr_Semester, @curr_Yearlevel, @curr_Batch)";
                    MySqlCommand cmd = new MySqlCommand(q, dbc);

                    //dito magdagdag ng code para mabasa ang datagrid
                    //convert the datagrid data into datatable so we can access the "rows"
                    DataTable insert = new DataTable();
                    insert = ((DataView)Import_list.ItemsSource).ToTable();
                    //Define the parameter bago magloop instead of clearing the parameters every loop
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_ID", MySqlDbType.Int16));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Code", MySqlDbType.VarChar));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Title", MySqlDbType.VarChar));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Units", MySqlDbType.Int16));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Pre_Req", MySqlDbType.VarChar));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Semester", MySqlDbType.VarChar));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Yearlevel", MySqlDbType.Int16));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Department", MySqlDbType.VarChar));
                    _ = cmd.Parameters.Add(new MySqlParameter("@curr_Batch", MySqlDbType.Int16));

                    //Nested for loop to access both the rows and column
                    for (int i = 0; i < insert.Rows.Count; i++)
                    {
                        //this is for the columns
                        for (int j = 0; j < Import_list.Columns.Count; j++)
                        {
                            //Let's insert the data into the database
                            Console.Write(insert.Rows[i][j].ToString());
                            cmd.Parameters["@curr_ID"].Value = 0;
                            cmd.Parameters["@curr_Code"].Value = insert.Rows[i][0];
                            cmd.Parameters["@curr_Title"].Value = insert.Rows[i][1];
                            cmd.Parameters["@curr_Units"].Value = insert.Rows[i][2];
                            //check if the cell was blank
                            if (!String.IsNullOrEmpty(insert.Rows[i][3].ToString()) == true)
                            {
                                cmd.Parameters["@curr_Pre_Req"].Value = insert.Rows[i][3];
                            }
                            else
                            {
                                cmd.Parameters["@curr_Pre_Req"].Value = " ";
                            }
                            cmd.Parameters["@curr_Semester"].Value = insert.Rows[i][4];
                            if (!String.IsNullOrEmpty(insert.Rows[i][5].ToString()) == true)
                            {
                                cmd.Parameters["@curr_Yearlevel"].Value = insert.Rows[i][5];
                            }
                            else
                            {
                                cmd.Parameters["@curr_Yearlevel"].Value = 0;
                            }
                            cmd.Parameters["@curr_Department"].Value = insert.Rows[i][6];
                            cmd.Parameters["@curr_Batch"].Value = insert.Rows[i][7];
                            cmd.CommandTimeout = 60;
                        }

                        //Used for executing queries that does not return any data
                        _ = cmd.ExecuteNonQuery();
                    }

                    //close the connection
                    dbc.Close();

                    _ = MessageBox.Show("Succesfully saved into the database!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    addActivityImportCurr();
                    refresh();
                }
                else if (Import_list.Items.IsEmpty)
                {
                    _ = MessageBox.Show("Please choose the file you want to save.");
                }

                //closing else
                else
                {
                    _ = MessageBox.Show("Please make sure all the fields are filled.");
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show("There is something wrong with your file. " +
                    "Check if the data are in the correct format or if the file is used by another application, close it and try again."
                    , "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void updateCurriculum()
        {
            //try
            //{
            if (txt_Filepath.Text != "" && Import_list.Items.IsEmpty == false)
            {
                MySqlConnection dbc = new MySqlConnection(connectionString);
                dbc.Open();
                String q = "UPDATE `tbl_curriculum` SET `curr_Title`= @curr_Title,`curr_Units`= @curr_Units," +
                "`curr_Pre_Req`= @curr_Pre_Req,`curr_Semester`= @curr_Semester,`curr_Yearlevel`= @curr_Yearlevel," +
                "`curr_Batch`= @curr_Batch,`curr_Department`= @curr_Department WHERE `curr_Code`= @curr_Code AND " +
                "`curr_Batch`= @curr_Batch AND `curr_Department`= @curr_Department";
                MySqlCommand cmd = new MySqlCommand(q, dbc);

                //dito magdagdag ng code para mabasa ang datagrid
                //convert the datagrid data into datatable so we can access the "rows"
                _ = new DataTable();
                DataTable update = ((DataView)Import_list.ItemsSource).ToTable();
                //Define the parameter bago magloop instead of clearing the parameters every loop
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_ID", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Code", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Title", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Units", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Pre_Req", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Semester", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Yearlevel", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Department", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@curr_Batch", MySqlDbType.Int16));

                //Nested for loop to access both the rows and column
                for (int i = 0; i < update.Rows.Count; i++)
                {
                    //this is for the columns
                    for (int j = 0; j < Import_list.Columns.Count; j++)
                    {
                        //Let's insert the data into the database
                        Console.Write(update.Rows[i][j].ToString());
                        cmd.Parameters["@curr_ID"].Value = 0;
                        cmd.Parameters["@curr_Code"].Value = update.Rows[i][0];
                        cmd.Parameters["@curr_Title"].Value = update.Rows[i][1];
                        cmd.Parameters["@curr_Units"].Value = update.Rows[i][2];
                        //check if the cell was blank
                        if (!String.IsNullOrEmpty(update.Rows[i][3].ToString()) == true)
                        {
                            cmd.Parameters["@curr_Pre_Req"].Value = update.Rows[i][3];
                        }
                        else
                        {
                            cmd.Parameters["@curr_Pre_Req"].Value = " ";
                        }
                        cmd.Parameters["@curr_Semester"].Value = update.Rows[i][4];
                        if (!String.IsNullOrEmpty(update.Rows[i][5].ToString()) == true)
                        {
                            cmd.Parameters["@curr_Yearlevel"].Value = int.Parse((string)update.Rows[i][5]);
                        }
                        else
                        {
                            cmd.Parameters["@curr_Yearlevel"].Value = 0;
                        }
                        cmd.Parameters["@curr_Department"].Value = update.Rows[i][6];
                        cmd.Parameters["@curr_Batch"].Value = int.Parse((string)update.Rows[i][7]);
                        cmd.CommandTimeout = 60;
                    }

                    //Used for executing queries that does not return any data
                    _ = cmd.ExecuteNonQuery();
                }

                //close the connection
                dbc.Close();

                _ = MessageBox.Show("Succesfully updated the curriculum!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                refresh();
                addActivityUpdateCurr();

            }
            else if (Import_list.Items.IsEmpty)
            {
                _ = MessageBox.Show("Please choose the file you want to save.");
            }

            //closing else
            else
            {
                _ = MessageBox.Show("Please make sure all the fields are filled.");
            }
            //}
            //catch (Exception)
            //{
            //MessageBox.Show("There is something wrong with your file. " +
            //    "Check if the data are in the correct format or if the file is used by another application, close it and try again."
            //    , "", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        public void refresh()
        {
            btn_saveCurriculum.IsEnabled = false;
            txt_Filepath.Text = "";
            Import_list.ItemsSource = null;
            Import_list.Items.Clear();
        }

        private void btn_saveCurriculum_Click(object sender, RoutedEventArgs e)
        {
            if (Import_list.Items.IsEmpty == false && txt_Filepath.Text != "")
            {
                checkCurriculum();
            }
            else
            {
                _ = MessageBox.Show("Please choose a valid file (.csv) first and try again."
                    , "", MessageBoxButton.OK, MessageBoxImage.Information);
                refresh();
            }
        }

        //MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
        //MessageBox.Show("The file was either corrupted or data are incorrect format.");
        public static object ToDBNull(object value)
        {
            if (null != value)
            {
                return value;
            }

            return DBNull.Value;
        }
        //refresah the curriculum list
        public void refresh_List(object sender, RoutedEventArgs e)
        {
            check_BSCE.IsChecked = false;
            check_BSEE.IsChecked = false;
            check_BSIT.IsChecked = false;
            check_firstsem.IsChecked = false;
            check_secsem.IsChecked = false;
            check_year1.IsChecked = false;
            check_year2.IsChecked = false;
            check_year3.IsChecked = false;
            check_year4.IsChecked = false;
            txt_searchCurr.Text = "";
            Curriculumlist();
        }

        //This is the method for filtering the curriculum list
        public void FilterResult()
        {
            int year;
            string semester1 = "First Sem"; string semester3 = "Second Sem";
            string semester2 = "First Semester"; string semester4 = "Second Semester";
            string dep1 = "Civil Engineering";
            string dep3 = "Information Technology";
            string dep2 = "Electrical Engineering";

            //If the filter for semester was selected (First Semester only)
            if (check_secsem.IsChecked == true && check_BSCE.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_secsem.IsChecked == false && check_year1.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                string query1 = "Select * From `tbl_curriculum` where `curr_Semester` LIKE '"
                    + semester2 + "%' OR `curr_Semester` LIKE '"
                    + semester1 + "%'";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //If the filter for semester was selected (Second Semester)
            else if (check_secsem.IsChecked == true && check_BSCE.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_secsem.IsChecked == false && check_year1.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                semester1 = "Second Sem";
                semester2 = "Second Semester";
                string query1 = "Select * From `tbl_curriculum` where `curr_Semester` LIKE '"
                    + semester2 + "%' OR `curr_Semester` LIKE '"
                    + semester1 + "%'";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for department was selected (BSCE)
            else if (check_BSCE.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_year1.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` LIKE '"
                    + dep1 + "%' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if only the filter for department was selected (BSEE)
            if (check_BSEE.IsChecked == true && check_secsem.IsChecked == false && check_BSCE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_year1.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` LIKE '"
                    + dep2 + "%'";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for department was selected (BSIT)
            if (check_BSIT.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSCE.IsChecked == false && check_firstsem.IsChecked == false && check_year1.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` LIKE '"
                    + dep3 + "%'";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for yearLevel was selected (1st year)
            if (check_year1.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_BSCE.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Yearlevel` = "
                    + year + "";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for yearLevel was selected (2nd year)
            if (check_year2.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_BSCE.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Yearlevel` = "
                    + year + "";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for yearLevel was selected (3rd year)
            if (check_year3.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_BSCE.IsChecked == false
                && check_year2.IsChecked == false && check_year1.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Yearlevel` = "
                    + year + "";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //if the filter for yearLevel was selected (4th year)
            if (check_year4.IsChecked == true && check_secsem.IsChecked == false && check_BSEE.IsChecked == false
                && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false && check_BSCE.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year1.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Yearlevel` = "
                    + year + "";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //First year BSCE, first sem
            else if (check_BSCE.IsChecked == true && check_firstsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //First year BSEE, first sem
            else if (check_BSEE.IsChecked == true && check_firstsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                     + dep2 + "' AND (`curr_Semester` ='"
                     + semester2 + "' OR `curr_Semester` = '"
                     + semester1 + "') AND `curr_Yearlevel` ='"
                     + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //first year BSIT, first sem
            else if (check_BSIT.IsChecked == true && check_firstsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_secsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                     + dep3 + "' AND (`curr_Semester` ='"
                     + semester2 + "' OR `curr_Semester` = '"
                     + semester1 + "') AND `curr_Yearlevel` ='"
                     + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //first year BSCE, second sem
            else if (check_BSCE.IsChecked == true && check_secsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //First year BSEE, second sem
            else if (check_BSEE.IsChecked == true && check_secsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep2 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //First year BSIT, second sem
            else if (check_BSIT.IsChecked == true && check_secsem.IsChecked == true && check_year1.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_firstsem.IsChecked == false
                && check_year2.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 1;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Second year BSCE, first sem
            else if (check_BSCE.IsChecked == true && check_firstsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //second year BSEE, first sem
            else if (check_BSEE.IsChecked == true && check_firstsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep2 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                Course_list.Items.Clear();
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Second year BSIT, first sem
            else if (check_BSIT.IsChecked == true && check_firstsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)

            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Second year BSCE second sem
            else if (check_BSCE.IsChecked == true && check_secsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Second year BSEE, second sem
            else if (check_BSEE.IsChecked == true && check_secsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                     + dep2 + "' AND (`curr_Semester` ='"
                     + semester3 + "' OR `curr_Semester` = '"
                     + semester4 + "') AND `curr_Yearlevel` ='"
                     + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Second year BSIT, second sem
            else if (check_BSIT.IsChecked == true && check_secsem.IsChecked == true && check_year2.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSEE.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year3.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 2;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Third year BSCE, first sem
            else if (check_BSCE.IsChecked == true && check_firstsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //third year BSEE, first sem
            else if (check_BSEE.IsChecked == true && check_firstsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep2 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                Course_list.Items.Clear();
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CourseCurriculum _subjects = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Third year BSIT, first sem
            else if (check_BSIT.IsChecked == true && check_firstsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Third year BSCE, second sem
            else if (check_BSCE.IsChecked == true && check_secsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //third year BSEE, second sem
            else if (check_BSEE.IsChecked == true && check_secsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                     + dep2 + "' AND (`curr_Semester` ='"
                     + semester3 + "' OR `curr_Semester` = '"
                     + semester4 + "') AND `curr_Yearlevel` ='"
                     + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Third year BSIT, second sem
            else if (check_BSIT.IsChecked == true && check_secsem.IsChecked == true && check_year3.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year4.IsChecked == false)
            {
                year = 3;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth year BSCE, first sem
            else if (check_BSCE.IsChecked == true && check_firstsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth year BSEE, first sem
            else if (check_BSEE.IsChecked == true && check_firstsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                     + dep2 + "' AND (`curr_Semester` ='"
                     + semester2 + "' OR `curr_Semester` = '"
                     + semester1 + "') AND `curr_Yearlevel` ='"
                     + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth yeat BSIT, first sem
            else if (check_BSIT.IsChecked == true && check_firstsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_secsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester2 + "' OR `curr_Semester` = '"
                    + semester1 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth year BSCE, second sem
            else if (check_BSCE.IsChecked == true && check_secsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep1 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth year BSEE, second sem
            else if (check_BSEE.IsChecked == true && check_secsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSCE.IsChecked == false && check_BSIT.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep2 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }

            //Fourth year BSIT, second sem
            else if (check_BSIT.IsChecked == true && check_secsem.IsChecked == true && check_year4.IsChecked == true
                && check_BSEE.IsChecked == false && check_BSCE.IsChecked == false && check_firstsem.IsChecked == false
                && check_year1.IsChecked == false && check_year2.IsChecked == false && check_year3.IsChecked == false)
            {
                year = 4;
                string query1 = "Select * From `tbl_curriculum` where `curr_Department` = '"
                    + dep3 + "' AND (`curr_Semester` ='"
                    + semester3 + "' OR `curr_Semester` = '"
                    + semester4 + "') AND `curr_Yearlevel` ='"
                    + year + "' ";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
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
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseSem = reader.GetString(5),
                            CourseYearlvl = reader.GetString(6),
                            CourseBatch = reader.GetString(7),
                        };

                        _ = Course_list.Items.Add(_subjects);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                databaseConnection.Close();
            }

            //Last else statemenet
            else
            {
                Course_list.Items.Clear();
                {
                    check_BSCE.IsChecked = false;
                    check_BSEE.IsChecked = false;
                    check_BSIT.IsChecked = false;
                    check_firstsem.IsChecked = false;
                    check_secsem.IsChecked = false;
                    check_year1.IsChecked = false;
                    check_year2.IsChecked = false;
                    check_year3.IsChecked = false;
                    check_year4.IsChecked = false;
                    txt_searchCurr.Text = "";
                    Curriculumlist();
                }

                //Closing
            }

        }

        private void filter_clicked(object sender, RoutedEventArgs e)
        {
            Course_list.Items.Clear();
            FilterResult();
        }

        //add activity logs
        public void addActivityImportCurr()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Import Curriculum");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " imported a new curriculum from a CSV file.");
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

        public void addActivityUpdateCurr()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Update Curriculum");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " updated a curriculum from a CSV file.");
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
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Logout");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " logout of the system.");
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

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
            HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }
    }

}



