using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName;
    public int nextLevelvalue;

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("LevelReached", nextLevelvalue);       // used to save our game in unity // here we setint it and we can retrieve it via getint
        Time.timeScale = 1;     // this is the normal time scale
        Checkpoint.savedPosition = Vector2.zero;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
    }
}
