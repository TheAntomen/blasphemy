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
    private Vector2 direction;
    private Vector2 force;

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

        if (currentWaypoint + unit.range >= path.vectorPath.Count)
        {



            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * unit.speed * Time.deltaTime;

        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float distanceToTarget = Vector2.Distance(rb.position, knight.transform.position);

        if (distance < nextWaypointDistance) currentWaypoint++;

        // If the player is within range of the enemy, it will attack
        if (distanceToTarget < unit.range) unit.Attack();

        // Avoid value too close 0 to nullify artifacts, flip sprite based on direction
        if (!Mathf.Approximately(direction.x, 0.0f))
        {
            if (direction.x >= 0)
            {
                unit.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                unit.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    /// <summary>
    /// Method to make the enemy run away from the player, if this is a desired behaviour
    /// </summary>
    private void EnemyKiting()
    {
        direction = -((Vector2)knight.transform.position - rb.position).normalized;
        force = direction * unit.speed * Time.deltaTime;
        rb.AddForce(force);
    }
}
