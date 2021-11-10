using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class BtreeNode
    {
        public BtreeNode Parent;
        private int T { get; }
        public int Length;
        public readonly int?[] Keys;
        public readonly BtreeNode[] Childrens;
        public readonly bool IsLeaf;

        public BtreeNode(int t)
        {
            T = t;
            Length = 0;
            Keys = new int?[2 * t - 1];
            Childrens = new BtreeNode[2 * t];

            IsLeaf = false;
        }

        public BtreeNode(int t, bool leaf)
        {
            T = t;
            Length = 0;
            Keys = new int?[2 * t - 1];
            Childrens = new BtreeNode[2 * t];

            IsLeaf = leaf;
        }

        public int? GetFirstKey() => Length == 0 ? null : Keys[0];

        public int? GetFirstKeyIndex() => Length == 0 ? null : 0;

        public int? GetLastKey() => Length == 0 ? null : Keys[Length - 1];

        public int? GetLastKeyIndex() => Length == 0 ? null : Length - 1;

        public int? GetLastChildrenIndex()
        {
            for (var i = Childrens.Length - 1; i >= 0; i--)
                if (Childrens[i] != null)
                    return i;

            return null;
        }

        public BtreeNode GetLastChildren()
        {
            for (var i = Childrens.Length - 1; i >= 0; i--)
                if (Childrens[i] != null)
                    return Childrens[i];

            return null;
        }

        public int?[] GetHashCodeOfChildrens()
        {
            var result = new int?[2 * T];

            for (var i = 0; i < 2 * T; i++)
                if (Childrens[i] != null)
                    result[i] = Childrens[i].GetHashCode();

            return result;
        }

        public (int? key, int? index) FindMaxKey()
        {
            if (IsLeaf)
                return (GetLastKey(), GetLastKeyIndex());
            else
            {
                return GetLastChildren().FindMaxKey();
            }
        }

        public void InsertInOrder(int key)
        {
            if (Length == 0)
                Keys[0] = key;
            else
            {
                var i = Length - 1;
                while (i >= 0 && key < Keys[i])
                {
                    Keys[i + 1] = Keys[i];
                    i--;
                }

                Keys[i + 1] = key;
            }

            Length++;
        }

        public void InsertNonFull(int key)
        {
            if (IsLeaf)
                InsertInOrder(key);
            else
            {
                var i = Length - 1;

                while (i >= 0 && Keys[i] > key)
                    i--;

                i++;

                if (Childrens[i].Length == 2 * T - 1)
                {
                    SplitChild(i);

                    if (key > Keys[i])
                        i++;
                }

                Childrens[i].InsertNonFull(key);
            }
        }

        public void SplitChild(int index)
        {
            var y = Childrens[index];
            var z = new BtreeNode(T, y.IsLeaf)
            {
                Parent = y.Parent,
                Length = T - 1
            };

            y.Length = T - 1;

            for (var j = 0; j < T - 1; j++)
            {
                z.Keys[j] = y.Keys[j + T];
                y.Keys[j + T] = null;
            }

            if (!y.IsLeaf)
                for (var j = 0; j < T; j++)
                {
                    z.Childrens[j] = y.Childrens[j + T];
                    y.Childrens[j + T] = null;

                    z.Childrens[j].Parent = z;
                }

            for (var j = Length - 1; j >= index; j--)
                Keys[j + 1] = Keys[j];

            for (var j = Length; j >= index + 1; j--)
                Childrens[j + 1] = Childrens[j];

            Childrens[index + 1] = z;
            Keys[index] = y.Keys[T - 1];
            y.Keys[T - 1] = null;
            Length++;
        }

        public (BtreeNode node, int index) Search(int key)
        {
            var i = 0;
            while (i < Length && key > Keys[i])
                i++;

            if (i < Length && key == Keys[i])
                return (this, i);

            if (IsLeaf)
                return (null, 0);

            return Childrens[i].Search(key);
        }

        private void MargeTwoNodes(bool leftSibling, int thisNodeIndex)
        {
            var mergedNode = new BtreeNode(T, IsLeaf) { Parent = this.Parent };
            BtreeNode siblingNode;

            if (!leftSibling)
            {
                InsertThisNode();

                InsertParentKey();

                InsertSibling();

                Parent.Childrens[thisNodeIndex] = mergedNode;
                MoveParentChildrens(thisNodeIndex + 1);
            }
            else
            {
                InsertSibling();

                InsertParentKey();

                InsertThisNode();

                Parent.Childrens[thisNodeIndex - 1] = mergedNode;
                MoveParentChildrens(thisNodeIndex);
            }

            void InsertThisNode()
            {
                var length = mergedNode.Length;
                for (var i = 0; i < Length; i++)
                {
                    mergedNode.Keys[length + i] = Keys[i];
                    Keys[i] = null;
                    mergedNode.Length++;
                }

                for (var i = 0; i < GetLastChildrenIndex(); i++)
                {
                    mergedNode.Childrens[i] = Childrens[i];
                    Childrens[i] = null;
                }
            }

            void InsertSibling()
            {
                siblingNode = !leftSibling ? Parent.Childrens[thisNodeIndex + 1] : Parent.Childrens[thisNodeIndex - 1];

                var length = mergedNode.Length;
                for (var i = 0; i < siblingNode.Length; i++)
                {
                    mergedNode.Keys[length + i] = siblingNode.Keys[i];
                    siblingNode.Keys[i] = null;
                    mergedNode.Length++;
                }

                for (var i = 0; i < siblingNode.GetLastChildrenIndex(); i++)
                {
                    mergedNode.Childrens[length + i] = siblingNode.Childrens[i];
                    siblingNode.Childrens[i] = null;
                }
            }

            void InsertParentKey()
            {
                int index;
                if (!leftSibling)
                    index = thisNodeIndex;
                else
                    index = thisNodeIndex - 1;

                mergedNode.Keys[mergedNode.Length] = Parent.Keys[index];

                for (var i = index; i < 2 * T - 1; i++)
                {
                    if (Parent.Keys[i] == null)
                        break;

                    Parent.Keys[i] = Parent.Keys[i + 1];
                }

                mergedNode.Length++;
            }

            void MoveParentChildrens(int startIndex)
            {
                for (var i = startIndex; i < 2 * T - 1; i++)
                    Parent.Childrens[i] = Parent.Childrens[i + 1];
            }
        }

        public void Delete(int index)
        {
            DeletePhaseOne(index);

            DeletePhaseTwo();
        }

        private int? DeletePhaseOne(int index)
        {
            return IsLeaf ? DeleteFromLeaf(index) : DeleteFromNoLeaf(index);
        }

        private int? DeleteFromLeaf(int index)
        {
            var temp = Keys[index];

            for (var i = index; i < Length - 1; i++)
                Keys[i] = Keys[i + 1];

            Length--;

            for (var i = Length; i < 2 * T - 1; i++)
            {
                if (Keys[i] == null)
                    break;

                Keys[i] = null;
            }

            return temp;
        }

        private int? DeleteFromNoLeaf(int index)
        {
            var temp = Keys[index];

            var (key, _) = Childrens[index].FindMaxKey();
            var (node, i) = Search(key ?? default(int));
            Keys[index] = key;

            node.Delete(i);

            return temp;
        }

        private void DeletePhaseTwo()
        {
            if (Parent == null
                || Length >= T - 1)
                return;

            var thisNodeIndex = Array.IndexOf(Parent.Childrens, this);

            if (thisNodeIndex > 0
                && Parent.Childrens[thisNodeIndex - 1].Length > T - 1)
            {
                MoveKeyOverParent(true);
                return;
            }
            else if (thisNodeIndex < 2 * T
              && Parent.Childrens[thisNodeIndex + 1] != null
              && Parent.Childrens[thisNodeIndex + 1].Length > T - 1)
            {
                MoveKeyOverParent(false);
                return;
            }

            if (thisNodeIndex >= 0 && thisNodeIndex < 2 * T
                && Parent.Childrens[thisNodeIndex + 1] != null
                && Parent.Childrens[thisNodeIndex + 1].Length >= T - 1)
            {
                MargeTwoNodes(false, thisNodeIndex);
                Parent.DeletePhaseTwo();
            }
            else if (thisNodeIndex > 0 && thisNodeIndex <= 2 * T
                && Parent.Childrens[thisNodeIndex - 1].Length >= T - 1)
            {
                MargeTwoNodes(true, thisNodeIndex);
                Parent.DeletePhaseTwo();
            }

            void MoveKeyOverParent(bool leftSibling)
            {
                int siblingIndex;
                int siblingKeyIndex;
                int keyIndex;

                if (leftSibling)
                {
                    siblingIndex = thisNodeIndex - 1;
                    siblingKeyIndex = Parent.Childrens[siblingIndex].GetLastKeyIndex() ?? default(int);
                    keyIndex = thisNodeIndex - 1;
                }
                else
                {
                    siblingIndex = thisNodeIndex + 1;
                    siblingKeyIndex = Parent.Childrens[siblingIndex].GetFirstKeyIndex() ?? default(int);
                    keyIndex = thisNodeIndex;
                }

                InsertInOrder(Parent.Keys[keyIndex] ?? default(int));

                for (var i = GetLastChildrenIndex(); i < 2 * T - 1; i++)
                    Childrens[(int)i] = Childrens[(int)i];

                Childrens[0] = Parent.Childrens[siblingIndex].GetLastChildren();
                Parent.Childrens[siblingIndex].Childrens[GetLastChildrenIndex() ?? default(int)] = null;

                Parent.Keys[keyIndex] =
                    Parent.Childrens[siblingIndex].DeletePhaseOne(siblingKeyIndex);
            }
        }

        public void Print()
        {
            Console.WriteLine("this: {0}", GetHashCode());
            if (Parent != null)
                Console.WriteLine("parent: {0}", Parent.GetHashCode());
            Console.WriteLine("keys: [{0}]", String.Join(", ", Keys));
            Console.WriteLine("childrens: [{0}]", String.Join(", ", GetHashCodeOfChildrens()));
            Console.WriteLine("leaf: {0}", IsLeaf);
            Console.WriteLine();

            foreach (var children in Childrens)
                children?.Print();
        }

        public void PrintRoot()
        {
            if (Parent == null)
                Print();
            else
                Parent.PrintRoot();
        }
    }
}
