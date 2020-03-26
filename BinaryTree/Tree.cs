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
        private Node currentNode, minNode;
        private bool ready, found, min_search, min_first;
        public event RestartEventHandler evRestart;
        public Node Root { get { return root; } set { root = value; } }
        public Node CurrentNode { get { return currentNode; } set { currentNode = value; } }
        public bool Ready { get { return ready; } set { ready = value; } }
        public bool Found { get { return found; } set { found = value; } }
        public bool Min_search { get { return min_search; } set { min_search = value; } }
        public bool Min_first { get { return min_first; } set { min_first = value; } }
        public Tree()
        {
            root = null;
            ready = false;
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
        public void Restart(Node n)
        {
            if (n != null)
            {
                n.Color = Color.FromArgb(255, 255, 255, 0);
                Restart(n.Left);
                Restart(n.Right);
            }
        }
        public string InsertOne(int key)
        {
            if (!ready) // первичное добавление
            {
                if (currentNode.Key >= key)
                {
                    if (currentNode.Left == null)
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        currentNode.Left = new Node(key);
                        currentNode.Left.Color = Color.FromArgb(255, 0, 255, 0);
                        string res = "Значение добавляемого ключа " + key + " меньше " + currentNode.Key + " и левая ветка свободна, добавляем элемент";
                        ready = true;
                        return res;
                    }
                    else
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        string res = "Значение добавляемого ключа " + key + " меньше " + currentNode.Key + ", необходимо искать свободную ветку в левом поддереве";
                        currentNode = currentNode.Left;
                        return res;
                    }
                }
                else
                {
                    if (currentNode.Right == null)
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        currentNode.Right = new Node(key);
                        currentNode.Right.Color = Color.FromArgb(255, 0, 255, 0);
                        string res = "Значение добавляемого ключа " + key + " больше " + currentNode.Key + " и правая ветка свободна, добавляем элемент";
                        ready = true;
                        return res;
                    }
                    else
                    {
                        currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                        string res = "Значение добавляемого ключа " + key + " больше " + currentNode.Key + ", необходимо искать свободную ветку в правом поддереве";
                        currentNode = currentNode.Right;
                        return res;
                    }
                }
            }
            else // повторное добавление
            {
                var res = MessageBox.Show("Хотите добавить ещё один элемент?", "Добавление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = root;
                    Restart(root);
                    ready = false;
                    // отправить событие текстбоксу на очистку!
                    evRestart();
                }
                return "\n";
            }
        }
        public int InsertOneControl(int key)
        {
            if (!ready) // первичное добавление
            {
                if (currentNode.Key >= key)
                {
                    if (currentNode.Left == null)
                    {
                        ready = true;
                        currentNode.Left = new Node(key);
                        return 1; // добавляем слева
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                        return 3; // ищем слева
                    }
                }
                else
                {
                    if (currentNode.Right == null)
                    {
                        ready = true;
                        currentNode.Right = new Node(key);
                        return 2; // добавляем справа
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                        return 4; // ищем справа
                    }
                }
            }
            else // повторное добавление
            {
                var res = MessageBox.Show("Хотите добавить ещё один элемент?", "Добавление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = root;
                    Restart(root);
                    ready = false;
                    evRestart();
                }
                return -1;
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
        public string SearchOne(int key)
        {
            if (!ready)
            {
                if (currentNode.Key == key)
                {
                    currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                    ready = true;
                    return "Элемент со значением " + key + " найден";
                }
                else
                {
                    if (key > currentNode.Key)
                    {
                        if (currentNode.Right == null)
                        {
                            ready = true;
                            return "Элемент со значением " + key + " не найден";
                        }
                        else
                        {
                            string res = "Искомое значение " + key + " больше, чем " + currentNode.Key + ", необходимо искать в правом поддереве";
                            currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                            currentNode = currentNode.Right;
                            return res;
                        }
                    }
                    else
                    {
                        if (currentNode.Left == null)
                        {
                            ready = true;
                            return "Элемент со значением " + key + " не найден";
                        }
                        else
                        {
                            string res = "Искомое значение " + key + " меньше, чем " + currentNode.Key + ", необходимо искать в левом поддереве";
                            currentNode.Color = Color.FromArgb(255, 255, 0, 0);
                            currentNode = currentNode.Left;
                            return res;
                        }
                    }
                }
            }
            else
            {
                var res = MessageBox.Show("Хотите повторить поиск?", "Поиск", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = root;
                    Restart(root);
                    ready = false;
                    evRestart();
                }
                return "\n";
            }
        }
        public int SearchOneControl(int key)
        {
            if (!ready)
            {
                if (currentNode.Key == key)
                {
                    ready = true;
                    return 1; //найден
                }
                else
                {
                    if (key > currentNode.Key)
                    {
                        if (currentNode.Right == null)
                        {
                            ready = true;
                            return 2; //не найден
                        }
                        else
                        {
                            currentNode = currentNode.Right;
                            return 4; //ищем справа
                        }
                    }
                    else
                    {
                        if (currentNode.Left == null)
                        {
                            ready = true;
                            return 2; //не найден
                        }
                        else
                        {
                            currentNode = currentNode.Left;
                            return 3; //ищем слева
                        }
                    }
                }
            }
            else
            {
                var res = MessageBox.Show("Хотите повторить поиск?", "Поиск", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = root;
                    Restart(root);
                    ready = false;
                    evRestart();
                }
                return -1;
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
        public string RemoveOne(int key)
        {
            if (!ready)
            {
                if (root.Key == key) // удаление корня
                {
                    if (root.Left == null && root.Right == null) // нет потомков
                        root = null;
                    else if (root.Left == null) // только правый
                    {
                        root = root.Right;
                        root.Right = null;
                        return "Удаление корневого элемента " + key + ", он замещается правым потомком " + root.Key;
                    }
                    else if (root.Right == null) // только левый
                    {
                        root = root.Left;
                        root.Left = null;
                        return "Удаление корневого элемента " + key + ", он замещается левым потомком " + root.Key;
                    }
                    else // есть оба потомка
                    {
                        //int k = Minimum(root.Right).Key; // находим минимального потомка                    
                        //root.Right = Remove(root.Right, k); // удаляем этого потомка
                        //root.Key = k;
                        root.Color = Color.FromArgb(255, 0, 0, 255);
                        if (min_search) // ещё не найден замещающий элемент
                        {
                            if (min_first) // начало поиска минимального
                            {
                                minNode = root.Right; // ищем в правом поддереве
                                minNode.Color = Color.FromArgb(255, 255, 0, 0);
                                min_first = false; // больше сюда не заходим
                            }
                            return MinimumOne();
                        }
                        else
                        {
                            int k = Minimum(root.Right).Key; // находим минимального потомка
                            string res = "Удаляемый элемент " + key + " найден, его место займёт наименьший из потомков в правом поддереве " + k;
                            root.Color = Color.FromArgb(255, 0, 255, 0);
                            root.Right = Remove(root.Right, k); // удаляем этого потомка
                            root.Key = k;
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
                        return MinimumOne();
                    }
                    else
                    {
                        int k = Minimum(currentNode.Right).Key; // находим минимального потомка
                        string res = "Удаляемый элемент " + key + " найден, его место займёт наименьший из потомков в правом поддереве " + k;
                        currentNode.Key = k;
                        currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                        currentNode.Right = Remove(currentNode.Right, currentNode.Key); // удаляем этого потомка
                        ready = true;
                        return res;
                    }
                }
                else if (currentNode.Left != null) // есть только левый потомок, он встаёт на место удаляемого
                {
                    if (!found)
                    {
                        string res = "Удаляемый элемент " + key + " найден, его место займёт левый потомок " + currentNode.Left.Key;
                        currentNode.Color = Color.FromArgb(255, 0, 0, 255);
                        currentNode.Left.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else
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
                    if (!found)
                    {
                        string res = "Удаляемый элемент " + key + " найден, его место займёт правый потомок " + currentNode.Right.Key;
                        currentNode.Color = Color.FromArgb(255, 0, 0, 255);
                        currentNode.Right.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else
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
                    if (!found)
                    {
                        string res = "Удаляемый элемент " + key + " найден, у него нет потомков";
                        currentNode.Color = Color.FromArgb(255, 0, 255, 0);
                        found = true;
                        return res;
                    }
                    else
                    {
                        string res = "Элемент " + key + " удалён";
                        currentNode.ReplaceParentsChild(null);
                        //currentNode = null;
                        ready = true;
                        found = false;
                        return res;
                    }
                }
            }
            else
            {
                var res = MessageBox.Show("Хотите удалить ещё один элемент?", "Удаление", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    currentNode = root;
                    Restart(root);
                    ready = false;
                    min_first = true;
                    min_search = true;
                    found = false;
                    evRestart();
                }                
                return "\n";
            }
        }
        public int RemoveOneControl(int key)
        {
            int res = 0;
            currentNode = Search(root, key);
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
            Remove(root, key);
            return res;
        }

    

    /* пошаговое удаление
    public int RemoveOneControl(int key)
    {
        if (!ready)
        {
            if (root.Key == key) // удаление корня
            {
                if (root.Left == null && root.Right == null) // нет потомков
                {
                    root = null;
                    return 8;
                }
                else if (root.Left == null) // только правый
                {
                    root = root.Right;
                    return 6;
                }
                else if (root.Right == null) // только левый
                {
                    root = root.Left;
                    return 5;
                }
                else // есть оба потомка
                {
                    if (min_search) // ещё не найден замещающий элемент
                    {
                        if (min_first) // начало поиска минимального
                        {
                            minNode = root.Right; // ищем в правом поддереве
                            min_first = false; // больше сюда не заходим
                        }
                        return MinimumOneControl();
                    }
                    else
                    {
                        int k = Minimum(root.Right).Key; // находим минимального потомка                            
                        root.Right = Remove(currentNode.Right, currentNode.Key); // удаляем этого потомка
                        root.Key = k;
                        ready = true;
                        return 12;
                    }                   

                }
            }
            if (currentNode == null)
            {
                ready = true;
                return 4;
            }
            if (key < currentNode.Key)
            {                   
                currentNode = currentNode.Left;
                return 1;
            }
            else if (key > currentNode.Key)
            {
                currentNode = currentNode.Right;
                return 2;
            }
            else if (currentNode.Left != null && currentNode.Right != null) // нашли удаляемый, у него есть оба потомка
            {
                //////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                int k = Minimum(root.Right).Key; // находим минимального потомка                    
                root.Right = Remove(root.Right, k); // удаляем этого потомка
                root.Key = k;
                return 3;
            }
            else if (currentNode.Left != null) // есть только левый потомок, он встаёт на место удаляемого
            {
                    currentNode = currentNode.Left;
                    ready = true;
                    return 5;

            }
            else if (currentNode.Right != null) // есть только правый потомок, он встаёт на место удаляемого
            {                    
                    currentNode = currentNode.Right;
                    ready = true;
                    return 6;

            }
            else // нет потомков, просто удаляем элемент
            {                  
                    currentNode.ReplaceParentsChild(null);
                    //currentNode = null;
                    ready = true;                        
                    return 8;                    
            }
        }
        else
        {
            var res = MessageBox.Show("Хотите удалить ещё один элемент?", "Удаление", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                currentNode = root;
                Restart(root);
                ready = false;
                min_first = true;
                min_search = true;
                found = false;
                evRestart();
            }
            return -1;
        }
    }

    private int MinimumOneControl()
    {
        if (minNode.Left == null)
        {              
            min_search = false;
            return 11;
        }
        else
        {
            minNode = minNode.Left;
            return 9;
        }
    }
    */
    private string MinimumOne()
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
    private Node Minimum(Node n) // наименьшее число в крайней левой ветке любого узла
    {
        if (n.Left == null) return n;
        else return Minimum(n.Left);
    }
    public Node Remove(Node n, int key) // удаление
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
    /*
    public void Remove_(int key)
    {
        Node n = Search(this.root, key);

        if (n == null) // удаляемый элемент не найден
        {
            MessageBox.Show("Not found");
            return;
        }
        else
        {
            // удаление корня
            if (n == root)
            {
                if (root.Left == null && root.Right == null)
                {
                    root = null;
                }
                else if (root.Left == null && root.Right != null)
                {
                    root.Right.Parent = null;
                    root = root.Right;

                }
                else if (root.Left != null && root.Right == null)
                {
                    root.Left.Parent = null;
                    root = root.Left;
                }
                else
                {

                }
            }
            else
            {
                // нет потомков
                if (n.Left == null && n.Right == null)
                {
                    n.ReplaceParentsChild(null);
                }
                else if (n.Left == null && n.Right != null) // нет левого, есть только правый
                {
                    n.ReplaceParentsChild(n.Right); // родитель удаляемого ссылается на правого потомка
                    n.Right.Parent = n.Parent; // у правого потомка новый родитель (родитель удаляемого)
                }
                else if (n.Left != null && n.Right == null) //есть левый, нет правого
                {
                    n.ReplaceParentsChild(n.Left); // родитель изменил ссылку на нового потомка                
                    n.Left.Parent = n.Parent; // потомок изменил ссылку на своего родителя
                }
                else // есть оба потомка
                {
                    Node tmp = n.Left;
                    while (tmp.Right != null)
                    {
                        tmp = tmp.Right;
                    }
                    tmp.ReplaceParentsChild(null);
                    tmp.Left = n.Left;
                    tmp.Right = n.Right; // и получает оба его потомка
                    tmp.Parent = n.Parent; // у крайнего элемента новый родитель - родитель удаляемого
                    n.ReplaceParentsChild(tmp); // родитель удаляемого получил нового потомка вместо удаляемого

                }
            }


                else // слева тоже есть потомок
                {

                }

            }

           }

    */

    public void PrintTreeSorted(Node root, StreamWriter sw)
    {
        if (root != null)
        {
            PrintTreeSorted(root.Left, sw);
            sw.WriteLine(root.Key);
            PrintTreeSorted(root.Right, sw);
        }
    }

}
}
