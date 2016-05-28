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
}

public class UpdateBehavior : MonoBehaviour 
{
	private static UpdateBehavior staticInstance = null;
	public static UpdateBehavior instance {get {return staticInstance;} set{}}

	private int nextUpdate = 0;
	private int startingScore = 0;
	[SerializeField] private UpdateLog[] updateLog = null;

	private int tempScore = 0;
	/// <summary>
	/// Temp
	/// </summary>

	private void Awake()
	{
		staticInstance = this;
	}

	private void Update()
	{
		tempScore += 5;///temp///
	}

	public void FactoryReset()
	{
		startingScore = tempScore;///TEMPORARY VARIABLE HERE///
		nextUpdate = 0;
	}

	public void ApplyUpdate()
	{
		//If not the last update
		if(nextUpdate != updateLog.Length)
		{
			//Can get next update
			while(updateLog[nextUpdate].scoreRequirement <= (tempScore - startingScore))
			{
				if(updateLog[nextUpdate].type < UpdateTypes.MAX_ABILITY_UPDATES)
				{
					WordBuffer.instance.AbilityWordUpdate((int)updateLog[nextUpdate].type, (int)updateLog[nextUpdate].updateIncrement);
				}
				//Else if in case more update types
				else if (updateLog[nextUpdate].type < UpdateTypes.MAX_UPDATES)
				{
					//Improve player stats
				}
				++nextUpdate;
				//break out loop on last update
				if(nextUpdate == updateLog.Length)
				{
					break;
				}
			}
		}
	}
}
