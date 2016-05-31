using UnityEngine;
using System.Collections;

public class Repair : AbilityWord
{
    [SerializeField] private float[] totalHeal = null;
    [SerializeField] private float[] healSpeed = null;
    private float totalHealAmount;

    protected override void Start ()
	{
		wordTiers = new string[] {"repair.exe", "restore.exe", "systemrestore.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        totalHealAmount = totalHeal[currentTier];
	}
	
	protected override void Behavior ()
	{
        float _healAmount = Time.deltaTime * healSpeed[currentTier];
        totalHealAmount -= _healAmount;
        if(totalHealAmount < 0)
        {
            _healAmount += totalHealAmount;
            EndBehavior();
        }
        if(Player.instance.Heal(_healAmount))
        {
            EndBehavior();
        }
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}
}