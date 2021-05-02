using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStack<T>
{
    private List<T> items = new List<T>();
    public int Count
    {
        get { return items.Count; }
        private set { }
    }

    public void Push(T item)
    {
        items.Add(item);
    }
    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }
        else
            return default(T);
    }
    public void Remove(int itemAtPosition)
    {
        items.RemoveAt(itemAtPosition);
    }

    public void Remove(T item)
    {
        items.Remove(item);
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }
}
