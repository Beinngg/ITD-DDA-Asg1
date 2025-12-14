using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
   
    public string med;  

    [Header("UI Prefab")]
    public CabinetUI uiPrefab;

    private CabinetUI currentUI;

    public void Interact()
    {
        // 防止重复打开 UI
        if (currentUI != null) return;

        currentUI = Instantiate(uiPrefab);
        currentUI.Show(med, OnConfirm, OnBack);}


    private void OnConfirm()
    {
        Inventory.I.Add(med);
        Debug.Log($"get：{med}");

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
