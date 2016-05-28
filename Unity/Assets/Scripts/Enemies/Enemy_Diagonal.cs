using UnityEngine;
using System.Collections;

public class Enemy_Diagonal : Enemy
{
    Vector2 moveToDir;
    float edgeWeight = 5f;
    // Use this for initialization
    void Start()
    {
        SetActor(10, 1, 5, 1f);
        moveToDir = new Vector2(15, 0);
        safeHealth = GetHealth();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Movement()
    {
        moveToDir.y = transform.position.y;
        if (Vector2.Distance(moveToDir, transform.position) < edgeWeight)
        {
            moveToDir *= -1;
        }

        transform.Translate(new Vector2(moveToDir.x / 10, -transform.up.y) * GetSpeed(), Space.World);
    }
}
