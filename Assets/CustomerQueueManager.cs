using System.Collections.Generic;
using UnityEngine;

public class CustomerQueueManager : MonoBehaviour
{
    [Header("Queue positions (set in Inspector)")]
    public Transform[] slots; // slot 0 = front, slot 1 = second, etc.

    [Header("Move speed")]
    public float moveSpeed = 1.5f;

    private readonly List<CustomerAgent> customers = new List<CustomerAgent>();

    public void Enqueue(CustomerAgent customer)
    {
        customers.Add(customer);
        customer.SetManager(this);
        RepositionAll();
    }

    public void Dequeue(CustomerAgent customer)
    {
        customers.Remove(customer);
        RepositionAll();
    }

    private void RepositionAll()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            if (i >= slots.Length) break;
            customers[i].MoveTo(slots[i].position, slots[i].rotation, moveSpeed);
        }
    }
}
