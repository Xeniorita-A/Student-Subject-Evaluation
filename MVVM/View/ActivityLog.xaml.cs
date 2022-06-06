using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Student_Subject_Evaluation.MVVM.View
{
    /// <summary>
    /// Interaction logic for ActivityLog.xaml
    /// </summary>
    public partial class ActivityLog : UserControl
    {
        public class ActivityLogList
        {
            public string? ActivityID { get; set; }
            public string? ActivityTime { get; set; }
            public string? ActivityDate { get; set; }
            public string? Activity { get; set; }
            public string? ActivityDetail { get; set; }
        }

        //Open a connection
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
        public ActivityLog()
        {
            InitializeComponent();
            showActivityLog();
            _ = new System.Globalization.CultureInfo("en-us");
            System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "yyyy-MM-dd";
        }
        public class DateFixerConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DateTime.Parse((string)value).ToString("yyyy-MM-dd");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DateTime.Parse((string)value);
            }
        }

        public void showActivityLog()
        {
            Activity_log.Items.Clear();
            DateTo.Text = "";
            DateFrom.Text = "";
            //Query so we can load the data into our datagrid
            String query = "Select * From `tbl_activitylog`";
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
                    ActivityLogList _activity = new ActivityLogList
                    {
                        ActivityID = reader.GetString(0),
                        ActivityTime = Convert.ToDateTime(reader.GetString(1)).ToString("H:mm"),
                        ActivityDate = Convert.ToDateTime(reader.GetString(2)).ToString("yyyy-MM-dd"),
                        Activity = reader.GetString(4),
                        ActivityDetail = reader.GetString(5)
                    };
                    _ = Activity_log.Items.Add(_activity);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            //close the connection
            databaseConnection.Close();
        }

        public void AddActivityExportedLogs()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`,`log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", MainWindow.MWinstance.AccountID);
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Exported Activity Log");
            _ = commandDatabase2.Parameters.AddWithValue("@details", MainWindow.MWinstance.AccountName + " exported a copy of activity log in excel(.xlsx) format.");
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

        public void AddActivityExportedFilteredLogs()
        {
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`, `log_Time`,`log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", MainWindow.MWinstance.AccountID);
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Exported Activity Log");
            _ = commandDatabase2.Parameters.AddWithValue("@details", MainWindow.MWinstance.AccountName +
                " exported a copy of activity log from " + DateTime.Parse(DateFrom.Text).ToString("yyyy-MM-dd") +
                " to " + DateTime.Parse(DateTo.Text).ToString("yyyy-MM-dd") + ".");
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
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_commission;";
            string query = "INSERT INTO  `tbl_activitylog` ( `log_ID`,`log_Time`, `log_Date`, `log_UserID`, `log_Activity`, `log_Detail`)  VALUES (@ID, @time, @date, @user, @activity, @details)";
            MySqlConnection databaseConnection2 = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection2);
            _ = commandDatabase2.Parameters.AddWithValue("@ID", 0);
            _ = commandDatabase2.Parameters.AddWithValue("@time", DateTime.Now.ToString("H:mm"));
            _ = commandDatabase2.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            _ = commandDatabase2.Parameters.AddWithValue("@user", MainWindow.MWinstance.AccountID);
            _ = commandDatabase2.Parameters.AddWithValue("@activity", "Logout");
            _ = commandDatabase2.Parameters.AddWithValue("@details", MainWindow.MWinstance.AccountName + " logout of the system.");
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

        private void btn_ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (DateTo.Text != "" && DateFrom.Text != "")
            {
                FilterLogs();
            }
            else
            {
                _ = MessageBox.Show("Please make sure you have pick a date range before clicking apply.");
            }
        }

        private void btn_exportLogs_Click(object sender, RoutedEventArgs e)
        {
            _ = ExportReports();
        }

        private async Task ExportReports()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //This where we will save the template
            FileInfo? rep = new FileInfo(fileName: @"C:\Student Subject Evaluation System\Activity Logs\Activity_Log.xlsx");
            if (DateTo.Text != "" && DateFrom.Text != "" && Activity_log.Items.IsEmpty == false)
            {
                await exportFiltered(rep);
            }
            else if ((DateTo.Text == "" && DateFrom.Text != "") || (DateTo.Text != "" && DateFrom.Text == ""))
            {
                _ = MessageBox.Show("If you wish to export activity log in custom date range, please make sure to pick dates.", "Info");
                showActivityLog();
            }
            else if (DateTo.Text == "" && DateFrom.Text == "")
            {
                await exportAllLog(rep);
            }
            else if (Activity_log.Items.IsEmpty == true)
            {
                showActivityLog();
            }

        }

        public void FilterLogs()
        {
            try
            {
                Activity_log.Items.Clear();
                string q2 = "SELECT * FROM tbl_activitylog WHERE `log_Date` BETWEEN '" +
                    DateTime.Parse(DateFrom.Text).ToString("yyyy-MM-dd") + "' AND '" +
                    DateTime.Parse(DateTo.Text).ToString("yyyy-MM-dd") + "'";
                MySqlConnection dbc = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase1 = new MySqlCommand(q2, dbc);
                commandDatabase1.CommandTimeout = 60;
                dbc.Open();
                MySqlDataReader reader = commandDatabase1.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityLogList _activity = new ActivityLogList
                        {
                            ActivityID = reader.GetString(0),
                            ActivityTime = Convert.ToDateTime(reader.GetString(1)).ToString("H:mm"),
                            ActivityDate = Convert.ToDateTime(reader.GetString(2)).ToString("yyyy-MM-dd"),
                            Activity = reader.GetString(4),
                            ActivityDetail = reader.GetString(5)
                        };
                        _ = Activity_log.Items.Add(_activity);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
            }
            catch (Exception)
            {
                showActivityLog();
            }
        }

        private static void deleteIfExists(FileInfo rep)
        {
            if (rep.Exists)
            {
                rep.Delete();
            }
        }
        private async Task exportFiltered(FileInfo file)
        {
            deleteIfExists(file);

            using (ExcelPackage? package = new ExcelPackage(file))
            {
                string q2 = "SELECT `log_ID`, `log_Time`, `log_Date`, `log_Activity`, `log_Detail` FROM `tbl_activitylog`" +
                    "WHERE `log_Date` BETWEEN '" +
                    DateTime.Parse(DateFrom.Text).ToString("yyyy-MM-dd") + "' AND '" +
                    DateTime.Parse(DateTo.Text).ToString("yyyy-MM-dd") + "'";
                MySqlConnection dbc = new MySqlConnection(connectionString);
                DataTable dt = new DataTable("EvalForms");
                try
                {
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    databaseConnection.Open();
                    MySqlCommand commandDatabase = new MySqlCommand(q2, databaseConnection);
                    _ = commandDatabase.ExecuteNonQuery();
                    MySqlDataAdapter returnVal = new MySqlDataAdapter(q2, databaseConnection);
                    //I passed the data from mySql to Data table
                    //this is so we can put the data from data table to excel
                    returnVal = new MySqlDataAdapter(commandDatabase);
                    _ = returnVal.Fill(dt);

                    MySqlCommandBuilder _cb = new MySqlCommandBuilder(returnVal);
                    databaseConnection.Close();
                }
                catch (Exception)
                {
                }
                //WS is the worksheet
                ExcelWorksheet? ws = package.Workbook.Worksheets.Add(Name: "Activity_Log");
                //add the data

                ExcelRangeBase? range = ws.Cells[Address: "A3"].LoadFromDataTable(dt, PrintHeaders: true);
                range.AutoFitColumns();

                //formats the header
                ws.Cells[Address: "A1"].Value = "ACTIVITY LOG";
                ws.Cells[Address: "A1:E1"].Merge = true;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Row(row: 1).Style.Font.Size = 20;
                ws.Row(row: 1).Style.Font.Bold = true;
                ws.Row(row: 2).Style.Font.Bold = true;
                ws.Row(row: 1).Style.Font.Color.SetColor(System.Drawing.Color.DarkOrange);

                //Name, student number, and the Course year and section
                ws.Cells[Address: "A2"].Value = "ID";
                ws.Cells[Address: "B2"].Value = "TIME";
                ws.Cells[Address: "C2"].Value = "DATE";
                ws.Cells[Address: "D2"].Value = "ACTIVITY";
                ws.Cells[Address: "E2"].Value = "DETAILS";
                ws.Row(row: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Row(row: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //format other thing
                ws.Column(col: 1).Width = 5;
                ws.Column(col: 2).Width = 8;
                ws.Column(col: 3).Width = 13;
                ws.Column(col: 4).Width = 20;
                ws.Column(col: 5).Width = 80;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 2).Style.Numberformat.Format = "h:MM";
                ws.Column(col: 3).Style.Numberformat.Format = "yyyy-MM-dd";
                ws.DeleteRow(3);
                await package.SaveAsync();
                package.SaveAs(new FileInfo("Evaluation Form.xlsx"));
                //TRYYY
                //convert the excel package to a byte array
                byte[] bin = package.GetAsByteArray();
                //create a SaveFileDialog instance with some properties
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel sheet";
                saveFileDialog1.Filter = "Excel files|*.xlsx|All files|*.*";
                saveFileDialog1.FileName = "Activity Log_From "
                    + DateTime.Parse(DateFrom.Text).ToString("yyyy-MM-dd") + " To "
                    + DateTime.Parse(DateTo.Text).ToString("yyyy-MM-dd") + ".xlsx";

                //check if user clicked the save button
                if (saveFileDialog1.ShowDialog() == true)
                {
                    //write the file to the disk
                    File.WriteAllBytes(saveFileDialog1.FileName, bin);
                    AddActivityExportedFilteredLogs();
                    showActivityLog();
                    DateFrom.Text = "";
                    DateTo.Text = "";
                }
            }
        }
        private async Task exportAllLog(FileInfo file)
        {
            deleteIfExists(file);

            using (ExcelPackage? package = new ExcelPackage(file))
            {
                string q2 = "SELECT `log_ID`, `log_Time`, `log_Date`, `log_Activity`, `log_Detail` FROM `tbl_activitylog`";
                DataTable dt = new DataTable("EvalForms");
                try
                {
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    databaseConnection.Open();
                    MySqlCommand commandDatabase = new MySqlCommand(q2, databaseConnection);
                    _ = commandDatabase.ExecuteNonQuery();
                    MySqlDataAdapter returnVal = new MySqlDataAdapter(q2, databaseConnection);
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
                //WS is the worksheet
                ExcelWorksheet? ws = package.Workbook.Worksheets.Add(Name: "Activity_Log");
                //add the data

                ExcelRangeBase? range = ws.Cells[Address: "A3"].LoadFromDataTable(dt, PrintHeaders: true);
                range.AutoFitColumns();

                //formats the header
                ws.Cells[Address: "A1"].Value = "ACTIVITY LOG";
                ws.Cells[Address: "A1:E1"].Merge = true;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Row(row: 1).Style.Font.Size = 20;
                ws.Row(row: 1).Style.Font.Bold = true;
                ws.Row(row: 2).Style.Font.Bold = true;
                ws.Row(row: 1).Style.Font.Color.SetColor(System.Drawing.Color.DarkOrange);

                //Name, student number, and the Course year and section
                ws.Cells[Address: "A2"].Value = "ID";
                ws.Cells[Address: "B2"].Value = "TIME";
                ws.Cells[Address: "C2"].Value = "DATE";
                ws.Cells[Address: "D2"].Value = "ACTIVITY";
                ws.Cells[Address: "E2"].Value = "DETAILS";
                ws.Row(row: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Row(row: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //format other thing
                ws.Column(col: 1).Width = 5;
                ws.Column(col: 2).Width = 8;
                ws.Column(col: 3).Width = 13;
                ws.Column(col: 4).Width = 20;
                ws.Column(col: 5).Width = 80;
                ws.Column(col: 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(col: 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Column(col: 2).Style.Numberformat.Format = "h:MM";
                ws.Column(col: 3).Style.Numberformat.Format = "yyyy-MM-dd";
                ws.DeleteRow(3);
                await package.SaveAsync();
                package.SaveAs(new FileInfo("Evaluation Form.xlsx"));
                //TRYYY
                //convert the excel package to a byte array
                byte[] bin = package.GetAsByteArray();

                //create a SaveFileDialog instance with some properties
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel sheet";
                saveFileDialog1.Filter = "Excel files|*.xlsx|All files|*.*";
                saveFileDialog1.FileName = "Activity Log_"
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                //check if user clicked the save button
                if (saveFileDialog1.ShowDialog() == true)
                {
                    //write the file to the disk
                    File.WriteAllBytes(saveFileDialog1.FileName, bin);
                    AddActivityExportedLogs();
                    showActivityLog();
                    DateFrom.Text = "";
                    DateTo.Text = "";
                }
            }
        }

        private void btn_refreshLog_Click(object sender, RoutedEventArgs e)
        {
            showActivityLog();
            DateFrom.Text = "";
            DateTo.Text = "";
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
            HelpModule w = new HelpModule();
            w.Content = new HelpPage();
            w.Show();
        }
    }
}
