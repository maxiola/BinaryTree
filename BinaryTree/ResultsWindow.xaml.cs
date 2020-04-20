using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BinaryTree
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow()
        {
            InitializeComponent();
            ShowResults();
        }

        public void ShowResults()
        {
            DataTable dt = new DataTable();            
            StreamReader sr = new StreamReader("results.csv");
            var headers = sr.ReadLine().Split(',');
            foreach (var header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                var s = sr.ReadLine().Split(',');
                dt.Rows.Add(s);            
            }            
            dataGrid.DataContext = dt.DefaultView;
            sr.Close();
        }
    }
}
