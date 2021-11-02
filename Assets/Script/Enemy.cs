using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform destination;
    public List<GameObject> points;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int currentHealth;
    public int maxHealth = 250;
    public HealthBar healthBar;
    public int damage;


    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Player ken;
    public GameObject coreItem;

    Vector3 dest, start, end;

    Animator animator;

    //RaycastWeapon weapon;


    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        agent = GetComponent<NavMeshAgent>();
        dest = start = points[0].transform.position;
        end = points[1].transform.position;
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

        Patroling();
        //if(!playerInSightRange && !playerInAttackRange)
        //{
        //}

        //if(playerInSightRange && !playerInAttackRange)
        //{
        //    ChasePlayer();
        //}

        if(playerInAttackRange && playerInSightRange)
        {
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

        if(agent.remainingDistance <= 0)
        {
            dest = (dest == start) ? end : start;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Reached walkpoint
        if(distanceToWalkPoint.magnitude < 1f)
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

    //private void ChasePlayer()
    //{

    //}

    private void AttackPlayer()
    {
        //Make enemy stay
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack 
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            SoundManager.PlaySound("gunshot");

            Vector3 velocity = (player.position - raycastOrigin.position).normalized * bulletSpeed;
            var bullet = CreateBullet(raycastOrigin.position, velocity);
            Debug.Log("bullet");
            bullets.Add(bullet);



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
        Debug.Log("set healthbar " + currentHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log(damage);

        if(currentHealth <= 0)
        {
            Instantiate(coreItem, transform.position, Quaternion.identity);
            animator.SetBool("isDead", true);
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring = false;
    public int fireRate = 25;
    public float bulletSpeed = 100.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    Enemy enemy;
    //public Player ken;
    //public GameObject magazine;
    public int bulletDamage;
    //public SkillPointsBar skillBar;


    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifetime = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }


    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifetime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = end - start;

        //int damage = 10;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifetime;
            Debug.Log(hitInfo.collider.name);
            if (hitInfo.collider.gameObject.tag.Equals("Enemy"))
            {
                Debug.Log("enemy dmg");
                enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                //ken.currentSkill += 2;
                //skillBar.SetSkill(ken.currentSkill);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);

        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

    }

    public void StopFiring()
    {
        isFiring = false;
    }

}
