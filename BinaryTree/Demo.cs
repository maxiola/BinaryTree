﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BinaryTree
{
    class Demo // режим демонстрации
    {
        public event RestartEventHandler evRestart; // событие перезапуска алгоритма
        private Node currentNode, minNode; // ссылки на текущий элемент и на минимальный элемент
        private bool ready, found, min_search, min_first, created; // флаги

        // свойства
        public Node CurrentNode { get { return currentNode; } set { currentNode = value; } }
        public bool Ready { get { return ready; } set { ready = value; } }
        public bool Found { get { return found; } set { found = value; } }
        public bool Min_search { get { return min_search; } set { min_search = value; } }
        public bool Min_first { get { return min_first; } set { min_first = value; } }
        public bool Created { get { return created; } set { created = value; } }
        public Demo() // конструктор класса
        {
            ready = false;
            created = false;
        }

        // методы поиска, добавления и удаления
        // возвращают текстовый комментарий после каждого шага алгоритма
        public string Search(int key, Tree t) // пошаговый поиск элемента 
        {            
            if (!ready) // алгоритм ещё не завершился
            {
                if (currentNode == null)
                {
                    ready = true;
                    return "Элемент со значением " + key + " не найден";
                }
                else if (currentNode.Key == key)
                {
                    currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                    ready = true;
                    return "Элемент со значением " + key + " найден";
                }
                else if (key > currentNode.Key)
                {
                    string res = "Искомое значение " + key + " больше, чем " + currentNode.Key + ", необходимо искать в правом поддереве";
                    currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                    currentNode = currentNode.Right;
                    return res;
                }
                else
                {
                    string res = "Искомое значение " + key + " меньше, чем " + currentNode.Key + ", необходимо искать в левом поддереве";
                    currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                    currentNode = currentNode.Left;
                    return res;
                }
            }
            else // алгоритм завершился, возможен перезапуск по запросу пользователя
            {
                var res = MessageBox.Show("Хотите повторить поиск?", "Поиск", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = t.Root;
                    t.Reset(t.Root);
                    ready = false;
                    evRestart(this);
                }
                return "\n";
            }
        }
        public string Insert(int key, Tree t) // пошаговое добавление элемента
        {
            if (!ready) // алгоритм ещё не завершился
            {
                if (currentNode.Key >= key) // добавляемое значение меньше или равно текущему
                {
                    if (currentNode.Left == null) // добавялем слева
                    {
                        if (!created) // создание элемента (без связи)
                        {
                            string res = "Значение добавляемого ключа " + key + " меньше или равно " + currentNode.Key + " и левая ветка свободна, создаём элемент";
                            currentNode.X -= currentNode.DX;
                            created = true;
                            return res;
                        }
                        else // связывание элемента с родителем
                        {
                            currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                            currentNode.Left = new Node(key);
                            currentNode.Left.Color = Color.FromArgb(255, 0, 255, 0);
                            string res = "Значение добавляемого ключа " + key + " меньше или равно " + currentNode.Key + " и левая ветка свободна, добавляем элемент";
                            ready = true;
                            return res;
                        }
                    }
                    else // ищем слева
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        string res = "Значение добавляемого ключа " + key + " меньше или равно " + currentNode.Key + ", необходимо искать свободную ветку в левом поддереве";
                        currentNode.Left.DX = currentNode.DX / TreeConst.CoeffDX;
                        currentNode.Left.X = currentNode.X - currentNode.DX;
                        currentNode.Left.Y = currentNode.Y + TreeConst.DY;
                        currentNode = currentNode.Left;
                        return res;
                    }
                }
                else // добавляемое значение больше текущего
                {
                    if (currentNode.Right == null) // добавляем справа
                    {
                        if (!created) // создание элемента (без связи)
                        {
                            string res = "Значение добавляемого ключа " + key + " больше " + currentNode.Key + " и правая ветка свободна, создаём элемент";
                            currentNode.X += currentNode.DX;
                            created = true;
                            return res;
                        }
                        else // связывание элемента с родителем
                        {
                            string res = "Элемент со значением " + key + " становится правым потомком элемента со значением " + currentNode.Key;
                            currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                            currentNode.Right = new Node(key);
                            currentNode.Right.Color = Color.FromArgb(255, 0, 255, 0);
                            ready = true;
                            return res;
                        }
                    }
                    else // ищем справа
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        string res = "Значение добавляемого ключа " + key + " больше " + currentNode.Key + ", необходимо искать свободную ветку в правом поддереве";
                        currentNode.Right.DX = currentNode.DX / TreeConst.CoeffDX;
                        currentNode.Right.X = currentNode.X + currentNode.DX;
                        currentNode.Right.Y = currentNode.Y + TreeConst.DY;
                        currentNode = currentNode.Right;
                        return res;
                    }
                }
            }
            else // алгоритм завершился, возможен перезапуск
            {
                var res = MessageBox.Show("Хотите добавить ещё один элемент?", "Добавление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = t.Root;
                    t.Reset(t.Root);
                    ready = false;
                    created = false;
                    evRestart(this);
                }
                return "\n";
            }
        }
        public string Remove(int key, Tree t) // пошаговое удаление элемента
        {
            if (!ready) // алгоритм ещё не завершился
            {
                if (t.Root.Key == key) // удаление корня
                {
                    if (t.Root.Left == null && t.Root.Right == null) // нет потомков
                        t.Root = null;
                    else if (t.Root.Left == null) // только правый
                    {
                        t.Root = t.Root.Right;
                        t.Root.Right = null;
                        return "Удаление корневого элемента " + key + ", он замещается правым потомком " + t.Root.Key;
                    }
                    else if (t.Root.Right == null) // только левый
                    {
                        t.Root = t.Root.Left;
                        t.Root.Left = null;
                        return "Удаление корневого элемента " + key + ", он замещается левым потомком " + t.Root.Key;
                    }
                    else // есть оба потомка
                    {
                        t.Root.Color = Color.FromArgb(255, 0, 0, 255);
                        if (min_search) // ещё не найден замещающий элемент
                        {
                            if (min_first) // начало поиска минимального
                            {
                                minNode = t.Root.Right; // ищем в правом поддереве
                                minNode.Color = Color.FromArgb(255, 255, 0, 0);
                                min_first = false; // больше сюда не заходим
                            }
                            return Minimum();
                        }
                        else // найден замещающий элемент
                        {
                            int k = t.Minimum(t.Root.Right).Key; // находим минимального потомка
                            string res = "Удаляемый элемент " + key + " найден, его место займёт наименьший из потомков в правом поддереве " + k;
                            t.Root.Color = Color.FromArgb(255, 0, 255, 0);
                            t.Root.Right = t.Remove(t.Root.Right, k); // удаляем этого потомка
                            t.Root.Key = k;
                            ready = true;
                            return res;
                        }
                    }
                }
                if (currentNode == null)
                {
                    ready = true;
                    return "Элемент с ключом " + key + " не найден";
                }
                if (key < currentNode.Key)
                {
                    currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                    string res = "Удаляемый элемент " + key + " меньше, чем " + currentNode.Key + ", необходмо искать в левом поддереве";
                    currentNode = currentNode.Left;
                    return res;
                }
                else if (key > currentNode.Key)
                {
                    currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                    string res = "Удаляемый элемент " + key + " больше, чем " + currentNode.Key + ", необходмо искать в правом поддереве";
                    currentNode = currentNode.Right;
                    return res;
                }
                else if (currentNode.Left != null && currentNode.Right != null) // нашли удаляемый, у него есть оба потомка
                {
                    currentNode.Color = Color.FromArgb(255, 0, 0, 255);
                    if (min_search) // ещё не найден замещающий элемент
                    {
                        if (min_first) // начало поиска минимального
                        {
                            minNode = currentNode.Right; // ищем в правом поддереве
                            minNode.Color = Color.FromArgb(255, 255, 0, 0);
                            min_first = false; // больше сюда не заходим
                        }
                        return Minimum();
                    }
                    else // замещающий элемент найден
                    {
                        int k = t.Minimum(currentNode.Right).Key; // находим минимального потомка
                        string res = "Удаляемый элемент " + key + " найден, его место займёт наименьший из потомков в правом поддереве " + k;
                        currentNode.Key = k;
                        currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                        currentNode.Right = t.Remove(currentNode.Right, currentNode.Key); // удаляем этого потомка
                        ready = true;
                        return res;
                    }
                }
                else if (currentNode.Left != null) // есть только левый потомок, он встаёт на место удаляемого
                {
                    if (!found) // выделение цветом удаляемого элемента и его родителя
                    {
                        string res = "Удаляемый элемент " + key + " найден, его место займёт левый потомок " + currentNode.Left.Key;
                        currentNode.Color = Color.FromArgb(255, 0, 0, 255);
                        currentNode.Left.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else // удаление элемента
                    {
                        string res = "Элемент " + key + " удалён";
                        currentNode.ReplaceParentsChild(currentNode.Left);
                        currentNode = currentNode.Left;
                        ready = true;
                        return res;
                    }
                }
                else if (currentNode.Right != null) // есть только правый потомок, он встаёт на место удаляемого
                {
                    if (!found) // выделение цветом удаляемого элемента и его родителя
                    {
                        string res = "Удаляемый элемент " + key + " найден, его место займёт правый потомок " + currentNode.Right.Key;
                        currentNode.Color = Color.FromArgb(255, 0, 0, 255);
                        currentNode.Right.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else // удаление элемента
                    {
                        string res = "Элемент " + key + " удалён";
                        currentNode.ReplaceParentsChild(currentNode.Right);
                        currentNode = currentNode.Right;
                        ready = true;
                        return res;
                    }
                }
                else // нет потомков, просто удаляем элемент
                {
                    if (!found) // выделение цветом удаляемого элемента
                    {
                        string res = "Удаляемый элемент " + key + " найден, у него нет потомков";
                        currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else // удаление элемента
                    {
                        string res = "Элемент " + key + " удалён";
                        currentNode.ReplaceParentsChild(null);
                        ready = true;
                        found = false;
                        return res;
                    }
                }
            }
            else // алгоритм завершился, возможен перезапуск
            {
                var res = MessageBox.Show("Хотите удалить ещё один элемент?", "Удаление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = t.Root;
                    t.Reset(t.Root);
                    ready = false;
                    min_first = true;
                    min_search = true;
                    found = false;
                    evRestart(this);
                }
                return "\n";
            }
        }
        private string Minimum() // пошаговый поиск минимального элемента
        {
            if (minNode.Left == null)
            {
                minNode.Color = Color.FromArgb(255, 0, 255, 0);
                string res = "Найден подходящий элемент со значением " + minNode.Key;
                min_search = false;
                return res;
            }
            else
            {
                string res = "У элемента " + minNode.Key + " есть левый потомок, продолжаем поиск";
                minNode.Left.Color = Color.FromArgb(255, 255, 0, 0);
                minNode = minNode.Left;
                return res;
            }
        }
    }
}
