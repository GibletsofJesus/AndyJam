using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputHUD : MonoBehaviour 
{
	private static InputHUD staticInstance = null;
	public static InputHUD instance {get {return staticInstance;} set{}}

	[SerializeField] private Text inputText = null;

	public Color successColour;
	public Color failColour;
	private Color defaultColour;

    [SerializeField]
    AudioClip failSound;

    private bool underscoreVisible = false;

	private void Awake()
	{
		staticInstance = this;
		defaultColour = inputText.color;
	}

	public void UpdateText(string _text)
    {
        inputText.text = _text;
	}

	public void Success()
	{
		inputText.color = successColour;
	}

	public void Fail()
    {
        soundManager.instance.playSound(failSound, 1);// Random.Range(0.9f, 1.1f));
        inputText.color = failColour;
	}

	public void Reset()
	{
		inputText.text = string.Empty;
		inputText.color = defaultColour;
	}

	public bool UpdateUnderscore(bool _underscore)
	{
		if(_underscore != underscoreVisible)
		{
			underscoreVisible = _underscore;
			if(underscoreVisible)
			{
				inputText.text += "_";
			}
			else
			{
				inputText.text = inputText.text.Remove(inputText.text.Length - 1);
			}
		}
		return underscoreVisible;
	}
}


