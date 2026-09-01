using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void loadCurrentScene()
    {
        Time.timeScale = 1;     // this is the normal time scale
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
