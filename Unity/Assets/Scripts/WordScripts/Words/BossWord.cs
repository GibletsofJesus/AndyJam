using UnityEngine;
using UnityEngine.UI;
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
	
	protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
        if (boss)
        {
               wordActive = false;
            boss.PasswordEntered();
            boss = null;
        }
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

    public override void Reset()
    {
        //This is needed to override base behavior to prevent if from happening
    }

    public void ForceBossDeath()
    {
        TriggerBehavior();
    }

    public float GetBossHealth()
    {
        if (boss)
        {
            return boss.ActorHealthPercent();
        }
        return 1.0f;
    }

}
