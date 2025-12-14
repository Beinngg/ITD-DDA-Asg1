using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    public string med;

    [Header("UI Image (PNG)")]
    public Sprite uiImage;   // ← drag PNG here

    public void Interact()
    {
        ShowImage();
    }

    private void ShowImage()
    {
        // For now just debug – actual display depends on your UI system
        Debug.Log($"Showing UI image for {med}");
    }

    private void OnConfirm()
    {
        Inventory.I.Add(med);
        Debug.Log($"get: {med}");
    }

    private void OnBack()
    {
        Debug.Log("Back pressed");
    }
}
