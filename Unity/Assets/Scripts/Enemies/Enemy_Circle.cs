using UnityEngine;
using System.Collections;

public class Enemy_Circle : Enemy 
{
    GameObject player;
    Vector2 moveTarget;
    bool circleJerks = false;
    int rotWay = 20;
 
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
        }
	}
	protected override void Movement()
    {
        if (player != null)
        {
            moveTarget = player.transform.position;
        }
        Vector2 pos = transform.position;
        float distance = Vector2.Distance(transform.position, moveTarget);
        if (!circleJerks)
        {
            if (distance > 7)
            {
				transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed*Time.deltaTime);
            }
            else
            {
                    circleJerks = true;
            }
        }
        if (circleJerks)
        {
            if (Camera.main.WorldToViewportPoint(player.transform.position).x>0.5f)
            {
                rotWay = 20;
            }
            else
            {
                rotWay = -20;
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

    protected override void Reset()
    {
        base.Reset();
        circleJerks = false;
    }
   
}
