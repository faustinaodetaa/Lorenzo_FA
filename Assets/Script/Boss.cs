using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    //public Transform destination;
    //public List<GameObject> points;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int currentHealth;
    public int maxHealth = 2000;
    public HealthBar healthBar;
    public int damage;


    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Player ken;
    //public GameObject coreItem;

    Vector3 dest, start, end;

    Animator animator;

    //RaycastWeapon weapon;


    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //weapon = GetComponentInChildren<RaycastWeapon>();
    }

    //private void Awake()
    //{
    //    player = GameObject.Find("Ken").transform;
    //    agent = GetComponent<NavMeshAgent>();
    //}

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Patroling();
        //if(!playerInSightRange && !playerInAttackRange)
        //{
        //}

        if (playerInSightRange && !playerInAttackRange)
        {
            Debug.Log("chasing");
            ChasePlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            Debug.Log("Attacking");
            AttackPlayer();
        }




    }

    private void Patroling()
    {
        //if (!walkPointSet)
        //{
        //    SearchWalkingPoint();
        //}
        //if (walkPointSet)
        //{
        //    agent.SetDestination(walkPoint);
        //}
        //Debug.Log(name);
        agent.SetDestination(dest);

        if (agent.remainingDistance <= 0)
        {
            dest = (dest == start) ? end : start;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Reached walkpoint
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    //private void SearchWalkingPoint()
    //{
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //    if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //    {
    //        walkPointSet = true;
    //    }
    //}

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make enemy stay
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack 
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            SoundManager.PlaySound("gunshot");

            //Vector3 velocity = (player.position - raycastOrigin.position).normalized * bulletSpeed;
            //var bullet = CreateBullet(raycastOrigin.position, velocity);
            //Debug.Log("bullet");
            //bullets.Add(bullet);



            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            ken.bloodSplatter.SetActive(true);
            ken.TakeDamage(damage);
        }
        //ken.bloodSplatter.SetActive(false);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        //weapon.StopFiring();
        ken.bloodSplatter.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        //currentHealth -= damage;
        //healthBar.SetHealth(currentHealth);

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log(damage);

        if (currentHealth <= 0)
        {
           
            animator.SetBool("isDead", true);
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }


   
}
