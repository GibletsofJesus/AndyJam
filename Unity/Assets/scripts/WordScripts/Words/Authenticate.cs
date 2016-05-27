using UnityEngine;
using System.Collections;

public class Authenticate : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"authenticate.exe", "authenticate.exe", "authenticate.exe"};
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