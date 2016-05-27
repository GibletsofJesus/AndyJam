using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{
    public static EnemyManager instance = null;
    public Enemy enemy;
    List<Enemy> enemyList = new List<Enemy>();
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
        if (instance == null)
        {
            instance = this;
        }
        currentCooldown = coolDown;
        transformList.Add(formation1);
        transformList.Add(formation2);
        transformList.Add(formation3);
	}

	public Enemy EnemyPooling()
    {
        for (int i=0;i<enemyList.Count;i++)
        {
            if (!enemyList[i].isActiveAndEnabled)
            {
                enemyList[i].enabled = true;
                return enemyList[i];
            }
        }
        Enemy e = Instantiate(enemy);
        enemyList.Add(e);
        return e;
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
                    e.gameObject.SetActive(true);

                }
                currentCooldown = 0;
                prevTrans = currentTrans;
            }
        }
    }
    void Cooldown()
    {
        if (currentCooldown<=coolDown)
        {
            currentCooldown += Time.deltaTime;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        Cooldown();
        SpawnEnemies();
	
	}
}
