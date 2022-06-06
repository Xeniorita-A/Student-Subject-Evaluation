using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for Evaluation.xaml
    /// </summary>
    public partial class Evaluation : UserControl
    {
        public Evaluation()
        {
            InitializeComponent();
            btn_exportEval.IsEnabled = false;
            txtUserID.Text = MainWindow.MWinstance.AccountID.Text;
            txtUserName.Text = MainWindow.MWinstance.AccountName.Text;
        }
        public class EvalForms
        {
            public int Subject_ID { get; set; }
            public string? Subject_Code { get; set; }
            public string? Subject_Title { get; set; }
            public int Units { get; set; }
            public string? Pre_Req { get; set; }
            public string? Final_Grade { get; set; }
            public string? Remarks { get; set; }
            public int Yearlevel { get; set; }
            public string? Semester { get; set; }
        }
        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
            HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }

        private async Task ExportEval()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //This where we will save the template
            FileInfo? file = new FileInfo(fileName: @"C:\Student Subject Evaluation System\Evaluation Forms\EvaluationForm.xlsx");
            await saveExcelFile(file);
        }

        //method for the save excel file
        //the problem is how we can put the data on the excel from datagrid :((
        private async Task saveExcelFile(FileInfo file)
        {
            deleteIfExists(file);

            using (ExcelPackage? package = new ExcelPackage(file))
            {
                //data table from the filter
                string query1 = "SELECT `curr_ID`, `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req` FROM `tbl_curriculum` where `curr_Batch` = "
               + int.Parse(txt_currYear.Text) + " AND `curr_Yearlevel` = "
               + int.Parse(cbx_evalYearlevel.Text) + "  AND `curr_Semester` = '"
               + cbx_evalSemester.Text + "'  AND `curr_Department` = '"
               + cbx_evalDepartment.Text + "'";

                DataTable dt = new DataTable("EvalForms");
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
                catch
                {
                    _ = MessageBox.Show("No record found in the database");
                }

                //WS is the worksheet
                ExcelWorksheet? ws = package.Workbook.Worksheets.Add(Name: "Evaluation_Form");
                //add the data

                ExcelRangeBase? range = ws.Cells[Address: "A6"].LoadFromDataTable(dt, PrintHeaders: true);
                range.AutoFitColumns();

                //formats the header
                ws.Cells[Address: "A1"].Value = "EVALUATION FORM";
                ws.Cells[Address: "B2:C2"].Merge = true;
                ws.Cells[Address: "B3:C3"].Merge = true;
                ws.Cells[Address: "E2:G2"].Merge = true;
                ws.Cells[Address: "E3:G3"].Merge = true;
                ws.Cells[Address: "A1:G1"].Merge = true;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Row(row: 1).Style.Font.Size = 20;
                ws.Row(row: 1).Style.Font.Bold = true;
                ws.Row(row: 2).Style.Font.Bold = true;
                ws.Row(row: 3).Style.Font.Bold = true;
                ws.Row(row: 5).Style.Font.Bold = true;
                ws.Cells[Address: "A5:G5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[Address: "A5:G5"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkOrange);
                ws.Row(row: 1).Style.Font.Color.SetColor(Color.White);
                ws.Row(row: 5).Style.Font.Color.SetColor(Color.White);
                ws.Cells[Address: "A1:G1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[Address: "A1:G1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkOrange);

                //Name, student number, and the Course year and section
                ws.Cells[Address: "A2"].Value = "Name (FN MI. LN):";
                ws.Cells[Address: "A3"].Value = "Student Number:";
                ws.Cells[Address: "D2"].Value = "Department:";
                ws.Cells[Address: "D3"].Value = "Curriculum Year:";
                ws.Cells[Address: "E2"].Value = cbx_evalDepartment.Text;
                ws.Cells[Address: "E3"].Value = int.Parse(txt_currYear.Text);
                ws.Cells[Address: "A5"].Value = "Subject ID";
                ws.Cells[Address: "B5"].Value = "Subject Code";
                ws.Cells[Address: "C5"].Value = "Subject Title";
                ws.Cells[Address: "D5"].Value = "Units";
                ws.Cells[Address: "E5"].Value = "Pre Requisites";
                ws.Cells[Address: "F5"].Value = "Final Grade";
                ws.Cells[Address: "G5"].Value = "Remarks";

                //format other thing
                ws.Column(col: 1).Width = 16;
                ws.Column(col: 2).Width = 13;
                ws.Column(col: 3).Width = 50;
                ws.Column(col: 4).Width = 15;
                ws.Column(col: 5).Width = 15;
                ws.Column(col: 6).Width = 13;
                ws.Column(col: 7).Width = 13;
                ws.Column(col: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(row: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Row(row: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Row(row: 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 6).Style.Numberformat.Format = "0.00";
                ws.DeleteRow(6);
                await package.SaveAsync();
                package.SaveAs(new FileInfo("Evaluation Form.xlsx"));
                btn_exportEval.IsEnabled = false;
                //TRYYY
                //convert the excel package to a byte array
                byte[] bin = package.GetAsByteArray();

                //create a SaveFileDialog instance with some properties
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel sheet";
                saveFileDialog1.Filter = "Excel files|*.xlsx|All files|*.*";
                saveFileDialog1.FileName = "Evaluation Form_" + cbx_evalDepartment.Text + "_"
                    + txt_currYear.Text + "_" + cbx_evalYearlevel.Text + "_"
                    + cbx_evalSemester.Text + "_"
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                //check if user clicked the save button
                if (saveFileDialog1.ShowDialog() == true)
                {
                    //write the file to the disk
                    addActivityExportEval();
                    File.WriteAllBytes(saveFileDialog1.FileName, bin);
                    cbx_evalDepartment.SelectedIndex = -1;
                    cbx_evalSemester.SelectedIndex = -1;
                    cbx_evalYearlevel.SelectedIndex = -1;
                    txt_currYear.Text = "";
                    Export_list.ItemsSource = null;
                    Export_list.Items.Clear();
                }
            }
        }

        //We well delete the file every time we call the package
        private static void deleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        //To display the filtered data to the datagrid
        public void PreviewExport()
        {
            Export_list.Items.Clear();
            if (cbx_evalDepartment.SelectedIndex > -1 && cbx_evalSemester.SelectedIndex > -1
                && cbx_evalYearlevel.SelectedIndex > -1 && txt_currYear.Text != "")
            {
                try
                {
                    string query1 = "SELECT `curr_ID`, `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req`, `curr_Semester`, `curr_Yearlevel` FROM `tbl_curriculum` where `curr_Batch` = "
                   + int.Parse(txt_currYear.Text) + " AND `curr_Yearlevel` = "
                   + int.Parse(cbx_evalYearlevel.Text) + "  AND `curr_Semester` = '"
                   + cbx_evalSemester.Text + "'  AND `curr_Department` = '"
                   + cbx_evalDepartment.Text + "'";
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
                            EvalForms output = new EvalForms
                            {
                                Subject_ID = reader.GetInt16(0),
                                Subject_Code = reader.GetString(1),
                                Subject_Title = reader.GetString(2),
                                Units = reader.GetInt16(3),
                                Pre_Req = reader.GetString(4),
                                Semester = reader.GetString(5),
                                Yearlevel = reader.GetInt16(6)
                            };
                            _ = Export_list.Items.Add(output);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        Export_list.Items.Clear();
                        _ = MessageBox.Show("No record found.");
                    }
                    databaseConnection.Close();
                }
                catch (Exception) { }
            }
        }

        private void btn_exportEval_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(async () => await ExportEval());
            if (Export_list.Items.IsEmpty == false && cbx_evalDepartment.SelectedIndex !=-1 
                && txt_currYear.Text != "" && cbx_evalSemester.SelectedIndex!=-1 && cbx_evalYearlevel.SelectedIndex != -1)
            {
                ExportEval();
            }
            else
            {
                MessageBox.Show("Please make sure that you have inputted necessary information. " +
                    "\nAlso, make sure to import curriculum first before creating an evaluation form.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_display_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_evalDepartment.SelectedIndex > -1 && cbx_evalSemester.SelectedIndex > -1 && cbx_evalYearlevel.SelectedIndex > -1 && txt_currYear.Text != "")
            {
                PreviewExport();
                btn_exportEval.IsEnabled = true;
            }
        }

        //add activity logs
        public void addActivityExportEval()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`,`log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID,  @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", int.Parse(txtUserID.Text));
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Export Evaluation Form");
            _ = commandDatabase2.Parameters.AddWithValue("@details", txtUserName.Text + " exported an evaluation form for batch "
                + int.Parse(txt_currYear.Text) + ", " + cbx_evalSemester.Text + " and year level" + int.Parse(cbx_evalYearlevel.Text) + ".");
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
    }
}
