using UnityEngine;
using System.Collections;

public class Antivirus : AbilityWord
{
	//Rockets to spawn tiers
	[SerializeField] private int[] numRockets;
	[SerializeField] private float[] rocketAOE;

	protected override void Start ()
	{
		wordTiers = new string[] {"antivirus.exe", "antivirus.exe", "antivirus.exe"};
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
