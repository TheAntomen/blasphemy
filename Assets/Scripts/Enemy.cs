using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int CurrentHealth { get; set; }

    [SerializeField]
    protected bool boss;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float attackRate;
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    public float range;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float kitingSpeed;
    [SerializeField]
    public Enemy[] rareVariants;
    [SerializeField]
    public AudioClip hitClip;


    protected bool damageTaken;
    private Color damageColor = new Color(1.000f, 0f, 0f, 1.000f);
    private int damageFlashCount = 6;
    protected Animator animator;
    protected Rigidbody2D rb;

    AudioSource audioSource;
    GameController controller;
    EnemyAI ai;
    
    protected void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ai = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        CurrentHealth = health;
    }

    public abstract void Attack(GameObject target);


    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !damageTaken)
        {
            if (hitClip != null)
            {
                PlaySound(hitClip);
            }

            StartCoroutine(DamageFlash());
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, health);

        if (CurrentHealth == 0)
        {
            if (boss) controller.FloorComplete();
            rb.simulated = false;
            ai.reachedEndOfPath = true;
            controller.Enemies.Remove(this);
            animator.SetTrigger("Dead");
        }
    }

    public IEnumerator DamageFlash()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color defaultColor = renderer.color;
        
        for (int state = 0; state < damageFlashCount; state++)
        {
            if ((state % 2) == 0)
            {
                renderer.color = damageColor;
            }else
            {
                renderer.color = defaultColor;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
