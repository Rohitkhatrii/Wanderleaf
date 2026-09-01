using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public float detectionRange = 10f;
    public float updateRate = 0.2f;

    private Transform player;
    private NavMeshAgent agent;
    private float nextUpdateTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        //These two lines are critical for 2D games. NavMesh was originally built for 3D. These lines stop the enemy from trying to tilt, flip, or rotate on 3D axes while moving across your 2D screen.
        agent.updateRotation = false;             
        agent.updateUpAxis = false;

        player = GameObject.FindWithTag("Player").transform;
    }


    //Calculating a path around obstacles takes processing power. Instead of recalculating the path 60 times a second, the script uses Time.time >= nextUpdateTime to wait. It only updates the path every 0.2f seconds (updateRate). This keeps the game running smoothly even if you have many enemies.
    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= detectionRange)
        {
            if(Time.time >= nextUpdateTime)
            {
                agent.SetDestination(player.position);
                nextUpdateTime = Time.time + updateRate;
            }
        }
        else
        {
            agent.ResetPath();
        }
    }
}
