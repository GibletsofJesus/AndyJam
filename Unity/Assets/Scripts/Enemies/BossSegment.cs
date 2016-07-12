using UnityEngine;
using System.Collections;

public class BossSegment : Enemy
{
    [SerializeField] protected Boss boss = null;
    [SerializeField] protected SpriteRenderer spriteRenderer = null;

    public override void TakeDamage(float _damage)
    {
        if (IsInvoking("revertColour"))
        {
            CancelInvoke("revertColour");
        }
        spriteRenderer.color = Color.red;
        Invoke("revertColour", .1f);
        boss.TakeDamageFromSegment(_damage);
    }

    protected override void Death()
    {
        if (boss.bossIsDefeated)
        {
            Reset();
            gameObject.SetActive(false);
        }
    }

    public override void Reset()
    {
        base.Reset();
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            _col.gameObject.GetComponent<Actor>().TakeDamage(contactHitDamage);
        }
    }
}


