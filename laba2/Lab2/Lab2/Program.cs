using System;
using System.Diagnostics;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            BTree t = new BTree(100);

            t.Insert(1, "v1");
            t.Insert(3, "v3");
            t.Insert(7, "v7");
            t.Insert(10, "v10");
            t.Insert(11, "v11");
            t.Insert(13, "v13");
            t.Insert(14, "v14");
            t.Insert(15, "v15");
            t.Insert(4, "v4");
            t.Insert(5, "v5");
            t.Insert(2, "v2");
            t.Insert(12, "v12");
            t.Insert(6, "v6");

            Console.WriteLine("Traversal of tree constructed is");
            t.Traverse();
            Console.WriteLine();

            t.Remove(6);
            Console.WriteLine("Traversal of tree after removing 6");
            t.Traverse();
            Console.WriteLine();

            t.Remove(13);
            Console.WriteLine("Traversal of tree after removing 13");
            t.Traverse();
            Console.WriteLine();

            t.Remove(7);
            Console.WriteLine("Traversal of tree after removing 7");
            t.Traverse();
            Console.WriteLine();

            t.Remove(4);
            Console.WriteLine("Traversal of tree after removing 4");
            t.Traverse();
            Console.WriteLine();

            t.Remove(2);
            Console.WriteLine("Traversal of tree after removing 2");
            t.Traverse();
            Console.WriteLine();

            t.Remove(16);
            Console.WriteLine("Traversal of tree after removing 16");
            t.Traverse();
            Console.WriteLine();

            int pkey = 15;
            Console.WriteLine($"Search key {pkey}: value = {t.Search(pkey)}");

            t.Change(15, "Hello world!");

            Console.WriteLine("Traversal of tree after removing 16");
            t.Traverse();
            Console.WriteLine();
            t.Save();
            t.Load();
            t.Traverse();
            Console.WriteLine();









            var rand = new Random();
            Console.WriteLine("Added BTree (t=100). Search 10 random keys:");
            for (int i = 0; i < 10000; i++)
            {
                t.Insert(i, "value_" + i);
            }

            for (int i = 0; i < 10; i++)
            {
                int k = rand.Next(10000);
                Console.WriteLine("Search key " + k + " Founded value: " + t.Search(k) + " Amount of passed nodes: " + BTreeNode.AmountOfPassedNodes);
            }
        }

    }
}