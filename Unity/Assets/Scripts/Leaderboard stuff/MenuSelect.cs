using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuSelect : MonoBehaviour 
{
    [SerializeField] public Text[] box;
    public int selectBox = 0;
    float coolDown = 0.2f;
    float maxCool = 0.2f;

	// Use this for initialization
	void Awake () 
    {
        coolDown = maxCool;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        MenuInput();
        SelectCoolDown();
        ChangeTextColour();
    }

    void MenuInput()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") != 0) && SelectCoolDown()))
        {
            if (Input.GetAxis("Vertical") < 0 || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectBox++;
                if (selectBox == box.Length)
                {
                    selectBox = 0;
                }
            }
            else if (Input.GetAxis("Vertical") > 0 || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectBox--;
                if (selectBox < 0)
                {
                    selectBox = box.Length - 1;
                }
            }
            coolDown = 0;
            soundManager.instance.playSound(0);
        }
    }

    public void SetSelection(int _select)
    {
        selectBox = _select;
    }

   protected virtual void SetFunctionToButton()
    {

    }

    void ChangeTextColour()
    {
        for (int i = 0; i < box.Length; i++)
        {
            if (selectBox == i)
            {
                box[i].color = new Color(0,.5f,1);
            }
            else
            {
                box[i].color = Color.white;
            }
        }
    }

   public bool SelectCoolDown()
    {
        if (coolDown < maxCool)
        {
            coolDown += 0.01f;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DoAction(Action _Function)
    {
        _Function();    
    }
}
