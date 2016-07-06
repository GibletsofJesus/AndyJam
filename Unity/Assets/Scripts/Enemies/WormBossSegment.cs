using UnityEngine;
using System.Collections;

public class WormBossSegment : Enemy
{
    [SerializeField] private WormBoss wormBoss = null;

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            Movement();
        }
    }

    public override void TakeDamage(float _damage)
    {
        wormBoss.TakeDamage(_damage);
    }

    protected override void Movement()
    {
        
    }
}
