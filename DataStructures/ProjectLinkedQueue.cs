using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class ProjectLinkedQueue<T> : IEnumerable<T>
    {
        Node start;
        Node end;
        public int Count { get; private set; }

        private void AddFirst(T value) // O(1) - O(c)
        {
            Node n = new Node(value);
            n.Next = start;
            start = n;
            if (end == null) end = n;
            Count++;
        }
        private void AddLast(T value)  // O(1)
        {
            if (start == null)
            {
                AddFirst(value);
                return;
            }

            Node n = new Node(value);
            end.Next = n;
            end = n;
            Count++;
        }
        public void EnQueue(T value) // insert last
        {
            AddLast(value);
        }
        public void DeQueue() // remove first
        {
            RemoveFirst();
        }
        public bool IsEmpty()
        {
            if (start == null) return true;
            else return false;
        }
        public bool Peek(out T item)
        {
            if (IsEmpty())
            {
                item = default;
                return false;
            }

            item = start.Data;
            return true;
        }
        private bool RemoveFirst() // O(1)
        {
            if (start == null) return false;
            else
            {
                start = start.Next;
                if (start == null) end = start;
                Count--;
                return true;
            }
        }
        public bool GetAt(int index, out T item) // O(n)
        {
            item = default; // 0, false, null
            if (index < 0 || index >= Count) return false;

            Node tmp = start;
            for (int i = 0; i < index; i++)
            {
                tmp = tmp.Next;
            }
            item = tmp.Data;
            return true;
        }

        public override string ToString()
        {
            StringBuilder allValues = new StringBuilder();

            Node tmp = start;
            while (tmp != null)
            {
                allValues.Append(tmp.Data + " ");
                tmp = tmp.Next;
            }
            return allValues.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = start;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() // for old, non-generic environments
        {
            return GetEnumerator();
        }

        public class Node
        {
            internal T Data { get; set; }
            internal Node Next { get; set; } //reference to the next Node

            public Node(T value)
            {
                this.Data = value;
                Next = null;
            }
            public Node(T value, Node nextVal)
            {
                this.Data = value;
                Next = nextVal;
            }
        }
    }
}

