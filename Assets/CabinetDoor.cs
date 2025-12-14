using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    [Header("medname to get")]
    public string med;

    private bool alreadyGiven = false;

    // 被 ARTapInteractor 调用
    public void Interact()
    {

        if (alreadyGiven)
        {
            Debug.Log($"you already get：{med}");
            return;
        }

        Inventory.I.Add(med);
        alreadyGiven = true;

        Debug.Log($"get：{med}");
    }
}
