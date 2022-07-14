using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Class for handling the ghouls pathing and attacks towards the knight
/// </summary>
public class GhoulAI : MonoBehaviour
{
    // Properties
    public float speed { get; set; }

    // Public variables
    public GameObject knight;
    public float nextWaypointDistance = 3f;

    // Private variables
    private Vector2 lookDirection;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Vector2 direction;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;
    private Ghoul enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Ghoul>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knight = GameObject.FindGameObjectWithTag("Player");
        speed = 200f;


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


        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float distanceToTarget = Vector2.Distance(rb.position, knight.transform.position);


        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (!Mathf.Approximately(direction.x, 0.0f))
        {
            lookDirection.x = direction.x;
            lookDirection.Normalize();
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Velocity", rb.velocity.x);
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        Knight player = other.gameObject.GetComponent<Knight>();

        if (player != null)
        {
            int dmg = enemy.DAMAGE;
            animator.SetFloat("Direction", lookDirection.x);
            animator.SetTrigger("Attack");
            player.ChangeHealth(-dmg);
        }

    }

}
