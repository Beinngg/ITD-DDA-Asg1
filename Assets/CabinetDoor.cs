using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    [Header("herb name to get")]
    public string med;

    private bool alreadyGiven = false;


    public void Interact()
    {
        if (alreadyGiven)
        {
            DialogUI.I?.Show($"You already got: {med}");
            return;
        }

        if (Inventory.I == null)
        {
            DialogUI.I?.Show("Inventory not found.");
            return;
        }

        if (Inventory.I.IsFull())
        {
            DialogUI.I?.Show("Bag is full (2/2). Craft first!");
            return; 
        }

        bool added = Inventory.I.Add(med);
        if (!added)
        {
            DialogUI.I?.Show("Bag is full.");
            return;
        }

        alreadyGiven = true;
        DialogUI.I?.Show($"Got herb: {med}");
    }
}
