using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Speeds")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Detection")]
    public float chaseRange = 8f;
    public float chaseLoseRange = 15f;
    public float viewAngle = 60f;            
    public float eyeHeight = 0.8f;             

    [Header("Chase On Hit")]
    public float chaseOnHitTime = 4f;        

    Transform player;
    NavMeshAgent agent;
    Transform currentTarget;

    bool isChasing = false;
    float chaseOnHitTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        currentTarget = pointA;

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0.5f;
        agent.updateRotation = false;

        if (currentTarget != null)
            agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        if (player == null || agent == null) return;

        
        if (chaseOnHitTimer > 0f)
        {
            chaseOnHitTimer -= Time.deltaTime;
            isChasing = true;
        }

      
        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
            isChasing = true;

      
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (isChasing && distToPlayer > chaseLoseRange && chaseOnHitTimer <= 0f && !canSeePlayer)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            agent.stoppingDistance = 0.5f;

            if (currentTarget != null)
                agent.SetDestination(currentTarget.position);
        }

        if (isChasing)
            Chase();
        else
            Patrol();

     
        Vector3 lookPos = isChasing ? player.position : (currentTarget != null ? currentTarget.position : transform.position + transform.forward);
        LookAtFlat(lookPos);
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0.5f;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            agent.SetDestination(currentTarget.position);
        }
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.stoppingDistance = 0.8f;

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(targetPos);
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist > chaseRange) return false;

     
        Vector3 flat = toPlayer; flat.y = 0f;
        float angle = Vector3.Angle(transform.forward, flat);
        if (angle > viewAngle * 0.5f) return false;

        // Raycast
        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 dir = (player.position - eyePos);
        dir.y = 0f;

        if (Physics.Raycast(eyePos, dir.normalized, out RaycastHit hit, chaseRange))
        {
            return hit.transform == player;
        }

        return false;
    }

    void LookAtFlat(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }

    
    public void OnHit()
    {
        chaseOnHitTimer = chaseOnHitTime;
        isChasing = true;
    }
}
