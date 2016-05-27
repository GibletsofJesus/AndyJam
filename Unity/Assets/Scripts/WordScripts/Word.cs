using UnityEngine;
using System.Collections;

public abstract class Word : MonoBehaviour 
{
	protected string thisWord = string.Empty;
	public string word {get {return thisWord;} set{ thisWord = value;}}

	[Range(0.0f, 300.0f)]public float wordCooldown = 10.0f;
	private float currentCooldown;

	protected delegate void BehaviorDelegate();
	protected BehaviorDelegate behavior = null;

	[SerializeField] private WordHUD wordHUD = null;

	protected virtual void Start()
	{
		currentCooldown = wordCooldown;
	}

	private void Update()
	{
		currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, wordCooldown);
		wordHUD.UpdateCooldown (currentCooldown / wordCooldown);
	}

	public bool Match(string _word)
	{
		if(thisWord == _word)
		{
			ActivateBehavior();
			return true;
		}
		return false;
	}

	private void ActivateBehavior()
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

	protected abstract void Behavior();
}
