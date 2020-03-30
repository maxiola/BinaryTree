using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BinaryTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    enum Mode
    {
        L_Search,
        L_Insert,
        L_Remove,
        C_Search,
        C_Insert,
        C_Remove
    }
    public partial class MainWindow : Window
    {
        Tree t;
        Random rnd;
        Mode mode;
        int score, total;
        int step;
        const int QCOUNT = 5;
        InputBox inputBox;
        public MainWindow()
        {
            rnd = new Random();
            score = 0;
            total = 0;
            t = new Tree();
            InitializeComponent();
            inputBox = new InputBox();
            inputBox.Show();            
        }
        private int CheckedAnswer()
        {
            if (ans1rButton.IsChecked == true) return 1;
            if (ans2rButton.IsChecked == true) return 2;
            if (ans3rButton.IsChecked == true) return 3;
            if (ans4rButton.IsChecked == true) return 4;
            return 0;
        }
        private void ShowAnswers()
        {
            ans1rButton.Visibility = Visibility.Visible;
            ans2rButton.Visibility = Visibility.Visible;
            ans3rButton.Visibility = Visibility.Visible;
            ans4rButton.Visibility = Visibility.Visible;
            labelQuestion.Visibility = Visibility.Visible;
            labelQuestion.Content = "Выберите правильный ответ";
        }
        private void HideAnswers()
        {
            ans1rButton.Visibility = Visibility.Hidden;
            ans2rButton.Visibility = Visibility.Hidden;
            ans3rButton.Visibility = Visibility.Hidden;
            ans4rButton.Visibility = Visibility.Hidden;
            labelQuestion.Visibility = Visibility.Hidden;
        }
        private void SearchAnswers()
        {
            ans1rButton.Content = "Элемент найден";
            ans2rButton.Content = "Элемент не найден";
            ans3rButton.Content = "Ищем элемент слева";
            ans4rButton.Content = "Ищем элемент справа";
        }
        private void InsertAnswers()
        {
            ans1rButton.Content = "Добавляем элемент слева";
            ans2rButton.Content = "Добавляем элемент справа";
            ans3rButton.Content = "Ищем элемент слева";
            ans4rButton.Content = "Ищем элемент справа";
        }
        private void RemoveAnswers()
        {
            ans1rButton.Content = "У элемента нет потомков, он просто удаляется.";
            ans2rButton.Content = "Элемент замещается правым потомком.";
            ans3rButton.Content = "Элемент замещается левым потомком.";
            ans4rButton.Content = "Элемент замещается мин. потомком справа.";
        }
        private void Uncheck()
        {
            ans1rButton.IsChecked = false;
            ans2rButton.IsChecked = false;
            ans3rButton.IsChecked = false;
            ans4rButton.IsChecked = false;
        }
        private void NewLine()
        {
            if (txtInfo.Text != string.Empty)
            {
                string last = txtInfo.Text.Substring(txtInfo.Text.Length - 2, 2);
                if (last != "\n\n")
                {
                    txtInfo.Text += "\n";
                }
            }
        }
        private void ResToFile(int score, int total)
        {
            StreamWriter sw = new StreamWriter("results.csv", true);
            sw.WriteLine(DateTime.Now.ToString() + ","
                         + inputBox.Stud_name + ","
                         + inputBox.Group + ","
                         + score + ","
                         + total);
            sw.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string r = string.Empty;
            if (textBox1.Text != String.Empty)
            {                
                step++;
                int key = int.Parse(textBox1.Text);
                learningRButton.IsEnabled = false;
                controlRButton.IsEnabled = false;
                switch (mode)
                {                    
                    case Mode.L_Search:
                        if (step == 1)
                        {
                            txtInfo.Text += "ПОИСК КЛЮЧА " + key + '\n';
                        }
                        r = t.SearchOne(key);
                        if (r != "\n")
                        {
                            txtInfo.Text += step + ". " + r + '\n';
                        }
                        else
                        {
                            NewLine();
                        }
                        break;

                    case Mode.L_Insert:
                        if (step == 1)
                        {
                            txtInfo.Text += "ДОБАВЛЕНИЕ КЛЮЧА " + key + '\n';
                        }
                        r = t.InsertOne(key);
                        if (r != "\n")
                        {
                            txtInfo.Text += step + ". " + r + '\n';
                        }
                        else
                        {
                            NewLine();
                        }
                        break;

                    case Mode.L_Remove:
                        if (step == 1)
                        {
                            txtInfo.Text += "УДАЛЕНИЕ КЛЮЧА " + key + '\n';
                        }
                        r = t.RemoveOne(key);
                        if (r != "\n")
                        {
                            txtInfo.Text += step + ". " + r + '\n';
                        }
                        else
                        {
                            NewLine();
                        }
                        break;

                    case Mode.C_Search:
                        if (step == 1)
                        {
                            txtInfo.Text += "ПОИСК КЛЮЧА " + key + '\n';
                        }
                        if (ans1rButton.Visibility == Visibility.Hidden)
                        {
                            ShowAnswers();
                            SearchAnswers();                            
                        }
                        else
                        {                            
                            if (CheckedAnswer() == 0)
                            {
                                MessageBox.Show("Выберите вариант ответа");
                                return;
                            }
                            total++;
                            var correct_ans = t.SearchOneControl(key);
                            if (correct_ans == -1)
                            {
                                Uncheck();
                                NewLine();
                                score = 0;
                                total = 0;
                            }
                            else if (CheckedAnswer() == correct_ans)
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                txtInfo.Text += "Верно!\n";
                                score++;
                            }
                            else
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                txtInfo.Text += "Неверно!";
                                switch (correct_ans)
                                {
                                    case 1:
                                        txtInfo.Text += " Элемент уже найден.\n";
                                        break;

                                    case 2:
                                        txtInfo.Text += " Такого элемента нет.\n";
                                        break;

                                    case 3:
                                        txtInfo.Text += " Нужно искать слева.\n";
                                        break;

                                    case 4:
                                        txtInfo.Text += " Нужно искать справа.\n";
                                        break;
                                }
                            }
                            if (correct_ans == 1 || correct_ans == 2)
                            {
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
                                MessageBox.Show("Правильных ответов: " + score + " из " + total);
                                ResToFile(score, total);
                                total = 0;
                                score = 0;
                                NewLine();
                            }
                        }
                        break;

                    case Mode.C_Insert:
                        if (step == 1)
                        {
                            txtInfo.Text += "ДОБАВЛЕНИЕ КЛЮЧА " + key + '\n';
                        }
                        if (ans1rButton.Visibility == Visibility.Hidden)
                        {
                            ShowAnswers();
                            InsertAnswers();
                        }
                        else
                        {
                            if (CheckedAnswer() == 0)
                            {
                                MessageBox.Show("Выберите вариант ответа");
                                return;
                            }
                            total++;
                            var correct_ans = t.InsertOneControl(key);
                            if (correct_ans == -1)
                            {
                                Uncheck();
                                NewLine();
                                score = 0;
                                total = 0;
                            }
                            else if (CheckedAnswer() == correct_ans)
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                txtInfo.Text += "Верно!\n";
                                score++;
                            }
                            else
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                txtInfo.Text += "Неверно!";
                                switch (correct_ans)
                                {
                                    case 1:
                                        txtInfo.Text += " Нужно добавить элемент слева.\n";
                                        break;

                                    case 2:
                                        txtInfo.Text += " Нужно добавить элемент справа.\n";
                                        break;

                                    case 3:
                                        txtInfo.Text += " Нужно искать слева.\n";
                                        break;

                                    case 4:
                                        txtInfo.Text += " Нужно искать справа.\n";
                                        break;
                                }
                            }
                            if (correct_ans == 1 || correct_ans == 2)
                            {
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
                                MessageBox.Show("Правильных ответов: " + score + " из " + total);
                                ResToFile(score, total);
                                total = 0;
                                score = 0;
                                NewLine();
                            }
                        }
                        break;

                    case Mode.C_Remove:
                        if (step == 1)
                        {
                            txtInfo.Text += "УДАЛЕНИЕ КЛЮЧА " + key + '\n';
                        }
                        if (ans1rButton.Visibility == Visibility.Hidden)
                        {
                            ShowAnswers();
                            RemoveAnswers();
                        }
                        else
                        {
                            if (CheckedAnswer() == 0)
                            {
                                MessageBox.Show("Выберите вариант ответа");
                                return;
                            }
                            if (!t.Ready)
                            {
                                if (total < QCOUNT)
                                {
                                    total++;
                                    var correct_ans = t.RemoveOneControl(key);
                                    if (CheckedAnswer() == correct_ans)
                                    {
                                        t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                        txtInfo.Text += "Верно!\n";
                                        score++;
                                    }
                                    else
                                    {
                                        t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                        txtInfo.Text += "Неверно!";
                                        switch (correct_ans)
                                        {
                                            case 1:
                                                txtInfo.Text += " У элемента нет потомков, он просто удаляется.\n";
                                                break;

                                            case 2:
                                                txtInfo.Text += " Элемент замещается правым потомком.\n";
                                                break;

                                            case 3:
                                                txtInfo.Text += " Элемент замещается левым потомком.\n";
                                                break;

                                            case 4:
                                                txtInfo.Text += " Элемент замещается минимальным потомком справа.\n";
                                                break;
                                        }
                                    }
                                    if (total < QCOUNT)
                                    {
                                        List<int> keys = t.GetKeys(new List<int>(), t.Root);
                                        int i = rnd.Next(0, keys.Count);
                                        textBox1.Text = keys[i].ToString();
                                        //t.Restart(t.Root);
                                    }
                                    
                                }
                                else
                                {
                                    canvas1.Children.Clear();
                                    DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
                                    MessageBox.Show("Правильных ответов: " + score + " из " + total);
                                    ResToFile(score, total);
                                    total = 0;
                                    score = 0;
                                    NewLine();
                                    t.Ready = true;
                                }
                            }
                            else
                            {
                                var res = MessageBox.Show("Хотите добавить ещё один элемент?", "Добавление", MessageBoxButton.YesNo);
                                if (res == MessageBoxResult.Yes)
                                {                                    
                                    t.CurrentNode = t.Root;
                                    t.Restart(t.Root);
                                    t.Ready = false;
                                    tree_Restart();
                                }                                
                            }
                            break;

                        }
                        break;
                }
                canvas1.Children.Clear();
                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);

            }
        }
        private void DrawTree(Node n, double x, double y, double dx, double dy, double size, int lr)
        {
            if (n == null)
            {
                return;
            }
            else
            {
                Ellipse myEllipse = new Ellipse();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = n.Color;
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = 2;
                myEllipse.Stroke = Brushes.Black;
                myEllipse.Width = size;
                myEllipse.Height = size;
                Canvas.SetLeft(myEllipse, x);
                Canvas.SetTop(myEllipse, y);

                Label label1 = new Label();
                label1.FontSize = 15; //!
                label1.Content = n.Key;
                Canvas.SetLeft(label1, x);
                Canvas.SetTop(label1, y);

                if (lr != 0)
                {
                    Line l = new Line();
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 2;
                    l.X1 = x + size / 2;
                    l.Y1 = y;
                    l.Y2 = y - dy + size;
                    if (lr > 0)
                    {
                        //right                 
                        l.X2 = x - dx * 1.5 + size / 2;
                    }
                    else
                    {
                        // left
                        l.X2 = x + dx * 1.5 + size / 2;
                    }
                    canvas1.Children.Add(l);
                }

                canvas1.Children.Add(myEllipse);
                canvas1.Children.Add(label1);

                DrawTree(n.Left, x - dx, y + dy, dx / 1.5, dy, size, -1);
                DrawTree(n.Right, x + dx, y + dy, dx / 1.5, dy, size, 1);
            }
        }
        private void tree_Restart()
        {
            textBox1.Clear();
            txtInfo.Text += "\n";
            step = 0;
            learningRButton.IsEnabled = true;
            controlRButton.IsEnabled = true;
            learningRButton.IsChecked = true;
            HideAnswers();
        }
        private void MakeTree(int n)
        {
            t = new Tree();
            //keys = new List<int>(n);            
            t.evRestart += new RestartEventHandler(tree_Restart);
            for (int i = 0; i < n; i++)
            {
                if (t.Root == null)
                {
                    int r = rnd.Next(50, 61);
                    t.Insert(t.Root, r, 0);
                }
                else
                {
                    int r = rnd.Next(10, 100);
                    while (t.Root != null && t.Search(t.Root, r) != null)
                    {
                        r = rnd.Next(10, 100);
                    }
                    t.Insert(t.Root, r, 0);
                }
                //keys.Add(r);
            }
            canvas1.Children.Clear();
            DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Uncheck();
            HideAnswers();
            controlRButton.IsEnabled = true;
            learningRButton.IsEnabled = true;
            learningRButton.IsChecked = true;

            step = 0;
            NewLine();
            textBox1.Clear();
            MakeTree(rnd.Next(8, 11));
            if (learningRButton.IsChecked == true)
                mode = Mode.L_Search;
            else if (controlRButton.IsChecked == true)
                mode = Mode.C_Search;
            t.CurrentNode = t.Root;
            t.Ready = false;
        }
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            Uncheck();
            HideAnswers();
            controlRButton.IsEnabled = true;
            learningRButton.IsEnabled = true;
            learningRButton.IsChecked = true;

            step = 0;
            NewLine();
            textBox1.Clear();
            MakeTree(rnd.Next(4, 6));
            if (learningRButton.IsChecked == true)
                mode = Mode.L_Insert;
            else if (controlRButton.IsChecked == true)
                mode = Mode.C_Insert;
            t.CurrentNode = t.Root;
            t.Ready = false;
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Uncheck();
            HideAnswers();
            controlRButton.IsEnabled = true;
            learningRButton.IsEnabled = true;
            learningRButton.IsChecked = true;

            step = 0;
            NewLine();
            textBox1.Clear();
            MakeTree(rnd.Next(10, 13));
            if (learningRButton.IsChecked == true)
                mode = Mode.L_Remove;
            else if (controlRButton.IsChecked == true)
                mode = Mode.C_Remove;
            t.CurrentNode = t.Root;
            t.Ready = false;
            t.Found = false;
            t.Min_search = true;
            t.Min_first = true;
        }
        private void learningRButton_Checked(object sender, RoutedEventArgs e)
        {
            if (mode == Mode.C_Insert) mode = Mode.L_Insert;
            if (mode == Mode.C_Remove) mode = Mode.L_Remove;
            if (mode == Mode.C_Search) mode = Mode.L_Search;
            step = 0;
        }
        private void controlRButton_Checked(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            total = 0;
            score = 0;
            step = 0;
            if (mode == Mode.L_Search)
            {
                mode = Mode.C_Search;
                textBox1.Text = rnd.Next(10, 100).ToString();
            }
            if (mode == Mode.L_Insert)
            {
                mode = Mode.C_Insert;
                textBox1.Text = rnd.Next(10, 100).ToString();
            }
            if (mode == Mode.L_Remove)
            {
                mode = Mode.C_Remove;
                List<int> keys = t.GetKeys(new List<int>(), t.Root);
                int i = rnd.Next(0, keys.Count);
                textBox1.Text = keys[i].ToString();
            }            

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inputBox.Ok = true;
            inputBox.Close();
        }
        private void Teacher_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

