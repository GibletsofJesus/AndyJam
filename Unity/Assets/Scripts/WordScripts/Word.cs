using UnityEngine;
using System.Collections;

public abstract class Word : MonoBehaviour 
{
	private string thisWord = string.Empty;
	public string word {get {return thisWord;} set{ thisWord = value;}}

	[Range(0.0f, 300.0f)]public float wordCooldown = 10.0f;
	private float currentCooldown;

	private delegate void BehaviorDelegate();
	private BehaviorDelegate behavior = null;

	private void Start()
	{
		currentCooldown = wordCooldown;
	}

	private void Update()
	{
		currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, wordCooldown);
	}

	public bool Match(string _word)
	{
		if(thisWord == word)
		{
			ActivateBehavior();
			return true;
		}
		return false;
	}

	public void ActivateBehavior()
	{
		//Activate behavior
		if(wordCooldown == currentCooldown)
		{
			currentCooldown = 0.0f;
			behavior();
		}
		//Behavior on cooldown, do something to show this (flicker red?)
		else
		{

		}
	}

	public abstract void Behavior();
}
