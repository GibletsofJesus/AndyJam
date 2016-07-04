using UnityEngine;
using System.Collections;

public abstract class Word : MonoBehaviour 
{
	protected string thisWord = string.Empty;
	public string word {get {return thisWord;} set{ thisWord = value;}}

	protected bool wordActive = false;

	protected bool behaviorActive = false;

	protected virtual void Start()
	{
		wordActive = false;
        behaviorActive = false;
    }

	protected virtual void Update()
	{
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (behaviorActive)
            {
                Behavior();
            }
        }
	}

	public bool Match(string _word)
	{
		if (!wordActive) 
		{
			return false;
		}
		if(thisWord == _word)
		{
			TriggerBehavior();
			return true;
		}
		return false;
	}

	protected virtual void TriggerBehavior ()
	{
		behaviorActive = true;
	}

	protected abstract void Behavior();

	protected virtual void EndBehavior()
	{
		behaviorActive = false;
	}

    public virtual void Reset()
    {
        Start();
    }
}
