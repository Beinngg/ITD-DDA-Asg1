using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
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

    [Header("Spawn")]
    public Transform spawnPoint;
    public float yOffset = 0.08f;

    private GameObject currentPillObj;
    private string currentPillName;

    private readonly HashSet<string> herbSet = new HashSet<string>
    {
        "人参","黄芪","龟板","夏枯草","水牛角","生姜","熟地黄","山茱萸"
    };

    public void Interact() => TryCraft();

    public void TryCraft()
    {
        if (Inventory.I == null)
        {
            DialogUI.I?.Show("Inventory not found.");
            return;
        }

  
        if (TryRecipe("人参","黄芪","大力补", vitalityTonicPill_Gold)) return;
        if (TryRecipe("龟板","夏枯草","龟苓丹", heatReliefPill_Blue)) return;
        if (TryRecipe("水牛角","生姜","羚角散", coldReliefPowder_Red)) return;
        if (TryRecipe("熟地黄","山茱萸","六味地黄丸", kidneyNourishPill_Gray)) return;

        // 否则做坏丹（吃任意两味草药）
        if (TryConsumeAnyTwoHerbs(out string a, out string b))
        {
            Make("坏丹", badPillPrefab);
            DialogUI.I?.Show($"Craft failed: {a} + {b} -> 坏丹");
            return;
        }

        DialogUI.I?.Show("Not enough herbs to craft.");
    }

    private bool TryRecipe(string a, string b, string medName, GameObject prefab)
    {
        if (!Inventory.I.Has(a) || !Inventory.I.Has(b)) return false;
        if (!Inventory.I.Remove2(a, b)) return false;

        Make(medName, prefab);
        DialogUI.I?.Show($"Crafted: {medName}");
        return true;
    }

    private void Make(string medName, GameObject prefab)
    {
        ClearTable();             
        currentPillName = medName; 

        if (prefab == null)
        {
            DialogUI.I?.Show("Missing pill prefab.");
            return;
        }

        Vector3 basePos = spawnPoint ? spawnPoint.position : transform.position;
        Vector3 pos = basePos + Vector3.up * yOffset;

        currentPillObj = Instantiate(prefab, pos, Quaternion.identity);
    }


    public bool TakePill(out string pillName, out GameObject pillObj)
    {
        pillName = currentPillName;
        pillObj  = currentPillObj;

        if (pillObj == null || string.IsNullOrEmpty(pillName))
            return false;

        currentPillName = null;
        currentPillObj  = null;
        return true;
    }

    private void ClearTable()
    {
        if (currentPillObj != null) Destroy(currentPillObj);
        currentPillObj = null;
        currentPillName = null;
    }

    private bool TryConsumeAnyTwoHerbs(out string herbA, out string herbB)
    {
        herbA = null;
        herbB = null;

        var all = Inventory.I.GetAllItems();
        var herbsOwned = all.FindAll(i => herbSet.Contains(i));
        if (herbsOwned.Count < 2) return false;

        herbA = herbsOwned[0];
        herbB = herbsOwned[1];

        Inventory.I.Remove(herbA);
        Inventory.I.Remove(herbB);
        return true;
    }
}
