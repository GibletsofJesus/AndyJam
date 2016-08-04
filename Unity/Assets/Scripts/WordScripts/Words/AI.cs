using UnityEngine;

public class AI : Word
{
    private enum AIWords
    {
        UPDATE = 0,
        FIREWALL,
        ANTIVIRUS,
        REPAIR,
        SCAN,
        FILTER,
        AUTH,
        BACKUP,
        ENCRYPT,
        RESET,
        CLOSE,
        BOSS,
        COUNT
    }

    [SerializeField] private float typeRate = 0.1f;
    private float waitRate = 0.0f;
    [SerializeField] private float waitRateSucceed = 0.75f;
    [SerializeField] private float waitRateNoWord = 2.0f;
    private float typeTime = 0.0f;

    private int currentIndex = 0;
    private string currentWord = string.Empty;

    [SerializeField] private Word[] aiWords = null;
    private AIWords lastWord = AIWords.COUNT;

    private bool frameTriggered = false;

    private void Awake()
    {
        thisWord = "ai-typer";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior()
    {
        base.TriggerBehavior();
        currentWord = string.Empty;
        ChooseWord();
        typeTime = 0.0f;
        VisualCommandPanel.instance.AddMessage("A.I. typer enabled");
        frameTriggered = true;
    }

    protected override void Behavior()
    {
        if(frameTriggered)
        {
            frameTriggered = false;
        }
        else
        {
            foreach (char _c in Input.inputString)
            {
                //If player starts typing turn off
                if (_c == '\b' || _c == '\r')
                {
                    EndBehavior();
                    return;
                }
                else if ((int)_c > 32 && (int)_c < 127)
                {
                    EndBehavior();
                    return;
                }
            }
        }

        typeTime += Time.deltaTime;
        if (currentWord == string.Empty)
        {
            if (typeTime >= waitRate)
            {
                typeTime = 0.0f;
                ChooseWord();
            }
        }
        else
        {
            if (typeTime >= typeRate)
            {
                typeTime = 0.0f;
                if (currentIndex == currentWord.Length)
                {
                    WordBuffer.instance.EnterCommand();
                    currentWord = string.Empty;
                    waitRate = waitRateSucceed;
                }
                else if (WordBuffer.instance.AddCharacter(currentWord[currentIndex]))
                {
                    foreach (Enemy_KeyLogger _logger in FindObjectsOfType<Enemy_KeyLogger>())
                    {
                        _logger.OverrideShoot();
                    }
                    ++currentIndex;
                }
            }
        }
    }

    protected override void EndBehavior()
    {
        base.EndBehavior();
        VisualCommandPanel.instance.AddMessage("A.I. typer disabled");
    }

    private void ChooseWord()
    {
        currentIndex = 0;
        if (Player.instance.ActorHealthPercent() < 0.1f && Player.instance.numLives == 1)
        {
            if (TryWord(AIWords.RESET))
            {
                return;
            }
        }
        if (Player.instance.ActorHealthPercent() < 0.4f)
        {
            if(TryWord(AIWords.REPAIR))
            {
                return;
            }
        }
        if(BossWord.instance.GetBossHealth() < 0.175f)
        {
            if (TryWord(AIWords.BOSS))
            {
                return;
            }
        }
        if(AdManager.instance.numActiveAds > 0)
        {
            if (TryWord(AIWords.CLOSE))
            {
                return;
            }
        }
        if (Player.instance.ActorHealthPercent() < 0.8f)
        {
            if(Enemy.numAliveEnemies > 2 || BossWord.instance.WordAvailable())
            {
                if ((!aiWords[(int)AIWords.FILTER].isBehaviourActive) && (!aiWords[(int)AIWords.ENCRYPT].isBehaviourActive) && (!aiWords[(int)AIWords.FIREWALL].isBehaviourActive))
                {
                    if (aiWords[(int)AIWords.FILTER].WordAvailable() && Random.Range(0, 2) == 0)
                    {
                        if (TryWord(AIWords.FILTER))
                        {
                            return;
                        }
                    }
                    else if(aiWords[(int)AIWords.ENCRYPT].WordAvailable())
                    {
                        if (TryWord(AIWords.ENCRYPT))
                        {
                            return;
                        }
                    }
                }
            }
        }
        if (UpdateBehavior.instance.updatesAvailable > 4)
        {
            if (TryWord(AIWords.UPDATE))
            {
                return;
            }
        }
        if(lastWord == AIWords.SCAN)
        {
            if (TryWord(AIWords.ANTIVIRUS))
            {
                return;
            }
        }
        if (TryWord(AIWords.FIREWALL))
        {
            return;
        }
        if (Enemy.numAliveEnemies > 9 || BossWord.instance.WordAvailable())
        {
            if (TryWord(AIWords.AUTH))
            {
                return;
            }
        }
        if (lastWord != AIWords.AUTH)
        {
            if (Enemy.numAliveEnemies > 4 || BossWord.instance.WordAvailable())
            {
                if (TryWord(AIWords.ANTIVIRUS))
                {
                    return;
                }
            }
            if (Enemy.numAliveEnemies > 2 || BossWord.instance.WordAvailable())
            {
                if (TryWord(AIWords.SCAN))
                {
                    return;
                }
            }
        }
        if (TryWord(AIWords.BACKUP))
        {
            return;
        }

        if (Player.instance.ActorHealthPercent() < 0.95f && Enemy.numAliveEnemies < 2 && (!BossWord.instance.WordAvailable()))
        {
            if (TryWord(AIWords.REPAIR))
            {
                return;
            }
        }

        if (UpdateBehavior.instance.updatesAvailable > 1)
        {
            if (TryWord(AIWords.UPDATE))
            {
                return;
            }
        }
        
        lastWord = AIWords.COUNT;
        currentWord = string.Empty;
        waitRate = waitRateNoWord;
    }

    private bool TryWord(AIWords _aiWord)
    {
        if(aiWords[(int)_aiWord].WordAvailable())
        {
            currentWord = aiWords[(int)_aiWord].word;
            lastWord = _aiWord;
            return true;
        }
        return false;
    }

    public override void Reset()
    {
        behaviorActive = false;
    }
}
