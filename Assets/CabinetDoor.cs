using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    [Header("medname to get")]
    public string med;

    private bool alreadyGiven = false;


    public void Interact()
    {
        if (alreadyGiven)
        {
            Debug.Log($"you already get：{med}");
            return;
        }

        if (Inventory.I == null)
        {
            Debug.LogError("Inventory not found!");
            return;
        }


        if (Inventory.I.IsFull())
        {
            Debug.Log("bag is full, cannot get more herbs");
            return;
        }


        bool added = Inventory.I.Add(med);

        if (!added)
        {
            Debug.Log("Bag is full, cannot get more herbs");
        }

 
        alreadyGiven = true;

        Debug.Log($"get：{med}");
    }
}
