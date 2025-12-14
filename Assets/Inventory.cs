using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory I;

    private HashSet<string> items = new HashSet<string>();

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public bool Has(string itemName) => items.Contains(itemName);

    public void Add(string itemName)
    {
        items.Add(itemName);
        Debug.Log($"get：{itemName}");
    }

    public bool Remove(string itemName)
    {
        bool ok = items.Remove(itemName);
        if (ok) Debug.Log($"remove：{itemName}");
        return ok;
    }


    public List<string> GetAllItems()
    {
        return new List<string>(items);
    }
}
