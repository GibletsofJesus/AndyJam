using UnityEngine;
using System.Collections;

public class FactoryReset : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"factory-reset.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        foreach(Enemy _e in FindObjectsOfType<Enemy>())
        {
            if(_e.isActiveAndEnabled)
            {
                if(_e is Boss)
                {
                    _e.TakeDamage(3000);
                }
                else
                {
                    _e.Death(false);
                }
            }
        }
        Player.instance.Reset();
        UpdateBehavior.instance.FactoryReset();
        AdManager.instance.EnableAdBlock();
        AdManager.instance.DisableAdBlock();
        base.EndBehavior();
	}
	
	protected override void Behavior ()
	{

	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}
}