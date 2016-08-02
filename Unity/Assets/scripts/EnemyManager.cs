using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public struct Level
{
    public float waveCoolDown;
    public EnemyTypes[] enemypatterns;
    public int numWavesTillBoss;
    public Boss boss;
}

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
	KEYLOGGER,
    KEYLOGGER_HARD,
    DOS,
    DOS_HARD,
    TROJAN,
    TROJAN_HARD,
    SPYWARE,
    SPYWARE_HARD,
	SPAM,
    SPAM_HARD,
	MALWARE,
    MALWARE_HARD,
	ADWARE,
    ADWARE_HARD,
    GREEN,
    BLACK,
    COUNT 
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public AudioClip bossMusic,standardMusic;
    List<TheWave> waves = new List<TheWave>();
    
    List<Enemy> enemyList = new List<Enemy>();
    List<Enemy> swarmEnemy = new List<Enemy>();

    [SerializeField] private EnemyPatterns[] enemyPatterns = null;

    float currentWaveCooldown = 0;
    int prevSpawnPos = 19;
    private int currentWave = 0;

    private int currentLevel = 0;
    [SerializeField] private Level[] levels = null;
  
    bool bossSpawned = false;

    bool logicPaused = true;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void fadeInMusic()
    {
        soundManager.instance.music.clip = standardMusic;
        soundManager.instance.music.enabled = false;
        soundManager.instance.music.enabled = true;
        soundManager.instance.music.DOFade(1, 3);
    }

    public void Begin()
    {
        LevelText.instance.TutorialElementFinished(true);
        logicPaused = false;
        currentLevel = 0;
        soundManager.instance.music.DOFade(0, 3);
        Invoke("fadeInMusic", 3.25f);
        //currentLevel = levels.Length - 2;
        //NextLevel();
    }

    public void NextLevel()
    {
        soundManager.instance.music.DOFade(0, 3);
        Invoke("fadeInMusic",3.25f);

        foreach (Enemy _e in FindObjectsOfType<Enemy>())
        {
            if (_e.isActiveAndEnabled)
            {
                _e.Death(true);
            }
        }
        ++currentLevel;
        if (currentLevel == levels.Length)
        {
            GameComplete();
        }
        else
        {
            currentWave = 0;
            currentWaveCooldown = 0;
            bossSpawned = false;
            LevelText.instance.SetText("LEVEL " + (currentLevel + 1).ToString());
            LevelText.instance.ShowText();
        }
}

    public void GameComplete()
    {
        GameStateManager.instance.WinState();
        logicPaused = true;
    }

    public void ShortCut()
    {
        if (!logicPaused)
        {
            if (currentWave < levels[currentLevel].numWavesTillBoss)
            {
                currentWave = levels[currentLevel].numWavesTillBoss;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Number of enemies " + Enemy.numAliveEnemies);
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!logicPaused)
            {
                if (currentWave < levels[currentLevel].numWavesTillBoss)
                {
                    if (WaveCooldown())
                    {
                        TheWave wave = new TheWave();
                        wave.eTypes = PickRandomEnemy();
                        wave.spawnAmount = Random.Range(enemyPatterns[(int)wave.eTypes].minSpawn, enemyPatterns[(int)wave.eTypes].maxSpawn + 1);
                        wave.currentSpawnCool = enemyPatterns[(int)wave.eTypes].spawnRate;
                        waves.Add(wave);

                    }
                    for (int i = 0; i < waves.Count; i++)
                    {
                        if (SpawnRateCoolDown(waves[i]))
                        {
                            Enemy e = EnemyPooling(enemyPatterns[(int)waves[i].eTypes].enemy);

                            int spawnPos = Random.Range(0, enemyPatterns[(int)waves[i].eTypes].spawnLocations.Length);
                            while (spawnPos == prevSpawnPos)
                            {
                                spawnPos = Random.Range(0, enemyPatterns[(int)waves[i].eTypes].spawnLocations.Length);

                            }
                            prevSpawnPos = spawnPos;
                            EnemyPatterns _pat = enemyPatterns[(int)waves[i].eTypes];
                            e.transform.position = _pat.spawnLocations[spawnPos].position;
                            e.OnSpawn();
                            e.gameObject.SetActive(true);
                            waves[i].spawnAmount--;
                            if (waves[i].spawnAmount == 0)
                            {
                                waves.RemoveAt(i);
                                currentWave++;
                                --i;
                            }
                        }
                    }
                }
                else if (!bossSpawned && Enemy.numAliveEnemies == 0)
                {
                    Invoke("SpawnBoss", 3);
                    bossSpawned = true;
                    waves.Clear();
                }

                CircleSwarm();

            }
        }
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

    void SpawnBoss()
    {
        soundManager.instance.music.clip = bossMusic;
        soundManager.instance.music.enabled = false;
        soundManager.instance.music.enabled = true;
        Enemy b = EnemyPooling(levels[currentLevel].boss);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));
        spawnPos.z = 0;
        b.transform.position = spawnPos;
        b.OnSpawn();
        b.gameObject.SetActive(true);
        bossSpawned = true;
        VisualCommandPanel.instance.AddMessage("Type the boss password to defeat it");
    }

    EnemyTypes PickRandomEnemy()
    {
        EnemyTypes et;
        et = levels[currentLevel].enemypatterns[Random.Range(0, levels[currentLevel].enemypatterns.Length)];
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
        if (currentWaveCooldown < levels[currentLevel].waveCoolDown)
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
