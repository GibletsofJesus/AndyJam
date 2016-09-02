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
    public bool isBufferFree { get { return messageBuffer.Count == 0 ? true : false; } }

	[SerializeField] private float typeRate = 0.05f;
	private float typeCooldown = 0.0f;
	private int charReader = 0;

	private bool underscoreVisible = false;
	[SerializeField] private float underscoreFlickerRate = 0.05f;
	private float underscoreFlickerCooldown = 0.0f;
	[SerializeField] private float underscoreAppearRate = 0.1f;
	private float underscoreAppearCooldown = 0.0f;

	private TextGenerationSettings settings; 
	[SerializeField] private RectTransform rectTransform = null;

	private string currentMessage = string.Empty;

	private void Awake()
	{
		staticInstance = this;
		settings = textField.GetGenerationSettings(new Vector2(rectTransform.rect.width, rectTransform.rect.height));
		settings.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void Start()
	{
        AddMessage("TypeFighter sequence engaged", string.Empty);
        AddMessage("Recommended action");
        AddMessage("Type update.exe");
    }

	private void Update()
	{        
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            //Determine if should be typing out current message
            if (currentMessage != string.Empty)
            {
                typeCooldown += Time.deltaTime;
                //Progressively type message
                if (typeCooldown >= typeRate)
                {
                    typeCooldown = 0.0f;
                    //Type message till message complete
                    if (currentMessage.Length > charReader)
                    {
                        textField.text += currentMessage[charReader];
                        ++charReader;

                        //To get the text to scroll need to force create new updated text generator
                        TextGenerator generator = new TextGenerator();
                        generator.Populate(textField.text, settings);
                        if (generator.lineCount > 20)//should find the realtime amount of possible lines
                        {
                            textField.text = textField.text.Substring(textField.cachedTextGenerator.GetLinesArray()[1].startCharIdx);
                        }
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
            else if (messageBuffer.Count > 0)
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
                if (underscoreAppearCooldown >= underscoreAppearRate)
                {
                    underscoreFlickerCooldown += Time.deltaTime;
                    if (underscoreFlickerCooldown >= underscoreFlickerRate)
                    {
                        underscoreFlickerCooldown = 0.0f;
                        UpdateUnderscore(!underscoreVisible);
                    }
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

    public void AddLine(char _symbol, string _preMessage = "\n")
    {
        string _message = string.Empty;
        for(int i = 0; i < 44; ++i)
        {
            _message += _symbol;
        }
        messageBuffer.Add(_preMessage + _message);
    }

    public void ClearPanel()
    {
        messageBuffer.Clear();
        charReader = 0;
        currentMessage = string.Empty;
        textField.text = " ";
    }

    public void TryMessage(string _message, string _preMessage = "\n", int _buffer = 0)
    {
        if (messageBuffer.Count <= _buffer)
        {
            messageBuffer.Add(_preMessage + _message);
        }
    }
}
