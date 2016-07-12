using UnityEngine;
using System.Collections;

public class Enemy_InfectedGreen : Enemy 
{
    [SerializeField] private SpriteRenderer spRend = null;
    [SerializeField] private BoxCollider2D col = null;
    [SerializeField] private ParticleSystem entrance = null;

    private Enemy_InfectedGreen e;
    float screenSide;
    float screenHeight;
    float screenHeightMin;
    bool oneShot = false;
    int currentAmount;
    int maxAmount = 15;
    float cool = 0;
    float maxCool = 2;
    public bool impotence = false;
    protected override void Awake()
    {
        base.Awake();
        
       
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!Cool())
        {
            shootCooldown = 0;
        }
        else if (oneShot)
        {
            Shoot(projData, -Vector2.up, shootTransform);
        }
        
        if (Cool())
        {
            if (!oneShot)
            {
                entrance.Play();
                EnableComponents(true);
                if (currentAmount < maxAmount)
                {
                    SpawnNew();
                }
                oneShot = true;
            }
        }
    }
    protected override void Movement()
    {
        ShakeABit();
        base.Movement();
    }
    void ShakeABit()
    {
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Random.Range(-0.051f, 0.051f)), 
            transform.position.y + Mathf.Sin(Random.Range(-0.051f, 0.051f)), transform.position.z);
    }

    Vector2 RandomPointInSpace(float _XMax, float _XMin, float _YMax, float _YMin)
    {
        return new Vector2(Random.Range(_XMin, _XMax), Random.Range(_YMin, _YMax));
    }

   public void EnableComponents(bool _enable)
    {
        spRend.enabled = _enable;
        col.enabled = _enable;
    }
    public void SetPosition(Vector2 _pos)
   {
       transform.position = _pos;
   }
    void SpawnNew()
    {
        if (!impotence)
        {
            e = (Enemy_InfectedGreen)EnemyManager.instance.EnemyPooling(this);
            e.OnSpawn();
            e.gameObject.SetActive(true);
            e.currentAmount = currentAmount+=1;
        }
    }
    bool Cool()
    {
        if (cool<maxCool)
        {
            cool += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
    protected override void Death()
    {
        if (e != null)
        { 
            e.oneShot = false; 
        }
        else
        {
            if (!impotence)
            {
                SpawnNew();
                e.impotence = true;
            }
        }
        EnableComponents(true);
        base.Death();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        screenSide = Camera.main.ViewportToWorldPoint(new Vector3(.35f, -.5f, .3f)).x;
        screenHeight = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.8f, 0)).y;
        screenHeightMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
        SetPosition(RandomPointInSpace(screenSide, -screenSide, screenHeight, screenHeightMin));
        EnableComponents(false);
        cool = 0;
        oneShot = false;
        impotence = false;
        currentAmount = 0;
            
    }
}
