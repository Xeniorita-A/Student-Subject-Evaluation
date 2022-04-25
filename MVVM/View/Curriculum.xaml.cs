using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

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
                   CourseCurriculum _subjects1 = new CourseCurriculum
                        {
                            CourseCode = reader.GetString(1),
                            CourseTitle = reader.GetString(2),
                            CourseUnits = reader.GetInt16(3),
                            CoursePrereq = reader.GetString(4),
                            CourseYearlvl = reader.GetString(6),
                            CourseSem = reader.GetString(7),
                            CourseBatch = reader.GetString(8),
                        };
                            Course_list.Items.Add(_subjects1);
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
                        CourseCode = reader.GetString(1),
                        CourseTitle = reader.GetString(2),
                        CourseUnits = reader.GetInt16(3),
                        CoursePrereq = reader.GetString(4),
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
                System.Windows.Application.Current.Shutdown();
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
                    if (dt.Rows.Count > 0)
                    {
                        //We will make the datatable the source for the datagrid
                        Import_list.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There's something wrong with the file choosen. File not found or file was corrupted.");
            }
        }

        //Button choose file
        private void btnChoose(object sender, RoutedEventArgs e)
        {
            //Let's clear these fields first.
            txt_Filepath.Text = "";
            cbx_currDepartment.SelectedIndex = -1;
            txt_currBatch.Text = "";
            try
            {
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
                    var lines = File.ReadAllLines(fName);
                    txt_Filepath.Text = fName.ToString();
                    bindDataCSV(txt_Filepath.Text);
                    lObjStreamReader.Close();
                }
                else
                {
                    MessageBox.Show("File not found!");
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
                    MessageBox.Show("Please input the file path or click choose");
                }
                else if (txt_Filepath.Text != "")
                {
                    bindDataCSV(txt_Filepath.Text);
                }
            }
        }

        private void btn_saveCurriculum_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                if (txt_Filepath.Text != "" && Import_list.Items.IsEmpty == false && txt_currBatch.Text != "")
                {
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    dbc.Open();
                    String q = "Insert into `tbl_curriculum` (`curr_ID`,`curr_Code`,`curr_Title`,`curr_Units`, `curr_Pre_Req`, `curr_Department`, `curr_Semester`, `curr_Yearlevel`, `curr_Batch`)" +
                      " Values(@curr_ID, @curr_Code,@curr_Title,@curr_Units, @curr_Pre_Req, @curr_Department,@curr_Semester, @curr_Yearlevel, @curr_Batch)";
                    MySqlCommand cmd = new MySqlCommand(q, dbc);

                    //dito magdagdag ng code para mabasa ang datagrid
                    //convert the datagrid data into datatable so we can access the "rows"
                    DataTable dt = new DataTable();
                    dt = ((DataView)Import_list.ItemsSource).ToTable();
                    //Define the parameter bago magloop instead of clearing the parameters every loop
                    cmd.Parameters.Add(new MySqlParameter("@curr_ID", MySqlDbType.Int16));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Code", MySqlDbType.VarChar));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Title", MySqlDbType.VarChar));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Units", MySqlDbType.Int16));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Pre_Req", MySqlDbType.VarChar));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Semester", MySqlDbType.VarChar));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Yearlevel", MySqlDbType.Int16));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Batch", MySqlDbType.Int16));
                    cmd.Parameters.Add(new MySqlParameter("@curr_Department", MySqlDbType.Int16));

                    //Nested for loop to access both the rows and column
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //this is for the columns
                        for (int j = 0; j < Import_list.Columns.Count; j++)
                        {
                            //Let's insert the data into the database
                            Console.Write((dt.Rows[i][j]).ToString());
                            cmd.Parameters["@curr_ID"].Value = 0;
                            cmd.Parameters["@curr_Code"].Value = dt.Rows[i][0].ToString();
                            cmd.Parameters["@curr_Title"].Value = dt.Rows[i][1].ToString();
                            cmd.Parameters["@curr_Units"].Value = dt.Rows[i][2];
                            cmd.Parameters["@curr_Pre_Req"].Value = dt.Rows[i][3].ToString();
                            cmd.Parameters["@curr_Semester"].Value = dt.Rows[i][4].ToString();
                            cmd.Parameters["@curr_Yearlevel"].Value = dt.Rows[i][5];
                            cmd.Parameters["@curr_Batch"].Value = Convert.ToInt16(txt_currBatch.Text);
                            if (cbx_currDepartment.SelectedIndex == 2)
                            {
                                cmd.Parameters["@curr_Department"].Value = 3;
                            }
                            else if (cbx_currDepartment.SelectedIndex == 0)
                            {
                                cmd.Parameters["@curr_Department"].Value = 1;
                            }
                            else if (cbx_currDepartment.SelectedIndex == 1)
                            {
                                cmd.Parameters["@curr_Department"].Value = 2;
                            }
                            else if (cbx_currDepartment.SelectedIndex == -1)
                            {
                                MessageBox.Show("Please select the department first and try again.");
                            }
                            else
                            {
                                MessageBox.Show("Department does not exist.");
                            }
                            cmd.CommandTimeout = 60;
                        }
                        //Used for executing queries that does not return any data
                        cmd.ExecuteNonQuery();
                    }

                    //close the connection
                    dbc.Close();
                    MessageBox.Show("Succesfully saved into the database!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Import_list.Items.IsEmpty)
                {
                    MessageBox.Show("Please choose the file you want to save.");
                }
                else
                {
                    MessageBox.Show("Please make sure all the fields are filled.");
                }

                DataGridCell GetCell(int row, int column)
                {
                    DataGridRow rowData = GetRow(row);
                    if (rowData != null)
                    {
                        DataGridCellsPresenter cellPresenter = GetVisualChild<DataGridCellsPresenter>(rowData);
                        DataGridCell cell = (DataGridCell)cellPresenter.ItemContainerGenerator.ContainerFromIndex(column);
                        if (cell == null)
                        {
                            Import_list.ScrollIntoView(rowData, Import_list.Columns[column]);
                            cell = (DataGridCell)cellPresenter.ItemContainerGenerator.ContainerFromIndex(column);
                        }
                        return cell;
                    }
                    return null;
                }

                DataGridRow GetRow(int index)
                {
                    DataGridRow row = (DataGridRow)Import_list.ItemContainerGenerator.ContainerFromIndex(index);
                    if (row == null)
                    {
                        Import_list.UpdateLayout();
                        Import_list.ScrollIntoView(Import_list.Items[index]);
                        row = (DataGridRow)Import_list.ItemContainerGenerator.ContainerFromIndex(index);
                    }
                    return row;
                }

                static T GetVisualChild<T>(Visual parent) where T : Visual
                {
                    T child = default(T);
                    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                    for (int i = 0; i < numVisuals; i++)
                    {
                        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                        child = v as T;
                        if (child == null)
                        {
                            child = GetVisualChild<T>(v);
                        }
                        if (child != null)
                        {
                            break;
                        }
                    }
                    return child;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file was either corrupted or data are incorrect format.");
            }    
        }

        //refresah the curriculum list
        private void refresh_List(object sender, RoutedEventArgs e)
        {
            Curriculumlist();
        }
    }

 }



