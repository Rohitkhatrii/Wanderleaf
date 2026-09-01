using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction= dir;
        Destroy(gameObject, lifeTime);               // bullet gets destroyed after 3 seconds
    }

    void Update()
    {
        //translate method moves the object a specified distance and direction from its current position. //Time.deltaTime is used to work mechanics same on all device regardless of hardware. there should be no difference b/w 60 fps pc and 30 fps pc.
        transform.Translate(direction * speed * Time.deltaTime);    // suppose direction is (1,0). Then 1,0 * 10 * 0.016 ans= (0.16,0) again update method runs then 1,0 * 10 * 0.016 ans= (0.16,0) so translate will keep on moving the object. 
    }

    
}
