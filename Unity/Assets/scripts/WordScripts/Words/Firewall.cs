using UnityEngine;
using System.Collections;

public class Firewall : AbilityWord
{
    [SerializeField] private GameObject[] firewalls = null;

    protected override void Start ()
	{
        description = "stationary defensive barrier";
        wordTiers = new string[] {"firewall.exe", "firewall.exe", "firewall.exe"};
		base.Start ();
	}

	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        for(int i = 0; i < currentTier + 1; ++i)
        {
            firewalls[i].SetActive(true);
        }
	}
	
	protected override void Behavior ()
	{
        //If last firewall broken
        if (!firewalls[0].activeSelf)
        {
            EndBehavior();
        }
    }
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
    }
}
