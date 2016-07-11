using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelName : MonoBehaviour
{
    [SerializeField] private Text displayText = null;
    Vector2 startPos;
    Vector3 OffScreen;
    bool dropName = true;
	// Use this for initialization
	void Awake () 
    {
        startPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.75f, 0.5f));
        displayText.rectTransform.position = startPos;
        OffScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.75f,0.5f));
        OffScreen.z = displayText.rectTransform.position.z;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dropName)
        {
            if (displayText.rectTransform.position.y >= OffScreen.y)
            {
                displayText.rectTransform.position = new Vector3(displayText.rectTransform.position.x, displayText.rectTransform.position.y - (10f * Time.deltaTime), displayText.rectTransform.position.z);
            }
            else
            {
                dropName = false;
            }
        }
        else
        {
            displayText.rectTransform.position = startPos;
        }
        


	}

    public void SetText(int levelNum)
    {
        displayText.text = "LEVEL " + levelNum.ToString();
    }

    public void ShowLevelName()
    {
        dropName = true;
    }
}
