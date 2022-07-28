using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for projectile fired by enemies, may make this abstract later
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float launchForce;
    [SerializeField]
    private int damage;

    Rigidbody2D rigidbody2d;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.AddForce(direction * launchForce);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Knight player = other.collider.GetComponent<Knight>();

        if (player != null)
        {
            player.ChangeHealth(-damage);
        }

        animator.SetTrigger("Hit");
    }
}
