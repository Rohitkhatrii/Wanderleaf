using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName;
    public int nextLevelvalue;

    public void LoadNextLevel()
    {
        // Only update progress forward so replaying earlier levels doesn't relock progress
        if (nextLevelvalue > PlayerPrefs.GetInt("LevelReached", 1))
        {
            PlayerPrefs.SetInt("LevelReached", nextLevelvalue);
            PlayerPrefs.Save(); // Ensures it writes immediately to browser IndexedDB
        }

        Time.timeScale = 1;
        Checkpoint.savedPosition = Vector2.zero;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
    }
}