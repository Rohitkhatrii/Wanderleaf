using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject WinUI;          // we will take WinUI GameObject as a reference in Flag in the inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;      // means game time is essentially paused. Normal game time is Time.timeScale = 1; 

            WinUI.SetActive(true);
        }
    }

}
