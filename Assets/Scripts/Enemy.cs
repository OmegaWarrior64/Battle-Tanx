using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private HealthSystem health;

    private Transform bodyGraphic;
    private Transform barrelGraphic;
    private Transform shootPoint;
    private Transform player;

    [SerializeField] private float attackRange = 20;
    [SerializeField] private float bodyRotationSpeed = 5;
    [SerializeField] private float barrelRotationSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float reloadTime = 2;

    [SerializeField] private GameObject explosionEffect;
    
    private int pathIndex;
    private bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthSystem>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        bodyGraphic = transform.Find("Body Graphic");
        barrelGraphic = transform.Find("Barrel Graphic");
        shootPoint = transform.Find("Barrel Graphic/Shoot Point");
        player = GameObject.FindWithTag("Player").transform;

        health.HealthDepleted += () => { Destroy(gameObject); Instantiate(explosionEffect, transform.position, Quaternion.identity); };

        InvokeRepeating(nameof(SetMovementTarget), 1, Random.Range(3f, 5f));
        InvokeRepeating(nameof(SetShootTarget), reloadTime, 4);
    }

    void Update()
    {
        if (agent.hasPath)
        {
            MoveAndRotate();
        }
    }

    private void SetShootTarget()
    {
        Vector2 shootDir = player.position - transform.position;
        if (shootDir.magnitude <= attackRange && !Physics2D.Raycast(transform.position, shootDir.normalized, attackRange, obstacleMask) && canShoot)
        {
            canShoot = false;
            StartCoroutine(Shoot(shootDir));
        }
    }

    private void SetMovementTarget()
    {
        if (!agent.hasPath)
        {
            agent.SetDestination(player.position + (Vector3)(Random.onUnitCircle * Random.Range(3f, 4f)));
            pathIndex = 1;
        }
    }

    private IEnumerator Shoot(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            yield return new WaitUntil(() =>
            {
                barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, newRotation, barrelRotationSpeed * Time.deltaTime);
                return Quaternion.Angle(barrelGraphic.rotation, newRotation) < 1;
            });

            var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet>().moveDirection = dir.normalized;

            yield return new WaitForSeconds(reloadTime);
            canShoot = true;
    }

    private void MoveAndRotate()
    {
        Vector2 moveDir = agent.path.corners[pathIndex] - transform.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        if (Quaternion.Angle(bodyGraphic.rotation, newRotation) > 1)
        {
            bodyGraphic.rotation = Quaternion.Lerp(bodyGraphic.rotation, newRotation, bodyRotationSpeed * Time.deltaTime);
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if (moveDir.magnitude <= 0.1f && agent.remainingDistance > 0.1f)
        {
            pathIndex = (pathIndex + 1) % agent.path.corners.Length - 1;
        }

        if (canShoot)
        {
            barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, newRotation, barrelRotationSpeed * Time.deltaTime);
        }
    }
}
