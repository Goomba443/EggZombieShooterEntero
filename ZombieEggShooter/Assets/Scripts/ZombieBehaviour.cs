using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask ground;
    public LayerMask playerCollider;

    public static float zombieHealth = 100f;
    public static float zombieDamage = 20f;
    private static float takeDamageCoolDown = 1.5f;
    private static float lerpTimer = 0;
    private static bool restingFromBeingAttacked = false;
    public float damage;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float attackCoolDown;
    bool restingFromAttack;

    Vector3 velocity;

    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    private void Awake() {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerCollider);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerCollider);
        if(!playerInSightRange && !playerInAttackRange) Patrolling();
        if(playerInSightRange && !playerInAttackRange) ChasingPlayer();
        if(playerInAttackRange && !playerInSightRange) AttackingPlayer();
        if (restingFromBeingAttacked)
        {
            lerpTimer -= Time.deltaTime;
            if(lerpTimer < 0)
            {
                restingFromBeingAttacked = false;
            }
        }
    }

    private void Patrolling() {
        if(!walkPointSet) {
            SearchWalkPoint();
        } else {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
    }

    private void SearchWalkPoint(){
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, ground)) {
            walkPointSet = true;
        }
    }

    private void ChasingPlayer() {
        agent.SetDestination(player.position);
    }

    private void AttackingPlayer() {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!restingFromAttack) {
            restingFromAttack = true;
            PlayerHealth.TakeDamage(20f);
            Invoke(nameof(ResetAttack), attackCoolDown);
        }
    }

    private void ResetAttack() {
        restingFromAttack = false;
    }

    public static void ZombieTakesDamage(GameObject zombie, float damage) {
        if(!restingFromBeingAttacked)
        {
            restingFromBeingAttacked = true;
            lerpTimer = takeDamageCoolDown;
            zombieHealth -= damage;
        }

        if(zombieHealth <= 0) {
            Destroy(zombie);
        }
    }
    
} 
