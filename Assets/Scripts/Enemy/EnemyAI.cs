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
    public GameObject bulletSpawnPoint;

    public ParticleSystem muzzleFlash;
    public AudioClip gunShotSound;
    public AudioSource shootSound;

    public float timeBetweenAttacks;

    private float reloadTime = 1.5f;
    private float fireRate = 15.0f;
    private float nextTimeToFire = 0f;

    private int maxAmmo = 5;
    private int currentAmmo;
    private bool isReloading;

    // States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        // Reload
        if (currentAmmo <= 0 && !isReloading)
             StartCoroutine(Reload());

        // Look For Player
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();

        // Follow Player
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        // Attack Player
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

        // Attack
        if (!isReloading && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            // Instantiate Bullet
            GameObject projectileGameObj = Instantiate(projectile, bulletSpawnPoint.transform.position, Quaternion.identity);

            // Set Rotation Get RB
            projectileGameObj.transform.LookAt(player.transform);
            Rigidbody projectileRB = projectileGameObj.GetComponent<Rigidbody>();

            currentAmmo--;

            muzzleFlash.Play();
            shootSound.PlayOneShot(gunShotSound);

            projectileRB.AddForce((player.position - transform.position).normalized * 32f, ForceMode.Impulse);

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

    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}