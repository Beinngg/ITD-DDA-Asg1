using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject signupPanel;

    public void OpenLogin()
    {
        startPanel.SetActive(false);
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void OpenSignup()
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
    }
}
