using UnityEngine;
using UnityEngine.AI;

public class Sandbag : MonoBehaviour
{
    [SerializeField] private Sprite destroyedSprite;

    private HealthSystem health;
    private ParticleSystem particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthSystem>();
        particles = GetComponentInChildren<ParticleSystem>();

        health.HealthDepleted += () =>
        {
            GetComponent<SpriteRenderer>().sprite = destroyedSprite;
            particles.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
        };
    }
}
