using UnityEngine;
using System.Collections;

public class Filter : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"spamfilter.exe", "spamfilter.exe", "spamfilter.exe"};
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