using UnityEngine;
using System.Collections;

public class Backup : AbilityWord
{
    [SerializeField] private int[] numBackups = null;

    private float backupTime = 0.0f;
    [SerializeField] private float[] backupDuration = null;

	protected override void Start ()
	{
        description = "additional players";
        wordTiers = new string[] {"backup.exe", "backup.exe", "backup.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior()
    {
		base.TriggerBehavior ();
        Player.instance.SpawnBackups(numBackups[currentTier]);
	}
	
	protected override void Behavior ()
	{
        backupTime += Time.deltaTime;
        if(backupTime >= backupDuration[currentTier])
        {
            backupTime = 0;
            Player.instance.DestroyBackups();
            EndBehavior();
        }
        else if(!Player.instance.TestBackups())
        {
            EndBehavior();
        }
	}
	
	protected override void EndBehavior()
	{
        backupTime = 0.0f;
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