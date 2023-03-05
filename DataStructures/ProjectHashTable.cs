using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class ProjectHashTable<TKey, TValue> : IEnumerable<TKey>
    {
        ProjectLinkedList<Item>[] hashArray;
        private int itemsCount = 0;

        public int ItemsCount { get => itemsCount; set => itemsCount = value; }
        public ProjectHashTable(int capacity = 100)
        {
            hashArray = new ProjectLinkedList<Item>[capacity];
        }
        public void Add(TKey key, TValue value) // O(1) + O(n) when reallocation needed
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) hashArray[ind] = new ProjectLinkedList<Item>();
            else
            {
                if (ContainsKey(key))
                    throw new ArgumentException($"An item with the same key: {key} has already been added.");
            }

            hashArray[ind].AddLast(new Item(key, value));
            itemsCount++;

            if (itemsCount > hashArray.Length)
            {
                ReHash(); // resize and re-allocate all items (reuse Add + KeyToIndex)
            }
        }
        private void ReHash()  // O(n)
        {
            var tmp = hashArray;
            hashArray = new ProjectLinkedList<Item>[hashArray.Length * 2];
            itemsCount = 0;

            foreach (var list in tmp)
            {
                if (list != null)
                {
                    foreach (Item keyValueItem in list)
                    {
                        Add(keyValueItem.Key, keyValueItem.Value);
                    }
                }
            }
        }
        public double CalcAverLoad()
        {
            // return hashArray.Average(lst => (lst == null) ? 0 : lst.Count);
            return hashArray.Where(lst => lst != null).Average(lst => lst.Count);

        }
        public TValue GetValue(TKey key)
        {
            int ind = KeyToIndex(key);
            Item keyValue;
            if (hashArray[ind] != null)
            {
                keyValue = hashArray[ind].FirstOrDefault(item => item.Key.Equals(key));
                if (keyValue != null) return keyValue.Value;
            }
            throw new KeyNotFoundException($"No such key: {key}");
        }
        public bool ContainsKey(TKey key) // O(1)
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) return false;
            return hashArray[ind].Any(item => item.Key.Equals(key));
        }
        private int KeyToIndex(TKey key) // return valid index of array
        {
            int calcRes = Math.Abs(key.GetHashCode()); // !!!
            // normalize calcRes to be a valid index for current array size
            return calcRes % hashArray.Length;
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            for (int i = 0; i < hashArray.Length; i++)
            {
                if (hashArray[i] != null)
                {
                    foreach (var item in hashArray[i]) yield return item.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Item
        {
            private TValue itemValue;
            private TKey key;

            public TKey Key { get => key; set => key = value; }
            public TValue Value { get => itemValue; set => itemValue = value; }
            //bool isDeleted;

            public Item(TKey key, TValue value)
            {
                this.key = key;
                this.itemValue = value;
            }
        }
    }
}
