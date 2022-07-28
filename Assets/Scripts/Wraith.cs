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

    public override void Attack(Vector2 direction)
    {

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0)
        {
            animator.SetTrigger("Attack");
            Vector2 launchPos = transform.GetChild(0).transform.position;
            Vector2 relativePosition = new Vector2(launchPos.x, launchPos.y) + direction;
            GameObject projectile = Instantiate(projectilePrefab, launchPos, Quaternion.identity);
            projectile.transform.right = direction;
            projectile.GetComponent<Projectile>().Invoke(Launch(direction));
            attackCooldown = attackRate;
        }
    }
}
