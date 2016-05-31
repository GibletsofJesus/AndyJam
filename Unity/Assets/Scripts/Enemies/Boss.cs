using UnityEngine;
using System.Collections;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0,9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    public Enemy rareBastards;
    float maxCool = 5;
    float minicool = 0.8f;
    float minicooler = 0.8f;
    float cool;
    int dosCount = 0;
    bool move = false;
	// Use this for initialization
	void Start ()
    {
        //SetActor(100, 1, 1, 0.8f);
        cool = maxCool;
        //safeHealth = health;
	
	}

    void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    {
        Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
        e.transform.position = _spawnPoint;
        e.gameObject.SetActive(true);

    }
	// Update is called once per frame
	protected override void Update () 
    {
        
        MiniCool();

        if (cool<=maxCool)
        {
            cool += Time.deltaTime;
        }
              
       if(move)
       {
          EyeShooting();
       }
      
      
        //Debug.Log("count " + dosCount + " cool " + cool);

        base.Update();
	}

    protected override void Movement()
    {
		transform.position = Vector2.MoveTowards(transform.position, bossTarget,speed*2* Time.deltaTime);
		if (Vector2.Distance(transform.position,bossTarget)<1)
        {
            move = true;
        }
    }

    void EyeShooting()
    {
        if (dosCount<10&&cool>=maxCool)
        {
            if (MiniCool())
            {
              SpawnBossEnemies(eyeShot[Random.Range(0,eyeShot.Length - 1)].position, littleBastards);
                minicooler = 0;
                dosCount++;
            }
        }
        else
        {
            dosCount = 0;
        }
        if (dosCount >= 10 && cool >= maxCool)
        {
            cool = 0;
        }
    }
   
  
   bool MiniCool()
    {
        if (minicooler <= minicool)
        {
            minicooler += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
