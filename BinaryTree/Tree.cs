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
    public delegate void RestartEventHandler();
    enum Side
    {
        Left,
        Right
    }
    class Node
    {
        // поля класса
        private int key;
        private Node left, right;
        private Node parent;
        private Color color;
        private Side side;

        // свойства
        public int Key { get { return key; } set { key = value; } }
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
        public void ReplaceParentsChild(Node n)
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
        private Node root;
        public Node Root { get { return root; } set { root = value; } }
        public Tree()
        {
            root = null;            
        }
        public List<int> GetKeys(List<int> keys, Node root)
        {
            if (root != null)
            {
                keys.Add(root.Key);
                GetKeys(keys, root.Right);
                GetKeys(keys, root.Left);
            }
            return keys;
        }
        public void Reset(Node n)
        {
            if (n != null)
            {
                n.Color = Color.FromArgb(255, 255, 255, 0);
                Reset(n.Left);
                Reset(n.Right);
            }
        }
        public Node Search(Node root, int key)
        {
            if (root.Key == key)
            {
                return root;
            }
            else
            {
                if (key > root.Key)
                {
                    if (root.Right == null)
                        return null;
                    else
                        return Search(root.Right, key);
                }
                else
                {
                    if (root.Left == null)
                        return null;
                    else
                        return Search(root.Left, key);
                }
            }
        }
        public int Insert(Node root, int key, int level)
        {
            if (root == null)
            {
                this.root = new Node(key);
                this.root.Parent = null;
            }
            else
            {
                if (root.Key >= key)
                {
                    if (root.Left == null)
                    {
                        level++;
                        root.Left = new Node(key);
                        root.Left.Parent = root;
                        root.Left.Side = Side.Left;

                    }
                    else
                    {
                        level = Insert(root.Left, key, level + 1);
                    }
                }
                else
                {
                    if (root.Right == null)
                    {
                        level++;
                        root.Right = new Node(key);
                        root.Right.Parent = root;
                        root.Right.Side = Side.Right;

                    }
                    else
                    {
                        level = Insert(root.Right, key, level + 1);
                    }
                }
            }
            return level;
        }
        public Node Remove(Node n, int key) 
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
        public Node Minimum(Node n)
        {
            if (n.Left == null) return n;
            else return Minimum(n.Left);
        }    
    }
}
