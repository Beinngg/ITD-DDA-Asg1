using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory I { get; private set; }

    private readonly List<string> items = new List<string>();

    // ✅ 草药上限
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

    /// <summary>
    /// 尝试加入一个草药
    /// </summary>
    public bool Add(string item)
    {
        if (items.Count >= MAX_HERBS)
        {
            Debug.Log($"[INV] Inventory full ({MAX_HERBS}). Cannot add {item}");
            return false;
        }

        items.Add(item);
        Debug.Log($"[INV] + {item}  ({items.Count}/{MAX_HERBS})");
        return true;
    }

    public bool Has(string item) => items.Contains(item);

    public bool Remove(string item)
    {
        bool ok = items.Remove(item);
        Debug.Log(ok
            ? $"[INV] - {item}  ({items.Count}/{MAX_HERBS})"
            : $"[INV] Remove failed: {item}");
        return ok;
    }

    /// <summary>
    /// 安全移除两种草药（炼丹用）
    /// </summary>
    public bool Remove2(string a, string b)
    {
        if (!Has(a) || !Has(b))
        {
            Debug.Log($"[INV] Remove2 failed: need {a} + {b}");
            return false;
        }

        Remove(a);
        Remove(b);
        return true;
    }

    public List<string> GetAllItems() => new List<string>(items);

    /// <summary>
    /// 调试用：打印当前草药
    /// </summary>
    public void PrintAll()
    {
        Debug.Log("[INV] Current: " + string.Join(", ", items));
    }

    /// <summary>
    /// 是否已满
    /// </summary>
    public bool IsFull() => items.Count >= MAX_HERBS;

    /// <summary>
    /// 当前数量
    /// </summary>
    public int Count => items.Count;
}
