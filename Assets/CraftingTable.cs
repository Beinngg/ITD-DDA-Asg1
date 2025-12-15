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
    public GameObject vitalityTonicPill_Gold;   // Vitality Tonic Pill
    public GameObject heatReliefPill_Blue;      // Heat Relief Herbal Pill
    public GameObject coldReliefPowder_Red;     // Cold Relief Herbal Powder
    public GameObject kidneyNourishPill_Gray;   // Kidney Nourishing Herbal Pill
    public GameObject badPillPrefab;            // Bad Pill

    [Header("Spawn")]
    public Transform spawnPoint;
    public float yOffset = 0.08f;

    private GameObject currentPillObj;
    private string currentPillName;

    // Herbs in English
    private readonly HashSet<string> herbSet = new HashSet<string>
    {
        "Ginseng Root",
        "Astragalus Root",
        "Tortoise Plastron",
        "Selfheal Spike",
        "Water Buffalo Horn",
        "Fresh Ginger Rhizome",
        "Prepared Rehmannia Root",
        "Cornelian Cherry Fruit"
    };

    public void Interact() => TryCraft();

    public void TryCraft()
    {
        if (Inventory.I == null)
        {
            DialogUI.I?.Show("Inventory not found.");
            return;
        }

        // Recipes using the English names
        if (TryRecipe("Ginseng Root", "Astragalus Root", "Vitality Tonic Pill", vitalityTonicPill_Gold)) return;
        if (TryRecipe("Tortoise Plastron", "Selfheal Spike", "Heat Relief Herbal Pill", heatReliefPill_Blue)) return;
        if (TryRecipe("Water Buffalo Horn", "Fresh Ginger Rhizome", "Cold Relief Herbal Powder", coldReliefPowder_Red)) return;
        if (TryRecipe("Prepared Rehmannia Root", "Cornelian Cherry Fruit", "Kidney Nourishing Herbal Pill", kidneyNourishPill_Gray)) return;

        // If any two herbs but no valid recipe → bad pill
        if (TryConsumeAnyTwoHerbs(out string a, out string b))
        {
            Make("Bad Pill", badPillPrefab);
            DialogUI.I?.Show($"Craft failed: {a} + {b} -> Bad Pill");
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
        pillObj = currentPillObj;

        if (pillObj == null || string.IsNullOrEmpty(pillName))
            return false;

        currentPillName = null;
        currentPillObj = null;
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
