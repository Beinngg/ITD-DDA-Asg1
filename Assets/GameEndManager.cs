using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager I { get; private set; }

    public int targetCustomers = 2;
    public EndingCanvasController endingCanvas;

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

        if (servedCount >= targetCustomers)
            EndGame();
        else
            DialogUI.I?.Show($"Served {servedCount}/{targetCustomers}. One more customer left!");
    }

    private void EndGame()
    {
        if (ended) return;
        ended = true;

        DialogUI.I?.Show("DAY COMPLETE! All customers served.");

        if (endingCanvas != null)
            endingCanvas.Show();

        // 🔥 Add reputation safely
        var gm = FindObjectOfType<GeneralManager>();
        if (gm != null)
            gm.AddReputation(10);
    }
}
