using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Class for handling the ghouls pathing and attacks towards the knight
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // Public variables
    public bool reachedEndOfPath = false;
    public float nextWaypointDistance = 3f;
    public GameObject knight;
    public Enemy unit;

    // Private variables
    private int currentWaypoint = 0;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knight = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating("UpdatePath", 0f, 0.5f);        
    }

    void UpdatePath()
    {
        if (knight != null)
        {
            if (seeker.IsDone()) seeker.StartPath(rb.position, knight.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float distanceToTarget = Vector2.Distance(rb.position, knight.transform.position);
        float speed = unit.speed;

        FlipTransform(direction);

        // If the unit is supposed to flee from player and "kite", negate direction and slow speed
        if (unit.kitingSpeed > 0 && distanceToTarget < unit.range / 2)
        {
            direction = -direction;
            speed = unit.kitingSpeed;
        }

        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        // If the player is within range of the enemy, it will attack
        if (distanceToTarget < unit.range) unit.Attack(knight);

        
        if (distance < nextWaypointDistance) currentWaypoint++;

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    /// <summary>
    /// Flips the sprite of the unit based on the direction of it's path
    /// </summary>
    /// <param name="direction"></param>
    private void FlipTransform(Vector2 direction)
    {
        if (!Mathf.Approximately(direction.x, 0.0f))
        {
            if (direction.x >= 0)
            {
                unit.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                unit.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
    }
}
