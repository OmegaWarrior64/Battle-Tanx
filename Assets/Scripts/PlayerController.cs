using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action<float> BulletShot;

    private Rigidbody2D rb;
    private Camera mainCam;
    private LineRenderer lineRenderer;

    private AudioSource engineSound;

    private Transform bodyGraphic;
    private Transform barrelGraphic;
    private Transform shootPoint;

    private InputAction pressAction;
    private InputAction motionAction;

    private bool drawing = false;
    private int currentIndex;

    [SerializeField] private float moveSpeed = 25;
    [SerializeField] private float bodyRotationSpeed = 5;
    [SerializeField] private float barrelRotationSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float reloadTime = 2;

    private int moveIndex;
    private bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        
        bodyGraphic = transform.Find("Body Graphic");
        barrelGraphic = transform.Find("Barrel Graphic");
        shootPoint = transform.Find("Barrel Graphic/Shoot Point");

        engineSound = bodyGraphic.GetComponent<AudioSource>();

        pressAction = InputSystem.actions.FindAction("Press");
        motionAction = InputSystem.actions.FindAction("Motion");
    }

    // Update is called once per frame
    void Update()
    {
        if (pressAction.WasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
        {
            if (rb.OverlapPoint((Vector2)mainCam.ScreenToWorldPoint(motionAction.ReadValue<Vector2>())))
            {
                drawing = true;
                currentIndex = 0;
                moveIndex = 0;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(currentIndex, motionAction.ReadValue<Vector2>());
            }
            else
            {
                if (canShoot)
                {
                    StartCoroutine(FireBullet());
                }
            }
        }
        else if (pressAction.WasReleasedThisFrame())
        {
            drawing = false;
            lineRenderer.Simplify(0.15f);
        }

        if (drawing)
        {
            lineRenderer.positionCount = currentIndex + 1;
            lineRenderer.SetPosition(currentIndex, (Vector2)mainCam.ScreenToWorldPoint(motionAction.ReadValue<Vector2>()));
            currentIndex++;
        }

        if (rb.linearVelocity == Vector2.zero) engineSound.pitch = 1f; else engineSound.pitch = 1.5f;
    }

    void FixedUpdate()
    {
        if(!drawing && lineRenderer.positionCount >= 2 && moveIndex < lineRenderer.positionCount)
        {
            Vector2 moveDir = lineRenderer.GetPosition(moveIndex) - transform.position;
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            if (Quaternion.Angle(bodyGraphic.rotation, newRotation) > 1)
            {
                bodyGraphic.rotation = Quaternion.Lerp(bodyGraphic.rotation, newRotation, bodyRotationSpeed * Time.fixedDeltaTime);
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * moveDir.normalized;
            }

            if (moveDir.magnitude <= 0.1f)
            {
                moveIndex++;
            }

            if (canShoot)
            {
                barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, newRotation, barrelRotationSpeed * Time.fixedDeltaTime);
            }

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator FireBullet()
    {
        canShoot = false;

        Vector2 shootDir = (Vector2)(mainCam.ScreenToWorldPoint(motionAction.ReadValue<Vector2>()) - transform.position);
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        yield return new WaitUntil(() =>
        {
            barrelGraphic.rotation = Quaternion.Lerp(barrelGraphic.rotation, newRotation, barrelRotationSpeed * Time.deltaTime);
            return Quaternion.Angle(barrelGraphic.rotation, newRotation) < 1;
        });

        var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Bullet>().moveDirection = shootDir.normalized;
        BulletShot?.Invoke(reloadTime);

        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }
}