using System;
using System.Diagnostics;

namespace Lab2
{
    class Program
    {
        static unsafe void Main(string[] args)
        {


            var tree = new Btree(3);

            var insertArray = new[]
                { 5, 7, 15, 40, 2, 1, 3, 10, 12, 80, 200, 500, 150, 90, 20, 50, 16, 14, 13, 9, 38, 89, 101, 102, 201, 202, 91, 88, 30, 18 };
            foreach (var elem in insertArray)
                tree.Insert(elem);

            Console.WriteLine("Search(5): {0}", tree.Search(5));
            Console.WriteLine("Search(999): {0}", tree.Search(999));
            Console.WriteLine("FindMaxKey(): {0}", tree.FindMaxKey());

            Console.WriteLine("Delete(5): {0}", tree.Delete(5));
            Console.WriteLine("Search(5): {0}", tree.Search(5));

            var deleteArray = new[]
                { 15, 1, 80, 50, 14, 12, 500, 89, 90, 30, 150, 7, 13, 10, 16, 201, 102 };
            foreach (var elem in deleteArray)
                tree.Delete(elem);

            Console.WriteLine("FindMaxKey(): {0}", tree.FindMaxKey());

            tree.Print();


        }

    }
}