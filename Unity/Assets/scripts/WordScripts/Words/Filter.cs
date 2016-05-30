using UnityEngine;
using System.Collections;

public class Filter : AbilityWord
{
    [SerializeField] private GameObject[] filters = null;
    [SerializeField] private float filterDuration = 10.0f;
    private float filterCooldown = 0.0f;

    protected override void Start ()
	{
		wordTiers = new string[] {"spamfilter.exe", "spamfilter.exe", "spamfilter.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior ()
	{
		base.TriggerBehavior ();
        for (int i = 0; i < currentTier + 1; ++i)
        {
            filters[i].transform.rotation = Quaternion.Euler(0.0f,0.0f, i * (360.0f / (currentTier + 1)));
            filters[i].SetActive(true);
        }
        filterCooldown = 0.0f;
    }
	
	protected override void Behavior ()
	{
        filterCooldown += Time.deltaTime;
        if(filterCooldown >= filterDuration)
        {
            EndBehavior();
        }
    }
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
        for (int i = 0; i < currentTier + 1; ++i)
        {
            filters[i].SetActive(false);
        }
    }
}