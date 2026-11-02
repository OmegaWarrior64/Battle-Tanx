using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private HealthSystem health;
    [SerializeField] private GameObject explosionEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthSystem>();
        
        health.HealthDepleted += () => 
        {
            Destroy(gameObject);
            GameManager.Instance.CurrentScore += 100;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
