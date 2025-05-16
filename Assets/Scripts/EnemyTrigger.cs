using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public BaseEnemy parentEnemy;

    void Awake()
    {
        parentEnemy = GetComponentInParent<BaseEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        parentEnemy.OnCollisionOcuurred(other);
    }
}
