using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    public enum GameState { Paused,Gameplay,Misc};
    public GameObject pauseUI;
    public static GameStateManager instance;
    public GameState state;

	void Awake()
    {
        instance = this;
        state = GameState.Gameplay;
	}
	
	void Update ()
    {
        if (!instance)
        {
            instance = this;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (state != GameState.Paused)
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                state = GameState.Paused;
            }
            else
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                //Resume game
                state = GameState.Gameplay;
            }
        }
	}
}
