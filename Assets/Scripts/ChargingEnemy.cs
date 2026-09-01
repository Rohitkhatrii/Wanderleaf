using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    public float speed = 1f;
    public Transform[] points;              //in my code 2 points i have set

    public bool canCharge = false;
    public float chargeSpeed = 5f;
    public float sightDistance = 3f;
    public float loseDistance = 5f;
    public LayerMask playerLayer;

    private int i;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isCharging;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(canCharge && DetectPlayer())
        {
            isCharging = true;
        }
        if (isCharging)
        {
            ChargePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.25f)  
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);  // movetowards method contains parameters about current positon to target position.

        spriteRenderer.flipX = (transform.position.x - points[i].position.x) < 0f;          
    }
    
    bool DetectPlayer()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, sightDistance, playerLayer);

        if(hit.collider != null)
        {
            player  = hit.transform;
            return true;
        }
        
        return false;
    }
    void ChargePlayer()
    {
        if(player == null)
        {
            isCharging = false;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > loseDistance)
        {
            isCharging = false;
            player = null;
            return;
        }

        Vector2 target = new Vector2(player.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target, chargeSpeed * Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - player.position.x) < 0f;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
