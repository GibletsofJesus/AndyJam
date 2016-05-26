using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    float damage;
    float speed;
    float aliveTime;
    float currentAliveTime;

    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Fire(Vector2 _direction)
    {
        transform.Translate(_direction);
    }
    public void WeaponVariables(float _damage, float _speed,float _aliveTime)
    {
        damage = _damage;
        speed = _speed;
        aliveTime = _aliveTime;
        currentAliveTime = aliveTime;
    }
    void Alive()
    {
        if (currentAliveTime <=0)
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<playerMovement>())
        {
            //do a thing
        }
         gameObject.SetActive(false);
       
    }
}
