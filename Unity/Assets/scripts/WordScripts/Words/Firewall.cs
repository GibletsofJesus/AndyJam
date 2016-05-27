using UnityEngine;
using System.Collections;

public class Firewall : AbilityWord
{
	[SerializeField] private float wallHealth;

	protected override void Start ()
	{
		wordTiers = new string[] {"firewall.exe", "firewall.exe", "firewall.exe"};
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
