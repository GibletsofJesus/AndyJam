using UnityEngine;
using System.Collections;

public class Scan : AbilityWord
{
	[SerializeField] private float[] homingRadius = null;
	[SerializeField] private float[] scanDuration = null;
	private float scanCooldown = 0.0f;

	protected override void Start ()
	{
        description = "homing projectiles";
        wordTiers = new string[] {"quickscan.exe", "systemscan.exe", "fullscan.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior()
    {
		base.TriggerBehavior ();
		scanCooldown = 0.0f;
		Player.instance.HomingProjectiles (true, homingRadius [currentTier]);
	}
	
	protected override void Behavior ()
	{
		scanCooldown += Time.deltaTime;
		if(scanCooldown >= scanDuration[currentTier])
		{
			EndBehavior();
		}
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
		Player.instance.HomingProjectiles (false, homingRadius [currentTier]);
	}
}
