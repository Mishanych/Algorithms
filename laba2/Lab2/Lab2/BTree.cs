using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class BTree
    {
        private BTreeNode _root;
        private int _minPower;


        public BTree(int power)
        {
            this._root = null;
            this._minPower = power;
        }

        public void Traverse()
        {
            if (_root != null)
            {
                _root.Traverse();
            }
        }


        private BTreeNode SearchKey(int key)
        {
            BTreeNode.AmountOfPassedNodes = 0;
            return _root == null ? null : _root.SearchNode(key);
        }


        public void Insert(int key, string value)
        {

            if (_root == null)
            {
                _root = new BTreeNode(_minPower, true);
                _root.Keys[0].key = key;
                _root.Keys[0].value = value;
                _root.AmountOfKeys = 1;
            }
            else
            {
                if (_root.AmountOfKeys == 2 * _minPower - 1)
                {
                    BTreeNode node = new BTreeNode(_minPower, false);

                    node.Сhildren[0] = _root;
                    node.SplitChild(0, _root);

                    int i = 0;
                    if (node.Keys[0].key < key)
                    {
                        i++;
                    }

                    node.Сhildren[i].InsertNotFull(key, value);

                    _root = node;
                }
                else
                {
                    _root.InsertNotFull(key, value);
                }
            }
        }


        public void Remove(int key)
        {
            if (_root == null)
            {
                Console.WriteLine("The tree is empty");
                return;
            }

            _root.Remove(key);

            if (_root.AmountOfKeys == 0)
            {
                if (_root.isLeaf)
                {
                    _root = null;
                }
                else
                {
                    _root = _root.Сhildren[0];
                } 
            }
        }


        public string Search(int pkey)
        {
            BTreeNode.AmountOfPassedNodes = 0;

            if (_root != null)
            {
                return _root.SearchValueByKey(pkey);
            }
            else
            {
                return "not found";
            }

        }
        public void Change(int pkey, string val)
        {
            if (SearchKey(pkey) != null)
            {
                Remove(pkey);
                Insert(pkey, val);
                Console.WriteLine($"Changed key {pkey}: value = {Search(pkey)}");
            }
            else
            {
                Console.WriteLine($"key={pkey} not found! ");
            }
        }


        public void Save()
        {
            if (_root != null)
            {
                string fname = "data.txt";
                using (StreamWriter sw = new StreamWriter(fname, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(_root.TreeToString());
                }
            }
        }
        public void Load()
        {
            string fname = "data.txt";
            string s = "";
            using (StreamReader sr = new StreamReader(fname))
            {
                s = sr.ReadToEnd();
                string[] dataS = s.Split("$");
                _root = null;
                for (int i = 0; i < dataS.Length - 1; i += 2) Insert(Int32.Parse(dataS[i]), dataS[i + 1]);
            }
        }
    }
}
