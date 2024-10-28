using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private float timer;
    private Animator animator;
    private bool isAttacking = false;

    private enum State { Idle, Wander, Chase, Attack }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        currentState = State.Idle;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case State.Wander:
                HandleWanderState(distanceToPlayer);
                break;
            case State.Chase:
                HandleChaseState(distanceToPlayer);
                break;
            case State.Attack:
                HandleAttackState(distanceToPlayer);
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer < detectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                currentState = State.Wander;
                timer = wanderTimer;
            }
        }
    }

    private void HandleWanderState(float distanceToPlayer)
    {
        if (distanceToPlayer < detectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            if (!agent.hasPath)
            {
                Vector3 newDestination = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newDestination);
            }
        }
    }

    private void HandleChaseState(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRange)
        {
            currentState = State.Wander;
            timer = wanderTimer;
        }
        else if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
        else
        {
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
        }
    }

    private void HandleAttackState(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            currentState = State.Chase;
        }
        else
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("attack");
                Invoke("DealDamage", 0.5f); // Adjust delay to sync with attack animation timing
            }
        }
    }

    private void DealDamage()
    {
        // Apply damage to the player
        // Assuming the player has a method TakeDamage
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        isAttacking = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layerMask);

        return navHit.position;
    }
}
