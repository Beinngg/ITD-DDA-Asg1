using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject rootPanel; 
    public Text dialogueText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string text)
    {
        if (rootPanel != null) rootPanel.SetActive(true);
        if (dialogueText != null) dialogueText.text = text;
    }

    public void Hide()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
    }
}
