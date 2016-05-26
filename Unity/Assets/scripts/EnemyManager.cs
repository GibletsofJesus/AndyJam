using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{
    public Enemy enemy;
    List<Enemy> enemyList = new List<Enemy>();
	// Use this for initialization
	void Start () 
    {
	
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


	// Update is called once per frame
	void Update () {
	
	}
}
