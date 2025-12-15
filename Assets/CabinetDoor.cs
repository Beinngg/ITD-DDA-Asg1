using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    [Header("Herb name to get (must match CraftingTable)")]
    public string med; // Example: "Ginseng Root", "Astragalus Root", etc.

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
