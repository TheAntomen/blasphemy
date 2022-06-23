using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{

    private int hp;
    private float speed;
    private float range;

    public void Init(int hp, float speed, float range)
    {
        this.hp = hp;
        this.speed = speed;
        this.range = range;
    }

    public virtual int GetHitPoints() {return hp;}
    public virtual float GetSpeed() {return speed;}
    public virtual float GetRange() { return range; }
    public void TakeDamage(int dmg, GameObject creature)
    {
        hp -= dmg;

        Debug.Log("Creature " + creature + " took " + dmg + " damage. " + hp + " health left");
    }
}
