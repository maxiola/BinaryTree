using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Логика взаимодействия для InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        private string stud_name, group;
        private bool ok;
        public string Stud_name { get {return stud_name; } set {stud_name = value;} }
        public string Group { get { return group; } set { group = value; } }
        public bool Ok { get { return ok; } set { ok = value; } }
        public InputBox()
        {
            InitializeComponent();
            ok = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ok)
            {
                e.Cancel = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text == string.Empty || textBoxGroup.Text == string.Empty)
            {
                MessageBox.Show("Необходимо заполнить все поля");
            }
            else
            {
                stud_name = textBoxName.Text;
                group = textBoxGroup.Text;
                ok = true;
                Close();
            }
        }
    }
}
