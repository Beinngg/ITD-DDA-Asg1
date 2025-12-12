using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    // UI Panels
    public GameObject startPanel;   // initial start screen
    public GameObject loginPanel;   // login screen
    public GameObject signupPanel;  // signup screen
    public GameObject mainPanel;    // main screen after successful login/signup

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Show only the start panel initially
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    // Called by "Login" button on start screen
    public void ShowLoginPanel()
    {
        startPanel.SetActive(false);
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    // Called by "Signup" button on start screen
    public void ShowSignupPanel()
    {
        startPanel.SetActive(false);
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    // Called by "Login" button on login panel
    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception);
                return; // stay on login panel
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("Login successful: " + user.Email);

            // Show main panel only on success
            loginPanel.SetActive(false);
            startPanel.SetActive(false);
            signupPanel.SetActive(false);
            mainPanel.SetActive(true);
        });
    }

    // Called by "Signup" button on signup panel
    public void Signup()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sign Up Failed: " + task.Exception);
                return; // stay on signup panel
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("Sign Up Success! User: " + newUser.Email);

            // Show main panel only on success
            signupPanel.SetActive(false);
            loginPanel.SetActive(false);
            startPanel.SetActive(false);
            mainPanel.SetActive(true);
        });
    }

}
