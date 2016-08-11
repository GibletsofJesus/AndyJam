using UnityEngine;
using System.Collections;

public class Filter : AbilityWord
{
    [SerializeField] private GameObject[] filters = null;
    [SerializeField] private float[] filterDuration = null;
    private float filterCooldown = 0.0f;

    protected override void Start ()
	{
        description = "moving defensive barrier";
        wordTiers = new string[] {"spamfilter.exe", "spamfilter.exe", "spamfilter.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
		base.TriggerBehavior ();
        for (int i = 0; i < currentTier + 1; ++i)
        {
            filters[i].transform.rotation = Quaternion.Euler(0.0f,0.0f, i * (360.0f / (currentTier + 1)));
            filters[i].SetActive(true);
        }
        AdManager.instance.SpamFilter(true);
        filterCooldown = 0.0f;
    }
	
	protected override void Behavior ()
	{
        filterCooldown += Time.deltaTime;
        if(filterCooldown >= filterDuration[currentTier])
        {
            EndBehavior();
        }
    }
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
        AdManager.instance.SpamFilter(false);
        for (int i = 0; i < currentTier + 1; ++i)
        {
            filters[i].SetActive(false);
        }
    }
}