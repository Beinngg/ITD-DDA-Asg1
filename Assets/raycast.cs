using UnityEngine;

public class ARTapRaycast : MonoBehaviour
{
    [SerializeField] private Camera arCamera;

    private void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;
        var t = Input.GetTouch(0);
        if (t.phase != TouchPhase.Began) return;

        Ray ray = arCamera.ScreenPointToRay(t.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            // 点到药材 → 收集
            var ingredient = hit.collider.GetComponentInParent<IngredientObject>();
            if (ingredient != null)
            {
                ingredient.Collect();
                return;
            }

            // 点到桌子 → 炼丹
            var table = hit.collider.GetComponentInParent<AlchemyTable>();
            if (table != null)
            {
                table.TryMakeMedicine();
                return;
            }
        }
    }
}
