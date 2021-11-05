using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{

    //PRIM
    List<GameObject> vertex;
    Collider[] hitColliders, prevHitColliders;
    private static int V = 5;
    static int[] parent;

    //Effect
    public GameObject lightningEffect;

    //Radius Visualization
    public LineRenderer radiusLine;
    public int segments = 40;
    public float xradius = 10f;
    public float yradius = 10f;
    public float radius = 10f;
    private bool checkEnemyInRange = false;

    //Damage
    public int electricDamage = 125;

    public LayerMask whatIsEnemy;
    public Player player;
    public SkillPointsBar skillBar;

    public Text messageDisplay;
    public string message;
    public Animator animator;


    void initSpecialEffect()
    {
        vertex = new List<GameObject>();
        PerformSpecialEffect();
        initRadius();
    }

    void initRadius()
    {
        radiusLine.positionCount = segments + 1;
        //radiusLine.SetVertexCount(segments + 1);
        radiusLine.useWorldSpace = false;
        CreatePoints();
        radiusLine.gameObject.SetActive(false);
    }

    void CreatePoints()
    {
        float x;
        float y = 0f;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            radiusLine.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    IEnumerator turnOnRadiusLine()
    {
        checkEnemyInRange = true;
        radiusLine.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        radiusLine.gameObject.SetActive(false);
        checkEnemyInRange = false;
    }

    IEnumerator SetBombUIEnemyInRadius(Vector3 center)
    {
        float startTime = Time.time;
        float timeElapsed = 0;
        while (timeElapsed <= 5)
        {
            timeElapsed = Time.time - startTime;
            hitColliders = null;
            hitColliders = Physics.OverlapSphere(transform.position, radius, whatIsEnemy);
            V = hitColliders.Length;
            if (prevHitColliders != null)
            {
                foreach (var phc in prevHitColliders)
                {
                    bool flagExist = false;
                    foreach (var hc in hitColliders)
                    {
                        if (phc.Equals(hc))
                        {
                            flagExist = true;
                            break;
                        }
                    }
                    if (!flagExist)
                    {
                        phc.GetComponent<Enemy>().setInRange(false);
                    }
                }
            }
            foreach (var h in hitColliders)
            {
                h.GetComponent<Enemy>().setInRange(true);
            }

            prevHitColliders = hitColliders;
            yield return new WaitForSeconds(0);
        }
        foreach (var phc in prevHitColliders)
        {
            phc.GetComponent<Enemy>().setInRange(false);
        }
    }
    //#endregion


    static int minKey(float[] key, bool[] mstSet)
    {
        // Initialize min value
        float min = float.MaxValue;
        int min_index = -1;

        for (int v = 0; v < V; v++)
        {
            if (mstSet[v] == false && key[v] < min)
            {
                min = key[v];
                min_index = v;
            }
        }
        return min_index;
    }

    static void primMST(List<List<float>> graph)
    {

        parent = new int[V];

        float[] key = new float[V];

        bool[] mstSet = new bool[V];

        for (int i = 0; i < V; i++)
        {
            key[i] = float.MaxValue;
            mstSet[i] = false;
        }

        key[0] = 0;
        parent[0] = -1;

        if (V < 2)
        {
            parent[0] = 0;
        }

        for (int count = 0; count < V - 1; count++)
        {
            int u = minKey(key, mstSet);
            //Debug.Log(u);
            //Debug.Log(mstSet[u]);
            mstSet[u] = true;

            for (int v = 0; v < V; v++)
            {
                if (((List<float>)graph[u])[v] != 0 && mstSet[v] == false
                    && ((List<float>)graph[u])[v] < key[v])
                {
                    parent[v] = u;
                    key[v] = ((List<float>)graph[u])[v];
                }
            }
        }
    }

    void GiveElectricDamageAndLightningStrike()
    {
        List<List<string>> vertexConnections = new List<List<string>>(V);
        for (int i = 0; i < V; i++)
        {
            vertexConnections.Add(new List<string>());
        }

        for (int i = V > 1 ? 1 : 0; i < V; i++)
        {
            string vertex1 = vertex[i].name;
            string vertex2 = vertex[parent[i]].name;
            vertexConnections[i].Add(vertex2);


            GameObject lightning = Instantiate(lightningEffect, vertex[parent[i]].transform.position, Quaternion.identity);
            DigitalRuby.LightningBolt.LightningBoltScript lightningScript = lightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
            lightningScript.StartObject = vertex[parent[i]].gameObject;
            lightningScript.EndObject = vertex[i].gameObject;
            //lightningScript.Duration = 5f;
            Destroy(lightning, 6f);

            if (V > 1) 
            {
                vertexConnections[parent[i]].Add(vertex1);
            }
        }
        for (int i = 0; i < V; i++)
        {
            if (vertex[i].tag != "Player")
            {
                Enemy t = vertex[i].GetComponent<Enemy>();
                t.TakeDamage(electricDamage * vertexConnections[i].Count);
            }
        }
        player.currentSkill -= 75;
        skillBar.SetSkill(player.currentSkill);
        if(player.currentSkill <= 0)
        {
            animator.SetBool("isOpen", true);
            message = "Not enough skill!";
            messageDisplay.text = message;
        }
        //Debug.Log("Play sound");
        SoundManager.PlaySound("thunder");
    }
    void PerformSpecialEffect()
    {
        if (vertex.Count > 0)
            vertex.Clear();

        var adjacentList = new List<List<float>>();
        makeAdjacencyList(adjacentList);
        primMST(adjacentList);
        GiveElectricDamageAndLightningStrike();
    }

    void makeAdjacencyList(List<List<float>> adjacentList)
    {
        hitColliders = Physics.OverlapSphere(transform.position, radius, whatIsEnemy);

        foreach (Collider c in hitColliders)
        {
            vertex.Add(c.gameObject);
        }
        vertex.Add(gameObject);

        V = vertex.Count;

        foreach (var i in vertex)
        {
            var adjacentListRow = new List<float>();
            foreach (var j in vertex)
            {
                float edgeWeight;
                if (i.Equals(j))
                    edgeWeight = 0;
                else
                    edgeWeight = Vector3.Distance(j.transform.position, i.transform.position);
                adjacentListRow.Add(edgeWeight);
            }
            adjacentList.Add(adjacentListRow);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            initSpecialEffect();
        }
    }

}
