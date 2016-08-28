using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MenuSelect 
{
    Slider slide;
    Slider slide2;
	// Use this for initialization
	void Start ()
    {
        slide = box[0].GetComponentInChildren<Slider>();
        slide2 = box[1].GetComponentInChildren<Slider>();
	}
	
	// Update is called once per frame
	protected override void Update()
    {
        base.Update();
        SetFunctionToButton();
    }

    protected override void SetFunctionToButton()
    {
        if (selectBox == 0)
        {
            ChangeVolumeSlide(slide);
            soundManager.instance.volumeMultiplayer = slide.value;
        }
        else if (selectBox == 1)
        {
            ChangeVolumeSlide(slide2);
            soundManager.instance.music.volume = slide2.value;
        }
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.End))
        {
            if (selectBox == 2)
            {
                toggleFrameHolding();
            }
            else if (selectBox == 3)
            {
                toggleRetroMode();
            }
            else if (selectBox == 4)
                DoAction(QuitToMain);
            else if (selectBox == 5)
                DoAction(QuitGame);
        }
    }

    public void toggleFrameHolding()
    {
        frameHolder.instance.enableFrameholding = !frameHolder.instance.enableFrameholding;
        if (frameHolder.instance.enableFrameholding)
            box[2].text = "Frame hold: ON";
        else
            box[2].text = "Frame hold: OFF";
    }

    public void toggleRetroMode()
    {
        CameraScreenGrab.instance.SwitchMode(!CameraScreenGrab.instance.retroMode);
        if (CameraScreenGrab.instance.retroMode)
            box[3].text = "Nostalgia mode: ON";
        else
            box[3].text = "Nostalgia mode: OFF";
    }

    void QuitToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
    void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    void ChangeVolumeSlide(Slider _sl)
    {
        if (_sl.value > 0 || _sl.value < 1)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                if (Input.GetJoystickNames()[0] == "")
                    _sl.value += Input.GetAxis("Horizontal") * 10;
                else
                    _sl.value += Input.GetAxis("Horizontal") / 100;
            }
            else
            {
                _sl.value += Input.GetAxis("Horizontal") * 10;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _sl.value -= Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _sl.value += Time.fixedDeltaTime;
            }
        }
    }
}
