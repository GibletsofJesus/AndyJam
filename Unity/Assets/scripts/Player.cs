using UnityEngine;
using System.Collections;

public class Player : Actor 
{
	private static Player staticInstance = null;
	public static Player instance {get {return staticInstance;} set{}}

	protected float updatedDefaultHealth;
	[SerializeField] private int defaultLives = 3;
	private int lives;

    private int score = 0;

  
    //public float moveSpeed;
    public AudioClip[] shootSounds;
    public ParticleSystem[] muzzleflash;
    Vector3 rotLerp;
    //public GameObject target;
    Vector3 screenBottom, screenTop;
    public bool homingBullets;

	protected override void Awake()
	{
		staticInstance = this;
		base.Awake ();
		updatedDefaultHealth = defaultHealth;
		lives = defaultLives;
	}

    // Use this for initialization
    protected virtual void Start()
    {
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f, -.5f, .3f));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 1.5f, .3f));
	}
	
	// Update is called once per frame
    protected void FixedUpdate()
    {
        base.Update();


        //Whoever did this, you are scum.
       // if (transform.position.x <= -11f)
       /* if (homingBullets)
       {
            if (!target || !target.activeSelf)
            {
                target = EnemyManager.instance.FindClosestEnemyToPlayer(50, transform);
            }
        }*/

        //if (transform.position.x >=11f)
        //{
        //    rig.AddForce(-Vector2.right, ForceMode2D.Impulse);
        //}


        if (transform.position.x <= -screenBottom.x-1f)
        {
            rig.AddForce(Vector2.right*2, ForceMode2D.Impulse);
        }
        if (transform.position.x >= screenBottom.x+1f)
        {
            rig.AddForce(-Vector2.right*2, ForceMode2D.Impulse);
        }


        inputThings();

        Quaternion q = Quaternion.Euler(rotLerp);
        float angle=0;
        
        if (!rolling)
            angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotLerp.y, Time.deltaTime * 5);
        
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    bool rolling;
    IEnumerator doABarrrelRoll(int dir)
    {
        rolling = true;
        Vector3 initialRot = transform.rotation.eulerAngles;
        Vector3 goalRot = rotLerp;
        float t=0;

        Vector3 currentRot = initialRot;

        while (t < .5f)
        {
            currentRot.y = Mathf.Lerp(initialRot.y, rotLerp.y + ((dir > 0) ? 360 : -90), t / .5f);
            transform.rotation = Quaternion.Euler(currentRot);
            t += Time.deltaTime;
            yield return null;
        }

        rolling = false;
    }

    private void inputThings()
    {
        #region move
        if (Input.GetKey(KeyCode.LeftArrow))
        {
                if (!rolling)
                {
                    if (transform.rotation.eulerAngles.y > 314 && transform.rotation.eulerAngles.y < 317)
                        StartCoroutine(doABarrrelRoll(-1));

                    rotLerp = new Vector3(0, 45, 0);
                }
                rig.AddForce(-Vector2.right * speed, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!rolling)
            {
                if (transform.rotation.eulerAngles.y > 42 && transform.rotation.eulerAngles.y < 45)
                    StartCoroutine(doABarrrelRoll(1));
                rotLerp = new Vector3(0, -45, 0);
            }

            rig.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }

        else
        {
            rotLerp = Vector3.zero;
        }
        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rig.AddForce(-Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }*/
        #endregion

        #region shoot
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Shoot(projData, transform.up, shootTransform,homingBullets);
            {
                foreach (ParticleSystem ps in muzzleflash)
                {
                    ps.Emit(1);
                }
                soundManager.instance.playSound(shootSounds[Random.Range(0, shootSounds.Length - 1)]);

                if (CameraShake.instance.shakeDuration < 0.2f)
                    CameraShake.instance.shakeDuration = 0.2f;
                CameraShake.instance.shakeAmount = 0.15f;
            } 
        }
        #endregion
    }

    public override void TakeDamage(float _damage)
    {
		//Do not call base as players have lives
		health -= _damage;
        PlayerHUD.instance.UpdateHealth(health / updatedDefaultHealth);
		if(health <= 0)
		{
			//Out of lives then kill player
			if(lives == 0)
			{
				Death ();
			}
			//If not dead then reset health to the current upgraded amount
			else
			{
				health = updatedDefaultHealth;
                --lives;
                PlayerHUD.instance.UpdateLives(lives);
            }
		}
        if (CameraShake.instance.shakeDuration < 0.2f)
            CameraShake.instance.shakeDuration += 0.2f;
        CameraShake.instance.shakeAmount = 0.5f;
    }

	protected override void Reset()
	{
		base.Reset ();
		updatedDefaultHealth = defaultHealth;
        PlayerHUD.instance.UpdateHealth(health / updatedDefaultHealth);
        lives = defaultLives;
        PlayerHUD.instance.UpdateLives(lives);
        score = 0;
        PlayerHUD.instance.UpdateScore(score);
	}

    public int GetScore()
    {
        return score;
    }

    public void IncreaseScore(int _score)
    {
        score += _score;
        PlayerHUD.instance.UpdateScore(score);
    }

}
