using UnityEngine;
using System.Collections;

public class Enemy_InfectedBlack : Enemy 
{
    ///select random point in gamespace
    ///warp to pos wait for x seconds, fire
    ///repeat
    ///
    float screenSide;


    protected override void Awake()
    {
        base.Awake();
        screenSide = Camera.main.ViewportToWorldPoint(new Vector3(.3f, -.5f, .3f)).x;
    }
   Vector2 RandomPointInSpace(float _XMax, float _XMin,float _YMax,float _YMin)
    {
        return new Vector2(Random.Range(_XMin, _XMax), Random.Range(_YMin, _YMax));     
    }
}
