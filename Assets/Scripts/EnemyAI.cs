using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    // Layer Masks
    public LayerMask groundMask;
    public LayerMask playerMask;

    // Patrolling
    public Vector3 walkPoint;
    public float walkPointRange;
    private bool walkPointSet;

    // Attacking
    public GameObject projectile;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if (playerInAttackRange)
            AttactPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        else
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttactPlayer()
    {
        // Go to & look at the Player
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        Debug.DrawRay(transform.position, transform.forward, Color.red);

        // Attack
        if (!alreadyAttacked)
        {
            GameObject projectileGameObj = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
            Rigidbody projectileRB = projectileGameObj.GetComponent<Rigidbody>();

            projectileRB.AddForce((player.position - transform.position).normalized * 32f, ForceMode.Impulse);

            alreadyAttacked = true;

            StartCoroutine(ResetAttack());

            // Destroy Projectiles
            Destroy(projectileGameObj, 2.0f);
        }

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}