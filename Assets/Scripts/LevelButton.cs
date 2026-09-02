using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;

    void Start()
    {
        Button btn = GetComponent<Button>();

        // Defaults to 1 for first-time players so Level 1 is always unlocked
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);

        if (levelReached < level)
        {
            btn.interactable = false;
        }
    }
}