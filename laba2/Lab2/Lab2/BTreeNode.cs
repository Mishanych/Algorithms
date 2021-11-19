using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class BTreeNode
    {
        public struct Data
        {
            public int key;
            public string value;

        }
        public Data[] Keys;
        private int _minPower;
        public BTreeNode[] Сhildren;
        public int AmountOfKeys;
        public bool isLeaf;
        public static int AmountOfPassedNodes;


        public BTreeNode(int power, bool isLeaf)
        {

            this._minPower = power;
            this.isLeaf = isLeaf;
            this.Keys = new Data[2 * this._minPower - 1];
            this.Сhildren = new BTreeNode[2 * this._minPower];
            this.AmountOfKeys = 0;

        }

        
        public int FindKey(int key)
        {
            int index = 0;

            while (index < AmountOfKeys && Keys[index].key < key)
            {
                index++;
            }

            return index;
        }


        public void Remove(int key)
        {

            int index = FindKey(key);

            if (index < AmountOfKeys && Keys[index].key == key)
            {
                if (isLeaf)
                {
                    RemoveFromLeaf(index);
                }
                else
                {
                    RemoveFromNonLeaf(index);
                }
            }
            else
            {
                if (isLeaf)
                {
                    Console.WriteLine($"The key {key} is does not exist in the tree");
                    return;
                }


                bool keyExists = index == AmountOfKeys;

                if (Сhildren[index].AmountOfKeys < _minPower)
                {
                    Fill(index);
                }
                
                
                if (keyExists && index > AmountOfKeys)
                {
                    Сhildren[index - 1].Remove(key);
                }
                else
                {
                    Сhildren[index].Remove(key);
                }
            }
        }

        public void RemoveFromLeaf(int index)
        {

            for (int i = index + 1; i < AmountOfKeys; i++)
            {
                Keys[i - 1] = Keys[i];
            }

            AmountOfKeys--;
        }

        public void RemoveFromNonLeaf(int index)
        {

            int key = Keys[index].key;


            if (Сhildren[index].AmountOfKeys >= _minPower)
            {
                Data pred = GetPred(index);
                Keys[index].key = pred.key;
                Keys[index].value = pred.value;
                Сhildren[index].Remove(pred.key);
            }
            else if (Сhildren[index + 1].AmountOfKeys >= _minPower)
            {
                Data succ = GetSucc(index);
                Keys[index].key = succ.key;
                Keys[index].value = succ.value;
                Сhildren[index + 1].Remove(succ.key);
            }
            else
            {
                Merge(index);
                Сhildren[index].Remove(key);
            }
        }

        public Data GetPred(int idx)
        { 
            BTreeNode currNode = Сhildren[idx];
            Data rez;

            while (!currNode.isLeaf)
            {
                currNode = currNode.Сhildren[currNode.AmountOfKeys];
            }


            rez.key = currNode.Keys[currNode.AmountOfKeys - 1].key;
            rez.value = currNode.Keys[currNode.AmountOfKeys - 1].value;

            return rez;
        }

        public Data GetSucc(int idx)
        { 
            BTreeNode currNode = Сhildren[idx + 1];
            Data rez;

            while (!currNode.isLeaf)
            {
                currNode = currNode.Сhildren[0];
            }


            rez.key = currNode.Keys[0].key;
            rez.value = currNode.Keys[0].value;
            return rez;
        }

        public void Fill(int index)
        {

            if (index != 0 && Сhildren[index - 1].AmountOfKeys >= _minPower)
            {
                BorrowFromPrev(index);
            }
            else if (index != AmountOfKeys && Сhildren[index + 1].AmountOfKeys >= _minPower)
            {
                BorrowFromNext(index);
            }
            else
            {
                if (index != AmountOfKeys)
                {
                    Merge(index);
                }
                else
                {
                    Merge(index - 1);
                }
            }
        }

        private void BorrowFromPrev(int index)
        {

            BTreeNode child = Сhildren[index];
            BTreeNode sibling = Сhildren[index - 1];

            for (int i = child.AmountOfKeys - 1; i >= 0; i--)
            {
                child.Keys[i + 1] = child.Keys[i];
            }

            if (!child.isLeaf)
            {
                for (int i = child.AmountOfKeys; i >= 0; i--)
                {
                    child.Сhildren[i + 1] = child.Сhildren[i];
                }
            }

            child.Keys[0] = Keys[index - 1];
            if (!child.isLeaf) 
            {
                child.Сhildren[0] = sibling.Сhildren[sibling.AmountOfKeys];
            }


            Keys[index - 1] = sibling.Keys[sibling.AmountOfKeys - 1];
            child.AmountOfKeys += 1;
            sibling.AmountOfKeys -= 1;
        }


        private void BorrowFromNext(int index)
        {

            BTreeNode child = Сhildren[index];
            BTreeNode sibling = Сhildren[index + 1];

            child.Keys[child.AmountOfKeys] = Keys[index];

            if (!child.isLeaf)
            {
                child.Сhildren[child.AmountOfKeys + 1] = sibling.Сhildren[0];
            }

            Keys[index] = sibling.Keys[0];

            for (int i = 1; i < sibling.AmountOfKeys; i++)
            {
                sibling.Keys[i - 1] = sibling.Keys[i];
            }

            if (!sibling.isLeaf)
            {
                for (int i = 1; i <= sibling.AmountOfKeys; i++)
                {
                    sibling.Сhildren[i - 1] = sibling.Сhildren[i];
                }
            }
            child.AmountOfKeys += 1;
            sibling.AmountOfKeys -= 1;
        }


        private void Merge(int idx)
        {

            BTreeNode child = Сhildren[idx];
            BTreeNode sibling = Сhildren[idx + 1];


            child.Keys[_minPower - 1] = Keys[idx];


            for (int i = 0; i < sibling.AmountOfKeys; i++)
            {
                child.Keys[i + _minPower] = sibling.Keys[i];
            }


            if (!child.isLeaf)
            {
                for (int i = 0; i <= sibling.AmountOfKeys; i++)
                {
                    child.Сhildren[i + _minPower] = sibling.Сhildren[i];
                }
            }


            for (int i = idx + 1; i < AmountOfKeys; i++)
            {
                Keys[i - 1] = Keys[i];
            }


            for (int i = idx + 2; i <= AmountOfKeys; i++)
            {
                Сhildren[i - 1] = Сhildren[i];
            }
            
            child.AmountOfKeys += sibling.AmountOfKeys + 1;
            AmountOfKeys--;
        }


        public void InsertNotFull(int key, string value)
        {

            int i = AmountOfKeys - 1;

            if (isLeaf)
            {
                while (i >= 0 && Keys[i].key > key)
                {
                    Keys[i + 1] = Keys[i];
                    i--;
                }
                Keys[i + 1].key = key;
                Keys[i + 1].value = value;
                AmountOfKeys = AmountOfKeys + 1;
            }
            else
            {
                while (i >= 0 && Keys[i].key > key)
                {
                    i--;
                }


                if (Сhildren[i + 1].AmountOfKeys == 2 * _minPower - 1)
                {
                    SplitChild(i + 1, Сhildren[i + 1]);
                    if (Keys[i + 1].key < key)
                    {
                        i++;
                    }
                }


                Сhildren[i + 1].InsertNotFull(key, value);
            }
        }


        public void SplitChild(int index, BTreeNode node)
        {
            BTreeNode parentNode = new BTreeNode(node._minPower, node.isLeaf);
            parentNode.AmountOfKeys = _minPower - 1;

            for (int j = 0; j < _minPower - 1; j++)
            {
                parentNode.Keys[j] = node.Keys[j + _minPower];
            }


            if (!node.isLeaf)
            {
                for (int j = 0; j < _minPower; j++)
                {
                    parentNode.Сhildren[j] = node.Сhildren[j + _minPower];
                }
            }
            node.AmountOfKeys = _minPower - 1;

            for (int j = AmountOfKeys; j >= index + 1; j--)
            {
                Сhildren[j + 1] = Сhildren[j];
            }

            Сhildren[index + 1] = parentNode;

            for (int j = AmountOfKeys - 1; j >= index; j--)
            {
                Keys[j + 1] = Keys[j];
            }


            Keys[index] = node.Keys[_minPower - 1];

            AmountOfKeys = AmountOfKeys + 1;
        }


        public void Traverse()
        {
            int i;
            for (i = 0; i < AmountOfKeys; i++)
            {
                if (!isLeaf)
                {
                    Сhildren[i].Traverse();
                }

                Console.Write($" {Keys[i].key}-{Keys[i].value}");
            }

            if (!isLeaf)
            {
                Сhildren[i].Traverse();
            }
        }

        public string TreeToString()
        {
            string s = "";
            int i;
            for (i = 0; i < AmountOfKeys; i++)
            {
                if (!isLeaf)
                {
                    s += Сhildren[i].TreeToString();
                }

                s += Keys[i].key + "$" + Keys[i].value + "$";
            }

            if (!isLeaf)
            {
                s += Сhildren[i].TreeToString();
            }
            return s;
        }


        public BTreeNode SearchNode(int key)
        {
            int i = 0;
            while (i < AmountOfKeys && key > Keys[i].key)
            {
                i++;
            }

            if (Keys[i].key == key)
            {
                return this;
            }

            if (isLeaf)
            {
                return null;
            }


            return Сhildren[i].SearchNode(key);
        }


        public string SearchValueByKey(int key)
        {
            AmountOfPassedNodes++;
            int left = -1;
            int right = AmountOfKeys;


            while (left < right - 1)
            {
                int mid = (left + right) / 2;
                if (Keys[mid].key < key)
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }

            if (right < Keys.Length)
            {
                if (Keys[right].key == key)
                {
                    return Keys[right].value;
                }
            }


            if (isLeaf)
            {
                return key + " not found";
            }
            else
            {
                return Сhildren[right].SearchValueByKey(key);
            }
        }
    }

}
