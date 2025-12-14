using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    [Header("med Prefabs")]
    public GameObject vitalityTonicPill_Gold;      
    public GameObject heatReliefPill_Blue;        
    public GameObject coldReliefPowder_Red;        
    public GameObject kidneyNourishPill_Gray;      
    public GameObject badPillPrefab;              
    [Header("生成位置")]
    public Transform spawnPoint;

    // 你所有“药材名”（用于从背包里挑两种来炼坏丹）
    private readonly string[] herbNames = new string[]
    {
        "人参","黄芪","龟板","夏枯草","水牛角","生姜","熟地黄","山茱萸"
    };

    public void Interact()
    {
        TryCraft();
    }

    public void TryCraft()
    {
  
        if (Has2("人参","黄芪"))
        {
            Consume2("人参","黄芪");
            Make("大力补", vitalityTonicPill_Gold);
            return;
        }

        if (Has2("龟板","夏枯草"))
        {
            Consume2("龟板","夏枯草");
            Make("龟苓丹", heatReliefPill_Blue);
            return;
        }

        if (Has2("水牛角","生姜"))
        {
            Consume2("水牛角","生姜");
            Make("羚角散", coldReliefPowder_Red);
            return;
        }

        if (Has2("熟地黄","山茱萸"))
        {
            Consume2("熟地黄","山茱萸");
            Make("六味地黄丸", kidneyNourishPill_Gray);
            return;
        }

      
        if (TryConsumeAnyTwoHerbs(out string usedA, out string usedB))
        {
            Make("坏丹", badPillPrefab);
            Debug.Log($"fail：{usedA} + {usedB} => 坏丹");
            return;
        }

        Debug.Log("you dont have enough herbs to craft anything. ");
    }

    private bool Has2(string a, string b)
        => Inventory.I.Has(a) && Inventory.I.Has(b);

    private void Consume2(string a, string b)
    {
        Inventory.I.Remove(a);
        Inventory.I.Remove(b);
    }

    private void Make(string medicineName, GameObject prefab)
    {
        Inventory.I.Add(medicineName);
        Spawn(prefab);
        Debug.Log($"you get：{medicineName}");
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("theres no prefab（Inspector 里没拖）");
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : (transform.position + new Vector3(0, 0.05f, 0));
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        Instantiate(prefab, pos, rot);
    }

    private bool TryConsumeAnyTwoHerbs(out string herbA, out string herbB)
    {
        herbA = null;
        herbB = null;

        List<string> all = Inventory.I.GetAllItems();
        List<string> ownedHerbs = new List<string>();

        foreach (var item in all)
        {

            for (int i = 0; i < herbNames.Length; i++)
            {
                if (item == herbNames[i])
                {
                    ownedHerbs.Add(item);
                    break;
                }
            }
        }

        if (ownedHerbs.Count < 2) return false;

        herbA = ownedHerbs[0];
        herbB = ownedHerbs[1];

        Inventory.I.Remove(herbA);
        Inventory.I.Remove(herbB);

        return true;
    }
}
