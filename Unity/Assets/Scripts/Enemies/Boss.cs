using UnityEngine;
using System.Collections;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Transform[] eyeShot;
    public Enemy littleBastards;
    float maxCool = 5;
    float minicool = 0.8f;
    float minicooler = 0.8f;
    float cool;
    int dosCount = 0;
    bool move = false;
	// Use this for initialization

	protected override void Awake()
	{
		base.Awake ();
		cool = maxCool;
	}

	void Start ()
    {
        //SetActor(5000000, 1, 1, 0.8f);
        //cool = maxCool;
        //safeHealth = health;
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        
        MiniCool();
        if (cool<=maxCool)
        {
            cool += Time.deltaTime;
        }
        else if (cool>=maxCool)
        {
            dosCount = 0;
          

        }
        if (dosCount<10)
        {
            if (MiniCool())
            MouthShooting();
          
        }
        else
        {
            
            cool = 0;
        }
		base.Update ();
	}
    protected override void Movement()
    {

        if (!move)
        {
			transform.Translate(-transform.up * Time.deltaTime, Space.World);
        }
        if (transform.position.y >= Camera.main.ViewportToWorldPoint(new Vector2(0, 0.6f)).y)
        {
            move = true;
            // body.AddForce(new Vector2(-80, 0));
        }

        //base.Movement();
    }
    void MouthShooting()
    {
        EnemyManager.instance.SpawnBossEnemies(mouthShot.transform.position,littleBastards);
        minicooler = 0;
            dosCount++;   
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
