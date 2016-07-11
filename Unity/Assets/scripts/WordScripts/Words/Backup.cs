using UnityEngine;
using System.Collections;

public class Backup : AbilityWord
{
    [SerializeField] private int[] numBackups = null;

	protected override void Start ()
	{
		wordTiers = new string[] {"backup.exe", "backup.exe", "backup.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        Player.instance.SpawnBackups(numBackups[currentTier]);
	}
	
	protected override void Behavior ()
	{
        if(!Player.instance.TestBackups())
        {
            EndBehavior();
        }
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}

    public override void Reset()
    {
        if (behaviorActive)
        {
            EndBehavior();
        }
        base.Reset();
    }
}