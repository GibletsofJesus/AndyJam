using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    public enum GameState { Paused,Gameplay};
    public GameObject pauseUI, GameOverUI,buttons;

    public Text youScored,gameOverText;
    public AudioClip gameOverSounds;

    public static GameStateManager instance;
    public GameState state;
    [SerializeField]
    private GameObject enterName;
	void Awake()
    {
        instance = this;
        state = GameState.Gameplay;
    }

    public void GameOver()
    {
        if (LeaderBoard.instance.CheckIfHighScore(Player.instance.score))
        {
            enterName.SetActive(true);
        }

        else
        StartCoroutine(gameOveranims());
    }

    public void WinState()
    {
        foreach(Enemy _e in GameObject.FindObjectsOfType<Enemy>())
        {
            if(_e.isActiveAndEnabled)
            {
                _e.Death(true);
            }
        }
        gameOverText.text = "You won!";
        StartCoroutine(gameOveranims());
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Loadlevel(int l)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(l);
    }

    public void RunGameOver()
    {
        StartCoroutine(gameOveranims());
    }

    IEnumerator gameOveranims()
    {
        yield return new WaitForSeconds(2);

            GameOverUI.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            soundManager.instance.playSound(gameOverSounds);
            youScored.text = "You scored <color=red> " + Player.instance.score + "</color>";
            yield return new WaitForSeconds(2);
         buttons.SetActive(true);
    }

    void Update()
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
