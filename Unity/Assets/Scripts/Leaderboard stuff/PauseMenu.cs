using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MenuSelect 
{
    [SerializeField]
    Slider[] sliders;

	protected override void Update()
    {
        base.Update();
        SetFunctionToButton();
    }

    protected override void SetFunctionToButton()
    {
        if (selectBox == 0)
        {
            ChangeVolumeSlide(sliders[0]);
            soundManager.instance.volumeMultiplayer = sliders[0].value;
        }
        else if (selectBox == 1)
        {
            ChangeVolumeSlide(sliders[1]);
            soundManager.instance.music.volume = sliders[1].value;
        }
        else if (selectBox == 2)
        {
            ChangeVolumeSlide(sliders[2]);
            //Change screen shake amount
            box[2].text = "Screen shake " + (int)(sliders[2].value*100)+"%";
            CameraShake.instance.shakeMultiplier = sliders[2].value;
        }
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.End))
        {
            if (selectBox == 3)
            {
                toggleFrameHolding();
            }
            else if (selectBox == 4)
            {
                toggleRetroMode();
            }
            else if (selectBox == 5)
                DoAction(QuitToMain);
            else if (selectBox == 6)
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
        if (_sl.value > 0 || _sl.value < _sl.maxValue)
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
