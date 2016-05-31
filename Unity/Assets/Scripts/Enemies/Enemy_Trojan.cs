using UnityEngine;
using System.Collections;

public class Enemy_Trojan : Enemy
{
   
	     
	protected override void Awake()
	{
		base.Awake ();
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

   

	
   
}


