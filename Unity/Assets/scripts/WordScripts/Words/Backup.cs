using UnityEngine;
using System.Collections;

public class Backup : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"backup.exe", "backup.exe", "backup.exe"};
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