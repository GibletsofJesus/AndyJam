using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    private enum Tutorials
    {
        MOVEMENT_AND_SHOOT = 0,
        UPDATE,
        FIREWALL,
        KEYLOGGER,
        ADS,
        BOSS,
        COUNT
    }

    private static Tutorial tutorial = null;
    public static Tutorial instance { get { return tutorial; } }

    [SerializeField] private Skip skipWord = null;

    private Tutorials currentTutorial = Tutorials.MOVEMENT_AND_SHOOT;


    //Update.exe

    //Firewall.exe

    //Keylogger

    //Closing Ads

    //Boss

	private void Awake()
    {
        tutorial = this;
    }

    private void Start()
    {
        LevelText.instance.SetText("Joystick to move\nSquare to shoot", 20);
        LevelText.instance.ShowText();
    }

    private void Update()
    {
        switch(currentTutorial)
        {
            case Tutorials.MOVEMENT_AND_SHOOT:
                if(Player.instance.transform.position.x > 1.0f)
                {
                    currentTutorial += 1;
                }
                break;
            case Tutorials.UPDATE:
                BeginGame();
                //Track when antivirus becomes active
                break;
            case Tutorials.FIREWALL:
                //Track firewall active
                break;
            case Tutorials.KEYLOGGER:
                //Track keylogger not active
                break;
            case Tutorials.ADS:
                //Track when no adds
                break;
            case Tutorials.BOSS:
                //Track boss not active
                break;
            default:
                break;
        }
    }

    public void Skip()
    {
        BeginGame();
    }

    private void BeginGame()
    {
        skipWord.TutorialFinished();
        EnemyManager.instance.Begin();
        enabled = false;
    }


}
