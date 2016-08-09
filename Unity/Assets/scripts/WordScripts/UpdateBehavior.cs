using UnityEngine;
using System.Collections;

public enum UpdateTypes
{
	UPDATE = 0,
	FIREWALL,
	ANTIVIRUS,
	REPAIR,
	QUICKSCAN,
	SPAMFILTER,
	AUTHENTICATE,
	BACKUP,
	ENCRYPT,
	FACTORYRESET,
	MAX_ABILITY_UPDATES,
	HEALTH,
	FIRERATE,
	SPEED,
	DAMAGE,
	MAX_UPDATES
}

[System.Serializable]
public struct UpdateLog
{
	public UpdateTypes type;
	public float updateIncrement;
	public float scoreRequirement;
	public string updateMessage;
}

public class UpdateBehavior : MonoBehaviour 
{
	private static UpdateBehavior staticInstance = null;
	public static UpdateBehavior instance {get {return staticInstance;} set{}}

	private int nextUpdate = 0;
	private int numUpdates = 0;
    public int updatesAvailable { get { return updating ? 0 : numUpdates; } }
	private int startingScore = 0;
	[SerializeField] private UpdateLog[] updateLog = null;
	private bool updating = false;

    private bool everythingUpdated = false;

	[SerializeField] private UpdateWord updateWord = null;  

	private void Awake()
	{
		staticInstance = this;
	}

	private void Update()
	{
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!everythingUpdated)
            { 
                //If not currently updating, determine if there are any further updates
                if (!updating)
                {
                    if ((nextUpdate + numUpdates) < updateLog.Length)
                    {
                        int _prevUpdates = numUpdates;
                        while (updateLog[nextUpdate + numUpdates].scoreRequirement <= (Player.instance.GetScore() - startingScore))
                        {
                            ++numUpdates;
                            if ((nextUpdate + numUpdates) >= updateLog.Length)
                            {
                                break;
                            }
                        }
                        if (_prevUpdates != numUpdates)
                        {
                            VisualCommandPanel.instance.AddMessage(numUpdates.ToString() + " updates available!");
                        }
                    }
                }
            }
        }
	}

	public void FactoryReset()
	{
		startingScore = Player.instance.GetScore();///TEMPORARY VARIABLE HERE///
		nextUpdate = 0;
		numUpdates = 0;
        everythingUpdated = false;
        updating = false;
        foreach(Word _w in FindObjectsOfType<Word>())
        {
            _w.Reset();
        }
	}

	public int PrepareUpdates()
	{
		updating = numUpdates > 0 ? true : false;
		return numUpdates;
	}

	public void ApplyNextUpdate ()
	{
		if(updateLog[nextUpdate].type < UpdateTypes.MAX_ABILITY_UPDATES)
		{
			WordBuffer.instance.AbilityWordUpdate((int)updateLog[nextUpdate].type, (int)updateLog[nextUpdate].updateIncrement);
		}
		//Else if in case more update types
		else if (updateLog[nextUpdate].type < UpdateTypes.MAX_UPDATES)
		{
            Player.instance.ImproveStat(updateLog[nextUpdate].type, (int)updateLog[nextUpdate].updateIncrement);
			//Improve player stats
		}

		VisualCommandPanel.instance.AddMessage (updateLog[nextUpdate].updateMessage);
		++nextUpdate;
        if (nextUpdate == updateLog.Length)
        {
            everythingUpdated = true;
        }
    }

	public void FinishUpdate()
	{
		updating = false;
		VisualCommandPanel.instance.AddMessage(numUpdates.ToString() + "/" + numUpdates.ToString() + " Updates installed");
		numUpdates = 0;
	}

	public void UnneccessaryUpdate()
	{
		updating = false;
		VisualCommandPanel.instance.AddMessage("Up to date");
		numUpdates = 0;
	}

    public void UnlockAll()
    {
        numUpdates = 0;
        while(!everythingUpdated)
        {
            ApplyNextUpdate();
            ++numUpdates;
        }
        FinishUpdate();
        updateWord.Reset();
    }

    public bool EverythingUnlocked()
    {
        return everythingUpdated;
    }
}
