using System.Collections;
using UnityEngine;

// we dont want the player to jump on falling platform two or three times because that is not good for working of the game and will give bugs later. thats why we are using isFalling variable
public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall = 0.4f;
    public float destroyGameObject = 2f;

    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    { 
        isFalling = true;            // isFalling value is true now and will stay true 
        yield return new WaitForSeconds(timeBeforeFall);        // This line means wait for seconds then do what told  
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(transform.parent.gameObject, destroyGameObject);    // it takes 2 parameters , hover over on the methodName to know 
    }
}