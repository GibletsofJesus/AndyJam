using UnityEngine;
using System.Collections;

public class Player : Actor 
{
	private static Player staticInstance = null;
	public static Player instance {get {return staticInstance;} set{}}

    [SerializeField] private SpriteRenderer spriteRenderer = null;

	protected float updatedDefaultHealth;
	[SerializeField] private int defaultLives = 3;
	private int lives;

    public int score = 0;
    public bool advertAttack = false;
    //public float moveSpeed;
    public AudioClip[] shootSounds;
    public ParticleSystem[] muzzleflash;
    Vector3 rotLerp;
    //public GameObject target;
    Vector3 screenBottom, screenTop;
    Vector2 verticalBoundsBot, verticalBoundsTop;

    private bool isInvincible = false;
    private float invincibleFlickerRate = 0.25f;
    private float invincibleFlickerCooldown = 0.0f;
    private bool flickerDown = true;
    private int addAmount = 0;
    private int randomAdAmount;
    private float adCool = 0;
    private float maxAdCool;
    protected override void Awake()
	{
        maxAdCool = Random.Range(1, 3);
        randomAdAmount = Random.Range(2, 5);
		staticInstance = this;
		base.Awake ();
		updatedDefaultHealth = defaultHealth;
		lives = defaultLives;
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f, -.5f, .3f));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 1.5f, .3f));
        verticalBoundsBot = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 0f));
        verticalBoundsTop = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f));
        GetComponent<SpriteRenderer>().sprite = GreenShip.instance.ship;
    }
    void Start()
    {
        Destroy(GreenShip.instance.gameObject);
    }
	// Update is called once per frame
    protected void FixedUpdate()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();

            if (transform.position.x <= -screenBottom.x - 1f)
            {
                rig.AddForce(Vector2.right * 2, ForceMode2D.Impulse);
            }
            if (transform.position.x >= screenBottom.x + 1f)
            {
                rig.AddForce(-Vector2.right * 2, ForceMode2D.Impulse);
            }
            if (transform.position.y >= verticalBoundsTop.y / 2 - 1f)
            {
                rig.AddForce(-Vector2.up * 2, ForceMode2D.Impulse);
            }
            if (transform.position.y <= verticalBoundsBot.y + 5f)
            {
                rig.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }

            inputThings();

            float angle = 0;

            if (!rolling)
                angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotLerp.y, Time.deltaTime * 5);

            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        //Invincible flicker logic
        if(isInvincible)
        {   
            if(flickerDown)
            {
                invincibleFlickerCooldown = Mathf.Max(invincibleFlickerCooldown - Time.deltaTime, 0.0f);
                if (invincibleFlickerCooldown == 0)
                {
                    flickerDown = false;
                }
            }
            else
            {
                invincibleFlickerCooldown = Mathf.Min(invincibleFlickerCooldown + Time.deltaTime, invincibleFlickerRate);
                if (invincibleFlickerCooldown == invincibleFlickerRate)
                {
                    flickerDown = true;
                }
            }
            Color _flickerColour = spriteRenderer.color;
            _flickerColour.a = 0.25f + ((invincibleFlickerCooldown / invincibleFlickerRate) / 0.75f);
            spriteRenderer.color = _flickerColour;
        }
        AdwareAds();
    }

    bool rolling;
    IEnumerator doABarrrelRoll(int dir)
    {
        rolling = true;
        Vector3 initialRot = transform.rotation.eulerAngles;
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
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            if (!rolling)
            {
                if (transform.rotation.eulerAngles.y > 314 && transform.rotation.eulerAngles.y < 317)
                    StartCoroutine(doABarrrelRoll(-1));

                rotLerp = new Vector3(0, 45, 0);
            }
            rig.AddForce(-Vector2.right * speed, ForceMode2D.Impulse);
        }
        else if (Input.GetAxis("Horizontal") > 0.1f)
        {
            if (!rolling)
            {
                if (transform.rotation.eulerAngles.y > 42 && transform.rotation.eulerAngles.y < 45)
                    StartCoroutine(doABarrrelRoll(1));
                rotLerp = new Vector3(0, -45, 0);
            }
            rig.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
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

        if (Input.GetAxis("Vertical") > 0.1f)
        {
            rig.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
        else if (Input.GetAxis("Vertical") < -0.1f)
        {
            rig.AddForce(-Vector2.up * speed, ForceMode2D.Impulse);
        }

        #endregion

        #region shoot
        if (Mathf.Abs(Input.GetAxis("Fire1")) > 0.1f)
        {
            if (Shoot(projData, transform.up, shootTransform))
            {
                foreach (ParticleSystem ps in muzzleflash)
                {
                    ps.Emit(1);
                }

                soundManager.instance.playSound(shootSounds[Random.Range(0, shootSounds.Length)]);

                if (CameraShake.instance.shakeDuration < 0.2f)
                {
                    CameraShake.instance.shakeDuration = 0.2f;
                    CameraShake.instance.shakeAmount = 0.15f;
                }
            }
        }
        #endregion
    }

    public override void TakeDamage(float _damage)
    {
        soundManager.instance.playSound(0);
        if (!isInvincible)
        {
            health -= _damage;
            if (health <= 0)
            {

                health = updatedDefaultHealth;
                --lives;
                PlayerHUD.instance.UpdateLives(lives);
                //Out of lives then kill player
                if (lives == 0)
                {
                    Death();
                }
                //If not dead then reset health to the current upgraded amount
                else
                {
                    Explosion ex = ExplosionManager.instance.PoolingExplosion(transform, 1);
                    ex.transform.position = transform.position;
                    gameObject.SetActive(false);
                    ex.gameObject.SetActive(true);
                    ex.explode();

                    Invoke("Respawn", 2);
                }
            }
            PlayerHUD.instance.UpdateHealth(health / updatedDefaultHealth);

            if (CameraShake.instance.shakeDuration < 0.2f)
                CameraShake.instance.shakeDuration += 0.2f;
            CameraShake.instance.shakeAmount = 0.5f;

            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("revertColour", .1f);
        }
    }

    protected override void Death()
    {
        Explosion ex = ExplosionManager.instance.PoolingExplosion(transform, 1);
        ex.transform.position = transform.position;
        gameObject.SetActive(false);
        ex.gameObject.SetActive(true);
        ex.explode();
        soundManager.instance.music.Stop();
        GameStateManager.instance.GameOver();
    }

    void Respawn()
    {
        transform.position = verticalBoundsBot + Vector2.up;
        gameObject.SetActive(true);
    }

    public virtual bool Heal(float _heal)
    {
        health = Mathf.Min(health + _heal, updatedDefaultHealth);
        PlayerHUD.instance.UpdateHealth(health / updatedDefaultHealth);
        if(health == updatedDefaultHealth)
        {
            return true;
        }
        return false;
    }

    void revertColour()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

	public override void Reset()
	{
		base.Reset ();
		updatedDefaultHealth = defaultHealth;
        PlayerHUD.instance.UpdateHealth(health / updatedDefaultHealth);
        lives = defaultLives;
        PlayerHUD.instance.UpdateLives(lives);
        //score = 0;
        //PlayerHUD.instance.UpdateScore(score);
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

	public void HomingProjectiles(bool _homing, float _radius)
	{
		projData.homingBullets = _homing;
		projData.homingRadius = _radius;
	}

	public void ExplodingProjectiles(bool _rocket, float _damage, float _radius)
	{
		projData.explodingBullets = _rocket;
		projData.explosionDamage = _damage;
		projData.explosionRadius = _radius;
	}

    public void ImproveStat(UpdateTypes _type, float _improve)
    {
        switch (_type)
        {
            case UpdateTypes.HEALTH:
                float _percent = health / updatedDefaultHealth;
                updatedDefaultHealth += _improve;
                health = updatedDefaultHealth * _percent;
                break;
            case UpdateTypes.FIRERATE:
                shootRate -= _improve;
                break;
            case UpdateTypes.SPEED:
                speed += _improve;
                break;
            case UpdateTypes.DAMAGE:
                projData.projDamage += _improve;
                break;

        }
    }

    public void SetInvincible(bool _invincible)
    {
        isInvincible = _invincible;
        invincibleFlickerCooldown = invincibleFlickerRate;
        spriteRenderer.color = Color.white;
        flickerDown = true;
    }

    void AdwareAds()
    {
        if (advertAttack)
        {
            if (adCool < maxAdCool)
            {
                adCool += Time.deltaTime;
            }
            else
            {
                if (addAmount < randomAdAmount)
                {
                    AdManager.instance.TryGenerateAd(new Vector3(25, 25, 0));
                    addAmount++;
                    adCool = 0;
                }
                else
                {
                    addAmount = 0;
                    adCool = 0;
                    advertAttack = false;
                }
            }
        }
    }
}
