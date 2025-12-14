using UnityEngine;

public class HerbManager : MonoBehaviour
{
    public static HerbManager I;

    [Header("你目前拥有的药材（勾选/修改来测试）")]
    public bool has人参;
    public bool has黄芪;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public void AddHerb(string herbName)
    {
        if (herbName == "人参") has人参 = true;
        else if (herbName == "黄芪") has黄芪 = true;

        Debug.Log($"收集药材：{herbName}");
    }

    public bool Consume(string herbName)
    {
        if (herbName == "人参" && has人参) { has人参 = false; return true; }
        if (herbName == "黄芪" && has黄芪) { has黄芪 = false; return true; }
        return false;
    }
}
