using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Transform bodyGraphic;

    [SerializeField] private float chaseRange = 10;
    [SerializeField] private float rotationSpeed = 5;

    private int pathIndex;
    private bool inRange = false;
    private Vector3 currentDestination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        bodyGraphic = transform.Find("Body Graphic");

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        InvokeRepeating(nameof(SetMovementTarget), Random.Range(0.5f, 1f), Random.Range(3f, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
        {
            MoveAndRotate();
        }
    }

    private void SetMovementTarget()
    {
        if ((player.position - transform.position).magnitude > chaseRange)
        {
            agent.SetDestination(player.position);
            pathIndex = 1;
            inRange = false;
        }
        else
        {
            if (!inRange)
            {
                currentDestination = player.position + (Vector3)(Random.onUnitCircle * Random.Range(2f, 3.5f));
                agent.SetDestination(currentDestination);
                inRange = true;
            }

            agent.SetDestination(currentDestination);
            pathIndex = 1;

            if (agent.remainingDistance < 0.1f)
            {
                currentDestination = player.position + (Vector3)(Random.onUnitCircle * Random.Range(2f, 3f));
            }
        }
    }

    private void MoveAndRotate()
    {
        Vector2 moveDir = agent.path.corners[pathIndex] - transform.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        if (Quaternion.Angle(bodyGraphic.rotation, newRotation) > 1)
        {
            bodyGraphic.rotation = Quaternion.Lerp(bodyGraphic.rotation, newRotation, rotationSpeed * Time.deltaTime);
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
    }
}
