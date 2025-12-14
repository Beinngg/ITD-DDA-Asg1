using UnityEngine;

public interface IInteractable
{
    void Interact();
}

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
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase != TouchPhase.Began) return;

        Ray ray = arCamera.ScreenPointToRay(t.position);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableMask))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interact();
        }
    }
}
