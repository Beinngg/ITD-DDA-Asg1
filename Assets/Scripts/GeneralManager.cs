using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using TMPro;

public class GeneralManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    public string userId;

    // Input fields
    [Header("Login Inputs")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    [Header("Signup Inputs")]
    public TMP_InputField signupEmailInput;
    public TMP_InputField signupPasswordInput;

    // UI Panels
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject mainPanel;

    // Reputation UI
    public TMP_Text reputationText;      // number only
    public GameObject reputationLabelPNG; // your "Reputation:" PNG

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Show only start panel
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        mainPanel.SetActive(false);

        // Hide reputation UI until Start is pressed
        if (reputationText != null)
            reputationText.gameObject.SetActive(false);
        if (reputationLabelPNG != null)
            reputationLabelPNG.SetActive(false);
    }

    #region Panel Controls
    public void ShowLoginPanel()
    {
        startPanel.SetActive(false);
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void ShowSignupPanel()
    {
        startPanel.SetActive(false);
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void BackToStart()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        mainPanel.SetActive(false);
    }
    #endregion

    #region Firebase Authentication
    public void Login()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Login failed: " + task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;
                userId = user.UserId;

                PlayerPrefs.SetString("userId", userId);
                PlayerPrefs.Save();

                Debug.Log("Login successful: " + user.Email);

                loginPanel.SetActive(false);
                mainPanel.SetActive(true);

                InitializeUserData();
            });
    }

    public void Signup()
    {
        string email = signupEmailInput.text;
        string password = signupPasswordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Sign Up Failed: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result.User;
                userId = newUser.UserId;

                PlayerPrefs.SetString("userId", userId);
                PlayerPrefs.Save();

                Debug.Log("Sign Up Success! User: " + newUser.Email);

                signupPanel.SetActive(false);
                mainPanel.SetActive(true);

                InitializeUserData();
            });
    }
    #endregion

    #region Reputation System
    private void InitializeUserData()
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.Result.Exists)
                {
                    dbRef.Child("users").Child(userId).Child("reputation").SetValueAsync(0);
                }
            });
    }

    private void ActivateReputationUI()
    {
        if (reputationText != null)
            reputationText.gameObject.SetActive(true);
        if (reputationLabelPNG != null)
            reputationLabelPNG.SetActive(true);

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
    #endregion

    // Called by "Start" button in mainPanel
    public void StartGame()
    {
        // Hide the main panel
        mainPanel.SetActive(false);

        // Activate reputation UI
        ActivateReputationUI();

        // Start your AR session here if needed
        // e.g., arSessionPrefab.SetActive(true);
    }
}
