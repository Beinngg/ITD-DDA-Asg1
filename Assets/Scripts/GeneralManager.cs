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
    private string userId;

    [Header("Login Inputs")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    [Header("Signup Inputs")]
    public TMP_InputField signupEmailInput;
    public TMP_InputField signupPasswordInput;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject mainPanel;

    [Header("Reputation UI")]
    public GameObject reputationUIRoot;
    public TMP_Text reputationText;

    [Header("Extra UI")]
    public GameObject extraUIParent;

    [Header("Menu Content Parents")]
    public GameObject[] menuContentParents;

    [Header("Error Messages")]
    public TMP_Text loginErrorText;
    public TMP_Text signupErrorText;

    private int currentMenuIndex = -1;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        SetInitialUI();
    }

    void SetInitialUI()
    {
        SafeSet(startPanel, true);
        SafeSet(loginPanel, false);
        SafeSet(signupPanel, false);
        SafeSet(mainPanel, false);
        SafeSet(reputationUIRoot, false);
        SafeSet(extraUIParent, false);

        HideAllMenuContent();
    }

    public void ShowLoginPanel()
    {
        SafeSet(startPanel, false);
        SafeSet(loginPanel, true);
        SafeSet(signupPanel, false);
        loginErrorText.text = "";
    }

    public void ShowSignupPanel()
    {
        SafeSet(startPanel, false);
        SafeSet(signupPanel, true);
        SafeSet(loginPanel, false);
        signupErrorText.text = "";
    }

    public void BackToStart()
    {
        SafeSet(startPanel, true);
        SafeSet(loginPanel, false);
        SafeSet(signupPanel, false);
        SafeSet(mainPanel, false);
    }

    public void Login()
    {
        loginErrorText.text = "";

        auth.SignInWithEmailAndPasswordAsync(loginEmailInput.text, loginPasswordInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    loginErrorText.text = ParseAuthError(task.Exception, true);
                    Debug.LogError("Login failed: " + task.Exception);
                    return;
                }

                OnAuthSuccess(task.Result.User);
            });
    }

    public void Signup()
    {
        signupErrorText.text = "";

        auth.CreateUserWithEmailAndPasswordAsync(signupEmailInput.text, signupPasswordInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    signupErrorText.text = ParseAuthError(task.Exception, false);
                    Debug.LogError("Signup failed: " + task.Exception);
                    return;
                }

                OnAuthSuccess(task.Result.User);
            });
    }

    void OnAuthSuccess(FirebaseUser user)
    {
        userId = user.UserId;
        PlayerPrefs.SetString("userId", userId);
        PlayerPrefs.Save();

        SafeSet(loginPanel, false);
        SafeSet(signupPanel, false);
        SafeSet(mainPanel, true);

        InitUserData();
        LoadReputation();
    }

    void InitUserData()
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.Result.Exists)
                    dbRef.Child("users").Child(userId).Child("reputation").SetValueAsync(0);
            });
    }

    public void LoadReputation()
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                int rep = 0;
                if (task.Result.Exists)
                    int.TryParse(task.Result.Value.ToString(), out rep);

                reputationText.text = rep.ToString();
            });
    }

    public void AddReputation(int amount)
    {
        if (string.IsNullOrEmpty(userId)) return;

        DatabaseReference repRef = dbRef.Child("users").Child(userId).Child("reputation");

        repRef.RunTransaction(mutableData =>
        {
            int currentRep = 0;

            if (mutableData.Value != null)
                int.TryParse(mutableData.Value.ToString(), out currentRep);

            mutableData.Value = currentRep + amount;
            return TransactionResult.Success(mutableData);

        }).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                LoadReputation();
                Debug.Log($"+{amount} Reputation added");
            }
            else
            {
                Debug.LogError("Failed to add reputation");
            }
        });
    }

    void HideAllMenuContent()
    {
        foreach (var panel in menuContentParents)
            if (panel != null) panel.SetActive(false);

        currentMenuIndex = -1;
    }

    public void OpenMenuContent(int index)
    {
        if (index < 0 || index >= menuContentParents.Length) return;

        // Ensure parent is active
        if (extraUIParent != null && !extraUIParent.activeSelf)
            extraUIParent.SetActive(true);

        HideAllMenuContent();
        menuContentParents[index].SetActive(true);
        currentMenuIndex = index;
    }

    public void BackFromMenuContent()
    {
        if (currentMenuIndex == -1) return;

        menuContentParents[currentMenuIndex].SetActive(false);
        currentMenuIndex = -1;
    }

    public void StartGame()
    {
        SafeSet(mainPanel, false);
        SafeSet(reputationUIRoot, true);
        LoadReputation();
    }

    public void ToggleExtraUI()
    {
        if (extraUIParent == null) return;

        bool state = extraUIParent.activeSelf;
        extraUIParent.SetActive(!state);

        // Only hide menu content when closing
        if (state)
            HideAllMenuContent();
    }

    public void CloseMenuContentEntirely()
    {
        HideAllMenuContent();
        if (extraUIParent != null)
            extraUIParent.SetActive(false);
    }

    void SafeSet(GameObject obj, bool state)
    {
        if (obj != null) obj.SetActive(state);
    }

    private string ParseAuthError(AggregateException exception, bool isLogin)
    {
        if (exception == null || exception.InnerExceptions == null || exception.InnerExceptions.Count == 0)
            return isLogin ? "Login failed. Try again." : "Signup failed. Try again.";

        foreach (var e in exception.InnerExceptions)
        {
            string msg = e.Message.ToLower();

            if (msg.Contains("invalid-email"))
                return "Email format is invalid.";
            if (msg.Contains("user-not-found") && isLogin)
                return "No account found with this email.";
            if (msg.Contains("wrong-password") && isLogin)
                return "Password is incorrect.";
            if (msg.Contains("email-already-in-use") && !isLogin)
                return "This email is already registered.";
            if (msg.Contains("weak-password") && !isLogin)
                return "Password too weak (min 6 characters).";
            if (msg.Contains("network-request-failed"))
                return "Network error. Please try again.";
        }

        return isLogin ? "Login failed. Try again." : "Signup failed. Try again.";
    }
}
