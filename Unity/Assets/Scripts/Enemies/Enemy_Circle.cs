using UnityEngine;
using System.Collections;

public class Enemy_Circle : Enemy 
{
    GameObject player;
    Vector2 moveTarget;
    bool circleJerks = false;
 
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
       base.Update();
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
				transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * 2 );//* Time.deltaTime);
            }
            else
            {
                    circleJerks = true;
            }
        }
        if (circleJerks)
        {
              transform.RotateAround(moveTarget, transform.forward, 45 * speed);
            
            if (distance<=6)
            {
				transform.position = Vector2.MoveTowards(transform.position, (pos + moveTarget), speed * 2);// * Time.deltaTime);
            }
            else if (distance>7)
            {
				transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * 2 );//* Time.deltaTime);
            }
        }
    }

    protected override void Reset()
    {
        base.Reset();
        circleJerks = false;
    }
    //public override void Shoot(Vector2 _direction, GameObject[] _shootTransform, string _ignore)
    //{
    //    if (transform.position.y > player.transform.position.y)
    //    {
    //        base.Shoot(_direction, _shootTransform, _ignore);
    //    }
    //}
}
