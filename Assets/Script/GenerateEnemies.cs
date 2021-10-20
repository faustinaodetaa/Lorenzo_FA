using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject theEnemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemySpawn());
    }
        IEnumerator EnemySpawn()
        {
            while (enemyCount < 4)
            {
                xPos = -17;
                zPos = -46;
                Instantiate(theEnemy, new Vector3(xPos, 0.55f, zPos), Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                enemyCount += 1;
            }
        }
}
