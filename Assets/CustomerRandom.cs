using UnityEngine;

public class CustomerRandom : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class Case
    {
        [TextArea] public string symptomText;
        public string correctMedicine; // 内部判定用，不展示
    }

    [Header("病例池（症状 + 正确丹药）")]
    public Case[] cases;

    [Header("所有可能的丹药")]
    public string[] allMedicines =
    {
        "大力补",
        "龟苓丹",
        "羚角散",
        "六味地黄丸",
        "坏丹"
    };

    private Case currentCase;
    private bool hasSpoken = false;

    private void Start()
    {
        PickNewCase();
    }

    public void Interact()
    {
        // 第一次点：只说症状
        if (!hasSpoken)
        {
            hasSpoken = true;
            Debug.Log($"顾客：{currentCase.symptomText}");
            Debug.Log("（你需要自己判断要做哪一种丹药）");
            return;
        }

        // 第二次点：尝试给药
        TryGiveMedicine();
    }

    private void TryGiveMedicine()
    {
        string medicinePlayerHas = GetAnyMedicineFromInventory();

        if (medicinePlayerHas == null)
        {
            Debug.Log("顾客：你还没给我药。");
            return;
        }

        // 消耗玩家给的药
        Inventory.I.Remove(medicinePlayerHas);

        // ✅ 给对药
        if (medicinePlayerHas == currentCase.correctMedicine)
        {
            Debug.Log("顾客：嗯……好多了，谢谢你！");
            Disappear();   // 治好后也直接消失
        }
        // ❌ 给错药
        else
        {
            Debug.Log("顾客：这个不对！我走了！");
            Disappear();   // 立刻消失
        }
    }

    private string GetAnyMedicineFromInventory()
    {
        foreach (var med in allMedicines)
        {
            if (Inventory.I.Has(med))
                return med;
        }
        return null;
    }

    private void Disappear()
    {
        // 方式一（推荐）：直接隐藏，之后你可以再 SetActive(true) 复用
        gameObject.SetActive(false);

        // 方式二（如果你不复用顾客，直接删）
        // Destroy(gameObject);
    }

    private void PickNewCase()
    {
        if (cases == null || cases.Length == 0)
        {
            Debug.LogWarning("CustomerRandom: cases 没有设置！");
            return;
        }

        int idx = Random.Range(0, cases.Length);
        currentCase = cases[idx];
        hasSpoken = false;

        Debug.Log("来了一位新顾客（点击顾客听症状）");
    }
}
