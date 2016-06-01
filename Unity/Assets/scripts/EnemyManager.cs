using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct EnemyPatterns
{
    public Enemy enemy;
    public int minSpawn;
    public int maxSpawn;
    public float spawnRate;
    public Transform[] spawnLocations;

}

public class TheWave
{
    public EnemyTypes eTypes;
    public int spawnAmount;
    public float currentSpawnCool;

}
public enum EnemyTypes
{
    TROJAN,
    DOS,
    KEYLOGGER,
    SPYWARE,
    WORM,
    MALWARE,
    SPAM,
    ADWARE,
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public AudioClip bossMusic;
    List<TheWave> waves = new List<TheWave>();
    public Boss bigBoss;
    Enemy currentType;
    List<Enemy> enemyList = new List<Enemy>();
    List<Enemy> swarmEnemy = new List<Enemy>();

    float waveCoolDown = 10;
    float currentWaveCooldown = 0;
    int prevSpawnPos = 19;
    int counting = 0;
    int climbList = 0;
    [SerializeField]
    private EnemyPatterns[] enemyPatterns = null;

    bool boss = false;
    bool bossSpawned = false;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        climbList = enemyPatterns.Length - 1;

      //  

    }

    // Update is called once per frame
    void Update()
    {
        if (WaveCooldown())
        {
            TheWave wave = new TheWave();
            wave.eTypes = PickRandomEnemy();
            wave.spawnAmount = Random.Range(enemyPatterns[(int)wave.eTypes].minSpawn, enemyPatterns[(int)wave.eTypes].maxSpawn);
            wave.currentSpawnCool = enemyPatterns[(int)wave.eTypes].spawnRate;
            waves.Add(wave);

        }
        for (int i = 0; i < waves.Count; i++)
        {
            if (SpawnRateCoolDown(waves[i]))
            {
                Enemy e = EnemyPooling(enemyPatterns[(int)waves[i].eTypes].enemy);
               
                int spawnPos = Random.Range(0, enemyPatterns[(int)waves[i].eTypes].spawnLocations.Length - 1);
                while (spawnPos == prevSpawnPos)
                {
                    spawnPos = Random.Range(0, enemyPatterns[(int)waves[i].eTypes].spawnLocations.Length - 1);

                }
                        prevSpawnPos = spawnPos;

                e.transform.position = enemyPatterns[(int)waves[i].eTypes].spawnLocations[spawnPos].position;
                e.OnSpawn();
                e.gameObject.SetActive(true);
                waves[i].spawnAmount--;
                if (waves[i].spawnAmount == 0)
                {
                    waves.RemoveAt(i);
                    counting++;
                    --i;
                }
            }
        }
        if (counting == 7 && !bossSpawned)
        {
            Invoke("SpawnBoss", 3);
            bossSpawned = true;
        }

        CircleSwarm();
     

    }

    public Enemy EnemyPooling(Enemy en)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].isActiveAndEnabled&&enemyList[i].name == en.name)
            {
                enemyList[i].enabled = true;
                return enemyList[i];
            }
        }

        Enemy e = Instantiate(en);
        e.transform.parent = transform;
        enemyList.Add(e);
        return e;
    }
    void CircleSwarm()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            if (enemyList[i].gameObject.name == "DOS" && enemyList[i].isActiveAndEnabled)  // if (waves[i].eTypes == EnemyTypes.DOS)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    
                    if (enemyList[j].isActiveAndEnabled && !swarmEnemy.Contains(enemyList[j]))
                    {
                        swarmEnemy.Add(enemyList[j]);
                    }
                }
                for (int k = 0; k < swarmEnemy.Count; k++)
                {
                    for (int j = 0; j < enemyList.Count; j++)
                    {
                        if (k != j)
                        {
                            Vector2 kPos = enemyList[k].transform.position;
                            Vector2 jPos = enemyList[i].transform.position;


                            if (Vector2.Distance(kPos, jPos) < 3)
                            {
                                enemyList[k].rig.AddForce(kPos - jPos);
                            }
                        }
                    }
                }
                for (int j = 0; j < swarmEnemy.Count; ++j)
                {
                    if (!swarmEnemy[j].isActiveAndEnabled)
                    {
                        swarmEnemy.RemoveAt(j);
                        --j;
                    }
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
        //int currentTrans = Random.Range(0, transformList.Count - 1);
        //formation = transformList[currentTrans];
        //if (currentTrans != prevTrans)
        //{
        //    if (currentCooldown >= coolDown)
        //    {
        //        if (currentType!=keyLogger&&currentType!=circle)
        //        {
        //             foreach (Transform t in formation)
        //            {
        //    
        //             }
        //        }
        //        else if (currentType == circle)
        //        {
        //            for (int i=0;i<2;i++)
        //            {
        //                foreach (Transform t in formation)
        //                {
        //                    Enemy e = EnemyPooling();
        //                    e.transform.position = t.position;// *= (i+1); //This was *= but it cause the formations to exponetially move upwards over time
        //                    //e.ResetEnemy();
        //                    e.gameObject.SetActive(true);
        //                    totalEnemyCount++;
        //                }
        //            }
        //        }

        //        else if (currentType == keyLogger)
        //        {
        //            //Debug.Log("keylogger");
        //             Enemy e = EnemyPooling();
        //            e.transform.position = formation[Random.Range(0,formation.Length - 1)].position;
        //            //e.ResetEnemy();
        //            e.gameObject.SetActive(true);
        //            totalEnemyCount++;
        //        }
        //        currentCooldown = 0;
        //        prevTrans = currentTrans;
        //    }
        //}
    }

    void SpawnBoss()
    {
        soundManager.instance.music.clip = bossMusic;
        soundManager.instance.music.enabled = false;
        soundManager.instance.music.enabled = true;
        Enemy b = EnemyPooling(bigBoss);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));
        spawnPos.z = 0;
        b.transform.position = spawnPos;
        b.OnSpawn();
        b.gameObject.SetActive(true);
        bossSpawned = true;
    }

    EnemyTypes PickRandomEnemy()
    {
        EnemyTypes et;
        et = (EnemyTypes)Random.Range(0, climbList);

        climbList = Mathf.Min(climbList + 1, enemyPatterns.Length - 1);

        return et;
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

    bool WaveCooldown()
    {
        if (currentWaveCooldown < waveCoolDown)
        {
            currentWaveCooldown += Time.deltaTime;
            return false;
        }
        currentWaveCooldown = 0;
        return true;
    }
    bool SpawnRateCoolDown(TheWave w)
    {

        if (w.currentSpawnCool < enemyPatterns[(int)w.eTypes].spawnRate)
        {
            w.currentSpawnCool += Time.deltaTime;
            return false;
        }
        w.currentSpawnCool = 0;
        return true;
    }
}
