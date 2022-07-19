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
    public int CurrentHealth { get; set; }
    public bool DamageTaken { get; set; }

    // Public variables
    public AudioClip hitClip;
    public bool boss;

    // Private variables
    private int currentHp;
    Animator animator;
    AudioSource audioSource;
    GameController controller;
    GhoulAI ai;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ai = GetComponent<GhoulAI>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        currentHp = health;
    }

    protected override void Update()
    {

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

    /// <summary>
    /// Change the ghoul´s health based on the amount of damage taken
    /// </summary>
    /// <param name="amount"></param>
    /// 
    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !damageTaken)
        {
            PlaySound(hitClip);
            StartCoroutine(DamageFlash());
        }
        currentHp = Mathf.Clamp(currentHp + amount, 0, health);

        if (currentHp == 0)
        {
            if (boss) controller.FloorComplete();
            ai.reachedEndOfPath = true;
            controller.enemies.Remove(this.gameObject);
            animator.SetBool("Dead", true);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
