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
        BOSS_DAMAGE,
        BOSS_PASSWORD,
        COUNT
    }

    private static Tutorial tutorial = null;
    public static Tutorial instance { get { return tutorial; } }

    [SerializeField] private Skip skipWord = null;

    private Tutorials currentTutorial = Tutorials.MOVEMENT_AND_SHOOT;

    [SerializeField] private Word wordRef = null;
    [SerializeField] private GameObject firewallRef = null;
    [SerializeField] private Enemy_KeyLogger keyloggerRef = null;
    [SerializeField] private Transform spawnRef = null;
    private Enemy tutorialEnemy = null;

    private float currentTime = 0.0f;

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
        LevelText.instance.SetText("joystick to move\nsquare to shoot", 20);
        LevelText.instance.ShowText();
    }

    private void Update()
    {
        switch(currentTutorial)
        {
            case Tutorials.MOVEMENT_AND_SHOOT:
                if(Mathf.Abs(Player.instance.transform.position.x) > 3.0f)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("type update.exe\nto unlock more abilities", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                }
                break;
            case Tutorials.UPDATE:
                if(wordRef.isWordActive)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("type firewall.exe\nto create a shield", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                }
                break;
            case Tutorials.FIREWALL:
                if(firewallRef.activeSelf)
                {
                    currentTutorial += 1;
                    tutorialEnemy = EnemyManager.instance.EnemyPooling(keyloggerRef);
                    tutorialEnemy.transform.position = spawnRef.position;
                    tutorialEnemy.OnSpawn();
                    tutorialEnemy.gameObject.SetActive(true);
                    LevelText.instance.SetText("keylogger\ndefeat enemies by shooting them", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                }
                break;
            case Tutorials.KEYLOGGER:
                if(!tutorialEnemy.isActiveAndEnabled)
                {
                    currentTutorial += 1;
                    AdManager.instance.TryGenerateAd(new Vector3(25, 25, 0));
                    LevelText.instance.SetText("type close\nto remove ads", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                }
                break;
            case Tutorials.ADS:
                if(AdManager.instance.numActiveAds == 0)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("deal damage to reveal the boss password", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                    currentTime = 0.0f;
                }
                break;
            case Tutorials.BOSS_DAMAGE:
                currentTime += Time.deltaTime;
                if(currentTime > 7.5f || !tutorialEnemy.isActiveAndEnabled)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("type the password\nto defeat the boss", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                    currentTime = 0.0F;
                }
                break;
            case Tutorials.BOSS_PASSWORD:
                if (!tutorialEnemy.isActiveAndEnabled)
                {
                    BeginGame();
                }
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
