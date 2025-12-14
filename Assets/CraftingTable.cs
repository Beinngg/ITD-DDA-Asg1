using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    // ✅ 全局唯一桌子
    public static CraftingTable I { get; private set; }

    private void Awake()
    {
        I = this;
    }

    [Header("Medicine Prefabs")]
    public GameObject vitalityTonicPill_Gold;
    public GameObject heatReliefPill_Blue;
    public GameObject coldReliefPowder_Red;
    public GameObject kidneyNourishPill_Gray;
    public GameObject badPillPrefab;

    public Transform spawnPoint;
    public float yOffset = 0.08f;

    private GameObject currentPillObj;
    private string currentPillName;

    private readonly HashSet<string> herbs = new HashSet<string>
    {
        "人参","黄芪","龟板","夏枯草","水牛角","生姜","熟地黄","山茱萸"
    };

    public void Interact()
    {
        TryCraft();
    }

    public void TryCraft()
    {
        if (Inventory.I == null) return;

        if (TryRecipe("人参","黄芪","大力补", vitalityTonicPill_Gold)) return;
        if (TryRecipe("龟板","夏枯草","龟苓丹", heatReliefPill_Blue)) return;
        if (TryRecipe("水牛角","生姜","羚角散", coldReliefPowder_Red)) return;
        if (TryRecipe("熟地黄","山茱萸","六味地黄丸", kidneyNourishPill_Gray)) return;

        if (TryConsumeAnyTwoHerbs())
            Make("坏丹", badPillPrefab);
        else
            Debug.Log("not enough herbs");
    }

    private bool TryRecipe(string a, string b, string name, GameObject prefab)
    {
        if (!Inventory.I.Has(a) || !Inventory.I.Has(b)) return false;
        Inventory.I.Remove2(a, b);
        Make(name, prefab);
        return true;
    }

    private void Make(string name, GameObject prefab)
    {
        // ✅ 先清旧丹药
        ClearTable();

        currentPillName = name;
        Vector3 pos = (spawnPoint ? spawnPoint.position : transform.position) + Vector3.up * yOffset;
        currentPillObj = Instantiate(prefab, pos, Quaternion.identity);

        Debug.Log($"[TABLE] Made {name}");
    }

    // ✅ 客人专用：从桌子取走丹药
    public bool TakePill(out string pillName, out GameObject pillObj)
    {
        pillName = currentPillName;
        pillObj = currentPillObj;

        if (pillObj == null || string.IsNullOrEmpty(pillName))
            return false;

        currentPillName = null;
        currentPillObj = null;

        return true;
    }

    private void ClearTable()
    {
        if (currentPillObj != null)
            Destroy(currentPillObj);

        currentPillObj = null;
        currentPillName = null;
    }

    private bool TryConsumeAnyTwoHerbs()
    {
        var all = Inventory.I.GetAllItems();
        var list = all.FindAll(i => herbs.Contains(i));
        if (list.Count < 2) return false;

        Inventory.I.Remove(list[0]);
        Inventory.I.Remove(list[1]);
        return true;
    }
}
