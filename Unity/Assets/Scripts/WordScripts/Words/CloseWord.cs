using UnityEngine;
using System.Collections;

public class CloseWord : Word
{
	protected override void Start ()
	{
		base.Start ();
		wordActive = true;
		word = "close";
	}
	
	protected override void Update()
	{
		base.Update ();
	}
	
	protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
		//base.TriggerBehavior ();
		AdManager.instance.closeAd ();
	}
	
	protected override void Behavior ()
	{
		
	}
	
	protected override void EndBehavior()
	{
		//base.EndBehavior ();
	}
}
