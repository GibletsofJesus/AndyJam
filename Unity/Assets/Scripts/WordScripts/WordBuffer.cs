using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordBuffer : MonoBehaviour 
{
	private static WordBuffer staticInstance = null;
	public static WordBuffer instance {get {return instance;} set{}}

	private string currentWord;
	private const int MaxCharacters = 20;

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
					currentWord += _c;
					//Test to see if any words match
					foreach (Word _w in words) 
					{
						if(_w.Match(currentWord))
						{
							currentWord = string.Empty;
							break;
						}
					}
				}
			}
		}
	}
}
