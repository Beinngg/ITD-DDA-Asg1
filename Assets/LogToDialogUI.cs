using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{
    public static DialogUI I { get; private set; }

    public TMP_Text dialogText;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
    }

    public void Show(string text)
    {
        if (dialogText == null) return;
        dialogText.text = text;
    }

    public void Clear()
    {
        if (dialogText == null) return;
        dialogText.text = "";
    }
}
