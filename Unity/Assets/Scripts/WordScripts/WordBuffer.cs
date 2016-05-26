using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class WordBuffer : MonoBehaviour 
{
	private static WordBuffer staticInstance = null;
	public static WordBuffer instance {get {return staticInstance;} set{}}

	private string currentWord = string.Empty;
	private const int MaxCharacters = 15;

	[SerializeField] private List<Word> words = new List<Word>();

	private float submitCooldown = 1.0f;
	private float currentSubmitCooldown = 0.0f;

	void Start()
	{
		staticInstance = this;
	}

	void Update()
	{
		currentSubmitCooldown = Mathf.Max (0.0f, currentSubmitCooldown - Time.deltaTime);
		//Can input text
		if(currentSubmitCooldown == 0.0f)
		{
			foreach (char _c in Input.inputString) 
			{
				//If backspace then clear the word
				if(_c == '\b')
				{
					currentWord = string.Empty;
					InputHUD.instance.UpdateText(currentWord);
				}
				//Else other empty space value then do nothing
				else if((int)_c < 33 || (int)_c > 126)
				{
				}
				//Add to word string
				else
				{
					if(currentWord.Length <= MaxCharacters)
					{
						currentWord += char.ToUpper(_c);
						InputHUD.instance.UpdateText(currentWord);
						//Test to see if any words match
						foreach (Word _w in words) 
						{
							if(_w.Match(currentWord))
							{
								InputHUD.instance.Success();
								currentSubmitCooldown = submitCooldown;
								break;
							}
						}
						//If max word reached
						if(currentWord.Length == MaxCharacters)
						{
							InputHUD.instance.Fail();
							currentSubmitCooldown = submitCooldown;
						}
					}
				}
			}
		}
		//Cannot input text
		else
		{
			if(currentSubmitCooldown < 0.2f)
			{
				currentWord = string.Empty;
				InputHUD.instance.Reset();
			}
		}
	}


}
