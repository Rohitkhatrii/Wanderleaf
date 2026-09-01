using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector2 savedPosition = Vector2.zero;             //Vector2.zero means (0,0) // static - It means this variable is global and shared   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            savedPosition = collision.transform.position;
        }
    }
}
