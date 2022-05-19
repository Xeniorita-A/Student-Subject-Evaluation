using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        }
        public class EvalForms
        {
            public string? Subject_Code { get; set; }
            public string? Subject_Title { get; set; }
            public int Units { get; set; }
            public string? Pre_Req { get; set; }
            public string? Final_Grade { get; set; }
            public string? Remarks { get; set; }
        }
        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out and exit application?", "EXIT",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private async Task ExportEval()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(fileName: @"C:\Users\XenioritaAlondra\source\repos\Student Subject Evaluation\Evaluation Forms\EvaluationForm.xlsx");

            //var eval = GetSetUpData();

            await saveExcelFile( file);
        }

        //method for the save excel file
        //the problem is how we can put the data on the excel from datagrid :((
        private async Task saveExcelFile( FileInfo file)
        {
            deleteIfExists(file);

            using (var package = new ExcelPackage(file))
            {
                //data table
                string query1 = "SELECT `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req` FROM `tbl_curriculum` where `curr_Batch` LIKE '"
               + txt_currYear.Text + "%' AND `curr_Yearlevel` LIKE '"
               + cbx_evalYearlevel.Text + "%'  AND `curr_Semester`  LIKE '"
               + cbx_evalSemester.Text + "%'  AND `curr_Department`  LIKE '"
               + cbx_evalDepartment.Text + "%' ";
                
                DataTable dt = new DataTable("EvalForms");
                try
                {
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    databaseConnection.Open();
                    MySqlCommand commandDatabase = new MySqlCommand(query1, databaseConnection);
                    commandDatabase.ExecuteNonQuery();
                    MySqlDataAdapter returnVal = new MySqlDataAdapter(query1, databaseConnection);

                    returnVal = new MySqlDataAdapter(commandDatabase);
                    returnVal.Fill(dt);

                    MySqlCommandBuilder _cb = new MySqlCommandBuilder(returnVal);
                    databaseConnection.Close();
                }
                catch
                {
                    MessageBox.Show("No record found in the database");
                }
                
                //WS is the worksheet
                var ws = package.Workbook.Worksheets.Add(Name: "Evaluation_Form");
                //add the data
                var range = ws.Cells[Address: "A6"].LoadFromDataTable(dt, PrintHeaders: true);
                range.AutoFitColumns();

                //formats the header
                ws.Cells[Address: "A1"].Value = "EVALUATION FORM";
                ws.Cells[Address: "A1:F1"].Merge = true;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(row: 1).Style.Font.Size = 20;
                ws.Row(row: 1).Style.Font.Bold = true;
                ws.Row(row: 2).Style.Font.Bold = true;
                ws.Row(row: 3).Style.Font.Bold = true;
                ws.Row(row: 5).Style.Font.Bold = true;
                ws.Row(row: 1).Style.Font.Color.SetColor(Color.Orange);

                //Name, student number, and the Course year and section
                ws.Cells[Address: "A2"].Value = "Name:";
                ws.Cells[Address: "A3"].Value = "Student Number:";
                ws.Cells[Address: "C2"].Value = "Curriculum Year:";
                ws.Cells[Address: "C3"].Value = "Department:";
                ws.Cells[Address: "A5"].Value = "Subject Code";
                ws.Cells[Address: "B5"].Value = "Subject Title";
                ws.Cells[Address: "C5"].Value = "Units";
                ws.Cells[Address: "D5"].Value = "Pre Requisite/s";
                ws.Cells[Address: "E5"].Value = "Final Grade";
                ws.Cells[Address: "F5"].Value = "Remarks";
                ws.Row(row: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Row(row: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //format other thing
                ws.Column(col: 1).Width = 15;
                ws.Column(col: 2).Width = 50;
                ws.Column(col: 3).Width = 15;
                ws.Column(col: 4).Width = 30;
                ws.Column(col: 5).Width = 13;
                ws.Column(col: 6).Width = 13;
                ws.Column(col: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[Address: "C2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[Address: "C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
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
                saveFileDialog1.FileName = "Evaluation Form_" + cbx_evalDepartment.Text +"_"
                    + txt_currYear.Text + "_" + cbx_evalYearlevel.Text + "_"
                    + cbx_evalSemester.Text +"_"
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                //check if user clicked the save button
                if (saveFileDialog1.ShowDialog() == true)
                {
                    //write the file to the disk
                    File.WriteAllBytes(saveFileDialog1.FileName, bin);
                    Export_list.Items.Clear();
                    cbx_evalDepartment.SelectedIndex = -1;
                    cbx_evalSemester.SelectedIndex = -1;
                    cbx_evalYearlevel.SelectedIndex = -1;
                    txt_currYear.Text = "";
                }
            }
        }
        

        private static void deleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public void PreviewExport()
        {
            Export_list.Items.Clear();
            if (cbx_evalDepartment.SelectedIndex != -1 && cbx_evalSemester.SelectedIndex != -1 && cbx_evalYearlevel.SelectedIndex != -1 && txt_currYear.Text != "")
            {
                string query1 = "Select * From `tbl_curriculum` where `curr_Batch` LIKE '"
               + txt_currYear.Text + "%' AND `curr_Yearlevel` LIKE '"
               + cbx_evalYearlevel.Text + "%'  AND `curr_Semester`  LIKE '"
               + cbx_evalSemester.Text + "%'  AND `curr_Department`  LIKE '"
               + cbx_evalDepartment.Text + "%' ";
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
                        if (reader.GetString(4) == "" || reader.GetString(4) == null)
                        {
                            EvalForms output = new EvalForms
                            {
                                Subject_Code = reader.GetString(1),
                                Subject_Title = reader.GetString(2),
                                Units = reader.GetInt16(3),
                                Pre_Req = "None"
                            };
                            Export_list.Items.Add(output);
                        }
                        else
                        {
                            EvalForms output = new EvalForms
                            {
                                Subject_Code = reader.GetString(1),
                                Subject_Title = reader.GetString(2),
                                Units = reader.GetInt16(3),
                                Pre_Req = reader.GetString(4)
                            };
                            Export_list.Items.Add(output);
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    Export_list.Items.Clear();
                    MessageBox.Show("No record found.");
                }
                
                databaseConnection.Close();
                
            }
        }

        //i need to convert the data on datagrid to datatable

               
        private void btn_exportEval_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(async () => await ExportEval());
            ExportEval();
        }

        private void btn_display_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_evalDepartment.SelectedIndex != -1 && cbx_evalSemester.SelectedIndex != -1 && cbx_evalYearlevel.SelectedIndex != 1 && txt_currYear.Text != "")
            {
                PreviewExport();
                btn_exportEval.IsEnabled = true;
            }
        }
    }
}
