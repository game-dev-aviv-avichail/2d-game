
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float damageDistance = 0.8f;

    private PlayerHealth playerHealth;
    private float nextAttackTime;

    void Start()
    {
        // Find the player automatically
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError(
                "PlayerHealth not found in scene!"
            );
        }
    }

    void Update()
    {
        if (playerHealth == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        // Measure distance between zombie and player
        float distance = Vector2.Distance(
            transform.position,
            playerHealth.transform.position
        );

        // Damage player when close enough
        if (distance <= damageDistance)
        {
            playerHealth.TakeDamage(damage);

            Debug.Log("Zombie attacked player!");

            nextAttackTime =
                Time.time + attackCooldown;
        }
    }
}
