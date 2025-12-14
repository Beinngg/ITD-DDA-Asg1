using UnityEngine;
using UnityEngine.InputSystem;

public class ARTapInteractor : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private LayerMask interactableMask;

    private void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;
    }

    private void Update()
    {
        // ===== NEW INPUT SYSTEM =====

        // 👉 手机 / 平板触控
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos =
                Touchscreen.current.primaryTouch.position.ReadValue();

            RaycastAtPosition(touchPos);
            return;
        }

        // 👉 Editor / PC 用鼠标测试
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            RaycastAtPosition(mousePos);
        }
    }

    private void RaycastAtPosition(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableMask))
        {
            CabinetDoor door = hit.collider.GetComponentInParent<CabinetDoor>();
            if (door != null)
            {
                door.Interact();
                return;
            }

            CraftingTable table = hit.collider.GetComponentInParent<CraftingTable>();
            if (table != null)
            {
                table.Interact();
                return;
            }

            CustomerRandom customer = hit.collider.GetComponentInParent<CustomerRandom>();
            if (customer != null)
            {
                customer.Interact();
                return;
            }
        }
    }
}
