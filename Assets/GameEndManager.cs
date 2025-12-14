using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager I { get; private set; }

    public int targetCustomers = 2;
    public bool pauseOnEnd = true;

    private int servedCount = 0;
    private bool ended = false;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
    }

    public void NotifyCustomerServed()
    {
        if (ended) return;

        servedCount++;
        Debug.Log($"[END] served {servedCount}/{targetCustomers}");

        if (servedCount >= targetCustomers)
            EndGame();
    }

    private void EndGame()
    {
        if (ended) return;
        ended = true;

        Debug.Log("Ganme Over: All customers served!");

        if (pauseOnEnd)
            Time.timeScale = 0f;
    }
}
