using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boss : Enemy 
{
	[SerializeField] private Text passwordText = null;
	private string password;
	private string currentPassword;
	[SerializeField] private string[] passwordSelections = null;
	private int charactersHidden;

    protected bool bossDefeated = false;
    public bool bossIsDefeated { get { return bossDefeated; } }

    public override void TakeDamage(float _damage)
    {
        soundManager.instance.playSound(0);
        if (!bossDefeated)
        {
            health = Mathf.Max(0.0f, health - _damage);
            RevealWord();
        }
    }

    private void RevealWord()
    {
        while ((health / defaultHealth) < ((float)charactersHidden / (float)password.Length))
        {
            int i = 0;
            while (true)
            {
                if (currentPassword[i] == '_')
                {
                    if (Random.Range(0, 3) == 0)
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
                if (i == currentPassword.Length)
                {
                    i = 0;
                }
            }
        }
    }

    public override void OnSpawn()
    {
        password = passwordSelections[Random.Range(0, passwordSelections.Length)];
        charactersHidden = password.Length;
        for (int i = 0; i < password.Length; ++i)
        {
            currentPassword += '_';
        }
        passwordText.text = currentPassword;
        BossWord.instance.BossActive(this, password);
    }

    public void PasswordEntered()
    {
        if (!bossDefeated)
        {
            Death();
        }
    }

    public override void Reset()
    {
        currentPassword = string.Empty;
        base.Reset();
        bossDefeated = false;
    }

    protected override void Death()
    {
        //do explosions
        bossDefeated = true;
        Player.instance.IncreaseScore(score);
        StartCoroutine(bossDeath());
    }

    protected virtual IEnumerator bossDeath()
    {
        base.Death();
        EnemyManager.instance.NextLevel();
        yield return new WaitForSeconds(0.1f);
    }

    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            _col.gameObject.GetComponent<Actor>().TakeDamage(contactHitDamage);
        }
    }
}