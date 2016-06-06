using UnityEngine;
using System.Collections;

public class BossWord : Word
{
	private static BossWord staticInstance = null;
	public static BossWord instance {get {return staticInstance;} set{}}

	private Boss boss = null;
	
	private void Awake ()
	{
		staticInstance = this;
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
		wordActive = false;
		boss.PasswordEntered ();
        base.EndBehavior();
	}
	
	protected override void Behavior ()
	{

	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}

	public void BossActive(Boss _boss, string _password)
	{
		boss = _boss;
		word = _password;
		wordActive = true;
	}

}
