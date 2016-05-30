using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    int damage;
    float maxAlive;
    float aliveTime = 1f;
    float speed;
    Vector2 direction;
    Rigidbody2D body;
    string ignoreActor;
    public TrailRenderer trail;
    public ParticleSystem ps;
    Vector3 screenBottom,screenTop;
    bool homing;
    GameObject target;

    // Use this for initialization
    public virtual void Start()
    {
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f ,-.5f, .3f));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 1.5f, .3f));
        body = GetComponent<Rigidbody2D>();
    }
  
    void Alive()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        {
            DeactivateProj();
        }

        if (transform.position != Vector3.zero)
            ps.Play();
    }

    public virtual void Update()
    {
        if (homing && target)
        {
            if (target.activeSelf)
            {
                //Turn z axis
                Quaternion rot = new Quaternion();
                float z = Mathf.Atan2((target.transform.position.x - transform.position.x), (target.transform.position.y - transform.position.y)) * Mathf.Rad2Deg;
                rot.eulerAngles = new Vector3(0, 0, z);

                rot.eulerAngles = new Vector3(0, 0, -z);

                transform.rotation = Quaternion.Lerp(transform.rotation, rot, (1 / aliveTime) * Time.deltaTime * 5);
                body.velocity = transform.up * speed;
            }
            else
                body.velocity = transform.up * speed;
        }
        else
        {
            body.velocity = transform.up * speed;
                //direction * (speed);// * Time.deltaTime);
        }

        Alive();
        OffScreen();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != ignoreActor && col.gameObject.tag != "TargetFinder")
        {
            if (col.gameObject.GetComponent<Actor>())
            {
                Debug.Log("Projectile hit " +col.gameObject.name);
                col.gameObject.GetComponent<Actor>().TakeDamage(damage);
                DeactivateProj();
            }
        }
    }

    void OffScreen()
    {
        if (transform.position.y > screenTop.y || transform.position.y < screenBottom.y)
        {
            DeactivateProj();
        }
    }

    void DeactivateProj()
    {
        target = null;
        trail.time = 0.00001f;
        trail.enabled = false;
        aliveTime = maxAlive;
        gameObject.SetActive(false);
        gameObject.GetComponent<Projectile>().enabled = false;        
    }

    public void SetProjectile(int _damage, Vector2 _direction,string _ignoreActor,bool _homing,Actor _owner, float _speed = 50
        , float _aliveTime = 2, float trailRendTime = .05f)
    {
        homing = _homing;
        if (_owner.GetComponent<playerMovement>())
            target = _owner.GetComponent<playerMovement>().target;;
        trail.probeAnchor = transform;
        damage = _damage;
        direction = _direction;
        maxAlive = _aliveTime;
        speed = _speed;
        transform.up = _direction;
        ignoreActor = _ignoreActor;

        trail.enabled = false;
        trail.time = trailRendTime;
    }
}
