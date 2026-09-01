using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int i;

    void Start()
    {
        transform.position = points[0].position;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.01f)   // when i=0 distance is 0 so 0 < 0.01f is True. When i = 1 the distance is reduced means distance is now slight greater than 0 suppose it is 0.005 so 0.005<0.01f is true. 
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")      // collision kisse hua lets understand. GameObject Container hai toh container ka collison kisse hua player se hua that's why we wrote collision.gameObject which will give player 
        {
            collision.transform.SetParent(transform);  // player ka transorm jo hai vo set krdo container ka transform
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {     
        if(collision.gameObject.tag == "Player")
        {
            if (gameObject.activeInHierarchy)
            {
            collision.transform.SetParent(null);
            }
        }
    }
    
}
