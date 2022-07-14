using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Class for the player-controlled knight
/// </summary>
public class Knight : MonoBehaviour
{

    // Properties
    public float mouseAngle { get; private set; }

    // Public variables
    public const int MAX_HP = 100;
    public const float MAX_SPEED = 5.0f;
    public const float ATTACK_SPEED = 0.5f;
    public float timeInvincible = 1.0f;
    public GameObject SlashAttack;
    public AudioClip swordAttack;

    // Private variables
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

    private void Awake()
    {
           
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        armpivot = transform.GetChild(0).GetComponent<ArmPivot>();
        s_renderer = GetComponent<SpriteRenderer>();
        sword_renderer = armpivot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        currentHp = MAX_HP;
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
        float speed = MAX_SPEED;

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
            attackTimer = ATTACK_SPEED;
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

            InvokeRepeating("DamageTaken", 0.0f, 0.15f);
        }
        currentHp = Mathf.Clamp(currentHp + amount, 0, MAX_HP);
        UIHealthBar.instance.SetValue(currentHp / (float)MAX_HP);
    }

    /// <summary>
    /// Displays effects of the knight taking damage, swithcing between colors
    /// </summary>
    public void DamageTaken()
    {
        Color damageColor = new Color(1.000f, 0.552f, 0.552f, 1.000f);
        Color normalColor = new Color(1.000f, 1.000f, 1.000f, 1.000f);

        if (String.Equals(s_renderer.color.ToString(), damageColor.ToString()))
        {
            s_renderer.color = normalColor;
        }
        else if (String.Equals(s_renderer.color.ToString(), normalColor.ToString()))
        {
            s_renderer.color = damageColor;
        }
        else
        {
            s_renderer.color = normalColor;
        }
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
