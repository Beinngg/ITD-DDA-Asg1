using UnityEngine;

public class AlchemyTable : MonoBehaviour
{
    [Header("丹药生成")]
    public GameObject 大力丸Prefab;
    public Transform spawnPoint; // 桌面上方放一个空物体当生成点

    public void TryMakeMedicine()
    {
        // if：有人参 + 黄芪 = 大力丸
        if (HerbManager.I.has人参 && HerbManager.I.has黄芪)
        {
            HerbManager.I.Consume("人参");
            HerbManager.I.Consume("黄芪");

            Instantiate(大力丸Prefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("成功炼制：大力丸！");
            return;
        }

        Debug.Log("药材不足，无法炼制");
    }
}
