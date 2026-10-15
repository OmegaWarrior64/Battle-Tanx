using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [HideInInspector] public Vector2 moveDirection;
    
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float velocity = 5;
    [SerializeField] private int damage = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime * moveDirection));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthSystem health))
        {
            health.CurrentHealth -= damage;
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
