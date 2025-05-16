using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float attackRate = 2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int currentSlimeLevel = 2;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float invulnerabilityDuration = 1f;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private MonsterDataPerState monsterDataPerState;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private float nextAttackTime = 0f;
    private bool isAttacking = false;

    private bool isInvulnerable = false;
    private bool canMove = true;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    public Action OnPlayerKilled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //audioSource = GetComponent<AudioSource>();
        SetupCharacterValuesAccordingToLevel();
    }

    void Start()
    {
     
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }
        // Movimiento horizontal (solo con A y D)
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Revisar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        animator.SetBool("IsWalking", (Mathf.Abs(rb.linearVelocity.x) > 0.1f) && !IsAttacking);
        animator.SetBool("IsJumping", !isGrounded && !IsAttacking);
    }

    void Attack()
    {
        IsAttacking = true;
        animator.SetBool("IsAttacking", IsAttacking);
        SoundManager.Instance.PlayLoopingSFX(attackSound);
    }

    public void EndAttack()
    {
        IsAttacking = false;
        animator.SetBool("IsAttacking", IsAttacking);
        SoundManager.Instance.StopSFX();
    }

    public void EnemyKilled()
    {
        int maxLevel = monsterDataPerState.GetSlimeDataPerLevel().Keys.Count - 1;
        if (currentSlimeLevel + 1 <= maxLevel) 
        {
            currentSlimeLevel++;
            SetupCharacterValuesAccordingToLevel();
        }
    }

    private void SetupCharacterValuesAccordingToLevel()
    {
        Dictionary<int, SlimeData> slimeDataPerLevel = monsterDataPerState.GetSlimeDataPerLevel();
        SlimeData currentSlimeData = slimeDataPerLevel[currentSlimeLevel];
        if (currentSlimeData != null)
        {
            moveSpeed = currentSlimeData.moveSpeed;
            jumpForce = currentSlimeData.jumpForce;
            attackRate = currentSlimeData.attackRate;
            transform.localScale = currentSlimeData.scale;
        }
    }

    public void HitByEnemy(BaseEnemy baseEnemy)
    {
        if (isInvulnerable)
        {
            return;
        }

        SoundManager.Instance.PlaySFX(damageSound);

        // 1. Activar invulnerabilidad
        StartCoroutine(InvulnerabilityTimer());

        // 2. Calcular dirección del knockback
        Vector2 knockbackDir = (transform.position - baseEnemy.transform.position).normalized;
        knockbackDir.y = 0.5f; // opcional: impulso un poco hacia arriba // TODO check
        rb.linearVelocity = Vector2.zero; // resetea velocidad actual
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        spriteRenderer.color = Color.red;

        currentSlimeLevel--;
        if (currentSlimeLevel < 1)
        {
            OnPlayerKilled?.Invoke();
            Destroy(gameObject, damageSound.length);
        }
        else
        {
            SetupCharacterValuesAccordingToLevel();
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        canMove = false;    
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        canMove = true;
        spriteRenderer.color = Color.white;
    }
}
