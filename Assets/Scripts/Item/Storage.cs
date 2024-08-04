using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    public event Action Changed;

    public int Count => _items.Count;

    public void AddItem(Item item)
    {
        _items.Add(item);
        Changed?.Invoke();
    }

    public void RemoveLastItem() 
    {
        _items.Remove(_items[_items.Count - 1]);
        Changed?.Invoke();
    }
}
