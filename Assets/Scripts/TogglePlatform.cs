using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite; 
    public float onDuration = 1f;
    public float offDuration = 1f;
    public bool startOn = true;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool isOn;
    private float timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()             // executes code exactly once in a script's lifetime
    {
        isOn = startOn;
        col.enabled = isOn;
        sr.sprite = isOn ? onSprite : offSprite;
        timer = isOn ? onDuration : offDuration;
    }

    void Update()              // update method runs again and again so here is will make sprite on off on off // this code below will work like false false offsprite offduration then true true onsprite onduration then again false false offsprite offduration
    {                          
        timer -= Time.deltaTime;         // Count down the timer in real-world seconds // basically suppose game is running at 60fps then 1/60= 0.01666 . 0.0166 × 60 frames = exactly 1.0 second. so it took exactly 1 second. basically time.deltaTime works like a stopwatch.
        if(timer <= 0)
        {
            isOn = !isOn;
            col.enabled = isOn;
            sr.sprite = isOn ? onSprite : offSprite;
            timer = isOn ? onDuration : offDuration;
        }
    }
}
