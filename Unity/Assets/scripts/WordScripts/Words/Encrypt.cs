using UnityEngine;
using System.Collections;

public class Encrypt : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"encrypt.exe", "encrypt.exe", "encrypt.exe"};
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