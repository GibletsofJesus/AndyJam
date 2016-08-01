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
        TUTORIAL_COMPLETE,
        COUNT
    }

    private static Tutorial tutorial = null;
    public static Tutorial instance { get { return tutorial; } }

    [SerializeField] private Skip skipWord = null;

    private Tutorials currentTutorial = Tutorials.MOVEMENT_AND_SHOOT;

    [SerializeField] private Word wordRef = null;
    [SerializeField] private GameObject firewallRef = null;
    [SerializeField] private Enemy_KeyLogger keyloggerRef = null;
    [SerializeField] private TutorialBoss bossRef = null;
    [SerializeField] private Transform spawnRef = null;
    private Enemy tutorialEnemy = null;
    private TutorialBoss tutorialBoss = null;
    [SerializeField] private AudioClip bossMusic = null;

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
                    AdManager.instance.TryGenerateAd(new Vector3(25, 25, 0), true);
                    LevelText.instance.SetText("type close\nto remove ads", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                }
                break;
            case Tutorials.ADS:
                if(AdManager.instance.numActiveAds == 0)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("deal damage\n to reveal the boss password", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                    currentTime = 0.0f;

                    soundManager.instance.music.clip = bossMusic;
                    soundManager.instance.music.enabled = false;
                    soundManager.instance.music.enabled = true;
                    tutorialBoss = (TutorialBoss)EnemyManager.instance.EnemyPooling(bossRef);
                    Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));
                    spawnPos.z = 0;
                    tutorialBoss.transform.position = spawnPos;
                    tutorialBoss.OnSpawn();
                    tutorialBoss.gameObject.SetActive(true);
                }
                break;
            case Tutorials.BOSS_DAMAGE:
                currentTime += Time.deltaTime;
                if(currentTime > 7.5f || !tutorialBoss.isActiveAndEnabled)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("type the password\nto defeat the boss", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                    currentTime = 0.0f;
                }
                break;
            case Tutorials.BOSS_PASSWORD:
                if (!tutorialBoss.isActiveAndEnabled)
                {
                    currentTutorial += 1;
                    LevelText.instance.SetText("Tutorial Complete", 20);
                    LevelText.instance.TutorialElementFinished(false);
                    LevelText.instance.ShowText();
                    currentTime = 0.0f;
                }
                break;
            case Tutorials.TUTORIAL_COMPLETE:
                currentTime += Time.deltaTime;
                if (currentTime > 7.5f)
                {
                    VisualCommandPanel.instance.AddMessage("Type skip to skip the tutorial in future playthroughs");
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
        if(tutorialEnemy.isActiveAndEnabled)
        {
            tutorialEnemy.Death(false);
        }
        else if(tutorialBoss.isActiveAndEnabled)
        {
            BossWord.instance.ForceBossDeath();
        }
    }

    private void BeginGame()
    {
        skipWord.TutorialFinished();
        EnemyManager.instance.Begin();
        enabled = false;
    }


}
