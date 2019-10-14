using System;
using System.Collections.Generic;

namespace PriorityQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            PriorityQueue<int> q = new PriorityQueue<int>(new MaxComparer());
            q.Push(1);
            q.Push(2);
            q.Push(3);
            q.Push(4);
            q.Push(5);
            q.Push(6);
            q.Push(7);
            q.Push(8);

            q.LevelTraversal();

            while (!q.IsEmpty())
            {
                Console.WriteLine(q.PopTopNode());
                q.LevelTraversal();
            }
        }
    }

    class DefaultComparer : IComparer<IComparable>
    {
        public int Compare(IComparable x, IComparable y)
        {
            return x.GetHashCode() - y.GetHashCode();
        }
    }

    class MaxComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x - y;
        }
    }

    class MinComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }

    class PriorityQueue<T> where T : IComparable
    {
        private List<T> list = new List<T>();
        IComparer<T> comparer;

        public PriorityQueue(IComparer<T> cpr)
        {
            list = new List<T>();
            comparer = cpr;
            if (comparer == null)
            {
                comparer = new DefaultComparer() as IComparer<T>;
            }
        }

        public void Push(T n)
        {
            list.Add(n);
            int index = list.Count - 1;
            while (GetParentIndex(index) >= 0 && comparer.Compare(list[index], list[GetParentIndex(index)]) > 0)
            {
                T tmp = list[index];
                list[index] = list[GetParentIndex(index)];
                list[GetParentIndex(index)] = tmp;
                index = GetParentIndex(index);
            }
        }

        public T GetTopNode()
        {
            if (list.Count > 0) return list[0];
            throw new Exception("Empty queue");
        }

        public T PopTopNode()
        {
            if (IsEmpty()) throw new Exception("Empty queue");

            T top = list[0];
            list[0] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            int index = 0;
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);
            while (leftChildIndex >= 0 || rightChildIndex >= 0)
            {
                if (rightChildIndex < 0)
                {
                    if (comparer.Compare(list[leftChildIndex], list[index]) > 0)
                    {
                        T tmp1 = list[index];
                        list[index] = list[leftChildIndex];
                        list[leftChildIndex] = tmp1;
                    }
                    break;
                }

                int targetIndex = comparer.Compare(list[leftChildIndex], list[rightChildIndex]) > 0 ? leftChildIndex : rightChildIndex;
                if (comparer.Compare(list[targetIndex], list[index]) > 0)
                {
                    T tmp2 = list[targetIndex];
                    list[targetIndex] = list[index];
                    list[index] = tmp2;

                    index = targetIndex;
                    leftChildIndex = GetLeftChildIndex(index);
                    rightChildIndex = GetRightChildIndex(index);
                }
                else
                {
                    break;
                }
            }

            return top;
        }

        public bool IsEmpty()
        {
            return list.Count <= 0;
        }

        private int GetParentIndex(int index)
        {
            if (index <= 0 || index >= list.Count) return -1;

            return (index - 1) / 2;
        }

        private int GetLeftChildIndex(int index)
        {
            if (index < 0 || index >= list.Count) return -1;

            int i = 2 * index + 1;
            if (i >= list.Count) i = -1;
            return i;
        }

        private int GetRightChildIndex(int index)
        {
            if (index < 0 || index >= list.Count) return -1;

            int i = 2 * index + 2;
            if (i >= list.Count) i = -1;
            return i;
        }

        public void LevelTraversal()
        {
            foreach (T i in list)
            {
                Console.Write(i.ToString());
            }
            Console.WriteLine();
        }
    }
}
