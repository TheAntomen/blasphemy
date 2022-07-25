using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : Enemy, IDamageable
{
    // Properties
    public bool DamageTaken { get; set; }

    // Public variables

    // Private variables

    protected override void Update()
    {
    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
