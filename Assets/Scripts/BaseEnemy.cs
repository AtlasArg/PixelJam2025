using System;
using UnityEngine;
using UnityEngine.Audio;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f; 
    [SerializeField] private AudioClip deathSound;

    private Transform target;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = pointB; 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - target.position.x) < 0.1f)
        {
            if (target == pointB)
            {
                target = pointA;
            }
            else
            {
                target = pointB;
            }

            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnCollisionOcuurred(Collider2D other)
    {
        PlayerController2D playerController = other.gameObject.GetComponent<PlayerController2D>();
        if (playerController != null)
        {
            if (playerController.IsAttacking) 
            {
                playerController.EnemyKilled();
                SoundManager.Instance.PlaySFX(deathSound);
                spriteRenderer.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject, deathSound.length);
            }
            else 
            {
                playerController.HitByEnemy(this);
            }
        }
    }
}
