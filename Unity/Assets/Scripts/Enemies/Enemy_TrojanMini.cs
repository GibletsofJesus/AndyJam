﻿using UnityEngine;
using System.Collections;

public class Enemy_TrojanMini : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
        }
    }
    protected override void Movement()
    {
        Vector2 movement = -transform.up * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }


    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        Vector3 shootDir = -(transform.position - new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y));

        if (shootDir.x > 0)
            shootDir.x = Mathf.Clamp(shootDir.x, 0, 5);
        else
            shootDir.x = Mathf.Clamp(shootDir.x, -5, 0);

        return base.Shoot(_projData, shootDir, _shootTransform);
    }
    
  
}