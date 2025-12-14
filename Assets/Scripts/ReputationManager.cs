using TMPro;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;

public class ReputationManager : MonoBehaviour
{
    public TMP_Text reputationText;       // TMP_Text showing number
    public GameObject reputationLabelPNG; // your "Reputation:" PNG GameObject
    public GameObject reputationCanvas;   // parent Canvas for the UI

    private DatabaseReference dbRef;
    private string userId;

    void Awake()
    {
        // Make sure Canvas is active so children can be enabled
        if (reputationCanvas != null)
            reputationCanvas.SetActive(true);

        // Keep children inactive initially
        if (reputationText != null)
            reputationText.gameObject.SetActive(false);

        if (reputationLabelPNG != null)
            reputationLabelPNG.SetActive(false);
    }

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        userId = PlayerPrefs.GetString("userId", "");

        Debug.Log("Loaded userId from PlayerPrefs: " + userId);

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("No userId found! Make sure user is logged in.");
            return;
        }

        // Delay activation one frame to make sure UI is ready
        StartCoroutine(ActivateAndLoad());
    }

    private IEnumerator ActivateAndLoad()
    {
        // Wait one frame
        yield return null;

        // Activate UI elements
        if (reputationText != null)
            reputationText.gameObject.SetActive(true);
        if (reputationLabelPNG != null)
            reputationLabelPNG.SetActive(true);

        // Load reputation from Firebase
        LoadReputation();
    }

    private void LoadReputation()
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                int reputation = 0;
                if (task.Result.Exists)
                    int.TryParse(task.Result.Value.ToString(), out reputation);

                if (reputationText != null)
                    reputationText.text = reputation.ToString();
            }
            else
            {
                Debug.LogError("Failed to load reputation: " + task.Exception);
            }
        });
    }

    public void AddReputation(int amount)
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                int current = 0;
                if (task.Result.Exists)
                    int.TryParse(task.Result.Value.ToString(), out current);

                int newReputation = current + amount;
                dbRef.Child("users").Child(userId).Child("reputation").SetValueAsync(newReputation);

                if (reputationText != null)
                    reputationText.text = newReputation.ToString();
            }
        });
    }
}
