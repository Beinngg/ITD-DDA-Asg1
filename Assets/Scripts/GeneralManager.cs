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

    private int currentMenuIndex = -1; // 🔑 track active menu

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        SetInitialUI();
    }

    /* ================= INITIAL ================= */

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

    /* ================= PANEL NAV ================= */

    public void ShowLoginPanel()
    {
        SafeSet(startPanel, false);
        SafeSet(loginPanel, true);
        SafeSet(signupPanel, false);
    }

    public void ShowSignupPanel()
    {
        SafeSet(startPanel, false);
        SafeSet(signupPanel, true);
        SafeSet(loginPanel, false);
    }

    public void BackToStart()
    {
        SafeSet(startPanel, true);
        SafeSet(loginPanel, false);
        SafeSet(signupPanel, false);
        SafeSet(mainPanel, false);
    }

    /* ================= AUTH ================= */

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(
            loginEmailInput.text,
            loginPasswordInput.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception);
                return;
            }

            OnAuthSuccess(task.Result.User);
        });
    }

    public void Signup()
    {
        auth.CreateUserWithEmailAndPasswordAsync(
            signupEmailInput.text,
            signupPasswordInput.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception);
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
    }

    /* ================= REPUTATION ================= */

    void InitUserData()
    {
        dbRef.Child("users").Child(userId).Child("reputation")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.Result.Exists)
                    dbRef.Child("users").Child(userId).Child("reputation").SetValueAsync(0);
            });
    }

    void LoadReputation()
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

    /* ================= MENU SYSTEM ================= */

    void HideAllMenuContent()
    {
        foreach (var panel in menuContentParents)
            if (panel != null) panel.SetActive(false);

        currentMenuIndex = -1;
    }

    public void OpenMenuContent(int index)
    {
        if (index < 0 || index >= menuContentParents.Length) return;

        HideAllMenuContent();
        menuContentParents[index].SetActive(true);
        currentMenuIndex = index;
    }

    // 🔥 THIS IS THE FIX
    public void BackFromMenuContent()
    {
        if (currentMenuIndex == -1) return;

        menuContentParents[currentMenuIndex].SetActive(false);
        currentMenuIndex = -1;
    }

    /* ================= BUTTON ACTIONS ================= */

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

        if (!state)
            HideAllMenuContent();
    }

    /* ================= UTIL ================= */

    void SafeSet(GameObject obj, bool state)
    {
        if (obj != null) obj.SetActive(state);
    }
    public void CloseMenuContentEntirely()
{
    // Close all opened content panels
    HideAllMenuContent();

    // Close the menu container itself
    if (extraUIParent != null)
        extraUIParent.SetActive(false);
}

}
