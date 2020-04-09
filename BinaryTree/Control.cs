using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BinaryTree
{
    class Control
    {
        public event RestartEventHandler evRestart;
        private Node currentNode, previousNode;
        private bool ready, found, min_search, min_first;

        public Node CurrentNode { get { return currentNode; } set { currentNode = value; } }
        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public bool Ready { get { return ready; } set { ready = value; } }
        public bool Found { get { return found; } set { found = value; } }
        public bool Min_search { get { return min_search; } set { min_search = value; } }
        public bool Min_first { get { return min_first; } set { min_first = value; } }
        public Control()
        {
            ready = false;
        }
        public int Search(int key, Tree t)
        {
            if (!ready)
            {
                if (currentNode == null)
                {
                    ready = true;
                    return 2; //не найден
                }
                else if (currentNode.Key == key)
                {
                    ready = true;
                    return 1; //найден
                }
                else if (key > currentNode.Key)
                {
                    currentNode = currentNode.Right;
                    return 4; //ищем справа
                }
                else
                {
                    currentNode = currentNode.Left;
                    return 3; //ищем слева
                }
            }
            else
            {
                var res = MessageBox.Show("Хотите повторить поиск?", "Поиск", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = t.Root;
                    t.Reset(t.Root);
                    ready = false;
                    evRestart(this);
                }
                return -1;
            }
        }
        public int Insert(int key, Tree t)
        {
            if (!ready) // первичное добавление
            {
                if (currentNode == null)
                {
                    ready = true;
                    if (previousNode.Key >= key)
                    {
                        previousNode.Left = new Node(key);
                        return 1; // добавляем слева
                    }
                    else
                    {
                        previousNode.Right = new Node(key);
                        return 2; // добавляем справа
                    }
                }
                else if (currentNode.Key >= key)
                {
                    previousNode = currentNode;
                    currentNode = currentNode.Left;
                    return 3; // ищем слева
                }
                else
                {
                    previousNode = currentNode;
                    currentNode = currentNode.Right;
                    return 4; // ищем справа
                }
            }
            else // повторное добавление
            {
                var res = MessageBox.Show("Хотите добавить ещё один элемент?", "Добавление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = t.Root;
                    t.Reset(t.Root);
                    ready = false;
                    evRestart(this);
                }
                return -1;
            }
        }
        public int Remove(int key, Tree t)
        {
            int res = 0;
            currentNode = t.Search(t.Root, key);
            if (currentNode.Left == null && currentNode.Right == null)
            {
                res = 1;
            }
            else if (currentNode.Left == null)
            {
                res = 2;
            }
            else if (currentNode.Right == null)
            {
                res = 3;
            }
            else
            {
                res = 4;
            }
            t.Remove(t.Root, key);
            return res;
        }
    }
}
