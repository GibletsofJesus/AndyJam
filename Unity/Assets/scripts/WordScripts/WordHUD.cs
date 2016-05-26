using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordHUD : MonoBehaviour 
{
	private static WordHUD staticInstance = null;
	public static WordHUD instance {get {return staticInstance;} set{}}

	[SerializeField] private Text inputText = null;

	private void Start()
	{
		staticInstance = this;
	}

	public void UpdateText(string _text)
	{
		inputText.text = _text;
	}
}
