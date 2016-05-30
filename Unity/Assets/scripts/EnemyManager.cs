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
    public Boss bigBoss;
    Enemy currentType;
    List<Enemy> enemyList = new List<Enemy>();
    List<Enemy> swarmEnemy = new List<Enemy>();
    public Transform[] formation1;
    public Transform[] formation2;
    public Transform[] formation3;
    
   // bool boss = false;
    Transform[] formation;
    List<Transform[]> transformList = new List<Transform[]>();
    float coolDown = 3;
    float currentCooldown;
    float circleCooldown = 1;
    int prevTrans = 4;
    int totalEnemyCount = 0;
    int maxCircleSpawn = 10;
    bool boss = false;
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
        if (totalEnemyCount <= 10)
        {
       //     SpawnEnemies();
        }

       // else if (CurrentlyActiveEnemies()<1)
        if (!boss)
        {
            currentType = bigBoss;
            SpawnBoss();
            boss = true;
        }
        CircleSwarm();

 
        //if (CurrentlyActiveEnemies() <=10)
        //{
        //    for (int i = 0; i < maxCircleSpawn; i++)
        //    {
        //        Cooldown();
        //        if (circleCooldown >= 1)
        //        {
        //            currentType = circle;
        //        ///    SpawnCircling();
        //        }
        //    }
        //}
    }



    public Enemy EnemyPooling()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].isActiveAndEnabled&&enemyList[i].tag == currentType.tag)
            {
                enemyList[i].enabled = true;
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
                            enemyList[i].rig.AddForce(iPos - jPos);
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
        if (currentType==bigBoss)
        {
            return null;
        }
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
                    //e.ResetEnemy();
                    e.gameObject.SetActive(true);
                    totalEnemyCount++;
                }
                currentCooldown = 0;
                prevTrans = currentTrans;
            }
        }
    }
    void SpawnBoss()
    {
        currentType = bigBoss;
            Enemy b = EnemyPooling();
            Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f,1.2f));
        spawnPos.z=0;
        b.transform.position = spawnPos;
        b.gameObject.SetActive(true);
       // boss = true;
              
    }

    public void SpawnBossEnemies(Vector2 _spawnPoint,Enemy _enemy)
    {
        currentType = _enemy;
        Enemy e = EnemyPooling();
        e.transform.position = _spawnPoint;
        //e.ResetEnemy();
        e.gameObject.SetActive(true);
        
    }
    void SpawnCircling()
    {
       
       
       //     for (int i = 0; i < maxCircleSpawn; i++)
       //     {
       //         if (circleCooldown >= 1)
       //         {
       // currentType = circle;
       //// if (currentCooldown >= coolDown)
        {
            Enemy e = EnemyPooling();
            e.transform.position = formation3[0].position;
            //e.ResetEnemy();
            e.gameObject.SetActive(true);
            circleCooldown = 0;
        //}
        //currentCooldown = 0;
        //    }
        }
    }
    int CurrentlyActiveEnemies()
    {
        int enemies = 0;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].isActiveAndEnabled)
            {
                enemies++;
            }
            else
            {
                continue;
            }
        }
        return enemies;
    }
    
    void Cooldown()
    {
        if (currentCooldown <= coolDown)
        {
            currentCooldown += Time.deltaTime;
        }
        circleCooldown += Time.deltaTime;
    }
}
