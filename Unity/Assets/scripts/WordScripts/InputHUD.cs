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

	private void Start()
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
		inputText.color = failColour;
	}

	public void Reset()
	{
		inputText.text = string.Empty;
		inputText.color = defaultColour;
	}
}


