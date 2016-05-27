using UnityEngine;
using System.Collections;

public class UpdateWord : AbilityWord
{
	protected override void Start ()
	{
		wordTiers = new string[] {"update.exe"};
		behavior = Behavior;
		base.Start ();
		wordActive = true;
		wordHUD.Activate ();
	}
	
	protected override void Behavior ()
	{
		UpdateBehavior.instance.ApplyUpdate ();
	}
}
