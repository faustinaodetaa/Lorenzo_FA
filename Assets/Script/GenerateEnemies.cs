using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateEnemies : MonoBehaviour
{

    public class EnemyGenerated
    {
        public GameObject enemy;
        public NavMeshAgent agent;
        public int patrolIndex;
        public Transform patrolPos;
        public bool arrived;

        public EnemyGenerated(GameObject e, NavMeshAgent a, int pI, Transform pP)
        {
            enemy = e;
            agent = a;
            patrolIndex = pI;
            patrolPos = pP;
            arrived = false;
        }

    }

    [System.Serializable]
    public class Patroli
    {
        public string enemyType;
        public GameObject enemyPrefabs;
        public Transform[] patrolPos;
        public Transform spawnPosition;
        public List<EnemyGenerated> enemyList;
        public bool[] satpamExist;
    }

    public List<Patroli> patrolPositions = new List<Patroli>();

    void Start()
    {
        Debug.Log("GenerateEnemies >> Start >> PatrolPositionCount:" + patrolPositions.Count);
        foreach (Patroli p in patrolPositions)
        {
            p.satpamExist = new bool[p.patrolPos.Length];
            p.enemyList = new List<EnemyGenerated>();
        }
    }

    void Update()
    {
        foreach (Patroli p in patrolPositions)
        {
            Debug.Log("GenerateEnemies >> " + p.enemyType + " Count: " + p.enemyList.Count);
            if (p.enemyList.Count < p.patrolPos.Length)
                generateEnemy(p);
            updateEnemyArrival(p);
        }
    }

    void updateEnemyArrival(Patroli p)
    {
        foreach (EnemyGenerated e in p.enemyList)
        {
            if (e.enemy == null)
            {
                Debug.Log("GenerateEnemies >> Enemy Null -> ded");
                p.enemyList.Remove(e);
                break;
            }
            else
            {
                if (!e.arrived)
                {
                    if (Vector3.Distance(e.enemy.transform.position, e.patrolPos.position) < 0.5f)
                    {
                        e.arrived = true;
                        e.agent.SetDestination(e.enemy.transform.position);
                        e.enemy.GetComponent<Enemy>().inPosition = true;
                    }
                    else
                    {
                        //Debug.Log("Setting Destination");
                        e.agent.SetDestination(e.patrolPos.position);
                    }
                }

            }

        }
    }

    void generateEnemy(Patroli p)
    {
        for (int i = 0; i < p.satpamExist.Length; i++)
        //for (int i = 0; i < 1; i++)
        {
            if (!p.satpamExist[i])
            {
                //Debug.Log("Masuk Posisi Patrol " + i + " Kosong");
                spawnEnemy(p, i);
                p.satpamExist[i] = true;
            }
        }
    }

    void spawnEnemy(Patroli p, int patrolIndex)
    {
        Transform destination = p.patrolPos[patrolIndex];
        GameObject enemy = Instantiate(p.enemyPrefabs, p.spawnPosition.position, Quaternion.identity);
        Debug.Log("GenerateEnemies >> Spawn: " + enemy.name + " Patrol Index: " + patrolIndex);
        enemy.GetComponent<Enemy>().patrolIndex = patrolIndex;
        enemy.GetComponent<Enemy>().patrolArea = destination;
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        for (int i = 0; i < destination.transform.childCount; i++)
        {
            enemy.GetComponent<Enemy>().patrolPoints.Add(destination.transform.GetChild(i));
        }

        agent.transform.LookAt(destination);
        agent.SetDestination(destination.position);
        //Debug.Log("Current Enemy Position: " + enemy.transform.position);
        //Debug.Log("Patrol Point: " + destination.position);
        //Debug.Log("Agent Target: " + agent.destination);
        p.enemyList.Add(new EnemyGenerated(enemy, agent, patrolIndex, destination));
    }

    Patroli findPatroli(string enemyType)
    {
        Debug.Log("GenerateEnemies >> PatrolPositionCount:" + patrolPositions.Count);
        foreach (Patroli p in patrolPositions)
        {
            Debug.Log("GenerateEnemies >> findPatroli >> enemyType:[" + enemyType+ "] pListEnemyType:["+ p.enemyType +"]");
            if (enemyType.ToLower().Contains(p.enemyType.ToLower()))
                return p;
        }
        return null;
    }

    public void cleanPatroliExist(string enemyType, int patrolIndex, int delay)
    {
        Debug.Log("GenerateEnemies >> Cleaning Patroli " + patrolIndex + "delay: " + delay);
        StartCoroutine(makePatrolPointEmpty(enemyType, patrolIndex, delay));
    }

    public IEnumerator makePatrolPointEmpty(string enemyType, int patrolIndex, int delay)
    {
        Patroli p = findPatroli(enemyType);
        if (p != null)
        {
            yield return new WaitForSeconds(delay);
            p.satpamExist[patrolIndex] = false;
            Debug.Log("GenerateEnemies >> " + patrolIndex + " Finished Cleaning");
        }
        else
        {
            Debug.Log("GenerateEnemies >> Null findPatroli");
        }
        yield return null;
    }
}
