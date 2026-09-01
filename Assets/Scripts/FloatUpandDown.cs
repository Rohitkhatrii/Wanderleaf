using UnityEngine;

public class FloatUpandDown : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 2f;

    private Vector3 startPos;   

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;            //Time.time is exactly how many seconds have passed since the moment your game started playing.
        transform.localPosition = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
