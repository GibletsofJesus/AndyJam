using UnityEngine;
using System.Collections;

public class Authenticate : AbilityWord
{
    [SerializeField] private LaserModule laser = null;
    [SerializeField] private GameObject chargeCannon = null;
    [SerializeField] private Transform chargeCannonOriginalPosition = null;
    [SerializeField] private Transform chargeCannonNewPosition = null;

    [SerializeField] private float[] chargeLaser = null;
    [SerializeField] private float[] damageLaser = null;
    [SerializeField] private float[] durationLaser = null;

    private float cannonMove = 0.0f;

	protected override void Start ()
	{
        description = "laser weapon";
        wordTiers = new string[] {"authenticate.exe", "authenticate.exe", "authenticate.exe"};
		base.Start ();
	}
	
	protected override void TriggerBehavior()
    {
		base.TriggerBehavior ();
        laser.SetParameters(chargeLaser[currentTier], durationLaser[currentTier], damageLaser[currentTier]);
        laser.FireLaser();
        cannonMove = 0.0f;
	}
	
	protected override void Behavior ()
	{
		if(!laser.IsLaserFiring())
        {
            cannonMove = Mathf.Max(0.0f, cannonMove - Time.deltaTime);
            if (cannonMove == 0.0f)
            {
                EndBehavior();
            }
        }
        else
        {
            cannonMove = Mathf.Min(1.0f, cannonMove + Time.deltaTime);
        }
        chargeCannon.transform.localPosition = Vector3.Lerp(chargeCannonOriginalPosition.localPosition, chargeCannonNewPosition.localPosition, cannonMove);
    }
	
	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}

    public override void Reset()
    {
        laser.Reset();
        cannonMove = 0.0f;
        chargeCannon.transform.localPosition = Vector3.Lerp(chargeCannonOriginalPosition.localPosition, chargeCannonNewPosition.localPosition, cannonMove);
        base.Reset();
    }
}