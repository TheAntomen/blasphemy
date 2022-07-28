using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the AI-controlled ghoul enemies
/// </summary>
public class Ghoul : Enemy, IDamageable
{
    // Properties
    public bool DamageTaken { get; set; }

    public override void Attack(Vector2 direction)
    {
        throw new NotImplementedException();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        Knight player = other.gameObject.GetComponent<Knight>();

        if (player != null)
        {
            int dmg = damage;
            animator.SetTrigger("Attack");
            player.ChangeHealth(-dmg);
        }
    }

}
