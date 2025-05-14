using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;
    public float attackRate = 2f;

    private float nextAttackTime = 0f;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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

        if (isAttacking)
        {
            // Detectar enemigos en el rango
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Golpeaste a " + enemy.name);
                // Acá podrías llamar a enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }

        animator.SetBool("IsWalking", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        animator.SetBool("IsJumping", !isGrounded);
    }

    void Attack()
    {
        // Trigger animación
        animator.SetBool("IsAttacking", true);
    }

    public void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

}
