using UnityEngine;
using System.Collections;

public abstract class Word : MonoBehaviour 
{
	protected string thisWord = string.Empty;
	public string word {get {return thisWord;} set{ thisWord = value;}}

	protected bool wordActive = false;
	
	protected delegate void BehaviorDelegate();
	protected BehaviorDelegate behavior = null;

	protected virtual void Start()
	{
		wordActive = false;
	}

	public bool Match(string _word)
	{
		if (!wordActive) 
		{
			return false;
		}
		if(thisWord == _word)
		{
			ActivateBehavior();
			return true;
		}
		return false;
	}

	protected virtual void ActivateBehavior ()
	{
		behavior();
	}

	protected abstract void Behavior();
}
