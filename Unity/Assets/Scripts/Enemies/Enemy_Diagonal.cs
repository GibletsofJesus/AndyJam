using UnityEngine;
using System.Collections;

public class Enemy_Diagonal : Enemy
{
    Vector2 moveToDir;
    Vector2 target;
    float edgeWeight = 5f;
    // Use this for initialization
    void Start()
    {
        moveToDir = new Vector2(15, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
            target = Player.instance.transform.position;

        }
    }

    protected override void Movement()
    {

        if (Camera.main.WorldToViewportPoint(transform.position).y<0.5f)
        {

            transform.position = Vector2.MoveTowards(transform.position, target, (speed*2) * Time.deltaTime);
        }
        else
        {
            moveToDir.y = transform.position.y;
            if (Vector2.Distance(moveToDir, transform.position) < edgeWeight)
            {
                moveToDir *= -1;
            }
            transform.Translate(new Vector2(moveToDir.x / 10, -transform.up.y) * speed * Time.deltaTime, Space.World);
        }
        float lookAngle = Mathf.Atan2(transform.position.x - target.x, 
            transform.position.y - target.y) * Mathf.Rad2Deg;
        Quaternion rot = new Quaternion(0,0,0,0);
        rot.eulerAngles = new Vector3(0, 0, -lookAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation,rot,
            ((speed*10)*Time.deltaTime)*Vector2.Distance(transform.position,target));
    }
}
