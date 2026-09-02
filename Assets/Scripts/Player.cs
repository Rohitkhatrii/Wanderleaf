using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public int coins;
    public int health = 100;
    public float moveSpeed = 3f;
    public float jumpForce = 3.5f;
    public float jumpForceContinues = 0.0005f;                    //for variable jumps game mechanic
    public Transform groundCheck;                       // made them public so that they can appear in Inspector
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Image healthImage;

    public AudioClip jumpClip;
    public AudioClip hurtClip;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;

    private float fireTimer;

    [Header("Mobile Touch Controls")]
    public TouchButton leftBtn;                     // directly using TouchButton class because we have already created a script called TouchButton.cs 
    public TouchButton rightBtn;
    public TouchButton jumpBtn;
    public TouchButton shootBtn;
    private bool wasJumpPressed;                    // Tracks jump button state to simulate GetKeyDown

    private Rigidbody2D rb;
    private bool isGrounded;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;

    public int extraJumpsValue = 1;
    private int extraJumps;

    public float coyoteTime = 0.3f;              //Coyote Time is a game mechanic that lets the player jump for a very short time after walking off a platform.
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.15f;         //jump buffer is a game mechanic that remembers a jump pressed slightly before landing.
    private float jumpBufferCounter;

    public float jumpHoldTime = 0.005f;
    private float jumpHoldCounter;

    public bool speedBoost;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    // we got the Rigidbody2D component from the components attached to the GameObject where this script is attached to. // we stored it in rb to access its members later on.
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        healthImage = GameObject.FindWithTag("Health").GetComponent<Image>();     //just note G is capital in GameObject so what GameObject does it basically searches for other objects in the scene

        extraJumps = extraJumpsValue;

        if(Checkpoint.savedPosition != Vector2.zero)                // checkpoint
        {
            transform.position = Checkpoint.savedPosition; 
        }
    }


    void Update()
    {   
        //move input can be either -1 or 1 or 0 .      
        float moveInput = Input.GetAxis("Horizontal");            // we are using old input manager   // we write input in Update method because we want to check every frame for the input. 
        
        // Mobile Touch override for horizontal movement
        if (leftBtn != null && leftBtn.isPressed) moveInput = -1f;
        else if (rightBtn != null && rightBtn.isPressed) moveInput = 1f;

        if(rb.linearVelocityX != 0)            // if player is moving left or right just not at 0 thats it 
        {
            if(rb.linearVelocityX > 0)           // means if player going right 
            {
                spriteRenderer.flipX = false;
            }
            else                                
            {
                spriteRenderer.flipX = true;
            }
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumps  = extraJumpsValue;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;        //Decrease the coyote-time counter as time passes.
        }

        // Mobile Touch + Keyboard override for Jump logic
        bool isJumpHeld = Input.GetKey(KeyCode.Space) || (jumpBtn != null && jumpBtn.isPressed);
        bool isJumpDown = Input.GetKeyDown(KeyCode.Space) || (isJumpHeld && !wasJumpPressed);
        wasJumpPressed = isJumpHeld; // Update the state for the next frame

        if (isJumpDown)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;         // decrease the jumpbuffer time window 
        }

        if (jumpBufferCounter > 0f)
        {
            if(coyoteTimeCounter > 0f)          // if the player is still within the coyote-time window, allow the jump.
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpHoldCounter = jumpHoldTime;
                PlaySFX(jumpClip);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                PlaySFX(jumpClip);
                jumpBufferCounter = 0f;
            }
        }

        if (isJumpHeld && jumpHoldCounter > 0f)
        {
            rb.AddForceY(jumpForceContinues * Time.deltaTime * 100f);
            jumpHoldCounter -= Time.deltaTime;
        }
        else
        {
            jumpHoldCounter = 0f;
        }
        
        SetAnimation(moveInput);        // calling method and taking arguement moveInput

        healthImage.fillAmount = health/100f;

        if(rb.linearVelocityY < 0)          // suppose a ground. y=0 means player is same level as ground , y = -1 he is below ground , y=1 above the ground
        {
            rb.gravityScale = 3f;       // making gravity scale 3 from by default value 2 in inspector
        }
        else
        {
            rb.gravityScale = 2f;      // gravityScale = 2 its by default set to 2 in Inpector
        }

        if(transform.position.y < -15)        // if player goes like -11 then if(-11 < -10) is true because in negative numbers number closer to 0 is greater
        {
            Die();
        }

        HandleShooting();
    }

    private void FixedUpdate()                    // we write physics-related work in Fixed Update 
    {
        // if its on ground then jump 
        // Check whether any 2D Collider is overlapping an imaginary circle at a specific position.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;   // isGrounded takes boolean value so to check if its null or not we use !=null

        float moveInput = Input.GetAxis("Horizontal");

        // Mobile Touch override for physics loop
        if (leftBtn != null && leftBtn.isPressed) moveInput = -1f;
        else if (rightBtn != null && rightBtn.isPressed) moveInput = 1f;

        // here we generate a good amount of force
        rb.AddForce(new Vector2(moveInput * moveSpeed * 50, 0f), ForceMode2D.Force);        

        // here then we instantly limited it via clamp method // clamp method: suppose linearvelocityx is 5 so clamp will be like clamp(5,-3,+3) answer= 3 for more understanding check notes.
        if(!speedBoost)        // When hitting the Speed Pad: speedBoost becomes true (see SpeedPad script for more understanding). The ! flips it to false. Because the if statement sees false, it completely ignores the clamp code. This allows the physics engine to throw your player at super speed without being artificially slowed down.
        {   
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocityX, -moveSpeed, moveSpeed), rb.linearVelocityY);   
        }    
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if(moveInput == 0)
            {
                animator.Play("Player_Idle");    // we write the name of animation also such as "Player_Idle"
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
            {
                animator.Play("Player_fall");
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)      // method which runs automatically when a collision happens 
    {
        if(collision.gameObject.tag == "Damage")               //collision.gmaeobject idar vo hai jisse collide kiya apna current script ka gameobject 
        {
            PlaySFX(hurtClip);
            health -= 25;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StartCoroutine(BlinkRed());                // we call coroutine by StartCoroutine();

            if(health <= 0)
            {
                Die();
            }
        }
        else if(collision.gameObject.tag == "BouncePad")
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2);

        }
    }
 
    private IEnumerator BlinkRed()                    // coroutine      // IEnumerator is just a Type
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);                 
        spriteRenderer.color = Color.white; 
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);               
    }
    public void PlaySFX(AudioClip audioClip, float volume = 1f)            
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Strawberry")
        {
            extraJumpsValue = 2;
            Destroy(collision.gameObject);
        }
    }

    private void HandleShooting()
    {
        //suppose game is running at 60fps. Then 1/60 = 0.016. then fireTimer -= Time.deltaTime will be 0-0.016 = -0.016 and it gets passed in if condition below. then fireTimer = fireRate; makes fireTimer to 0.3. Then 0.3-0.016=0.284 so now if condition is not true so again 0.284-0.016=0.268 .so until it gets less than 0 it will not execute if block and the moment if block runs it executes the shoot method the shoot method contains instantiate which clones the objects into the scene.
        fireTimer -= Time.deltaTime;

        // Combines Mouse Left Click and Touch Button
        bool isShooting = Input.GetMouseButton(1) || (shootBtn != null && shootBtn.isPressed);             //GetMouseButton(0) - LMB  , GetMouseButton(1) - RMB   

        if(isShooting  && fireTimer < 0f)             
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void Shoot()
    {

        //Instantiate(): This is Unity's built-in method to clone or spawn a new object into the scene. It takes 3 parameters.
        //Quaternion.identity is a built-in math shortcut that simply means "zero rotation" or "perfectly straight."
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (spriteRenderer.flipX)
        {
            bulletScript.SetDirection(Vector2.left);
        }
        else
        {
            bulletScript.SetDirection(Vector2.right);
        }
    }
}