using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    public enum GameState { Paused,Gameplay,GameOver};
    public GameObject pauseUI, GameOverUI,buttons;

    public Text youScored,gameOverText;
    public AudioClip gameOverSounds;

    public static GameStateManager instance;
    public GameState state;
    [SerializeField]
    private GameObject enterName = null;

    [SerializeField] private GameObject arrowFormation = null;

	void Awake()
    {
        instance = this;
        state = GameState.Gameplay;
    }

    public void GameOver()
    {
        state = GameState.GameOver;
        if (LeaderBoard.instance.CheckIfHighScore(Player.instance.score))
        {
            enterName.SetActive(true);
        }

        else
        StartCoroutine(gameOveranims());
    }

    public void WinState()
    {
        state = GameState.GameOver;
        foreach (Enemy _e in GameObject.FindObjectsOfType<Enemy>())
        {
            if(_e.isActiveAndEnabled)
            {
                _e.Death(true);
            }
        }

        StartCoroutine(WinAnimation());

        
    }

    IEnumerator WinAnimation()
    {
        while(Player.instance.transform.position != new Vector3(0.0f, -10.0f, 0.0f))
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, new Vector3(0.0f, -10.0f, 0.0f), Time.deltaTime * 5.0f);
            yield return null;
        }
        soundManager.instance.playSound(2);
        while (arrowFormation.transform.position.y < -10)
        {
            arrowFormation.transform.position += (Vector3.up * Time.deltaTime * 7.5f);
            yield return null;
        }

        while (arrowFormation.transform.position.y < 10)
        {
            Player.instance.transform.position += (Vector3.up * Time.deltaTime * 7.5f);
            arrowFormation.transform.position += (Vector3.up * Time.deltaTime * 7.5f);
            yield return null;
        }

        gameOverText.text = "You won!";
        if (LeaderBoard.instance.CheckIfHighScore(Player.instance.score))
        {
            enterName.SetActive(true);
        }
        else
        {
            StartCoroutine(gameOveranims());
        }

        while (arrowFormation.transform.position.y < 75)
        {
            Player.instance.transform.position += (Vector3.up * Time.deltaTime * 7.5f);
            arrowFormation.transform.position += (Vector3.up * Time.deltaTime * 7.5f);
            yield return null;
        }
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
