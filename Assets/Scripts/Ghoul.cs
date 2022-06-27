using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the AI-controlled ghoul enemies
/// </summary>
public class Ghoul : MonoBehaviour
{

    // Properties
    public int maxHp { get; set; }
    public int damage { get; set; }
    public int currentHp { get; set; }
    public float maxSpeed { get; set; }
    public float range { get; set; }

    // Public variables
    public int MAX_HP = 20;
    public int DAMAGE = 5;
    public float MAX_SPEED = 300.0f;
    public float RANGE = 1.5f;
    public AudioClip hitClip;

    // Private variables
    private Color regularColor;
    private Color damageColor;
    private bool damageTaken;
    private float flashTime = 0.4f;
    private float flashTimer;
    private int difficulty;
    Animator animator;
    SpriteRenderer s_renderer;
    Rigidbody2D rb;
    GhoulAI ai;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        difficulty = StateNameController.difficulty;

        maxHp = MAX_HP * difficulty;
        damage = DAMAGE * difficulty;
        currentHp = MAX_HP * difficulty;
        maxSpeed = MAX_SPEED * difficulty;
        range = RANGE;


        animator = GetComponent<Animator>();
        s_renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ai = GetComponent<GhoulAI>();
        audioSource = GetComponent<AudioSource>();

        ai.speed = maxSpeed;

        regularColor = s_renderer.color;
        damageColor = new Color(1.000f, 0f, 0f, 1.000f);
    }

    private void Update()
    {
        if (currentHp == 0)
        {
            animator.SetBool("Dead", true);
        }
        if (damageTaken)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0)
            {
                damageTaken = false;
                CancelInvoke();

            }
        }
    }

    /// <summary>
    /// Change the ghoul´s health based on the amount of damage taken
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !damageTaken)
        {
            PlaySound(hitClip);
            flashTimer = flashTime;
            damageTaken = true;
            InvokeRepeating("DamageTaken", 0.0f, 0.1f);
        }
        currentHp = Mathf.Clamp(currentHp + amount, 0, MAX_HP);
    }

    /// <summary>
    /// Displays effects of the ghoul taking damage, swithcing between colors
    /// </summary>
    public void DamageTaken()
    {
        if (String.Equals(s_renderer.color.ToString(), damageColor.ToString()))
        {
            s_renderer.color = regularColor;
        }
        else if (String.Equals(s_renderer.color.ToString(), regularColor.ToString()))
        {
            s_renderer.color = damageColor;
        }
        else
        {
            s_renderer.color = regularColor;
        }


    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
