using UnityEngine;
using System.Collections;

public class Enemy_Trojan : Enemy
{
    bool split = true;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
        }
    }
    protected override void Movement()
    {
        Vector2 movement = -transform.up * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }


    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        Vector3 shootDir = -(transform.position - new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y));

        if (shootDir.x > 0)
            shootDir.x = Mathf.Clamp(shootDir.x, 0, 5);
        else
            shootDir.x = Mathf.Clamp(shootDir.x, -5, 0);

        return base.Shoot(_projData, shootDir, _shootTransform);
    }
    protected override void Death()
    {
       if (split)
       {
           for (int i = 0; i < 2; i++)
           {
               Enemy e = EnemyManager.instance.EnemyPooling(this);
               e.GetComponent<Enemy_Trojan>().SetSplit(false);
               e.transform.position = new Vector2(e.transform.position.x + (i*1.5f),e.transform.position.y);
               e.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
           }

       }
       base.Death();


    }
    void SetSplit(bool b)
    {
        split = b;
    }

}