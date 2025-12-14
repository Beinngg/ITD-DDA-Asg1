using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCanvasController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
