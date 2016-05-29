using UnityEngine;
using System.Collections;

public class FactoryReset : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"factory-reset.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
	}
	
	protected override void Behavior ()
	{

	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}
}