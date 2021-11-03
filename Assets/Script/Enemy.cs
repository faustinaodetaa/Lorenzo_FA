using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    /*
     * Enemy Attribute
     */
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int currentHealth;
    public int maxHealth = 250;
    public HealthBar healthBar;
    public int damage;
    public int spawnDelay;


    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;
    

    public Player ken;
    public GameObject coreItem;

    

    Animator animator;

    //RaycastWeapon weapon;


    private void Start()
    {
        initEnemyPatrolAttackChase();
        ken = FindObjectOfType<Player>();
        player = FindObjectOfType<Player>().transform;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //weapon = GetComponentInChildren<RaycastWeapon>();
    }

    private void Update()
    {
        enemyRoutines();
    }

    #region EnemyPatrol, EnemyAttack, EnemyChase

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, canChasePlayer;
    public int currentPatrolIndex;
    public int patrolIndex;
    public bool inPosition, attackEnemyFirstTime;
    public Transform patrolArea;
    public List<Transform> patrolPoints;

    private void initEnemyPatrolAttackChase()
    {
        currentPatrolIndex = 0;
        inPosition = attackEnemyFirstTime = false;
    }

    private void enemyRoutines()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (inPosition && !playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }

        if (inPosition && playerInSightRange && !playerInAttackRange && canChasePlayer)
        {
            ChasePlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }

        if (!inPosition && attackEnemyFirstTime)
        {
            ReachPatrolPosition();
        }
    }

    private void ReachPatrolPosition()
    {
        transform.LookAt(patrolArea.position);
        agent.SetDestination(patrolArea.position);
    }

    private void Patroling()
    {
        transform.LookAt(patrolPoints[currentPatrolIndex].position);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if(agent.remainingDistance <= 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; 
        }
        
    }

    private void ChasePlayer()
    {
        transform.LookAt(player.position);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        attackEnemyFirstTime = true;
        transform.LookAt(player);
        agent.SetDestination(transform.position);

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
            bool destroyEnemy = false;
            if (!destroyEnemy)
            {
                destroyEnemy = true;
                DestroyEnemy();
            }
        }
    }

    private void DestroyEnemy()
    {
        bool alreadyClean = false;
        if(!alreadyClean)
        {
            alreadyClean = true;
            GenerateEnemies ge = FindObjectOfType<GenerateEnemies>();
            ge.cleanPatroliExist(name, patrolIndex, spawnDelay);
        }
        Destroy(gameObject,5f);
    }

    #endregion

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
