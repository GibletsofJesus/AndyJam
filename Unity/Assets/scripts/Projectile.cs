using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ProjectileData
{
	public Sprite projSprite;
	[HideInInspector] public float projDamage;
	public float defaultProjDamage;
	public float aliveTime;
	public float speed;
	public bool homingBullets;
	public float homingRadius;
	public bool explodingBullets;
	public float explosionRadius;
	public float explosionDamage;
	[HideInInspector] public Actor owner;
	[HideInInspector] public string parentTag;
}

public class Projectile : MonoBehaviour
{
	private ProjectileData projData = new ProjectileData ();

	[SerializeField] private Rigidbody2D rig = null;
	[SerializeField] private SpriteRenderer spriteRenderer = null;
	[SerializeField] private RadiusObjectFinder finder = null;
	[SerializeField] private ExplosionAoe aoe = null;
   
	float aliveCooldown = 0.0f;
    Vector2 direction; 
    public TrailRenderer trail;
    public ParticleSystem ps;
    Vector3 screenBottom,screenTop;


    // Use this for initialization
    protected virtual void Start()
    {
		aoe.deactivateProj = DeactivateProj;
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f ,-.5f, .3f));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 1.5f, .3f));
    }
  
    void Alive()
    {
		aliveCooldown += Time.deltaTime;
		if (aliveCooldown >= projData.aliveTime)
        {
            DeactivateProj();
        }


        if (transform.position != Vector3.zero)
        {
         //   ps.Play();
        }

    }

    public virtual void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            GameObject _target = finder.GetClosestObject();
            if (projData.homingBullets && _target)
            {
                //if (target.activeSelf)
                //{
                //Turn z axis
                Quaternion rot = new Quaternion();
                float z = Mathf.Atan2((_target.transform.position.x - transform.position.x), (_target.transform.position.y - transform.position.y)) * Mathf.Rad2Deg;
                rot.eulerAngles = new Vector3(0, 0, z);

                rot.eulerAngles = new Vector3(0, 0, -z);

                transform.rotation = Quaternion.Lerp(transform.rotation, rot, (1 / projData.aliveTime) * Time.deltaTime * 200);
                rig.velocity = transform.up * projData.speed;
                //}
                //else
                //	rig.velocity = transform.up * projData.speed;
            }
            else
            {
                rig.velocity = transform.up * projData.speed;
                //direction * (speed);// * Time.deltaTime);
            }

            Alive();
            OffScreen();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        /*if (col.gameObject.tag != ignoreActor && col.gameObject.tag != "TargetFinder")
        {
            if (col.gameObject.GetComponent<Actor>())
            {
                Debug.Log("Projectile hit " +col.gameObject.name);
                col.gameObject.GetComponent<Actor>().TakeDamage(damage);
                DeactivateProj();
            }
        }*/

		if (col.gameObject.tag != projData.parentTag)
		{
			if(col.gameObject.GetComponent<Actor>())
			{
				if(projData.explodingBullets)
				{
					aoe.ActivateExplosion("Enemy", projData.explosionDamage, projData.explosionRadius);

					//I hate myself
					int radius = 2;
					if(projData.explosionRadius < 2)
					{
						radius = 0;
					}
					else if(projData.explosionRadius < 3)
					{
						radius = 1;
					}

					Explosion ex = ExplosionManager.instance.PoolingExplosion(aoe.transform, radius);
					ex.transform.position = transform.position;
					ex.gameObject.SetActive(true);
					ex.explode();
				}
				else
				{
					col.gameObject.GetComponent<Actor>().TakeDamage(projData.projDamage);
					DeactivateProj();
				}
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
        //target = null;
		finder.Reset ();
        trail.time = 0.00001f;
        trail.enabled = false;
		aliveCooldown = 0.0f;
        gameObject.SetActive(false);
        enabled = false;        
    }

    public void SetProjectile(ProjectileData _data, Vector2 _direction, float trailRendTime = 0.05f)
    {
		projData = _data;
		spriteRenderer.sprite = projData.projSprite;
		
		if(_data.homingBullets)
		{
			finder.ActivateFinder("Enemy", _data.homingRadius);
		}
        //if (_owner.GetComponent<playerMovement>())
        //    target = _owner.GetComponent<playerMovement>().target;;
        trail.probeAnchor = transform;
        //damage = _damage;
        direction = _direction;
        //maxAlive = _aliveTime;
        //speed = _speed;
        transform.up = _direction;
        //ignoreActor = _ignoreActor;

        trail.enabled = false;
        trail.time = trailRendTime;
    }
}
