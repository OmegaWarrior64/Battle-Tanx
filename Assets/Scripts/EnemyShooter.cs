using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    private ParticleSystem shotParticles;

    private Transform barrelGraphic;
    private Transform shootPoint;
    private Transform player;

    [SerializeField] private float attackRange = 20;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float reloadTime = 2;

    private bool canShoot = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotParticles = GetComponentInChildren<ParticleSystem>();

        barrelGraphic = transform.Find("Barrel Graphic");
        shootPoint = transform.Find("Barrel Graphic/Shoot Point");
        player = GameObject.FindWithTag("Player").transform;

        InvokeRepeating(nameof(SetShootTarget), reloadTime, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && GetComponent<EnemyMover>() != null)
        {
            barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, transform.Find("Body Graphic").rotation, rotationSpeed * Time.deltaTime);
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

    private IEnumerator Shoot(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            yield return new WaitUntil(() =>
            {
                barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, newRotation, rotationSpeed * Time.deltaTime);
                return Quaternion.Angle(barrelGraphic.rotation, newRotation) < 1;
            });

            var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet>().moveDirection = dir.normalized;
            shotParticles.Play();

            yield return new WaitForSeconds(reloadTime);
            canShoot = true;
    }
}
