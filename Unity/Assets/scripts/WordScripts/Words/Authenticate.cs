using UnityEngine;
using System.Collections;

public class Authenticate : AbilityWord
{
    [SerializeField] private LaserModule laser = null;

	protected override void Start ()
	{
		wordTiers = new string[] {"authenticate.exe", "authenticate.exe", "authenticate.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        laser.FireLaser();
	}
	
	protected override void Behavior ()
	{
		if(!laser.IsLaserFiring())
        {
            EndBehavior();
        }
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}
}