using UnityEngine;
using System.Collections;

public abstract class AbilityWord : Word
{
	protected string[] wordTiers;
	protected int currentTier = 0;

	[Range(0.0f, 300.0f)]public float wordCooldown = 10.0f;
	private float currentCooldown;

	[SerializeField] protected WordHUD wordHUD = null;

	protected override void Start ()
	{
		word = wordTiers [0];
		currentTier = 0;
		currentCooldown = wordCooldown;
		wordHUD.Deactivate ();
		base.Start ();
	}

	private void Update()
	{
		currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, wordCooldown);
		wordHUD.UpdateCooldown (wordCooldown == 0.0f ? 1.0f : (currentCooldown / wordCooldown));
	}

	protected override void ActivateBehavior ()
	{
		//Activate behavior
		if(wordCooldown == currentCooldown)
		{
			currentCooldown = 0.0f;
			wordHUD.TriggerSuccess();
			behavior();
		}
		//Behavior on cooldown, do something to show this (flicker red?)
		else
		{
			
		}
	}

	protected override void Behavior ()
	{
	}

	public void SetTier(int _tier)
	{
		currentTier = _tier;
		if(currentTier == 0)
		{
			wordActive = true;
			wordHUD.Activate();
		}
		thisWord = wordTiers [currentTier];
		wordHUD.UpdateWord (thisWord);
	}


}
