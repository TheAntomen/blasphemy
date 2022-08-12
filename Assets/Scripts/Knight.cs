using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Class for the player-controlled knight
/// </summary>
public class Knight : MonoBehaviour, IDamageable
{

    // Properties
    public float mouseAngle { get; private set; }
    public int CurrentHealth { get; set; }

    // Public variables
    public GameObject SlashAttack;
    public AudioClip swordAttack;

    // Private variables
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float timeInvincible;

    private Rigidbody2D rb2d;
    private Animator animator;
    private ArmPivot armpivot;
    private SpriteRenderer s_renderer;
    private SpriteRenderer sword_renderer;
    private AudioSource audioSource;
    private CinemachineVirtualCamera vcam;
    private Vector2 mousePosition;
    private Vector2 knightPosition;
    private Vector2 relativePosition;
    private Vector2 offset;

    private float horizontal;
    private float vertical;
    private float invincibleTimer;
    private float attackTimer;
    private static int currentHp;
    private bool isInvincible;
    private bool dead;
    private bool attacking;

    protected bool damageTaken;
    private Color damageColor = new Color(1.000f, 0f, 0f, 1.000f);
    private int damageFlashCount = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        armpivot = transform.GetChild(0).GetComponent<ArmPivot>();
        s_renderer = GetComponent<SpriteRenderer>();
        sword_renderer = armpivot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        currentHp = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            knightPosition = transform.position;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseAngle = calcMouseAngle();

            animator.SetFloat("Angle", mouseAngle);
            animator.SetFloat("Speed", move.magnitude);

            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }

            if (currentHp == 0)
            {
                sword_renderer.enabled = false;
                dead = true;
                animator.SetBool("Dead", true);
                rb2d.simulated = false;   
            }
            else
            {
                if (isInvincible)
                {
                    invincibleTimer -= Time.deltaTime;
                    if (invincibleTimer < 0)
                    {
                        CancelInvoke();
                        isInvincible = false;
                    }
                }
            }

            if (attacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                {
                    attacking = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rb2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        if (!dead) rb2d.MovePosition(position);
    }

    /// <summary>
    /// Instantiates an attack if there is not a cooldown of an attack
    /// </summary>
    void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            attackTimer = attackRate;
            armpivot.AttackRotation();
            offset = relativePosition.normalized * 1.5f;
            GameObject attackObject = Instantiate(SlashAttack, rb2d.position + (Vector2.up * 0.5f) + offset, transform.rotation * Quaternion.Euler(0f, 0f, mouseAngle));
            PlaySound(swordAttack);
        }
    }

    /// <summary>
    /// Change the knights health based on the amount of damage taken and updates UI accordingly
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;

            StartCoroutine(DamageFlash());
        }
        currentHp = Mathf.Clamp(currentHp + amount, 0, health);
        UIHealthBar.instance.SetValue(currentHp / (float)health);

        if (currentHp == 0)
        {
            animator.SetTrigger("Dead");
        }

    }

    /// <summary>
    /// Displays effects of the knight taking damage, swithcing between colors
    /// </summary>
    public IEnumerator DamageFlash()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color defaultColor = renderer.color;

        for (int state = 0; state < damageFlashCount; state++)
        {
            if ((state % 2) == 0)
            {
                renderer.color = damageColor;
            }
            else
            {
                renderer.color = defaultColor;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    /// <summary>
    /// Calculates angle relative angle between knight and current mouse position
    /// </summary>
    /// <returns>float of the angle</returns>
    private float calcMouseAngle()
    {
        relativePosition = mousePosition - knightPosition;
        return (int)(Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
