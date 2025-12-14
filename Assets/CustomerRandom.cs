using UnityEngine;

public class CustomerRandom : MonoBehaviour
{
    [System.Serializable]
    public class Case
    {
        [TextArea] public string symptomText;
        public string correctMedicine; 
    }

    [Header("illness to medicine cases")]
    public Case[] cases;

    [Header("all medicines")]
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
        
        if (!hasSpoken)
        {
            hasSpoken = true;
            Debug.Log($"customer：{currentCase.symptomText}");
            return;
        }

       
        TryGiveMedicine();
    }

    private void TryGiveMedicine()
    {
        string medicinePlayerHas = GetAnyMedicineFromInventory();

        if (medicinePlayerHas == null)
        {
            Debug.Log("U have no medicine to give me!");
            return;
        }


        Inventory.I.Remove(medicinePlayerHas);


        if (medicinePlayerHas == currentCase.correctMedicine)
        {
            Debug.Log("thanks! feeling better already.");
            Disappear();   
        }
   
        else
        {
            Debug.Log("thats not what I needed...");
            Disappear();   
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
       
        gameObject.SetActive(false);

       
    }

    private void PickNewCase()
    {
        if (cases == null || cases.Length == 0)
        {
            Debug.LogWarning("CustomerRandom: cases ！");
            return;
        }

        int idx = Random.Range(0, cases.Length);
        currentCase = cases[idx];
        hasSpoken = false;

        Debug.Log("here comes a new customer ");
    }
}
