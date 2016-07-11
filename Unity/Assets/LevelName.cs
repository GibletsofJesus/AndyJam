using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelName : MonoBehaviour
{
    [SerializeField] private Canvas theCanvas;
    [SerializeField] private Text displayText;
    Vector2 startPos;
    Vector3 OffScreen;
    bool dropName = true;
	// Use this for initialization
	void Awake () 
    {
        startPos = transform.position;
        OffScreen = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f,0));
        OffScreen.z = transform.position.z;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dropName)
        {
            if (transform.position.y >= OffScreen.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (50f * Time.deltaTime), transform.position.z);
                displayText.transform.position = transform.position;
                theCanvas.transform.position = transform.position;
            }
            else
            {
                dropName = false;
            }
        }
        else
        {
            transform.position = startPos;
            displayText.transform.position = startPos;
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
