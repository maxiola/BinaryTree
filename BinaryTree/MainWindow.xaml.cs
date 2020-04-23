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
using System.Diagnostics;

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
        Demo demo;
        Control control;
        Random rnd;
        Mode mode;
        int score, total;
        int step;
        int key;
        const int QCOUNT = 5;
        InputBox inputBox;        
        
        public MainWindow()
        {
            rnd = new Random();
            score = 0;
            total = 0;
            t = new Tree();
            demo = new Demo();
            demo.evRestart += new RestartEventHandler(tree_Restart);
            control = new Control();
            control.evRestart += new RestartEventHandler(tree_Restart);
            InitializeComponent();
            inputBox = new InputBox();
            inputBox.Show();
            if (!File.Exists("results.csv"))
            {
                StreamWriter sw = new StreamWriter("results.csv");
                sw.WriteLine("Дата и время,ФИО,Группа,Операция,Правильных ответов,Всего вопросов");
                sw.Close();
            }

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
        }
        private void HideAnswers()
        {
            ans1rButton.Visibility = Visibility.Hidden;
            ans2rButton.Visibility = Visibility.Hidden;
            ans3rButton.Visibility = Visibility.Hidden;
            ans4rButton.Visibility = Visibility.Hidden;
            labelQuestion.Visibility = Visibility.Hidden;
        }
        private void SearchAnswers(int key)
        {
            ans1rButton.Content = "Элемент со значением " + key + " найден";
            ans2rButton.Content = "Элемент со значением " + key + " не найден";
            ans3rButton.Content = "Искомое значение " + key + " меньше, чем текущее, необходимо искать в левом поддереве";
            ans4rButton.Content = "Искомое значение " + key + " больше, чем текущее, необходимо искать в правом поддереве";
        }
        private void InsertAnswers(int key)
        {
            ans1rButton.Content = "Значение добавляемого ключа " + key + " меньше или равно текущему и левая ветка свободна, добавляем элемент";
            ans2rButton.Content = "Значение добавляемого ключа " + key + " больше текущего и правая ветка свободна, добавляем элемент";
            ans3rButton.Content = "Значение добавляемого ключа " + key + " меньше или равно текущему, необходимо искать свободную ветку в левом поддереве";
            ans4rButton.Content = "Значение добавляемого ключа " + key + " больше текущего, необходимо искать свободную ветку в правом поддереве";
        }
        private void RemoveAnswers(int key)
        {
            ans1rButton.Content = "Удаляемый элемент " + key + " найден, у него нет потомков, элемент удаляется";
            ans2rButton.Content = "Удаляемый элемент " + key + " найден, его место займёт правый потомок ";
            ans3rButton.Content = "Удаляемый элемент " + key + " найден, его место займёт левый потомок ";
            ans4rButton.Content = "Удаляемый элемент " + key + " найден, его место займёт наименьший из потомков в правом поддереве";
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
        private void ResToFile(string op, int score, int total)
        {
            StreamWriter sw = new StreamWriter("results.csv", true);
            sw.WriteLine(DateTime.Now.ToString() + ","
                         + inputBox.Stud_name + ","
                         + inputBox.Group + ","
                         + op + ","
                         + score + ","
                         + total);
            sw.Close();
        }
        private void RemoveNextQuetion()
        {
            List<int> keys = t.GetKeys(new List<int>(), t.Root);
            int i = rnd.Next(0, keys.Count);
            textBox1.Text = keys[i].ToString();
            txtInfo.Text += step + ". УДАЛЕНИЕ КЛЮЧА " + keys[i] + '\n';
            RemoveAnswers(keys[i]);
            ShowAnswers();
            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите результат работы алгоритма:";
            total++;
        }
        private void RemoveCheckAnswer()
        {
            key = int.Parse(textBox1.Text);
            int checked_answer = CheckedAnswer();            
            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите результат работы алгоритма:";
            var correct_ans = control.Remove(key, t);
            Uncheck();
            if (checked_answer == correct_ans)
            {
                control.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                score++;
                txtInfo.Text += "Верно! Правильных ответов: " + score + "\n";
            }
            else
            {
                control.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
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
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string r = string.Empty;
            if (textBox1.Text != String.Empty)
            {
                step++;
                key = int.Parse(textBox1.Text);
                switch (mode)
                {
                    case Mode.L_Search:
                        if (step == 1)
                        {
                            txtInfo.Text += "ПОИСК КЛЮЧА " + key + '\n';
                        }
                        r = demo.Search(key, t);
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
                        r = demo.Insert(key, t);
                        if (r != "\n")
                        {
                            txtInfo.Text += step + ". " + r + '\n';
                        }
                        else
                        {
                            NewLine();
                        }
                        if (r.Contains("создаём"))
                        {
                            DrawNode(Color.FromArgb(255, 255, 255, 0),
                                     key,
                                     demo.CurrentNode.X,
                                     demo.CurrentNode.Y + 60,
                                     30);
                            return;
                        } 
                        break;

                    case Mode.L_Remove:
                        if (step == 1)
                        {
                            txtInfo.Text += "УДАЛЕНИЕ КЛЮЧА " + key + '\n';
                        }
                        r = demo.Remove(key, t);
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
                        {
                            int checked_answer = CheckedAnswer();
                            if (checked_answer == 0 && !control.Ready)
                            {
                                MessageBox.Show("Выберите вариант ответа");
                                step--;
                                return;
                            }
                            total++;
                            var correct_ans = control.Search(key, t);
                            SearchAnswers(key);
                            Uncheck();
                            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";
                            if (correct_ans == -1)
                            {
                                NewLine();
                                score = 0;
                                total = 0;
                            }
                            else if (checked_answer == correct_ans)
                            {
                                if (control.CurrentNode != null)
                                    control.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                score++;
                                txtInfo.Text += (step - 1) + ". Верно! Правильных ответов: " + score + "\n";
                            }
                            else
                            {
                                if (control.CurrentNode != null)
                                    control.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                txtInfo.Text += (step - 1) + ". Неверно!";
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
                                HideAnswers();
                                txtInfo.Text += "Правильных ответов: " + score + " из " + total + "\n";
                                ResToFile("Поиск", score, total);
                                total = 0;
                                score = 0;                                
                                NewLine();
                            }
                        }
                        break;

                    case Mode.C_Insert:                      
                        
                        {
                            int checked_answer = CheckedAnswer();
                            if (checked_answer == 0 && !control.Ready)
                            {
                                MessageBox.Show("Выберите вариант ответа");
                                step--;
                                return;
                            }
                            total++;
                            var correct_ans = control.Insert(key, t);
                            InsertAnswers(key);
                            Uncheck();
                            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";
                            if (correct_ans == -1)
                            {
                                NewLine();
                                score = 0;
                                total = 0;
                            }
                            else if (checked_answer == correct_ans)
                            {
                                if (control.CurrentNode != null)
                                    control.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                score++;
                                txtInfo.Text += (step - 1) + ". Верно! Правильных ответов: " + score + "\n";
                            }
                            else
                            {
                                if (control.CurrentNode != null)
                                    control.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                txtInfo.Text += (step - 1) + ". Неверно!";
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
                                HideAnswers();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
                                txtInfo.Text += "Правильных ответов: " + score + " из " + total + "\n";
                                ResToFile("Добавление", score, total);
                                total = 0;
                                score = 0;
                                NewLine();
                            }
                        }
                        break;

                    case Mode.C_Remove:
                        if (!control.Ready)
                        {
                            if (total < QCOUNT)
                            {
                                if (CheckedAnswer() == 0)
                                {
                                    MessageBox.Show("Выберите вариант ответа");
                                    step--;
                                    return;
                                }
                                RemoveCheckAnswer();
                                if (t.Root == null)
                                {
                                    MakeTree(rnd.Next(10, 13));
                                }
                                RemoveNextQuetion();
                            }
                            else
                            {
                                if (CheckedAnswer() == 0)
                                {
                                    MessageBox.Show("Выберите вариант ответа");
                                    step--;
                                    return;
                                }
                                RemoveCheckAnswer();
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
                                HideAnswers();
                                txtInfo.Text += "Правильных ответов: " + score + " из " + total + "\n";
                                ResToFile("Удаление", score, total);
                                total = 0;
                                score = 0;
                                NewLine();
                                control.Ready = true;
                            }
                        }
                        else
                        {
                            var res = MessageBox.Show("Хотите повторить удаление?", "Удаление", MessageBoxButton.YesNo);
                            if (res == MessageBoxResult.Yes)
                            {
                                control.CurrentNode = t.Root;
                                control.Ready = false;
                                t.Reset(t.Root);
                                tree_Restart(this);
                                step = 1;
                                RemoveNextQuetion();
                            }
                        }
                        break;
                }

                canvas1.Children.Clear();
                DrawTree(t.Root, canvas1.Width / 2, 20, 150, 60, 30, 0);
            }
        }
        private void DrawNode(Color c, int key, double x, double y, double size)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = c;
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.Width = size;
            myEllipse.Height = size;
            Canvas.SetLeft(myEllipse, x);
            Canvas.SetTop(myEllipse, y);

            Label label1 = new Label();
            label1.FontSize = 15;
            label1.Content = key;
            Canvas.SetLeft(label1, x);
            Canvas.SetTop(label1, y);

            canvas1.Children.Add(myEllipse);
            canvas1.Children.Add(label1);
        }
        private void DrawLine(double x, double y, double dx, double dy, double size, int lr)
        {
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
                    l.X2 = x - dx * 1.8 + size / 2;
                }
                else
                {
                    // left
                    l.X2 = x + dx * 1.8 + size / 2;
                }
                canvas1.Children.Add(l);
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
                DrawNode(n.Color, n.Key, x, y, size);
                DrawLine(x, y, dx, dy, size, lr);
                DrawTree(n.Left, x - dx, y + dy, dx / 1.8, dy, size, -1);
                DrawTree(n.Right, x + dx, y + dy, dx / 1.8, dy, size, 1);
            }
        }
        private void tree_Restart(object sender)
        {
            if (sender.GetType().ToString().Contains("Demo"))
            {
                textBox1.Clear();
                step = 0;
            }
            else if (sender.GetType().ToString().Contains("Control")) // поиск или добавление
            {
                key = rnd.Next(10, 100);
                textBox1.Text = key.ToString();
                if (mode == Mode.C_Search)
                {
                    txtInfo.Text += "ПОИСК КЛЮЧА " + key + '\n';
                    step = 1;
                    ShowAnswers();
                    SearchAnswers(key);
                    labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";
                }
                else if (mode == Mode.C_Insert)
                {
                    txtInfo.Text += "ДОБАВЛЕНИЕ КЛЮЧА " + key + '\n';
                    step = 1;
                    ShowAnswers();
                    InsertAnswers(key);
                    labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";
                }
                
            }
            else // контроль удаления
            {
                List<int> keys = t.GetKeys(new List<int>(), t.Root);
                int i = rnd.Next(0, keys.Count);
                textBox1.Text = keys[i].ToString();
            }          
        }
        private void MakeTree(int n)
        {
            t = new Tree();
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
        private void Search_Demo_Click(object sender, RoutedEventArgs e)
        {
            Search_Click();
            mode = Mode.L_Search;
            demo.CurrentNode = t.Root;
            demo.Ready = false;
        }
        private void Search_Control_Click(object sender, RoutedEventArgs e)
        {
            Search_Click();
            mode = Mode.C_Search;
            Random rnd = new Random();
            total = 0;
            score = 0;
            step = 1;
            control.CurrentNode = t.Root;
            control.Ready = false;
            key = rnd.Next(10, 100);
            textBox1.Text = key.ToString();
            txtInfo.Clear();
            txtInfo.Text += "ПОИСК КЛЮЧА " + key + "\n\n";            
            ShowAnswers();
            SearchAnswers(key);
            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";

        }
        private void Search_Click()
        {
            Uncheck();
            HideAnswers();
            step = 0;
            NewLine();
            textBox1.Clear();
            txtInfo.Clear();
            MakeTree(rnd.Next(8, 11));
        }
        private void Insert_Demo_Click(object sender, RoutedEventArgs e)
        {
            Insert_Click();
            mode = Mode.L_Insert;
            demo.CurrentNode = t.Root;
            demo.CurrentNode.X = canvas1.Width / 2;
            demo.CurrentNode.Y = 20;
            demo.CurrentNode.DX = 150;
            demo.Ready = false;
            demo.Created = false;
        }
        private void Insert_Control_Click(object sender, RoutedEventArgs e)
        {
            Insert_Click();
            mode = Mode.C_Insert;
            control.CurrentNode = t.Root;
            control.Ready = false;
            Random rnd = new Random();
            total = 0;
            score = 0;
            step = 1;
            key = rnd.Next(10, 100);
            textBox1.Text = key.ToString();
            txtInfo.Clear();
            txtInfo.Text += "ДОБАВЛЕНИЕ КЛЮЧА " + key + "\n\n";
            ShowAnswers();
            InsertAnswers(key);
            labelQuestion.Content = "Вопрос " + step + ". " + "Укажите следующий шаг алгоритма:";            
        }
        private void Insert_Click()
        {
            Uncheck();
            HideAnswers();
            step = 0;
            NewLine();
            textBox1.Clear();
            txtInfo.Clear();
            MakeTree(rnd.Next(4, 6));
        }
        private void Remove_Demo_Click(object sender, RoutedEventArgs e)
        {
            Remove_Click();
            mode = Mode.L_Remove;
            demo.CurrentNode = t.Root;
            demo.Ready = false;
            demo.Found = false;
            demo.Min_search = true;
            demo.Min_first = true;
        }
        private void Remove_Control_Click(object sender, RoutedEventArgs e)
        {
            Remove_Click();
            mode = Mode.C_Remove;
            control.CurrentNode = t.Root;
            control.Ready = false;
            control.Found = false;
            control.Min_search = true;
            control.Min_first = true;
            Random rnd = new Random();
            total = 0;
            score = 0;
            txtInfo.Clear();

            step++;
            RemoveNextQuetion();
        }
        private void Remove_Click()
        {
            Uncheck();
            HideAnswers();
            step = 0;
            NewLine();
            textBox1.Clear();
            txtInfo.Clear();
            MakeTree(rnd.Next(10, 13));
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inputBox.Ok = true;
            inputBox.Close();
        }
        private void txtInfo_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtInfo.ScrollToEnd();
        }
        private void Teacher_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            if (passwordWindow.ShowDialog() == true)
            {
                if (passwordWindow.Password == "p@$$w0rD")
                {
                    var res = new ResultsWindow();
                    res.Show();
                }
                else
                {
                    MessageBox.Show("Неверный пароль!");
                }
            }
        }
        private void ReadMe_Click(object sender, RoutedEventArgs e)
        {            
            Process.Start("chrome.exe", Environment.CurrentDirectory + "\\readme.html");
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработчик: ...\n(c) 2020", "О программе");
        }
    }
}

