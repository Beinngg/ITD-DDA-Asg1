using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public string userId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
