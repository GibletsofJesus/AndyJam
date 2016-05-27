using UnityEngine;
using System.Collections;

public abstract class AbilityWord : Word
{
	protected string[] wordTiers;
	protected int currentTier = 0;

	protected override void Start ()
	{
		word = wordTiers [0];
		base.Start ();
	}

	protected override void Behavior ()
	{

	}

	public void SetTier(int _tier)
	{
		currentTier = _tier;
		thisWord = wordTiers [currentTier];
	}


}
