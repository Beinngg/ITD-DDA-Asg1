using UnityEngine;

public class CabinetDoor : MonoBehaviour, IInteractable
{
   
    public string 人参;  

    [Header("UI Prefab")]
    public CabinetUI uiPrefab;

    private CabinetUI currentUI;

    /*public void Interact()
    {
        // 防止重复打开 UI
        if (currentUI != null) return;

        currentUI = Instantiate(uiPrefab);
        currentUI.Show(
            人参,
            onConfirm: OnConfirm,
            onBack: OnBack
        );
    }*/

    private void OnConfirm()
    {
        Inventory.I.Add(人参);
        Debug.Log($"get：{人参}");

        CloseUI();
    }

    private void OnBack()
    {
        CloseUI();
    }

    private void CloseUI()
    {
        if (currentUI != null)
        {
            Destroy(currentUI.gameObject);
            currentUI = null;
        }
    }
}
