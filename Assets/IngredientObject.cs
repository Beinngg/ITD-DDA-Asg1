using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    public string herbName; // 例："人参" 或 "黄芪"

    public void Collect()
    {
        HerbManager.I.AddHerb(herbName);
        Destroy(gameObject); // 收集后消失
    }
}
