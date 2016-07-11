using UnityEngine;
using System.Collections;

public class Enemy_InfectedBlack : Enemy 
{
    ///select random point in gamespace
    ///warp to pos wait for x seconds, fire
    ///repeat
    ///
    public ParticleSystem entranceOrExit;
    float screenSide;
    float[] screenHeight = new float[2];
    SpriteRenderer spRend;
    Collider2D col;
    #region cooldown floats
    float moveCool = 0;
    float maxMoveCool = 5;
    float shotCool = 0;
    float maxShotCool = 0.8f;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        screenSide = Camera.main.ViewportToWorldPoint(new Vector3(.35f, -.5f, .3f)).x;
        screenHeight[0] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
        screenHeight[1] = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        spRend = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
       
        base.Update();
       
       CoolDowns();
       ShakeABit();
    }
    protected override void Movement()
    {
        base.Movement();
        if (MoveCooldown())
        {
            StartCoroutine("Teleport");
            moveCool = 0;
        }
    }


   Vector2 RandomPointInSpace(float _XMax, float _XMin,float _YMax,float _YMin)
    {
        return new Vector2(Random.Range(_XMin, _XMax), Random.Range(_YMin, _YMax));     
    }
    IEnumerator Teleport()
   {
       moveCool = 0;
       entranceOrExit.Play();
       EnableComponents(false);
       yield return new WaitForSeconds(1f);
       transform.position = RandomPointInSpace(screenSide, -screenSide, screenHeight[0], screenHeight[1]);
       entranceOrExit.Play();
        Shoot(projData, -transform.up.normalized, shootTransform);
       EnableComponents(true);
       moveCool = 0;
   }
    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (spRend.enabled&&!entranceOrExit.isPlaying)
        {
            Vector3 shootDir = -(transform.position - new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y));

            if (shootDir.x > 0)
                shootDir.x = Mathf.Clamp(shootDir.x, 0, 5);
            else
                shootDir.x = Mathf.Clamp(shootDir.x, -5, 0);

            return base.Shoot(_projData, shootDir, _shootTransform);
        }
        return false;
    }
   void EnableComponents(bool _enable)
    {
        spRend.enabled = _enable;
        col.enabled = _enable;
    }
  
   #region Cooldown Functions
   bool MoveCooldown()
   {
       if (moveCool < maxMoveCool)
       {
           moveCool += Time.deltaTime;
           return false;
       }
       else
           return true;
   }
    bool ShotCoolDown()
   {
       if (MoveCooldown())
       {
           if (shotCool < maxShotCool)
           {
               shotCool += Time.deltaTime;
               return false;
           }
           else
           {
               return true;
           }
       }
       else
       {
           return false;
       }
   }

    void ShakeABit()
    {
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Random.Range(-0.1f, 0.1f)), transform.position.y + Mathf.Sin(Random.Range(-0.1f, 0.1f)), transform.position.z);
    }
    void CoolDowns()
    {
        ShotCoolDown();
        MoveCooldown();
    }
   #endregion
}
