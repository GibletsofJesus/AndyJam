using UnityEngine;
using System.Collections;

public class Scan : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"quickscan.exe", "systemscan.exe", "fullscan.exe"};
		behavior = Behavior;
		base.Start ();
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
}
