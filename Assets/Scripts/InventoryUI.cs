using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Text inventoryText; 


    public string[] displayOrder =
    {
        "人参","黄芪","龟板","夏枯草","水牛角","生姜","熟地黄","山茱萸",
        "大力补","龟苓丹","羚角散","六味地黄丸","坏丹"
    };

    public void Render()
    {
        if (inventoryText == null || Inventory.I == null) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("inventory：");

        foreach (var item in displayOrder)
        {
            if (Inventory.I.Has(item))
                sb.AppendLine("• " + item);
        }

        inventoryText.text = sb.ToString();
    }
}
