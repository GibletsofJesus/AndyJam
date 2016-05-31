using UnityEngine;
using System.Collections;

public class Enemy_KeyLogger : Enemy 
{
    GameObject player;
    bool withinDist = false;
    Vector2 target;
    float screenSide;
    //float screenBottom;
	// Use this for initialization
    Vector2 moveHere = Vector2.zero;

	void Start () 
    {
       screenSide = Camera.main.ViewportToWorldPoint(new Vector3(.3f, -.5f, .3f)).x;
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

        if (Vector2.Distance(transform.position,new Vector2(screenSide,transform.position.y))<5)
        {
            moveHere =new Vector2(-screenSide, transform.position.y);//,speed*Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position,new Vector2(-screenSide,transform.position.y))<5)
        {
            moveHere =  new Vector2(screenSide, transform.position.y);//, speed * Time.deltaTime);
        }
        transform.position = Vector2.MoveTowards(transform.position,moveHere,speed*Time.deltaTime);
        //target = player.transform.position;

        //Vector2 lerp = Vector2.MoveTowards(transform.position, new Vector2(target.x, target.y), speed*10* Time.deltaTime);

        //if (lerp.y < target.y + 8)
        //{
        //    withinDist = true;
        //}
        //if (withinDist)
        //{
        //    lerp.y = target.y + 8;
        //    lerp.x = target.x;
        //}
        //transform.position = Vector2.MoveTowards(transform.position,lerp,speed*10* Time.deltaTime);
    }
    protected override void Reset()
    {
        base.Reset();
        withinDist = false;
    }
    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform, bool _homing)
    {
        if (Input.inputString!=string.Empty)
        {
            return base.Shoot(_projData, _direction, _shootTransform, _homing);
        }
        else
            return false;
    }
}
