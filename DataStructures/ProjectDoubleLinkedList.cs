using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class ProjectDoubleLinkedList<T> : IEnumerable<T>
    {
        public Node Head { get; private set; }
        public Node Tail { get; private set; }
        public int Count { get; private set; }

        public void AddFirst(T data)
        {
            Node n = new Node(data);
            n.Next = Head;
            if (Head != null) Head.Previous = n;
            Head = n;
            n.Previous = null;
            if (Tail == null) Tail = n;
            Count++;
        }
        public void AddLast(T data)
        {
            if (Head == null)
            {
                AddFirst(data);
                return;
            }
            Node n = new Node(data);
            n.Previous = Tail;
            Tail.Next = n;
            Tail = n;
            Count++;
        }
        public T RemoveFirst()
        {
            if (Head == null) return default;
            Node current = Head;
            Head = Head.Next;
            if (Head == null) Tail = Head;
            else Head.Previous = null;
            Count--;
            return current.Data;
        }
        public T RemoveLast()
        {
            if (Head == null) return default;
            Node current = Tail;
            Tail = Tail.Previous;
            if (Tail == null) Head = Tail;
            else Tail.Next = null;
            Count--;
            return current.Data;
        }
        public bool GetAt(int postition, out T data)
        {
            data = default;
            if (postition < 0 || postition >= Count) return false;

            Node tmp = Head;
            for (int i = 0; i < postition; i++)
            {
                tmp = tmp.Next;
            }
            data = tmp.Data;
            return true;
        }
        public bool AddAt(int position, T data)
        {
            if (position < 0 || position > Count) return false;
            if (position == 0)
            {
                AddFirst(data);
                return true;
            }
            if (position == Count)
            {
                AddLast(data);
                return true;
            }

            Node current = Head;
            Node n = new Node(data);

            for (int i = 0; i < position - 1; i++)
            {
                if (current != null) current = current.Next;
            }

            n.Next = current.Next;
            n.Previous = current;
            current.Next = n;
            if (n.Next != null) n.Next.Previous = n;

            Count++;
            return true;
        }

        public T RemoveByNode(Node node)
        {
            if (Head == null) return default;
            if (node == Head)
            {
                var tmp = node.Data;
                RemoveFirst();
                return tmp;
            }
            if (node == Tail)
            {
                var tmp = node.Data;
                RemoveLast();
                return tmp;
            }
            Node current = node;
            //while (current != node) // Already have the node - no need to loop through
            //{
            //    current = current.Next;
            //    if (current == Tail) return default;
            //}
            current.Previous.Next = current.Next;
            current.Next.Previous = current.Previous;
            Count--;
            return current.Data;
        }
        public Node GetPreviousNode(Node node)
        {
            if (Head == null) return default;
            Node current = node;
            return node.Previous;
        }
        public T GetDataByNode(Node node)
        {
            if (Head == null) return default;
            Node current = node;
            //while (current != node) // Already have the node - no need to loop through
            //{
            //    current = current.Next;
            //    if (current == Tail) return default;
            //}
            return current.Data;
        }
        public override string ToString()
        {
            StringBuilder sb1 = new StringBuilder();
            Node tmp = Head;
            while (tmp != null)
            {
                sb1.Append(tmp.Data + " ");
                tmp = tmp.Next;
            }
            return sb1.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = Head;
            while (node != null)
            {
                yield return node.Data;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Node
        {
            internal T Data { get; set; }
            internal Node Next { get; set; }
            internal Node Previous { get; set; }
            public Node(T data)
            {
                this.Data = data;
                this.Next = null;
                this.Previous = null;
            }
        }
    }
}



