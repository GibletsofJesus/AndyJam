using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class VisualCommandPanel : MonoBehaviour 
{
	private static VisualCommandPanel staticInstance = null;
	public static VisualCommandPanel instance {get {return staticInstance;} set{}}

	[SerializeField] private Text textField = null;

	private List<string> messageBuffer = new List<string>();

	[SerializeField] private float typeRate = 0.05f;
	private float typeCooldown = 0.0f;
	private int charReader = 0;

	private bool underscoreVisible = false;
	[SerializeField] private float underscoreFlickerRate = 0.05f;
	private float underscoreFlickerCooldown = 0.0f;
	[SerializeField] private float underscoreAppearRate = 0.1f;
	private float underscoreAppearCooldown = 0.0f;

	private string currentMessage = string.Empty;

	private void Awake()
	{
		staticInstance = this;
	}

	private void Start()
	{
		AddMessage ("Jam sequence Engaged", string.Empty);
	}

	private void Update()
	{
		//Determine if should be typing out current message
		if(currentMessage != string.Empty)
		{
			typeCooldown += Time.deltaTime;
			//Progressively type message
			if(typeCooldown >= typeRate)
			{
				typeCooldown = 0.0f;
				//Type message till message complete
				if(currentMessage.Length > charReader)
				{
					textField.text += currentMessage[charReader];
					++charReader;
				}
				//When complete reset
				else
				{
					currentMessage = string.Empty;
					charReader = 0;
				}
			}
		}
		//If no message to type out then determine if there is a new message
		else if(messageBuffer.Count > 0)
		{
			underscoreAppearCooldown = 0.0f;
			UpdateUnderscore(false);
			currentMessage = messageBuffer[0];
			messageBuffer.RemoveAt(0);
		}
		//No message so start flashing underscore
		else
		{
			underscoreAppearCooldown += Time.deltaTime;
			if(underscoreAppearCooldown >= underscoreAppearRate)
			{
				underscoreFlickerCooldown += Time.deltaTime;
				if(underscoreFlickerCooldown >= underscoreFlickerRate)
				{
					underscoreFlickerCooldown = 0.0f;
					UpdateUnderscore(!underscoreVisible);
				}
			}
		}
	}

	private void UpdateUnderscore(bool _underscore)
	{
		if(_underscore != underscoreVisible)
		{
			underscoreVisible = _underscore;
			if(underscoreVisible)
			{
				textField.text += "_";
			}
			else
			{
				textField.text = textField.text.Remove(textField.text.Length - 1);
			}
		}
	}

	public void AddMessage(string _message, string _preMessage = "\n")
	{
		messageBuffer.Add (_preMessage + _message);
	}
}
