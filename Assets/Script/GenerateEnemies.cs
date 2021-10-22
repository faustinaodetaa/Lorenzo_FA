using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject theEnemy;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemySpawn());
    }
        IEnumerator EnemySpawn()
        {
            while (enemyCount < 4)
            {
                Instantiate(theEnemy, spawnLocation.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                enemyCount += 1;
            }
        }
}
