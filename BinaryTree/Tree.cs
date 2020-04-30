using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.IO;
using System.Windows;

namespace BinaryTree
{
    public delegate void RestartEventHandler(object sender); // делегат события Restart
    enum Side // перечисляемый тип для ветвей дерева (левой и правой)
    {
        Left,
        Right
    }
    public static class TreeConst // класс констант
    {
        public const int DY = 60; // расстояние между узлами дерева по вертикали
        public const double CoeffDX = 1.8; // коэффициент уменьшения расстояния между узлами по горизонтали
        public const int QCOUNT = 5; // кол-во вопросов в режиме контроля для удаления
    }
    class Node // класс элемента (узла) дерева
    {
        private int key; // значение ключа
        private double x, y, dx; // координаты (для рисования)
        private Node left, right; // ссылки на левого и правого потомков
        private Node parent; // ссылка на родителя
        private Color color; // цвет
        private Side side; // расположение узла (слева или справа) относительно родителя

        // свойства
        public int Key { get { return key; } set { key = value; } }
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double DX { get { return dx; } set { dx = value; } }
        public Node Left { get { return left; } set { left = value; } }
        public Node Right { get { return right; } set { right = value; } }
        public Node Parent { get { return parent; } set { parent = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public Side Side { get { return side; } set { side = value; } }
        public Node(int key) // конструктор класса
        {
            this.key = key;
            this.left = null;
            this.right = null;
            this.color = Color.FromArgb(255, 255, 255, 0);
        }
        public void ReplaceParentsChild(Node n) // изменение ссылки у родительского элемента
        {
            if (this.Parent != null)
            {
                if (this.Side == Side.Left)
                    this.Parent.Left = n;
                else
                    this.Parent.Right = n;
            }
        }
    }
    class Tree
    {
        private Node root; // ссылка на корень
        public Node Root { get { return root; } set { root = value; } }
        public Tree() // конструктор класса
        {
            root = null;            
        }
        public List<int> GetKeys(List<int> keys, Node root) // возвращает список всех ключей дерева (рекурсивно)
        {
            if (root != null)
            {
                keys.Add(root.Key);
                GetKeys(keys, root.Right);
                GetKeys(keys, root.Left);
            }
            return keys;
        }
        public void Reset(Node n) // заменить цвет всех элементов дерева на жёлтый (рекурсивно)
        {
            if (n != null)
            {
                n.Color = Color.FromArgb(255, 255, 255, 0);
                Reset(n.Left);
                Reset(n.Right);
            }
        } 
        public Node Search(Node root, int key) // поиск элемента по ключу (рекурсивно)
        {
            if (root.Key == key) // элемент найден
            {
                return root;
            }
            else
            {
                if (key > root.Key) // искомое значение больше текущего
                {
                    if (root.Right == null) // не найдено
                        return null;
                    else
                        return Search(root.Right, key); // ищем справа
                }
                else // искомое значение меньше или равно текущему
                {
                    if (root.Left == null) // не найдено
                        return null;
                    else
                        return Search(root.Left, key); // ищем слева
                }
            }
        }        
        public void Insert(Node root, int key) // добавление элемента (рекурсивно)
        {
            if (root == null) // дерево пустое
            {
                this.root = new Node(key);
                this.root.Parent = null;
            }
            else
            {
                if (root.Key >= key)  // добавляемое значение меньше или равно текущему
                {
                    if (root.Left == null) // добавляем слева
                    {
                        root.Left = new Node(key);
                        root.Left.Parent = root;
                        root.Left.Side = Side.Left;

                    }
                    else
                    {
                        Insert(root.Left, key); // ищем слева
                    }
                }
                else // добавляемое значение больше текущего
                {
                    if (root.Right == null) // добавляем справа
                    {                        
                        root.Right = new Node(key);
                        root.Right.Parent = root;
                        root.Right.Side = Side.Right;

                    }
                    else
                    {
                        Insert(root.Right, key); // ищем справа
                    }
                }
            }            
        }
        public Node Remove(Node n, int key) // удаление элемента (рекурсивно)
        {
            if (root.Key == key) // удаление корня
            {
                if (root.Left == null && root.Right == null) // нет потомков
                    root = null;
                else if (root.Left == null) // только правый
                {
                    root = root.Right;
                }
                else if (root.Right == null) // только левый
                {
                    root = root.Left;
                }
                else // есть оба потомка
                {
                    int k = Minimum(root.Right).Key; // находим минимального потомка                    
                    root.Right = Remove(root.Right, k); // удаляем этого потомка
                    root.Key = k;
                }
            }
            if (n == null) // дошли до крайнего элемента, который будем удалять
                return n;
            if (key < n.Key) // удаляемое значение меньше текущего, ищем его слева
                n.Left = Remove(n.Left, key);
            else if (key > n.Key) // удаляемое значение больше текущего, ищем его справа
                n.Right = Remove(n.Right, key);
            else if (n.Left != null && n.Right != null) // нашли удаляемый, у него есть оба потомка
            {
                n.Key = Minimum(n.Right).Key; // находим минимального потомка
                n.Right = Remove(n.Right, n.Key); // удаляем этого потомка
            }
            else if (n.Left != null) // есть только левый потомок, он встаёт на место удаляемого
                n = n.Left;
            else if (n.Right != null) // есть только правый потомок, он встаёт на место удаляемого
                n = n.Right;
            else // нет потомков, просто удаляем элемент
                n = null;
            return n;

        }
        public Node Minimum(Node n) // поиск наименьшего потомка (рекурсивно)
        {
            if (n.Left == null) return n;
            else return Minimum(n.Left);
        }    
    }
}
