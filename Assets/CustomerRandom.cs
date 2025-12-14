using System.Collections.Generic;
using UnityEngine;

public class CustomerRandom : MonoBehaviour
{
    [System.Serializable]
    public class Case
    {
        [TextArea] public string symptomText;
        public string correctMedicine;
    }

    public Case[] cases;


    private static HashSet<int> usedCases = new HashSet<int>();

    private Case myCase;
    private bool spoken = false;
    private bool hasLeft = false;

    private void Start()
    {
        PickUniqueCase();
    }

    public void Interact()
    {
        if (hasLeft) return;
        if (myCase == null) return;

        if (!spoken)
        {
            spoken = true;
            Debug.Log($"customer: {myCase.symptomText}");
            return;
        }

        GiveMedicineAndLeave();
    }

    private void GiveMedicineAndLeave()
    {
        if (CraftingTable.I == null)
        {
            Debug.LogError("No CraftingTable in scene");
            return;
        }

        if (!CraftingTable.I.TakePill(out string pillName, out GameObject pillObj))
        {
            Debug.Log("桌子上没有丹药");
            return;
        }

        Destroy(pillObj);

        if (pillName == myCase.correctMedicine)
            Debug.Log("thanks! feeling better already.");
        else
            Debug.Log("that's not what I needed...");


        if (GameEndManager.I != null)
            GameEndManager.I.NotifyCustomerServed();
        else
            Debug.LogWarning("GameEndManager not found in scene!");

        hasLeft = true;
        Destroy(gameObject);
    }

    private void PickUniqueCase()
    {
        if (cases == null || cases.Length == 0)
        {
            myCase = null;
            return;
        }
 
        if (cases.Length == 1)
        {
            myCase = cases[0];
            return;
        }

        int idx = Random.Range(0, cases.Length);
        int safety = 200;

        while (usedCases.Contains(idx) && safety-- > 0)
            idx = Random.Range(0, cases.Length);

        usedCases.Add(idx);
        myCase = cases[idx];
    }
}
