using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject startMainMenu;
    public GameObject levelSelect;

    public void StartButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);         
    }

    public void GoToNextLevel()
    {
        startMainMenu.SetActive(false);
        levelSelect.SetActive(true); 
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
