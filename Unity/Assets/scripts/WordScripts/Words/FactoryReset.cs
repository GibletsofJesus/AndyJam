using UnityEngine;
using System.Collections;

public class FactoryReset : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"factory-reset.exe"};
		behavior = Behavior;
		base.Start ();
	}
	
	protected override void Behavior ()
	{

	}
}