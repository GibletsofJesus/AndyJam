using UnityEngine;
using System.Collections;

public class FactoryReset : AbilityWord
{
	protected override void Start ()
	{
        description = "player and enemy reset";
        wordTiers = new string[] {"factory-reset.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior()
    {
		base.TriggerBehavior ();
        foreach(Enemy _e in FindObjectsOfType<Enemy>())
        {
            if(_e.isActiveAndEnabled)
            {
                if(_e is Boss)
                {
                    _e.TakeDamage(5000);
                }
                else if(_e is WormBossSegment)
                {
                    _e.TakeDamage(100);
                }
                else
                {
                    _e.Death(false);
                }
            }
        }
        Player.instance.Reset();
        UpdateBehavior.instance.FactoryReset();
        AdManager.instance.DisableAdBlock();
        AdManager.instance.SpamFilter(false);
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