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

    protected bool damageTaken;
    private Color damageColor = new Color(1.000f, 0f, 0f, 1.000f);
    private int damageFlashCount = 6;
    protected Animator animator;
    

    AudioClip hitClip;
    AudioSource audioSource;
    GameController controller;
    EnemyAI ai;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ai = GetComponent<EnemyAI>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        CurrentHealth = health;
    }

    public abstract void Attack(GameObject target);


    public void ChangeHealth(int amount)
    {
        if (amount < 0 && !damageTaken)
        {
            PlaySound(hitClip);
            StartCoroutine(DamageFlash());
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, health);

        if (CurrentHealth == 0)
        {
            if (boss) controller.FloorComplete();
            ai.reachedEndOfPath = true;
            controller.enemies.Remove(this.gameObject);
            animator.SetBool("Dead", true);
        }
    }

    protected IEnumerator DamageFlash()
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
