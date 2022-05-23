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

        }

        //Ths is the method for the search field in the Student List tab
        private void TextSearchStuds_Changed(object sender, TextChangedEventArgs e)
        {

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
                    g.Subject_Title.ToString() + " " + g.Units.ToString() + " " + g.Pre_Req.ToString()+
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


        private void btn_saveStudGrade_click(object sender, RoutedEventArgs e)
        {

        }

        private void refresh_StudentList(object sender, RoutedEventArgs e)
        {

        }
    }
}
