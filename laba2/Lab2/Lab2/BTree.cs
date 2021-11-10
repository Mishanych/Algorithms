using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Btree
    {
        public int T { get; }
        public BtreeNode Root;

        public Btree(int t)
        {
            if (t <= 2)
                throw new ArgumentException("BTree");

            T = t;
            Root = new BtreeNode(t, true);
        }

        public void Insert(int key)
        {
            var root = Root;

            if (root.Length == 0)
                root.InsertInOrder(key);
            else if (root.Length == 2 * T - 1)
            {
                var newNode = new BtreeNode(T);
                newNode.Childrens[0] = root;
                Root = newNode;
                root.Parent = newNode;

                newNode.SplitChild(0);
                newNode.InsertNonFull(key);
            }
            else
                root.InsertNonFull(key);
        }

        public (BtreeNode node, int index) Search(int key) => Root.Search(key);

        public (int? key, int? index) FindMaxKey() => Root.FindMaxKey();

        public int? Delete(int key)
        {
            // Tree is empty
            if (Root.Length == 0)
                return null;

            var (node, index) = Search(key);

            // Node not found
            if (node == null)
                return null;

            node.Delete(index);

            return key;
        }

        public void Print()
        {
            Console.WriteLine("________________");
            Console.WriteLine("Btree");
            Console.WriteLine("t: {0}", T);

            if (Root == null) return;
            Console.WriteLine("\n________________");
            Console.WriteLine("R O O T:");
            Root.Print();
        }
    }
}
