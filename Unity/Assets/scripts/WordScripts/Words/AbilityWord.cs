using UnityEngine;
using System.Collections;

public abstract class AbilityWord : Word
{
	protected string[] wordTiers;
	protected int currentTier = 0;

	[Range(0.0f, 300.0f)]public float wordCooldown = 10.0f;
	private float currentCooldown;

	[SerializeField] protected WordHUD wordHUD = null;

	private const int pixels = 32;
	private float pixelCooldown;

	protected override void Start ()
	{
		pixelCooldown =(1.0f / (float)pixels) * 1000.0f;
		word = wordTiers [0];
		currentTier = 0;
		currentCooldown = wordCooldown;
		wordHUD.Deactivate ();
		base.Start ();
	}

	private void Update()
	{
		currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, wordCooldown);
		if(wordCooldown == 0.0f)
		{
			wordHUD.UpdateCooldown (1.0f);
		}
		else
		{
			float _cooldownPercent = currentCooldown / wordCooldown;
			wordHUD.UpdateCooldown (((_cooldownPercent * 1000.0f) - ((_cooldownPercent * 1000.0f) % pixelCooldown)) / 1000.0f);
		}

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
