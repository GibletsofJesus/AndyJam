using UnityEngine;
using System.Collections;

public class Scan : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"quickscan.exe", "systemscan.exe", "fullscan.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
	}
	
	protected override void Behavior ()
	{
		switch (currentTier) 
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		}
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}
}
