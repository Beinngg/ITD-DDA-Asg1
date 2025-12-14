using System;
using UnityEngine;
using UnityEngine.UI;

public class CabinetUI : MonoBehaviour
{
    [Header("UI References")]
    public Text titleText;          
    public Button confirmButton;    
    public Button backButton;      

    private Action onConfirm;
    private Action onBack;


    public void Show(string herbName, Action confirmAction, Action backAction)
    {

        if (titleText != null)
            titleText.text = "get：" + herbName;

        onConfirm = confirmAction;
        onBack = backAction;

        confirmButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();


        confirmButton.onClick.AddListener(() => onConfirm?.Invoke());
        backButton.onClick.AddListener(() => onBack?.Invoke());
    }
}
