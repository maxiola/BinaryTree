﻿using System;
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
        List<int> keys;
        int score, total, remove_questions;
        public MainWindow()
        {            
            rnd = new Random();
            score = 0;
            total = 0;
            remove_questions = 5;
            t = new Tree();
            InitializeComponent();
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
            ans4rButton.Content = "Элемент замещается минимальным потомком справа.";
        }
        private void Uncheck()
        {
            ans1rButton.IsChecked = false;
            ans2rButton.IsChecked = false;
            ans3rButton.IsChecked = false;
            ans4rButton.IsChecked = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                int key = int.Parse(textBox1.Text);
                learningRButton.IsEnabled = false;
                controlRButton.IsEnabled = false;
                switch (mode)
                {
                    case Mode.L_Search:                        
                        labelInfo.Content = t.SearchOne(key);
                        break;

                    case Mode.L_Insert:                        
                        labelInfo.Content = t.InsertOne(key);
                        break;

                    case Mode.L_Remove:                      
                        labelInfo.Content = t.RemoveOne(key);
                        break;

                    case Mode.C_Search:                        
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
                                labelInfo.Content = string.Empty;
                                score = 0;
                                total = 0;
                            }
                            else if (CheckedAnswer() == correct_ans)
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                labelInfo.Content = "Верно!";
                                score++;
                            }
                            else 
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                labelInfo.Content = "Неверно!";
                                switch (correct_ans)
                                {
                                    case 1:
                                        labelInfo.Content += " Элемент уже найден.";
                                        break;

                                    case 2:
                                        labelInfo.Content += " Такого элемента нет.";
                                        break;

                                    case 3:
                                        labelInfo.Content += " Нужно искать слева.";
                                        break;

                                    case 4:
                                        labelInfo.Content += " Нужно искать справа.";
                                        break;
                                }
                            }
                            if (correct_ans == 1 || correct_ans == 2)
                            {
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 200, 80, 50, 0);
                                MessageBox.Show("Правильных ответов: " + score + " из " + total);
                            }                            
                        }
                        break;

                    case Mode.C_Insert:
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
                                labelInfo.Content = string.Empty;
                                score = 0;
                                total = 0;
                            }
                            else if (CheckedAnswer() == correct_ans)
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                labelInfo.Content = "Верно!";
                                score++;
                            }
                            else
                            {
                                t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                labelInfo.Content = "Неверно!";
                                switch (correct_ans)
                                {
                                    case 1:
                                        labelInfo.Content += " Нужно добавить элемент слева.";
                                        break;

                                    case 2:
                                        labelInfo.Content += " Нужно добавить элемент справа.";
                                        break;

                                    case 3:
                                        labelInfo.Content += " Нужно искать слева.";
                                        break;

                                    case 4:
                                        labelInfo.Content += " Нужно искать справа.";
                                        break;
                                }
                            }
                            if (correct_ans == 1 || correct_ans == 2)
                            {
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 200, 80, 50, 0);
                                MessageBox.Show("Правильных ответов: " + score + " из " + total);
                            }
                        }
                        break;

                    case Mode.C_Remove:
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
                            if (remove_questions > 0)
                            {
                                int i = rnd.Next(0, keys.Count);
                                textBox1.Text = keys[i].ToString();
                                total++;
                                remove_questions--;
                                var correct_ans = t.RemoveOneControl(key);
                                if (correct_ans == -1)
                                {
                                    Uncheck();
                                    labelInfo.Content = string.Empty;
                                    score = 0;
                                    total = 0;
                                }
                                else if (CheckedAnswer() == correct_ans)
                                {
                                    t.CurrentNode.Color = Color.FromArgb(255, 0, 255, 0);
                                    labelInfo.Content = "Верно!";
                                    score++;
                                }
                                else
                                {
                                    t.CurrentNode.Color = Color.FromArgb(255, 255, 0, 0);
                                    labelInfo.Content = "Неверно!";
                                    switch (correct_ans)
                                    {
                                        case 1:
                                            labelInfo.Content += " У элемента нет потомков, он просто удаляется.";
                                            break;

                                        case 2:
                                            labelInfo.Content += " Элемент замещается правым потомком.";
                                            break;

                                        case 3:
                                            labelInfo.Content += " Элемент замещается левым потомком.";
                                            break;

                                        case 4:
                                            labelInfo.Content += " Элемент замещается минимальным потомком справа.";
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                canvas1.Children.Clear();
                                DrawTree(t.Root, canvas1.Width / 2, 20, 200, 80, 50, 0);
                                MessageBox.Show("Правильных ответов: " + score + " из " + total);
                            }
                        }
                        break;

                }
                canvas1.Children.Clear();
                DrawTree(t.Root, canvas1.Width / 2, 20, 200, 80, 50, 0);
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
                label1.FontSize = 30;
                label1.Content = n.Key;
                Canvas.SetLeft(label1, x);
                Canvas.SetTop(label1, y);

                if (lr != 0)
                {
                    Line l = new Line();
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 2;
                    l.X1 = x + 25;
                    l.Y1 = y;
                    l.Y2 = y - dy + size;
                    if (lr > 0)
                    {
                        //right                 
                        l.X2 = x - dx*1.5 + size / 2;
                    }
                    else
                    {
                        // left
                        l.X2 = x + dx*1.5 + size / 2;
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
            learningRButton.IsEnabled = true;
            controlRButton.IsEnabled = true;
            learningRButton.IsChecked = true;
            HideAnswers();
        }
        private void MakeTree(int n)
        {
            t = new Tree();
            keys = new List<int>(n);            
            t.evRestart += new RestartEventHandler(tree_Restart);
            for (int i = 0; i < n; i++)
            {
                int r = rnd.Next(10, 100);
                while (t.Root != null && t.Search(t.Root, r) != null)
                {
                    r = rnd.Next(10, 100);
                }
                t.Insert(t.Root, r, 0);
                keys.Add(r);
            }
            canvas1.Children.Clear();
            DrawTree(t.Root, canvas1.Width / 2, 20, 200, 80, 50, 0);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Uncheck();
            HideAnswers();
            controlRButton.IsEnabled = true;
            learningRButton.IsEnabled = true;
            learningRButton.IsChecked = true;
            
            labelInfo.Content = "";
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
            
            labelInfo.Content = "";
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
            
            labelInfo.Content = "";
            textBox1.Clear();
            MakeTree(rnd.Next(12, 15));
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
        }
        private void controlRButton_Checked(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
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
                int i = rnd.Next(0, keys.Count);
                textBox1.Text = keys[i].ToString();

            }

        }
    }
}