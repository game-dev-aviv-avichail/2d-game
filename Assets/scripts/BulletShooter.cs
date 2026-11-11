
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;

        GetComponent<Collider2D>().isTrigger = true;

        Debug.Log("Bullet created!");
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.0001f)
            direction = Vector2.down;

        rb.linearVelocity = direction.normalized * speed;

        Debug.Log("Bullet launched: " + direction);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet touched: " + other.gameObject.name);

        if (hasHit)
            return;

        NPCFollowPatrol zombie =
            other.GetComponentInParent<NPCFollowPatrol>();

        if (zombie != null)
        {
            hasHit = true;

            Debug.Log("ZOMBIE KILLED: " + zombie.name);

            Destroy(zombie.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Object is not a zombie: " + other.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(
            "Physical collision: " +
            collision.gameObject.name
        );
    }
}
