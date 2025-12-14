using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory I { get; private set; }

    private readonly List<string> items = new List<string>();
    private const int MAX_HERBS = 2;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsFull() => items.Count >= MAX_HERBS;

    public bool Add(string item)
    {
        if (IsFull()) return false;
        items.Add(item);
        return true;
    }

    public bool Has(string item) => items.Contains(item);

    public bool Remove(string item) => items.Remove(item);

    public bool Remove2(string a, string b)
    {
        if (!Has(a) || !Has(b)) return false;
        Remove(a);
        Remove(b);
        return true;
    }

    public List<string> GetAllItems() => new List<string>(items);
}
