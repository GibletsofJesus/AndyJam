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

	private List<Word> words = new List<Word>();

	void Start()
	{
		staticInstance = this;
	}

	void Update()
	{
		foreach (char _c in Input.inputString) 
		{
			//If backspace then clear the word
			if(_c == '\b')
			{
				currentWord = string.Empty;
				WordHUD.instance.UpdateText(currentWord);
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
					WordHUD.instance.UpdateText(currentWord);
					//Test to see if any words match
					foreach (Word _w in words) 
					{
						if(_w.Match(currentWord))
						{
							currentWord = string.Empty;
							WordHUD.instance.UpdateText(currentWord);
							break;
						}
					}
				}
			}
		}
	}
}
