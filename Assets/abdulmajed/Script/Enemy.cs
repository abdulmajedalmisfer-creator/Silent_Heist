using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    [Header("Hearing (Investigate)")]
    public float hearRange = 12f;
    public float investigateWaitTime = 2f;

    [Header("Game Over")]
    public GameOverManager gameOverManager;

    Transform player;
    NavMeshAgent agent;
    Transform currentTarget;
    Animator animator;

    bool isChasing = false;
    float chaseOnHitTimer = 0f;

    bool isInvestigating = false;
    Vector3 investigatePoint;
    float investigateTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

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

        
        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);

        // Chase on hit timer
        if (chaseOnHitTimer > 0f)
        {
            chaseOnHitTimer -= Time.deltaTime;
            isChasing = true;
        }

        // Vision
        bool canSeePlayer = CanSeePlayer();
        if (canSeePlayer)
            isChasing = true;

        // Priority: player over investigate
        if (isChasing)
            isInvestigating = false;

        // Lose chase
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (isChasing && distToPlayer > chaseLoseRange && chaseOnHitTimer <= 0f && !canSeePlayer)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            agent.stoppingDistance = 0.5f;
            if (currentTarget != null)
                agent.SetDestination(currentTarget.position);
        }

        // State actions
        if (isChasing)
            Chase();
        else if (isInvestigating)
            Investigate();
        else
            Patrol();

 
        if (agent.velocity.sqrMagnitude > 0.02f)
        {
            LookAtFlat(transform.position + agent.velocity);
        }
        else
        {
     
            Vector3 lookPos =
                isChasing ? player.position :
                isInvestigating ? investigatePoint :
                (currentTarget != null ? currentTarget.position : transform.position + transform.forward);

            LookAtFlat(lookPos);
        }

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

    void Investigate()
    {
        if (CanSeePlayer())
        {
            isInvestigating = false;
            isChasing = true;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            investigateTimer -= Time.deltaTime;
            if (investigateTimer <= 0f)
            {
                isInvestigating = false;
                agent.speed = patrolSpeed;
                agent.stoppingDistance = 0.5f;

                if (currentTarget != null)
                    agent.SetDestination(currentTarget.position);
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist > chaseRange) return false;

        Vector3 flat = toPlayer; flat.y = 0f;
        float angle = Vector3.Angle(transform.forward, flat);
        if (angle > viewAngle * 0.5f) return false;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 dir = (player.position - eyePos);
        dir.y = 0f;

        if (Physics.Raycast(eyePos, dir.normalized, out RaycastHit hit, chaseRange))
            return hit.transform == player;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
    }

    public void OnHit()
    {
        chaseOnHitTimer = chaseOnHitTime;
        isChasing = true;
        isInvestigating = false;
    }

    public void HearNoise(Vector3 noisePos)
    {
        if (isChasing) return;

        float d = Vector3.Distance(transform.position, noisePos);
        if (d > hearRange) return;

        isInvestigating = true;
        investigatePoint = noisePos;
        investigateTimer = investigateWaitTime;

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0.5f;
        agent.SetDestination(investigatePoint);
    }

    public void RespondToAlarm(Vector3 alarmPos)
    {
        isInvestigating = true;
        investigatePoint = alarmPos;
        investigateTimer = investigateWaitTime;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = 0.5f;

        Vector3 target = new Vector3(alarmPos.x, transform.position.y, alarmPos.z);
        agent.SetDestination(target);
    }
}
