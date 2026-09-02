using System.ComponentModel;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject Container;
    public GameObject touchControls;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Container.SetActive(true);
            touchControls.SetActive(false);
            Time.timeScale= 0;
        }
    }

    public void ResumeButton()
    {
        Container.SetActive(false);
        touchControls.SetActive(true);
        Time.timeScale= 1;
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
