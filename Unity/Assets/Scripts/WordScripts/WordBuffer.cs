using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class WordBuffer : MonoBehaviour 
{
	private static WordBuffer staticInstance = null;
	public static WordBuffer instance {get {return staticInstance;} set{}}

	private string currentWord = string.Empty;
	private const int MaxCharacters = 20;

	[SerializeField] private List<AbilityWord> abilityWords = new List<AbilityWord>();

	private float submitCooldown = 1.0f;
	private float currentSubmitCooldown = 0.0f;

	private bool underscoreVisible = false;
	[SerializeField] private float underscoreFlickerRate = 0.05f;
	private float underscoreFlickerCooldown = 0.0f;
	[SerializeField] private float underscoreAppearRate = 0.1f;
	private float underscoreAppearCooldown = 0.0f;

	private void Awake()
	{
		staticInstance = this;
	}

	private void Update()
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
					underscoreAppearCooldown = 0.0f;
					underscoreVisible = InputHUD.instance.UpdateUnderscore(false);
					currentWord = string.Empty;
					InputHUD.instance.UpdateText(currentWord);
				}
				else if(_c == '\r')
				{
					underscoreAppearCooldown = 0.0f;
					underscoreVisible = InputHUD.instance.UpdateUnderscore(false);
					bool _match = false;
					//Test to see if any words match
					foreach (AbilityWord _w in abilityWords) 
					{
						if(_w.Match(currentWord))
						{
							_match = true;
							InputHUD.instance.Success();
							currentSubmitCooldown = submitCooldown;
							break;
						}
					}
					if(!_match)
					{
						InputHUD.instance.Fail();
						VisualCommandPanel.instance.AddMessage("Unrecognised Command");
						currentSubmitCooldown = submitCooldown;
					}
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
						underscoreAppearCooldown = 0.0f;
						underscoreVisible = InputHUD.instance.UpdateUnderscore(false);
						currentWord += char.ToLower(_c);
						InputHUD.instance.UpdateText(currentWord);
						//If max word reached
						if(currentWord.Length == MaxCharacters)
						{
							InputHUD.instance.Fail();
							currentSubmitCooldown = submitCooldown;
						}
					}
				}
			}
			underscoreAppearCooldown += Time.deltaTime;
			if(underscoreAppearCooldown >= underscoreAppearRate)
			{
				underscoreFlickerCooldown += Time.deltaTime;
				if(underscoreFlickerCooldown >= underscoreFlickerRate)
				{
					underscoreFlickerCooldown = 0.0f;
					underscoreVisible = InputHUD.instance.UpdateUnderscore(!underscoreVisible);
				}
			}
		}
		//Cannot input text
		else
		{
			if(currentSubmitCooldown < 0.05f)
			{
				currentWord = string.Empty;
				InputHUD.instance.Reset();
			}
		}
	}

	public void AbilityWordUpdate(int _index, int _tier)
	{
		abilityWords [_index].SetTier (_tier);
	}




}
