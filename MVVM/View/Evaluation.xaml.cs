using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
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
        }

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

        private void btn_exportEval_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_evalDepartment.SelectedIndex != 0 && cbx_evalSemester.SelectedIndex != 0 
                && cbx_evalYearlevel.SelectedIndex !=0 && txt_currYear.Text != "")
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var file = new FileInfo(@"C:\Users\XenioritaAlondra\source\repos\Student Subject Evaluation\EvaluationForm.xlsx");

                var eval = GetSetUpData();
            }
        }

        //test lang muna
        static List<EvalForms> GetSetUpData()
        {
            List<EvalForms> output = new()
            {
                new() { code = "GE2", subjectTitle = "Reading in Philippine History", units = 3, prereq = "", finalGrade = "", remarks = "" },
                new() { code = "GE4", subjectTitle = "Mathematics in the Modern World", units = 3, prereq = "", finalGrade = "", remarks = "" }
            };
            return output;

        }
    }
}
