using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public Student()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjQ5NzM5QDMyMzAyZTMxMmUzMEh1YklJSHFoVTZuZ3BOREVtZEc3QkhiWXI1Mm5Zb0d5N05sQ3RCTWxnVkE9");
            InitializeComponent();
            showStudents();
            txtUserID.Text = MainWindow.MWinstance.AccountID.Text;
            txtUserName.Text = MainWindow.MWinstance.AccountName.Text;
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

        }

        public class StudentList
        {
            public int StudentID { get; set; }
            public string? StudentIDNum { get; set; }
            public string? StudentName { get; set; }
            public string? StudentDep { get; set; }
            public int StudentBatch { get; set; }
            public string? StudentEvalDate { get; set; }
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

        public DataTable grades { get; private set; }

        //choose from the files
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

        private async Task ExportReports()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //This where we will save the template
            FileInfo? rep = new FileInfo(fileName: @"C:\Student Subject Evaluation System\Reports\Generated_Report.xlsx");
            await saveExcelReport(rep);
        }

        private static void deleteIfExists(FileInfo rep)
        {
            if (rep.Exists)
            {
                rep.Delete();
            }
        }

        private async Task saveExcelReport(FileInfo rep)
        {
            deleteIfExists(rep);

            using (ExcelPackage? excelPackage = new ExcelPackage(rep))
            {
                // //data table from the filter
                string query1 = "SELECT `tbl_curriculum`.`curr_ID`, `tbl_curriculum`.`curr_Code`," +
                     " `tbl_curriculum`.`curr_Title`, `tbl_curriculum`.`curr_Units`, `tbl_curriculum`.`curr_Pre_Req`," +
                     " `tbl_grade_record`.`record_FinalGrade`, `tbl_grade_record`.`record_Remarks` " +
                     " FROM `tbl_curriculum` LEFT JOIN `tbl_grade_record` ON `tbl_grade_record`.`record_CourseID` = `tbl_curriculum`.`curr_ID`" +
                     " WHERE `tbl_grade_record`.`record_StudentID`= '" + txt_ReportStudID.Text + "'";
                DataTable dt = new DataTable("Report");
                try
                {
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    databaseConnection.Open();
                    MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
                    _ = commandDatabase.ExecuteNonQuery();
                    MySqlDataAdapter returnVal = new MySqlDataAdapter(query1, databaseConnection);
                    //I passed the data from mySql to Data table
                    //this is so we can put the data from data table to excel
                    returnVal = new MySqlDataAdapter(commandDatabase);
                    _ = returnVal.Fill(dt);

                    MySqlCommandBuilder _cb = new MySqlCommandBuilder(returnVal);
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.ToString() + " Error!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ExcelWorksheet? worksheet = excelPackage.Workbook.Worksheets.Add(Name: "Report");
                //add the data

                ExcelRangeBase? range = worksheet.Cells[Address: "A7"].LoadFromDataTable(dt, PrintHeaders: true);
                range.AutoFitColumns();

                //Get the total units in the curriculum
                try
                {
                    int total_units;
                    string q2 = "SELECT SUM(`curr_Units`) AS 'Total Units' FROM tbl_curriculum WHERE `curr_Batch`='" + txt_ReportstudBatch.Text + "' AND `curr_Department`='" + txt_ReportStudDep.Text + "'";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader dr = commandDatabase1.ExecuteReader();
                    while (dr.Read())
                    {
                        total_units = dr.GetInt16(0);
                        totalUnits.Text = total_units.ToString();
                    }
                    dbc.Close();
                }
                catch (Exception) { }

                //Get the units taken in the curriculum
                try
                {
                    int units_taken;
                    string q2 = "SELECT SUM(`tbl_curriculum`.`curr_Units`) AS 'Units Taken',`tbl_grade_record`.`record_CourseID`," +
                        " `tbl_grade_record`.`record_StudentID`, `tbl_curriculum`.`curr_ID` FROM " +
                        "`tbl_grade_record` LEFT JOIN `tbl_curriculum` ON `tbl_grade_record`.`record_CourseID` " +
                        "= `tbl_curriculum`.`curr_ID` WHERE `tbl_curriculum`.`curr_Batch`='" + txt_ReportstudBatch.Text + "' " +
                        "AND `tbl_grade_record`.`record_StudentID`=" + txt_ReportStudID.Text + "";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader dr = commandDatabase1.ExecuteReader();
                    while (dr.Read())
                    {
                        units_taken = dr.GetInt16(0);
                        unitsTaken.Text = units_taken.ToString();
                        //MessageBox.Show("Get student ID done.");
                    }
                    dbc.Close();
                }
                catch (Exception) { }

                //units lacking 

                unitsLacking.Text = (Convert.ToInt16(totalUnits.Text) - Convert.ToInt16(unitsTaken.Text)).ToString();
                //formats the header
                worksheet.Cells[Address: "A1"].Value = "GRADE REPORT";
                worksheet.Cells[Address: "A1:G1"].Merge = true;
                //input fields
                worksheet.Cells[Address: "B2:C2"].Merge = true;
                worksheet.Cells[Address: "B3:C3"].Merge = true;
                worksheet.Cells[Address: "B4:C4"].Merge = true;
                worksheet.Cells[Address: "E3:F3"].Merge = true;
                worksheet.Cells[Address: "E2:G2"].Merge = true;
                worksheet.Cells[Address: "E3:G3"].Merge = true;
                worksheet.Column(col: 1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Row(row: 1).Style.Font.Size = 20;
                worksheet.Row(row: 1).Style.Font.Bold = true;
                worksheet.Row(row: 2).Style.Font.Bold = true;
                worksheet.Row(row: 3).Style.Font.Bold = true;
                worksheet.Row(row: 4).Style.Font.Bold = true;
                worksheet.Row(row: 6).Style.Font.Bold = true;
                worksheet.Row(row: 1).Style.Font.Color.SetColor(Color.DarkOrange);

                //Name, student number, and the Course year and section
                worksheet.Cells[Address: "A2"].Value = "Name:";
                worksheet.Cells[Address: "A3"].Value = "Student No:";
                worksheet.Cells[Address: "A4"].Value = "Units Taken:";
                worksheet.Cells[Address: "B2"].Value = txt_ReportStudName.Text;
                worksheet.Cells[Address: "B3"].Value = txt_ReportStudNum.Text;
                worksheet.Cells[Address: "B4"].Value = int.Parse(unitsTaken.Text);
                worksheet.Cells[Address: "D2"].Value = "Department:";
                worksheet.Cells[Address: "D3"].Value = "Year:";
                worksheet.Cells[Address: "D4"].Value = "Lacking Units:";
                worksheet.Cells[Address: "E2"].Value = txt_ReportStudDep.Text;
                worksheet.Cells[Address: "E3"].Value = int.Parse(txt_ReportstudBatch.Text);
                worksheet.Cells[Address: "E4"].Value = int.Parse(unitsLacking.Text);

                //Sa table
                worksheet.Cells[Address: "A6"].Value = "Subject ID";
                worksheet.Cells[Address: "B6"].Value = "Subject Code";
                worksheet.Cells[Address: "C6"].Value = "Subject Title";
                worksheet.Cells[Address: "D6"].Value = "Units";
                worksheet.Cells[Address: "E6"].Value = "Pre Req(s)";
                worksheet.Cells[Address: "F6"].Value = "Final Grade";
                worksheet.Cells[Address: "G6"].Value = "Remarks";
                //format other thing

                worksheet.Column(col: 1).Width = 13;
                worksheet.Column(col: 2).Width = 8;
                worksheet.Column(col: 3).Width = 25;
                worksheet.Column(col: 4).Width = 13;
                worksheet.Column(col: 5).Width = 9;
                worksheet.Column(col: 6).Width = 9;
                worksheet.Column(col: 7).Width = 8;
                worksheet.Column(2).Style.WrapText = true;
                worksheet.Column(3).Style.WrapText = true;
                worksheet.Column(5).Style.WrapText = true;
                worksheet.Column(6).Style.WrapText = true;
                worksheet.Column(col: 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(col: 3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Column(col: 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(col: 5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(col: 6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(col: 7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[Address: "D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[Address: "D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[Address: "A4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[Address: "D4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Row(row: 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Row(row: 3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Row(row: 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Column(col: 6).Style.Numberformat.Format = "0.00";
                worksheet.DeleteRow(7);
                worksheet.PrinterSettings.FitToPage = true;
                // set some document properties
                excelPackage.Workbook.Properties.Title = "Grade Report for Student '" + txt_ReportStudNum.Text + "'.";
                excelPackage.Workbook.Properties.Author = txtUserName.Text;
                await excelPackage.SaveAsync();

                MessageBox.Show("Generated Report as Excel. Converting to PDF file, please wait..");
                using (ExcelEngine excelEngine = new ExcelEngine())
                {

                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;
                    string path = @"C:\Student Subject Evaluation System\Reports\Generated_Report.xlsx";
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    //FileStream inputStream = new FileStream("../../../Reports/Generated_Report.xlsx", FileMode.Open, FileAccess.Read);
                    IWorkbook workbook = application.Workbooks.Open(stream);
                    //MessageBox.Show("Able to read excel files");
                    //Initialize XlsIORendererSettings
                    XlsIORendererSettings settings = new XlsIORendererSettings();
                    //Initialize XlsIO renderer.
                    XlsIORenderer renderer = new XlsIORenderer();

                    //Convert Excel document into PDF document 
                    PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);
                    //Set layout option as FitAllColumnsOnOnePage
                    settings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;
                    //FileStream outputStream0 = new FileStream("Generated_Report_" + txt_ReportStudNum.Text + "_"
                    //+ "_" + txt_ReportStudDep.Text + "_"
                    //+ DateTime.Now.ToString("dd-MM-yyyy") + ".pdf", FileMode.Create, FileAccess.Write);
                    //pdfDocument.Save(outputStream0);
                    MessageBox.Show("Successfully Saved as a PDF file.");
                    #region Save


                    //Saving the workbook
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Files(*.pdf)|*.pdf";
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.DefaultExt = ".pdf";
                    saveFileDialog.FileName = "Generated_Report_" + txt_ReportStudNum.Text + "_"
                    + "_" + txt_ReportStudDep.Text + "_"
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";

                    if (saveFileDialog.ShowDialog() == true && saveFileDialog.CheckPathExists)
                    {
                        //Save the PDF Document to disk.
                        //pdfDocument.Save(saveFileDialog.FileName);
                        FileStream outputStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                        pdfDocument.Save(outputStream);

                        if (MessageBox.Show("Do you want to view the PDF file?", "PDF File Created", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                        //Message box confirmation to view the created PDF document.
                        {
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                            {
                                UseShellExecute = true
                            };
                            _ = process.Start();
                            //process.Start();
                        }
                        outputStream.Dispose();
                        stream.Dispose();
                        addActivityGenerateReport();
                        txt_ReportstudBatch.Text = "";
                        txt_ReportStudDep.Text = "";
                        txt_ReportStudName.Text = "";
                        txt_ReportStudID.Text = "";
                        txt_ReportStudNum.Text = "";
                        unitsLacking.Text = "";
                        unitsTaken.Text = "";
                        totalUnits.Text = "";
                        Report_list.ItemsSource = null;
                        Report_list.Items.Clear();
                    }
                    #endregion
                }
            }
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
             HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }

        //Code for exiting the app
       
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
                    _ = getDataTableFromExcel(txt_GradeFilepath.Text);
                }
                if (System.IO.File.Exists(fName) == true)
                {
                    txt_GradeFilepath.Text = fName.ToString();
                    //List<GradeRecord> grades = await LoadExcelFile(txt_GradeFilepath.Text);
                    //try
                    _ = getDataTableFromExcel(txt_GradeFilepath.Text);
                }
                else
                {
                    _ = MessageBox.Show("File not found!");
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
                    using (ExcelPackage? pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (FileStream? stream = System.IO.File.OpenRead(path))
                        {
                            pck.Load(stream);
                        }                        
                        ExcelWorksheet? ws = pck.Workbook.Worksheets.First();
                        DataTable tbl = new DataTable();
                        try
                        {
                            if (!String.IsNullOrEmpty(ws.Cells[Address: "B2"].Value.ToString()) == false)
                            {
                                _ = MessageBox.Show("It seems like you are missing a required field. Please check your data and try again.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                clearAfterSaved();
                            }
                            else if (!String.IsNullOrEmpty(ws.Cells[Address: "B3"].Value.ToString()) == false)
                            {
                                _ = MessageBox.Show("It seems like you are missing a required field. Please check your data and try again.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                clearAfterSaved();
                            }
                            else
                            {
                                txt_StudentName.Text = ws.Cells[Address: "B2"].Value.ToString();
                                txt_StudentNum.Text = ws.Cells[Address: "B3"].Value.ToString();
                                txt_StudentDep.Text = ws.Cells[Address: "E2"].Value.ToString();
                                txt_StudentCurrYear.Text = ws.Cells[Address: "E3"].Value.ToString();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        bool hasHeader = true;
                        foreach (ExcelRangeBase? firstRowCell in ws.Cells[5, 1, 1, ws.Dimension.End.Column])
                        {
                            _ = tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        int startRow = hasHeader ? 6 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            ExcelRange? wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow? row = tbl.NewRow();
                            foreach (ExcelRangeBase? cell in wsRow)
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
                clearAfterSaved();
                _ = MessageBox.Show("The file was either open in another app or you are missing a required fields. Check your file and try again", "", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        StudentBatch = reader.GetInt16(4),
                        StudentEvalDate = Convert.ToDateTime(reader.GetString(5)).ToString("yyyy-MM-dd")
                    };

                    _ = Students_list.Items.Add(studs);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            //close the connection
            databaseConnection.Close();
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
        public void searchStudents()
        {
            Students_list.Items.Clear();
            string query = "Select * From `tbl_student` where `student_StudentNo` LIKE '"
                 + txt_searchStudents.Text + "%' OR `student_Name` LIKE '"
                 + txt_searchStudents.Text + "%'  OR `student_Batch` LIKE '"
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
                        StudentBatch = reader.GetInt16(4),
                        StudentEvalDate = Convert.ToDateTime(reader.GetString(5)).ToString("yyyy-MM-dd")
                    };

                    _ = Students_list.Items.Add(studs);
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
                    string q2 = "SELECT * FROM `tbl_student` WHERE `student_StudentNo` LIKE '" + student_number + "%'";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader dr = commandDatabase1.ExecuteReader();
                    while (dr.Read())
                    {
                        txt_getStudID.Text = dr.GetValue(0).ToString();
                        txt_StudNameCheck.Text = dr.GetValue(2).ToString();
                        txt_StudentDep.Text = dr.GetValue(3).ToString();
                        txt_CheckStudentBatch.Text = dr.GetValue(4).ToString();
                    }
                    dbc.Close();
                        if (txt_StudNameCheck.Text != txt_StudentName.Text || txt_StudentDep.Text != txt_StudentDep.Text || txt_CheckStudentBatch.Text != txt_StudentCurrYear.Text)
                        {
                            if (MessageBox.Show("A student with ID number " + txt_StudentNum.Text + " was already registered with the following details: "
                            +"\nStudent Name: "+ txt_StudNameCheck.Text 
                            +"\nDepartment: " + txt_StudentDep.Text   
                            +"\nBatch: " + txt_CheckStudentBatch.Text 
                            +"\nIf you proceed the following details will be updated. Do you still want to proceed?", "Info", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                updateStudentDetails();
                            }
                        }
                        else
                        {
                        }
                }
                else if (count == 0)
                {
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
            catch (Exception) { }
        }

        public void insertStudents()
        {
            try
            {
                //Insert a new record for student
                String q1 = "INSERT INTO `tbl_student` (`student_ID`,`student_StudentNo`,`student_Name`,`student_Department`, `student_Batch`, `student_DateEvaluated`) VALUES (@stud_ID, @stud_no, @stud_name, @stud_dep, @stud_batch, @dateEval)";
                MySqlConnection dbConnection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand(q1, dbConnection);
                dbConnection.Open();
                _ = cmd.Parameters.AddWithValue("@stud_ID", 0);
                _ = cmd.Parameters.AddWithValue("@@stud_no", txt_StudentNum.Text);
                _ = cmd.Parameters.AddWithValue("@stud_name", txt_StudentName.Text);
                _ = cmd.Parameters.AddWithValue("@stud_dep", txt_StudentDep.Text);
                _ = cmd.Parameters.AddWithValue("@stud_batch", txt_StudentCurrYear.Text);
                _ = cmd.Parameters.AddWithValue("@dateEval", DateTime.Now.ToString("yyyy-MM-dd"));
                MySqlDataReader myReader = cmd.ExecuteReader();
                _ = MessageBox.Show("Successfully added student.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                addActivityInsertStudent();
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
        public void updateStudentDetails()
        {
            try
            {
                //Insert a new record for student
                String q1 = "UPDATE `tbl_student` SET `student_StudentNo`=@stud_no,`student_Name`=@stud_name,`student_Department`=@stud_dep, `student_Batch`=@stud_batch, `student_DateEvaluated`=@dateEval WHERE `student_ID`=@stud_ID" ;
                MySqlConnection dbConnection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand(q1, dbConnection);
                dbConnection.Open();
                _ = cmd.Parameters.AddWithValue("@stud_ID", txt_getStudID.Text);
                _ = cmd.Parameters.AddWithValue("@@stud_no", txt_StudentNum.Text);
                _ = cmd.Parameters.AddWithValue("@stud_name", txt_StudentName.Text);
                _ = cmd.Parameters.AddWithValue("@stud_dep", txt_StudentDep.Text);
                _ = cmd.Parameters.AddWithValue("@stud_batch", txt_StudentCurrYear.Text);
                _ = cmd.Parameters.AddWithValue("@dateEval", DateTime.Now.ToString("yyyy-MM-dd"));
                MySqlDataReader myReader = cmd.ExecuteReader();
                addActivityUpdateStudent();
                _ = MessageBox.Show("Successfully updated the student details of student "+txt_StudentNum.Text+".", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                addActivityInsertStudent();
                dbConnection.Close();

                //end of code for inserting student
                //checkGradeRecord();
                getStudentID();

                if (txt_getStudID.Text != "0" && txt_getStudID.Text != "")
                {
                    checkGradeRecord();
                }
            }
            catch (Exception) { }
        }

        //Check if the evaluation for a specific subject already existed
        public void checkGradeRecord()
        {
            _ = new DataTable();
            DataTable dt = ((DataView)Import_StudentGrade.ItemsSource).ToTable();
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //this is for the columns
                for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                {
                }
                object? subject_ID = dt.Rows[0][0];
                //Let's try the query to check if it exist
                string q2 = "SELECT * FROM `tbl_grade_record` WHERE `record_CourseID` = '" + subject_ID + "' AND `record_StudentID`='" + txt_getStudID.Text + "'";
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
                if (MessageBox.Show("This student already has a grade record from the imported file. Do you want to update the record of student "
                            + txt_StudentNum.Text + ", " + txt_StudentName.Text
                            + "?", "Info", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    updateGrade();
                }
                else
                {
                    clearAfterSaved();
                }
            }
            else if (count == 0)
            {
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
                _ = cmd.Parameters.Add(new MySqlParameter("@record_ID", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@record_StudentID", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@record_FinalGrade", MySqlDbType.Double));
                _ = cmd.Parameters.Add(new MySqlParameter("@record_Remarks", MySqlDbType.VarChar));
                _ = cmd.Parameters.Add(new MySqlParameter("@record_CourseID", MySqlDbType.Int16));
                _ = cmd.Parameters.Add(new MySqlParameter("@record_DateIssued", MySqlDbType.Date));
                //Nested for loop to access both the rows and column
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //this is for the columns
                    for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                    {
                        // Let's insert the data into the database
                        Console.Write(dt.Rows[i][j].ToString());
                        cmd.Parameters["@record_ID"].Value = 0;
                        cmd.Parameters["@record_StudentID"].Value = txt_getStudID.Text;
                        cmd.Parameters["@record_FinalGrade"].Value = dt.Rows[i][5];
                        cmd.Parameters["@record_Remarks"].Value = dt.Rows[i][6].ToString();
                        cmd.Parameters["@record_CourseID"].Value = dt.Rows[i][0];
                        cmd.Parameters["@record_DateIssued"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    cmd.CommandTimeout = 60;
                    //Used for executing queries that does not return any data
                    _ = cmd.ExecuteNonQuery();
                }
                dbc.Close();
                _ = MessageBox.Show("Succesfully saved into the database!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                addActivityInsertGrade();
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
            _ = new DataTable();
            DataTable dt = ((DataView)Import_StudentGrade.ItemsSource).ToTable();
            //Define the parameter bago magloop instead of clearing the parameters every loop
            _ = cmd.Parameters.Add(new MySqlParameter("@recordID", MySqlDbType.Int16));
            _ = cmd.Parameters.Add(new MySqlParameter("@studentID", MySqlDbType.Int16));
            _ = cmd.Parameters.Add(new MySqlParameter("@finalGrade", MySqlDbType.Double));
            _ = cmd.Parameters.Add(new MySqlParameter("@remarks", MySqlDbType.VarChar));
            _ = cmd.Parameters.Add(new MySqlParameter("@courseID", MySqlDbType.Int16));
            _ = cmd.Parameters.Add(new MySqlParameter("@dateIssued", MySqlDbType.Date));
            //Nested for loop to access both the rows and column
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //this is for the columns
                for (int j = 0; j < Import_StudentGrade.Columns.Count; j++)
                {
                    // Let's insert the data into the database
                    Console.Write(dt.Rows[i][j].ToString());
                    //cmd.Parameters["@recordID"].Value = 0;
                    cmd.Parameters["@studentID"].Value = txt_getStudID.Text;
                    cmd.Parameters["@finalGrade"].Value = dt.Rows[i][5];
                    cmd.Parameters["@remarks"].Value = dt.Rows[i][6].ToString();
                    cmd.Parameters["@courseID"].Value = dt.Rows[i][0];
                    cmd.Parameters["@dateIssued"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                }
                cmd.CommandTimeout = 60;
                //Used for executing queries that does not return any data
                _ = cmd.ExecuteNonQuery();
            }
            dbc.Close();
            _ = MessageBox.Show("Succesfully updated the grades of student " + txt_StudentNum.Text + ", "
                + txt_StudentName.Text + ".", "", MessageBoxButton.OK, MessageBoxImage.Information);
            addActivityUpdateGrade();

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
            Import_StudentGrade.ItemsSource = null;
            Import_StudentGrade.Items.Clear();
            btn_saveStudGrade.IsEnabled = false;
        }
        private void refresh_StudentList(object sender, RoutedEventArgs e)
        {
            txt_searchStudents.Text = "";
            customDate.Text = "";
            check_Today.IsChecked = false;
            showStudents();
        }

        private void btn_generateReport_click(object sender, RoutedEventArgs e)
        {
            _ = ExportReports();
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
                            _ = Report_list.Items.Add(reps);
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

        public void addActivityInsertStudent()
        {
            _ = MainWindow.MWinstance.AccountID.Text;
            _ = MainWindow.MWinstance.AccountName.Text;
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Insert Student");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " inserted a new grade for student "
                + txt_StudentName.Text + " (" + txt_StudentNum.Text + ") from Department of " + txt_StudentDep.Text
                + " batch " + int.Parse(txt_StudentCurrYear.Text) + ".");
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

        public void addActivityInsertGrade()
        {
            _ = MainWindow.MWinstance.AccountID.Text;
            _ = MainWindow.MWinstance.AccountName.Text;
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Insert Grade Record");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " inserted a new grade record for student "
                + txt_StudentName.Text + " (" + txt_StudentNum.Text + ").");
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

        public void addActivityUpdateStudent()
        {
            _ = MainWindow.MWinstance.AccountID.Text;
            _ = MainWindow.MWinstance.AccountName.Text;
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Update Student Details");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " updated the record of student "
                + txt_StudentName.Text + " (" + txt_StudentNum.Text + ").");
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

        public void addActivityUpdateGrade()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Update Grade Record");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " updated a grade record for student "
                + txt_StudentName.Text + " (" + txt_StudentNum.Text + ").");
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
            clearAfterSaved();
        }

        public void addActivityGenerateReport()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Generate Report");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " generated a report for student "
                + txt_ReportStudName.Text + " (" + txt_ReportStudNum.Text + ").");
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
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`, log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
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

        private void btn_populateFields_click(object sender, RoutedEventArgs e)
        {
            if (Students_list.SelectedIndex != -1)
            {
                int index = Tabs.SelectedIndex + 1;
                Tabs.SelectedIndex = index;
            }
            else
            {
                _ = MessageBox.Show("Please select a student first!");
            }
        }

        private void customDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            check_Today.IsChecked = false;
        }

        private void customDate_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            customDate.ToolTip = "Choose a custom date";
        }

        private void customDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            check_Today.IsChecked = false;
            try
            {
                if (customDate.Text != "" && check_Today.IsChecked == false)
                {
                    Students_list.Items.Clear();
                    string q2 = "SELECT * FROM tbl_student WHERE `student_DateEvaluated` ='" +
                        DateTime.Parse(customDate.Text).ToString("yyyy-MM-dd") + "'";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader reader = commandDatabase1.ExecuteReader();
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
                                StudentBatch = reader.GetInt16(4),
                                StudentEvalDate = Convert.ToDateTime(reader.GetString(5)).ToString("yyyy-MM-dd")
                            };

                            _ = Students_list.Items.Add(studs);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void check_Today_Checked(object sender, RoutedEventArgs e)
        {
            customDate.Text = "";
            try
            {
                if (customDate.Text == "" && check_Today.IsChecked == true)
                {
                    Students_list.Items.Clear();
                    string q2 = "SELECT * FROM tbl_student WHERE `student_DateEvaluated` ='" +
                    DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    MySqlConnection dbc = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                    commandDatabase1.CommandTimeout = 60;
                    dbc.Open();
                    MySqlDataReader reader = commandDatabase1.ExecuteReader();
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
                                StudentBatch = reader.GetInt16(4),
                                StudentEvalDate = Convert.ToDateTime(reader.GetString(5)).ToString("yyyy-MM-dd")
                            };

                            _ = Students_list.Items.Add(studs);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void check_Today_Unchecked(object sender, RoutedEventArgs e)
        {
            showStudents();
        }
    }
}
