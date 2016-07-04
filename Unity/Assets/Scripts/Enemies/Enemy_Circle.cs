using UnityEngine;
using System.Collections;

public class Enemy_Circle : Enemy 
{
    private GameObject player = null;
    Vector2 moveTarget;
    bool circleJerks = false;
    int rotWay = 20;
 	
	// Update is called once per frame
	protected override void Update () 
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
        }
        if (Player.instance)
        {
            moveTarget = Player.instance.transform.position;
        }
	}
	protected override void Movement()
    {
        Vector2 pos = transform.position;
        float distance = Vector2.Distance(transform.position, moveTarget);
        if (!circleJerks)
        {
            if (distance > 7)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
            }
            else
            {
                circleJerks = true;
            }
        }
        if (circleJerks)
        {
            if (player)
            {
                rotWay = Camera.main.WorldToViewportPoint(Player.instance.transform.position).x > 0.5f ? 20 : -20;
            }
            transform.RotateAround(moveTarget, transform.forward, rotWay* (speed*Time.deltaTime));
            
            if (distance<=6.5)
            {
				transform.position = Vector2.MoveTowards(transform.position, (pos + moveTarget), speed*Time.deltaTime);
            }
            else if (distance>7)
            {
				transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed*Time.deltaTime);
            }
        }
        float lookAngle = Mathf.Atan2(transform.position.x - moveTarget.x, transform.position.y - moveTarget.y) * Mathf.Rad2Deg;
        Quaternion newRot = new Quaternion(0, 0, 0, 0);
        newRot.eulerAngles = new Vector3(0, 0, -lookAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 20*(speed * Time.deltaTime));
    }

    public override void Reset()
    {
        base.Reset();
        circleJerks = false;
    }
    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        return false;
        //return base.Shoot(_projData, _direction, _shootTransform);
    }
   
}
