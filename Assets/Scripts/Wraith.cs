using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : Enemy, IDamageable
{
    // Properties
    public bool DamageTaken { get; set; }

    // Public variables

    // Private variables
    [SerializeField]
    private GameObject projectilePrefab;
    private float attackCooldown;

    public void Awake()
    {
        attackCooldown = attackRate;
    }

    public override void Attack(GameObject target)
    {

        attackCooldown -= Time.deltaTime;


        if (attackCooldown <= 0)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(InstantiateProjectile(target));
            attackCooldown = attackRate;
        }

    }

    private IEnumerator InstantiateProjectile(GameObject target)
    {
        yield return new WaitForSeconds(attackDelay);
        Vector2 launchPos = transform.GetChild(0).transform.position;
        GameObject projectile = Instantiate(projectilePrefab, launchPos, Quaternion.identity);

        //calculate aimed target
        BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
        Rigidbody2D targetbody = target.GetComponent<Rigidbody2D>();
        Vector2 aimTarget = (Vector2)targetCollider.bounds.center + targetbody.linearVelocity;

        Vector2 direction = (aimTarget - rb.position).normalized;
        projectile.transform.right = direction;
        if (direction.x < 0) projectile.transform.Rotate(180, 0, 0);
        projectile.GetComponent<Projectile>().Launch(direction);
    }
}
