using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{
    public static EnemyManager instance = null;
    public Enemy enemy;
    public Enemy_Circle circle;
    public Enemy_Diagonal diagonal;
    public Enemy_KeyLogger keyLogger;
    Enemy currentType;
    List<Enemy> enemyList = new List<Enemy>();
    List<Enemy> swarmEnemy = new List<Enemy>();
    public Transform[] formation1;
    public Transform[] formation2;
    public Transform[] formation3;
    Transform[] formation;
    List<Transform[]> transformList = new List<Transform[]>();
    float coolDown = 3;
    float currentCooldown;
    int prevTrans = 4;
	// Use this for initialization
	void Awake() 
    {
        currentType = enemy;
        if (instance == null)
        {
            instance = this;
        }
        currentCooldown = coolDown;
        transformList.Add(formation1);
        transformList.Add(formation2);
        transformList.Add(formation3);
	}

    // Update is called once per frame
    void Update()
    {
        Cooldown();
        SpawnEnemies();
        CircleSwarm();
    }
	//public Enemy EnemyPooling()


    public Enemy EnemyPooling()

    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].isActiveAndEnabled&&enemyList[i].tag == currentType.tag)
            {
                enemyList[i].enabled = true;

                // enemyList[i].gameObject.SetActive(true);

                return enemyList[i];
            }
        }
        Enemy e = Instantiate(currentType);
        enemyList.Add(e);
        return e;
    }
    void CircleSwarm()
    {
        if (currentType.name == "DOS")
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].isActiveAndEnabled&&!swarmEnemy.Contains(enemyList[i]))
                {
                    swarmEnemy.Add(enemyList[i]);
                }
            }
            for (int i=0;i<swarmEnemy.Count;i++)
            {
                for (int j=0;j<enemyList.Count;j++)
                {
                    if (i!=j)
                    {
                        Vector2 iPos = enemyList[i].transform.position;
                        Vector2 jPos = enemyList[j].transform.position;


                        if (Vector2.Distance(iPos,jPos)<2)
                        {
                            enemyList[i].body.AddForce(iPos - jPos);
                        }
                    }
                }
            }
            foreach (Enemy e in swarmEnemy)
            {
                if (!e.isActiveAndEnabled)
                {
                    swarmEnemy.Remove(e);
                }
            }
        }
    }

    public GameObject FindClosestEnemyToPlayer(float maxDistance, Transform origin)
    {
        GameObject target = this.gameObject;
        float lowestDistance = 999;

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<SpriteRenderer>().color = Color.white;
            float distance = Vector2.Distance(enemyList[i].transform.position, origin.position);
            if (distance < lowestDistance && distance < maxDistance)
            {
                target = enemyList[i].gameObject;
                lowestDistance = distance;
            }
        }
        if (target != this.gameObject)
        {
            target.GetComponent<SpriteRenderer>().color = Color.red;
            return target;
        }
        else
            return null;
    }

    void SpawnEnemies()
    {
        int currentTrans = Random.Range(0, 3);
        formation = transformList[currentTrans];
        if (currentTrans != prevTrans)
        {
            if (currentCooldown >= coolDown)
            {
                foreach (Transform t in formation)
                {
                    Enemy e = EnemyPooling();
                    e.transform.position = t.position;
                    e.ResetEnemy();
                    e.gameObject.SetActive(true);
                }
                currentCooldown = 0;
                prevTrans = currentTrans;
            }
        }
    }
    void Cooldown()
    {
        if (currentCooldown <= coolDown)
        {
            currentCooldown += Time.deltaTime;
        }
    }

    //// Update is called once per frame
    //void Update ()
    //{
    //    Cooldown();
    //    SpawnEnemies();
	
    //}

}
