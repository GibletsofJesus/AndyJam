using UnityEngine;
using System.Collections;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0,9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    float maxCool = 5;
    float minicool = 0.8f;
    float minicooler = 0.8f;
    float cool;
    int dosCount = 0;
    bool move = false;
	// Use this for initialization
	void Start ()
    {
        SetActor(50, 1, 1, 0.8f);
        cool = maxCool;
        safeHealth = health;
	
	}
	
	// Update is called once per frame
	public override void Update () 
    {
        
        MiniCool();
        if (cool<maxCool)
        {
            cool += Time.deltaTime;
        }
        else// if (cool>=maxCool)
        {
            dosCount = 0;
        }
        if (dosCount<10)
        {
            if (MiniCool()&&move)
            {
                MouthShooting();
                dosCount++;
            }
        }
        else
        {
            Debug.Log("count " + dosCount + " cool " + cool);
            cool = 0;
        }
        base.Update();
	}
    public override void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, bossTarget,GetSpeed()*2);
        if (Vector2.Distance(transform.position,bossTarget)<1)
        {
            move = true;
        }
    }
    void MouthShooting()
    {
        Debug.Log("doscount " + dosCount);
        EnemyManager.instance.SpawnBossEnemies(mouthShot.transform.position,littleBastards);
        minicooler = 0;
        cool = 0;
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
