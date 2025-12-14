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
            DialogUI.I?.Show($"Customer: {myCase.symptomText}\n(Give me the correct pill.)");
            return;
        }

        GiveMedicineAndLeave();
    }

    private void GiveMedicineAndLeave()
    {
        if (CraftingTable.I == null)
        {
            DialogUI.I?.Show("No CraftingTable found.");
            return;
        }

        if (!CraftingTable.I.TakePill(out string pillName, out GameObject pillObj))
        {
            DialogUI.I?.Show("No pill on the table.");
            return;
        }

        Destroy(pillObj); 

        if (pillName == myCase.correctMedicine)
            DialogUI.I?.Show("Customer: Thanks! I feel better.");
        else
            DialogUI.I?.Show("Customer: That's not what I needed...");

        GameEndManager.I?.NotifyCustomerServed();

        hasLeft = true;
        Destroy(gameObject);
    }

    private void PickUniqueCase()
    {
        if (cases == null || cases.Length == 0)
        {
            myCase = null;
            DialogUI.I?.Show("No illness cases set on this customer.");
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

    public static void ResetUsedCases()
    {
        usedCases.Clear();
    }
}
