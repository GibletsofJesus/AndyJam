using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Encrypt : AbilityWord
{
    [SerializeField] private float[] invincibleDuration = null;
    private float invincibleCooldown = 0.0f;
    [SerializeField] private GameObject lockedHealth = null;

	protected override void Start ()
	{
        description = "invincibility";
        wordTiers = new string[] {"encrypt.exe", "encrypt.exe", "encrypt.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior()
    {
		base.TriggerBehavior ();
        lockedHealth.gameObject.SetActive(true);
        Player.instance.SetInvincible(true);
	}
	
	protected override void Behavior ()
	{
        invincibleCooldown = Mathf.Min(invincibleCooldown + Time.deltaTime, invincibleDuration[currentTier]);
        if(invincibleCooldown == invincibleDuration[currentTier])
        {
            EndBehavior();
        }
	}
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
        invincibleCooldown = 0.0f;
        lockedHealth.gameObject.SetActive(false);
        Player.instance.SetInvincible(false);
    }
}