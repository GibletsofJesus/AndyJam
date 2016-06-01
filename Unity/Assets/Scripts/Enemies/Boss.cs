using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0,9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    public Enemy rareBastards;
    float maxCool = 5;
    float minicool = 0.8f;
    float minicooler = 0.8f;
    float cool;
    int dosCount = 0;
    bool move = false;

	[SerializeField] private Text passwordText = null;
	private string password;
	private string currentPassword;
	[SerializeField] private string[] passwordSelections = null;
	private int charactersHidden;

	// Use this for initialization
	void Start ()
    {
        //SetActor(100, 1, 1, 0.8f);
        cool = maxCool;
        //safeHealth = health;
	
	}

    void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    {
        Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
        e.transform.position = _spawnPoint;
        e.gameObject.SetActive(true);

    }
	// Update is called once per frame
	protected override void Update () 
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            MiniCool();

            if (cool <= maxCool)
            {
                cool += Time.deltaTime;
            }

            if (move)
            {
                EyeShooting();
            }


            //Debug.Log("count " + dosCount + " cool " + cool);

            base.Update();
        }
	}

	public override void TakeDamage(float _damage)
	{
		soundManager.instance.playSound(0);
		if (GetComponent<SpriteRenderer>() && tag == "Enemy")
		{
			GetComponent<SpriteRenderer>().color = Color.red;
			Invoke("revertColour", .1f);
		}
		health = Mathf.Max(0.0f, health - _damage);
		RevealWord ();
	} 

	private void RevealWord()
	{
		while((health / defaultHealth) < ((float)charactersHidden / (float)password.Length))
		{
			int i = 0;
			while(true)
			{
				if(currentPassword[i] == '_')
				{
					if(Random.Range(0, 3) == 0)
					{
						--charactersHidden;
						char[] array = currentPassword.ToCharArray();
						array[i] = password[i];
						currentPassword = new string(array);
						passwordText.text = currentPassword;
						return;
					}
				}
				++i;
				if(i == currentPassword.Length)
				{
					i = 0;
				}
			}
		}
	}

	public override void OnSpawn()
	{
		password = passwordSelections [Random.Range (0, passwordSelections.Length - 1)];
		charactersHidden = password.Length;
		for(int i = 0; i < password.Length; ++i)
		{
			currentPassword += '_';
		}
		passwordText.text = currentPassword;
		BossWord.instance.BossActive (this, password);
	}

	public void PasswordEntered()
	{
		Death ();
	}

	protected override void Reset()
	{
		currentPassword = string.Empty;
		base.Reset ();
	}

    protected override void Movement()
    {
		transform.position = Vector2.MoveTowards(transform.position, bossTarget,speed*2* Time.deltaTime);
		if (Vector2.Distance(transform.position,bossTarget)<1)
        {
            move = true;
        }
    }

    void EyeShooting()
    {
        if (dosCount<10&&cool>=maxCool)
        {
            if (MiniCool())
            {
              SpawnBossEnemies(eyeShot[Random.Range(0,eyeShot.Length - 1)].position, littleBastards);
                minicooler = 0;
                dosCount++;
            }
        }
        else
        {
            dosCount = 0;
        }
        if (dosCount >= 10 && cool >= maxCool)
        {
            cool = 0;
        }
    }
   
  
   bool MiniCool()
    {
        if (minicooler <= minicool)
        {
            minicooler += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
