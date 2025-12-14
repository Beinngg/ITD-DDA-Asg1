using UnityEngine;

public class CustomerAgent : MonoBehaviour
{
    private CustomerQueueManager manager;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float speed = 1.5f;
    private bool moving = false;

    public void SetManager(CustomerQueueManager m) => manager = m;

    public void MoveTo(Vector3 pos, Quaternion rot, float moveSpeed)
    {
        targetPos = pos;
        targetRot = rot;
        speed = moveSpeed;
        moving = true;
    }

    private void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            moving = false;
    }

    // Call this when customer should leave (wrong medicine)
    public void Leave()
    {
        if (manager != null) manager.Dequeue(this);
        Destroy(gameObject);
    }
}
