using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;
using ExcelDataReader;
using NUnit.Framework.Internal;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public Student()
        {
            InitializeComponent();
            showStudents();
            btn_saveStudGrade.IsEnabled = false;
        }

        public class GradeRecord
        {
            public int Subject_ID { get; set; }
            public string? Subject_Code { get; set; }
            public string? Subject_Title { get; set; }
            public int Units { get; set; }
            public string? Pre_Requisite { get; set; }
            public int Final_Grade { get; set; }
            public string? Remarks { get; set; }

            public List<GradeRecord> Employee { get; set; }
        }

        public class StudentList
        {
            public int StudentID { get; set; }
            public string? StudentIDNum { get; set; }
            public string? StudentName { get; set; }
            public string? StudentDep { get; set; }
            public int StudentBatch { get; set; }
        }

        public class studentReport
        {
            public int Rep_SubjectID { get; set; }
            public string? Rep_SubjectCode { get; set; }
            public string? Rep_SubjectTitle { get; set; }
            public int Rep_Units { get; set; }
            public string? Rep_Prereq { get; set; }
            public double Rep_Grade { get; set; }
            public string? Rep_Remarks { get; set; }
            public string? Rep_DateIssued { get; set; }
        }
        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
        StudentList currStudents = null;

        //try
        private void ImportGrade_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            txt_GradeFilepath.Text = "";
            try
            {
                //this will open the windows dialog so we can shoose the file
                Microsoft.Win32.OpenFileDialog lObjFileDlge = new Microsoft.Win32.OpenFileDialog();
                lObjFileDlge.Title = "Import Grade";
                lObjFileDlge.Filter = "Excel files|*.xlsx|All files|*.*";
                lObjFileDlge.FilterIndex = 1;
                lObjFileDlge.Multiselect = false;
                string fName = "";
                bool? lBlnUserclicked = lObjFileDlge.ShowDialog();
                if (lBlnUserclicked != null || lBlnUserclicked == true)
                {
                    fName = lObjFileDlge.FileName;
                    txt_GradeFilepath.Text = fName.ToString();
                    getDataTableFromExcel(txt_GradeFilepath.Text);
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    txt_GradeFilepath.Text = fName.ToString();
                    getDataTableFromExcel(txt_GradeFilepath.Text);
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

        private void GenerateReport_click(object sender, RoutedEventArgs e)
        {

        }

        //Ths is the method for the search field in the Student List tab
        private void TextSearchStuds_Changed(object sender, TextChangedEventArgs e)
        {
            //it means that if the search field is empty it will just show the data
            if (txt_searchStudents.Text == "")
            {
                showStudents();
            }
            else
            {
                searchStudents();
            }
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        //Code for exiting the app
        private void exitApp(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        //recently added for import of grades (Evaluation forms)
        private async void btnChooseFile(object sender, RoutedEventArgs e)
        {
            btn_saveStudGrade.IsEnabled = true;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            txt_GradeFilepath.Text = "";
            try
            {
                //this will open the windows dialog so we can shoose the file
                Microsoft.Win32.OpenFileDialog lObjFileDlge = new Microsoft.Win32.OpenFileDialog();
                lObjFileDlge.Title = "Import Grade";
                lObjFileDlge.Filter = "Excel files|*.xlsx|All files|*.*";
                lObjFileDlge.FilterIndex = 1;
                lObjFileDlge.Multiselect = false;
                string fName = "";
                bool? lBlnUserclicked = lObjFileDlge.ShowDialog();
                if (lBlnUserclicked != null || lBlnUserclicked == true)
                {
                    fName = lObjFileDlge.FileName;
                    txt_GradeFilepath.Text = fName.ToString();
                    //List<GradeRecord> grades = await LoadExcelFile(txt_GradeFilepath.Text);
                    //try
                    getDataTableFromExcel(txt_GradeFilepath.Text);
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    txt_GradeFilepath.Text = fName.ToString();
                    //List<GradeRecord> grades = await LoadExcelFile(txt_GradeFilepath.Text);
                    //try
                    getDataTableFromExcel(txt_GradeFilepath.Text);
                }
                else
                {
                    MessageBox.Show("File not found!");
                }
            }
            catch (Exception) { }
        }

        public DataTable getDataTableFromExcel(string path)
        {
            try
            {
                if (txt_GradeFilepath.Text != "")
                {
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = File.OpenRead(path))
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets.First();
                        DataTable tbl = new DataTable();
                        try
                        {
                            txt_StudentName.Text = ws.Cells[Address: "B2"].Value.ToString();
                            txt_StudentNum.Text = ws.Cells[Address: "B3"].Value.ToString();
                            txt_StudentCurrYear.Text = ws.Cells[Address: "E3"].Value.ToString();
                            txt_StudentDep.Text = ws.Cells[Address: "E2"].Value.ToString();
                        }catch (Exception)
                        {
                        }
                        bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
                        foreach (var firstRowCell in ws.Cells[5, 1, 1, ws.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 6 : 1;
                        for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            var row = tbl.NewRow();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text;
                            }
                            tbl.Rows.Add(row);

                        }
                        if (tbl.Rows.Count > 0)
                        {
                            //We will make the datatable the source for the datagrid
                            Import_StudentGrade.ItemsSource = tbl.DefaultView;
                        }
                        return tbl;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file was either open in another app or you are missing a required fields. Check your file and try again", "", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAfterSaved();
            }
            return null;
        }
        private void txt_GradeFilepath_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (txt_GradeFilepath.Text == "")
                {
                    MessageBox.Show("Please input the file path or click choose");
                }
                else if (txt_GradeFilepath.Text != "")
                {
                    getDataTableFromExcel(txt_GradeFilepath.Text);
                }
            }
        }
        //end of import grade 

        public void showStudents()
        {
            Students_list.Items.Clear();
            //Query so we can load the data into our datagrid
            String query = "Select * From `tbl_student`";
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
                    StudentList studs = new StudentList
                    {
                        StudentID = reader.GetInt16(0),
                        StudentIDNum = reader.GetString(1),
                        StudentName = reader.GetString(2),
                        StudentDep = reader.GetString(3),
                        StudentBatch = reader.GetInt16(4)
                    };

                    Students_list.Items.Add(studs);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            //close the connection
            databaseConnection.Close();
        }

        public void searchStudents()
        {
            Students_list.Items.Clear();
            string query = "Select * From `tbl_student` where `student_StudentNo` LIKE '"
                + txt_searchStudents.Text + "%'";
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
                    StudentList studs = new StudentList
                    {
                        StudentID = reader.GetInt16(0),
                        StudentIDNum = reader.GetString(1),
                        StudentName = reader.GetString(2),
                        StudentDep = reader.GetString(3),
                        StudentBatch = reader.GetInt16(4)
                    };

                    Students_list.Items.Add(studs);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            databaseConnection.Close();
        }

        //lets try saving the student first and then the grade
        private void btn_saveStudGrade_click(object sender, RoutedEventArgs e)
        {
            checkStudent();
        }

        //check if the student exist
        public void checkStudent()
        {
            try
            {
                string student_number = txt_StudentNum.Text;
                //First let's check if the student is already existing
                MySqlConnection dbConnection = new MySqlConnection(connectionString);
                String q1 = "SELECT * FROM `tbl_student` WHERE `student_StudentNo` LIKE '" + student_number + "%'";
                MySql.Data.MySqlClient.MySqlCommand commandDatabase = new MySql.Data.MySqlClient.MySqlCommand(q1, dbConnection);
                MySqlDataReader reader;
                dbConnection.Open();
                reader = commandDatabase.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count++;
                }
                if (count == 1)
                {
                    //Let's get the student ID and store it in a hidden label
                    string q2 = "SELECT `student_ID` FROM `tbl_student` WHERE `student_StudentNo` LIKE '" + student_number + "%'";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader dr = commandDatabase1.ExecuteReader();
                    while (dr.Read())
                    {
                        txt_getStudID.Text = dr.GetValue(0).ToString();
                    }
                    dbc.Close();
                    //MessageBox.Show("Get student ID done.");
                    if (MessageBox.Show("This student already existed. Do you want to update the record of student "
                                + txt_StudentNum.Text + ", " + txt_StudentName.Text
                                + "?", "Info", MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
                    //if may record na si student icheck if nagexist na yung grade sa mga subject na evaluate.
                    {
                        if (txt_getStudID.Text != "0" && txt_getStudID.Text != "")
                        {
                            checkGradeRecord();
                        }
                    }
                }
                else if (count == 0)
                {
                    MessageBox.Show("No existing record for student " + txt_StudentNum.Text + ".");
                    insertStudents();
                }
                dbConnection.Close();
            }
            catch (Exception) { }
        }

        public void getStudentID()
        {
            try
            {
                string student_number = txt_StudentNum.Text;
                //Let's get the student ID and store it in a hidden label
                string q2 = "SELECT `student_ID` FROM `tbl_student` WHERE `student_StudentNo` LIKE '" + student_number + "%'";
                MySqlConnection dbc = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                commandDatabase1.CommandTimeout = 60;
                dbc.Open();
                MySqlDataReader dr = commandDatabase1.ExecuteReader();
                while (dr.Read())
                {
                    txt_getStudID.Text = dr.GetValue(0).ToString();
                    //MessageBox.Show("Get student ID done.");
                }
                dbc.Close();
                if (txt_getStudID.Text == "" || txt_getStudID.Text == "0")
                {
                    //MessageBox.Show("Failed to get student ID");
                }
            }
            catch (Exception){}
        }

        public void insertStudents()
        {
            try
            {
                //Insert a new record for student
                String q1 = "INSERT INTO `tbl_student` (`student_ID`,`student_StudentNo`,`student_Name`,`student_Department`, `student_Batch`) VALUES (@stud_ID, @stud_no, @stud_name, @stud_dep, @stud_batch)";
                MySqlConnection dbConnection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand(q1, dbConnection);
                dbConnection.Open();
                cmd.Parameters.AddWithValue("@stud_ID", 0);
                cmd.Parameters.AddWithValue("@@stud_no", txt_StudentNum.Text);
                cmd.Parameters.AddWithValue("@stud_name", txt_StudentName.Text);
                cmd.Parameters.AddWithValue("@stud_dep", txt_StudentDep.Text);
                cmd.Parameters.AddWithValue("@stud_batch", txt_StudentCurrYear.Text);
                MySqlDataReader myReader = cmd.ExecuteReader();
                MessageBox.Show("Successfully added student.");
                dbConnection.Close();

                //end of code for inserting student
                //checkGradeRecord();
                getStudentID();

                if (txt_getStudID.Text != "0" && txt_getStudID.Text != "")
                {
                    insertGrade();
                }
            }
            catch (Exception) { }
        }

        //Check if the evaluation for a specific subject already existed
        public void checkGradeRecord()
        {
            DataTable dt = new DataTable();
            dt = ((DataView)Import_StudentGrade.ItemsSource).ToTable();
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //this is for the columns
                for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                {
                }
                var subject_ID = dt.Rows[i][0];
                MessageBox.Show(dt.Rows[i][0].ToString());
                //Let's try the query to check if it exist
                string q2 = "SELECT * FROM `tbl_grade_record` WHERE `record_CourseID` = '"+ subject_ID + "'";
                MySqlConnection dbc1 = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc1);
                commandDatabase1.CommandTimeout = 60;
                dbc1.Open();
                MySqlDataReader dr1 = commandDatabase1.ExecuteReader();
                if (dr1.HasRows)
                {
                    count++;
                }
                dbc1.Close();
            }
            if (count > 1)
            {
                if (MessageBox.Show("This student already has a grade record from the imported file. Do you want to update the record of student "
                            + txt_StudentNum.Text + ", " + txt_StudentName.Text
                            + "?", "Info", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                        updateGrade();
                }
            }
            else if (count == 0)
            {
                MessageBox.Show("No existing record for student " + txt_StudentNum.Text + ".");
                insertGrade();
            }
            //dbc.Close();
        }
        public void insertGrade()
        {
            try
            {
                MySqlConnection dbc = new MySqlConnection(connectionString);
                dbc.Open();
                String q = "Insert into `tbl_grade_record` (`record_ID`,`record_StudentID`,`record_FinalGrade`,`record_Remarks`, `record_CourseID`, `record_DateIssued`)" +
                  " Values(@record_ID, @record_StudentID,@record_FinalGrade,@record_Remarks, @record_CourseID, @record_DateIssued)";
                MySqlCommand cmd = new MySqlCommand(q, dbc);
                DataTable dt = new DataTable();
                dt = ((DataView)Import_StudentGrade.ItemsSource).ToTable();
                //Define the parameter bago magloop instead of clearing the parameters every loop
                cmd.Parameters.Add(new MySqlParameter("@record_ID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@record_StudentID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@record_FinalGrade", MySqlDbType.Double));
                cmd.Parameters.Add(new MySqlParameter("@record_Remarks", MySqlDbType.VarChar));
                cmd.Parameters.Add(new MySqlParameter("@record_CourseID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@record_DateIssued", MySqlDbType.Date));
                //Nested for loop to access both the rows and column
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //this is for the columns
                    for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                    {
                        // Let's insert the data into the database
                        Console.Write((dt.Rows[i][j]).ToString());
                        cmd.Parameters["@record_ID"].Value = 0;
                        cmd.Parameters["@record_StudentID"].Value = txt_getStudID.Text;
                        cmd.Parameters["@record_FinalGrade"].Value = dt.Rows[i][5];
                        cmd.Parameters["@record_Remarks"].Value = dt.Rows[i][6].ToString();
                        cmd.Parameters["@record_CourseID"].Value = dt.Rows[i][0];
                        cmd.Parameters["@record_DateIssued"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    cmd.CommandTimeout = 60;
                    //Used for executing queries that does not return any data
                    cmd.ExecuteNonQuery();
                }
                dbc.Close();
                MessageBox.Show("Succesfully saved into the database!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAfterSaved();
            }
            catch (Exception) { }   
        }
        //This method is to check if the student has a record in a specific grade
        public void updateGrade()
        {
                MySqlConnection dbc = new MySqlConnection(connectionString);
                dbc.Open();
                String q = "UPDATE `tbl_grade_record` SET `record_FinalGrade`= @finalGrade,`record_Remarks`= @remarks," +
                "`record_DateIssued`=@dateIssued WHERE `record_StudentID`= @studentID AND `record_CourseID`= @courseID";
                MySqlCommand cmd = new MySqlCommand(q, dbc);
                DataTable dt = new DataTable();
                dt = ((DataView)Import_StudentGrade.ItemsSource).ToTable();
                //Define the parameter bago magloop instead of clearing the parameters every loop
                cmd.Parameters.Add(new MySqlParameter("@recordID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@studentID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@finalGrade", MySqlDbType.Double));
                cmd.Parameters.Add(new MySqlParameter("@remarks", MySqlDbType.VarChar));
                cmd.Parameters.Add(new MySqlParameter("@courseID", MySqlDbType.Int16));
                cmd.Parameters.Add(new MySqlParameter("@dateIssued", MySqlDbType.Date));
                //Nested for loop to access both the rows and column
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //this is for the columns
                    for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                    {
                        // Let's insert the data into the database
                        Console.Write((dt.Rows[i][j]).ToString());
                        //cmd.Parameters["@recordID"].Value = 0;
                        cmd.Parameters["@studentID"].Value = txt_getStudID.Text;
                        cmd.Parameters["@finalGrade"].Value = dt.Rows[i][5];
                        cmd.Parameters["@remarks"].Value = dt.Rows[i][6].ToString();
                        cmd.Parameters["@courseID"].Value = dt.Rows[i][0];
                        cmd.Parameters["@dateIssued"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    cmd.CommandTimeout = 60;
                    //Used for executing queries that does not return any data
                    cmd.ExecuteNonQuery();
                }
                dbc.Close();
                MessageBox.Show("Succesfully updated the grades of student "+ txt_getStudID.Text+", "
                    + txt_StudentName.Text + ".", "", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAfterSaved();
        }
        public void clearAfterSaved()
        {
            txt_GradeFilepath.Text = "";
            txt_getStudID.Text = "0";
            txt_StudentCurrYear.Text = "";
            txt_StudentDep.Text = "";
            txt_StudentName.Text = "";
            txt_StudentNum.Text = "";
            Import_StudentGrade.ToolTip = "Excel data will be loaded in this List.";
            this.Import_StudentGrade.ItemsSource = null;
            Import_StudentGrade.Items.Clear();
            btn_saveStudGrade.IsEnabled = false;
        }
        private void refresh_StudentList(object sender, RoutedEventArgs e)
        {
            txt_searchStudents.Text = "";
            showStudents();
        }

        private void btn_generateReport_click(object sender, RoutedEventArgs e)
        {

        }
        
        private void btn_ToGenerateReport_click(object sender, RoutedEventArgs e)
        {
            if (Students_list.SelectedIndex != -1)
            {
                int index = Tabs.SelectedIndex + 1;
                Tabs.SelectedIndex = index;
                
            }
            else
            {
                MessageBox.Show("Please select an employee first!");
            }
        }

        private void Students_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Students_list.SelectedIndex > -1)
            {
                try
                {
                    currStudents = (StudentList)Students_list.SelectedItem;
                    txt_ReportStudID.Text = currStudents.StudentID.ToString();
                    txt_ReportStudNum.Text = currStudents.StudentIDNum;
                    txt_ReportStudName.Text = currStudents.StudentName;
                    txt_ReportStudDep.Text = currStudents.StudentDep;
                    txt_ReportstudBatch.Text = currStudents.StudentBatch.ToString();
                    //Get the data for the list
                    Report_list.Items.Clear();
                    string query = "SELECT `tbl_grade_record`.`record_StudentID`, " +
                    "`tbl_student`.`student_ID`,  " +
                    "`tbl_student`.`student_StudentNo`,  " +
                    "`tbl_grade_record`.`record_CourseID`, " +
                    "`tbl_curriculum`.`curr_Code`, " +
                    "`tbl_curriculum`.`curr_Title`, " +
                    "`tbl_curriculum`.`curr_Units`, " +
                    "`tbl_curriculum`.`curr_Pre_Req`, " +
                    "`tbl_grade_record`.`record_FinalGrade`, " +
                    "`tbl_grade_record`.`record_Remarks`," +
                    "`tbl_grade_record`.`record_DateIssued`" +
                    " FROM `tbl_grade_record` LEFT JOIN `tbl_student`" +
                    " ON `tbl_grade_record`.`record_StudentID` = `tbl_student`.`student_ID` " +
                    "LEFT JOIN `tbl_curriculum` " +
                    "ON `tbl_grade_record`.`record_CourseID` = `tbl_curriculum`.`curr_ID` " +
                    "WHERE `tbl_student`.`student_ID` LIKE '"
                            + txt_ReportStudID.Text + "%'";
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
                            studentReport reps = new studentReport
                            {
                                Rep_SubjectID = reader.GetInt16(3),
                                Rep_SubjectCode = reader.GetString(4),
                                Rep_SubjectTitle = reader.GetString(5),
                                Rep_Units = reader.GetInt16(6),
                                Rep_Prereq = reader.GetString(7),
                                Rep_Grade = reader.GetDouble(8),
                                Rep_Remarks = reader.GetString(9),
                                Rep_DateIssued = Convert.ToDateTime(reader.GetString(10)).ToString("yyyy-MM-dd")
                            };
                            Report_list.Items.Add(reps);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    databaseConnection.Close();
                }
                catch (Exception) { }
            }
        }
    }
}
