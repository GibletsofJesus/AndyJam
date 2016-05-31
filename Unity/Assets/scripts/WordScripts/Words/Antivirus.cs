using UnityEngine;
using System.Collections;

public class Antivirus : AbilityWord
{
	//Rockets to spawn tiers
	[SerializeField] private float[] rocketDamage = null;
	[SerializeField] private float[] rocketRadius = null;
	[SerializeField] private float[] antivirusDuration = null;
	private float antivirusCooldown = 0.0f;

	protected override void Start ()
	{
		wordTiers = new string[] {"antivirus.exe", "antivirus.exe", "antivirus.exe"};
		base.Start ();
	}

	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
		antivirusCooldown = 0.0f;
		Player.instance.ExplodingProjectiles(true, rocketDamage[currentTier], rocketRadius [currentTier]);
	}
	
	protected override void Behavior ()
	{
		antivirusCooldown += Time.deltaTime;
		if(antivirusCooldown >= antivirusDuration[currentTier])
		{
			EndBehavior();
		}
	}

	protected override void EndBehavior()
	{
		base.EndBehavior ();
		Player.instance.ExplodingProjectiles (false, rocketDamage[currentTier], rocketRadius [currentTier]);
	}
}
