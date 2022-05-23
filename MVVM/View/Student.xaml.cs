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
        }

        public class GradeRecord
        {
            public int Subject_ID { get; set; }
            public string? Subject_Code { get; set; }
            public string? Subject_Title { get; set; }
            public int Units { get; set; }
            public string? Pre_Req { get; set; }
            public int Final_Grade { get; set; }
            public string? Remarks { get; set; }

            public List<GradeRecord> Employee { get; set; }
        }

        public class StudentList
        {
            public string? StudentID { get; set; }
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
        }
        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";

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
                    GetDataTableFromExcel(txt_GradeFilepath.Text);
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    txt_GradeFilepath.Text = fName.ToString();
                    GetDataTableFromExcel(txt_GradeFilepath.Text);
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
        //try lang
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }

                }

                return tbl;
            }
        }
        private void GenerateReport_click(object sender, RoutedEventArgs e)
        {

        }

        //Ths is the method for the search field in the report tab
        private void TxtSearchStudReport_Changed(object sender, TextChangedEventArgs e)
        {
            //it means that if the search field is empty it will just show the data
            if (txt_searchStudReport.Text == "")
            {

            }
            else
            {
                StudentsReportSearch();
            }
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
                    List<GradeRecord> grades = await LoadExcelFile(txt_GradeFilepath.Text);
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    txt_GradeFilepath.Text = fName.ToString();
                    List<GradeRecord> grades = await LoadExcelFile(txt_GradeFilepath.Text);
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

        //try lang ayaw parin lumabas data sa datagrid :(
        private async Task<List<GradeRecord>> LoadExcelFile(string fName)
        {
            List<GradeRecord> output = new();
            using var package = new ExcelPackage(fName);
            await package.LoadAsync(fName);
            var ws = package.Workbook.Worksheets[PositionID: 0];
            txt_StudentName.Text = ws.Cells[Address: "B2"].Value.ToString();
            txt_StudentID.Text = ws.Cells[Address: "B3"].Value.ToString();
            txt_BatchNo.Text = ws.Cells[Address: "E2"].Value.ToString();
            txt_StudentDep.Text = ws.Cells[Address: "E3"].Value.ToString();
            int row = 5;
            int col = 1;
            while (string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()) == false)
            {
                GradeRecord g = new();
                g.Subject_ID = int.Parse(ws.Cells[row, col].Value.ToString());
                g.Subject_Code = ws.Cells[row, col + 1].Value.ToString();
                g.Subject_Title = ws.Cells[row, col + 2].Value.ToString();
                g.Units = int.Parse(ws.Cells[row, col + 3].Value.ToString());
                g.Pre_Req = ws.Cells[row, col + 4].Value.ToString();
                g.Final_Grade = int.Parse(ws.Cells[row, col + 5].Value.ToString());
                g.Remarks = ws.Cells[row, col + 6].Value.ToString();
                output.Add(g);
                row += 1;
                MessageBox.Show(g.Subject_ID.ToString() + " " + g.Subject_Code.ToString() + " " +
                    g.Subject_Title.ToString() + " " + g.Units.ToString() + " " + g.Pre_Req.ToString() +
                    " " + g.Final_Grade.ToString() + " " + g.Remarks.ToString());
            }
            this.Import_StudentGrade.ItemsSource = output;
            return output;
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
                    LoadExcelFile(txt_GradeFilepath.Text);
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
                        StudentID = reader.GetString(1),
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
                        StudentID = reader.GetString(1),
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

        //Search student in report module
        public void StudentsReportSearch()
        {
            Report_list.Items.Clear();
            string query = "SELECT `tbl_grade_record`.`record_StudentID`, " +
            "`tbl_student`.`student_StudentNo`,  " +
            "`tbl_grade_record`.`record_CourseID`, " +
            "`tbl_curriculum`.`curr_Code`, " +
            "`tbl_curriculum`.`curr_Title`, " +
            "`tbl_curriculum`.`curr_Units`, " +
            "`tbl_curriculum`.`curr_Pre_Req`, " +
            "`tbl_grade_record`.`record_FinalGrade`, " +
            "`tbl_grade_record`.`record_Remarks`" +
            " FROM `tbl_grade_record` LEFT JOIN `tbl_student`" +
            " ON `tbl_grade_record`.`record_StudentID` = `tbl_student`.`student_ID` " +
            "LEFT JOIN `tbl_curriculum` " +
            "ON `tbl_grade_record`.`record_CourseID` = `tbl_curriculum`.`curr_ID` " +
            "WHERE `tbl_student`.`student_StudentNo` LIKE '"
                    + txt_searchStudReport.Text + "%'";
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
                        Rep_SubjectID = reader.GetInt16(2),
                        Rep_SubjectCode = reader.GetString(3),
                        Rep_SubjectTitle = reader.GetString(4),
                        Rep_Units = reader.GetInt16(5),
                        Rep_Prereq = reader.GetString(6),
                        Rep_Grade = reader.GetDouble(7),
                        Rep_Remarks = reader.GetString(8),
                    };
                    StudentID.Text = reader.GetString(0);
                    Report_list.Items.Add(reps);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            databaseConnection.Close();
            fillStudentInfo();
        }
            public void fillStudentInfo()
            {
            string q = "SELECT * FROM `tbl_student` WHERE`student_ID`= " + StudentID.Text + "";
            MySqlConnection dbc = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase1 = new MySqlCommand(q, dbc);
            commandDatabase1.CommandTimeout = 60;
            MySqlDataReader reader1;
            dbc.Open();
            reader1 = commandDatabase1.ExecuteReader();
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    int d = reader1.GetInt16(0);
                    string id = reader1.GetString(1);
                    string name = reader1.GetString(2);
                    string dep = reader1.GetString(3);
                    int batch = reader1.GetInt16(4);

                    txt_ReportStudID.Text = id;
                    txt_ReportStudName.Text = name;
                    txt_ReposrtStudDep.Text = dep;
                    txt_ReportStudBatch.Text = batch.ToString();
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            dbc.Close();
        }

    private void btn_saveStudGrade_click(object sender, RoutedEventArgs e)
        {
        
        }

        private void refresh_StudentList(object sender, RoutedEventArgs e)
        {
            txt_searchStudents.Text = "";
            showStudents();
        }
    }
}
