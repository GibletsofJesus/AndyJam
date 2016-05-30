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
    List<Enemy> enemyTypeList = new List<Enemy>();
    float coolDown = 3;
    float currentCooldown;
    float circleCooldown = 1;
    int prevTrans = 4;
    int climbList = 1;
    int climbListMultiplier = 10;

    private int totalEnemyCount = 0;
	[SerializeField] private int maxEnemies = 50;

    int circleSpawnCount = 0;
    int maxCircleSpawn = 10;
    bool boss = false;
    bool bossSpawned = false;
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
        enemyTypeList.Add(currentType);
        enemyTypeList.Add(diagonal);
        enemyTypeList.Add(circle);
        enemyTypeList.Add(keyLogger);
	}

    // Update is called once per frame
    void Update()
    {
        Cooldown();
       
        if (boss&&!bossSpawned)
        {
          SpawnBoss();
        }
        CircleSwarm();
        //Debug.Log("total spawned "+totalEnemyCount + " climbListMultiplier "+climbListMultiplier+ "  climbList "+climbList );
        //Debug.Log("current Enemies " + CurrentlyActiveEnemies());
        SpawnEnemies();
        RandomizeEnemies();
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
			for (int i = 0; i < swarmEnemy.Count; ++i)
            {
				if (!swarmEnemy[i].isActiveAndEnabled)
                {
					swarmEnemy.RemoveAt(i);
					--i;
                }
            }
        }
    }

    /*public GameObject FindClosestEnemyToPlayer(float maxDistance, Transform origin)
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
    }*/

    void SpawnEnemies()
    {
        int currentTrans = Random.Range(0, transformList.Count - 1);
        formation = transformList[currentTrans];
        if (currentTrans != prevTrans)
        {
            if (currentCooldown >= coolDown)
            {
                if (currentType!=keyLogger&&currentType!=circle)
                {
                     foreach (Transform t in formation)
                    {
                    Enemy e = EnemyPooling();
                    e.transform.position = t.position;
                    //e.ResetEnemy();
                    e.gameObject.SetActive(true);
                    totalEnemyCount++;
                     }
                }
                else if (currentType == circle)
                {
                    for (int i=0;i<2;i++)
                    {
                        foreach (Transform t in formation)
                        {
                            Enemy e = EnemyPooling();
							e.transform.position = t.position;// *= (i+1); //This was *= but it cause the formations to exponetially move upwards over time
                            //e.ResetEnemy();
                            e.gameObject.SetActive(true);
                            totalEnemyCount++;
                        }
                    }
                }
                    
                else if (currentType == keyLogger)
                {
                    //Debug.Log("keylogger");
                     Enemy e = EnemyPooling();
                    e.transform.position = formation[Random.Range(0,formation.Length - 1)].position;
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
        bossSpawned = true;
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
        currentType = circle;
        if (circleCooldown >= 0.5f && circleSpawnCount < maxCircleSpawn)
        {
            Enemy e = EnemyPooling();
            e.transform.position = formation3[0].position;
            //e.ResetEnemy();
            e.gameObject.SetActive(true);
			Debug.Log ("Hey I just broke");
            circleCooldown = 0;
            circleSpawnCount++;
        }
    }
    void RandomizeEnemies()
    {
		currentType = enemyTypeList[Random.Range(0, Mathf.Min (enemyTypeList.Count - 1, climbList))];
      
        if (climbList < enemyTypeList.Count)
        {
            if (totalEnemyCount >= climbListMultiplier)
            {

                climbList++;
                climbListMultiplier += 10;
                coolDown -= 0.5f;
            }
        }
        else if (totalEnemyCount >= 20)
        {
            boss = true;
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
