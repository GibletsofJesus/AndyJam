using UnityEngine;
using System.Collections;

public class Enemy_KeyLogger : Enemy 
{
   
    Vector2 target;
    float screenSide;
	// Use this for initialization
    Vector2 moveHere = Vector2.zero;

	void Start () 
    {
       screenSide = Camera.main.ViewportToWorldPoint(new Vector3(.3f, -.5f, .3f)).x;
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

        if (Vector2.Distance(transform.position, new Vector2(screenSide, transform.position.y)) < 1)
        {
            moveHere = new Vector2(-screenSide, transform.position.y);
        }
        else if (Vector2.Distance(transform.position, new Vector2(-screenSide, transform.position.y)) < 1)
        {
            moveHere = new Vector2(screenSide, transform.position.y);
        }
        transform.position = Vector2.MoveTowards(transform.position, moveHere, speed * Time.deltaTime);
    }
    protected override void Reset()
    {
        base.Reset();
    }

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (Input.inputString!=string.Empty)
        {
            //
            return base.Shoot(_projData, -(transform.position - new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y)), _shootTransform);
        }
        else
            return false;
    }
}
