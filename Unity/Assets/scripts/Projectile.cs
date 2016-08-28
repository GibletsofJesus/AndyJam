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
    Vector3 screenBottom,screenTop;


    // Use this for initialization
    protected virtual void Start()
    {
		aoe.deactivateProj = DeactivateProj;
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f ,-0.1f, .3f));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(.7f, 1.15f, .3f));
    }
  
    void Alive()
    {
		aliveCooldown += Time.deltaTime;
		if (aliveCooldown >= projData.aliveTime)
        {
            DeactivateProj();
        }
    }

    public virtual void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay || GameStateManager.instance.state == GameStateManager.GameState.GameOver)
        {
            GameObject _target = finder.GetClosestObject();
            if (projData.homingBullets && _target && aliveCooldown < .75f)
            {
                //Turn z axis
                Quaternion rot = new Quaternion();
                float z = Mathf.Atan2((_target.transform.position.x - transform.position.x), (_target.transform.position.y - transform.position.y)) * Mathf.Rad2Deg;
                rot.eulerAngles = new Vector3(0, 0, -z);

                if (projData.owner.gameObject.tag == "Enemy")
                {
                    if (aliveCooldown < .35f)
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot, (1 / projData.aliveTime) * Time.deltaTime * 200);
                }
                else
                    transform.rotation = Quaternion.Lerp(transform.rotation, rot, (1 / projData.aliveTime) * Time.deltaTime * 200);

                rig.velocity = transform.up * projData.speed;
            }
            else
            {
                rig.velocity = transform.up * projData.speed;
            }

            Alive();
            OffScreen();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag != projData.parentTag)
		{
			if(col.gameObject.GetComponent<Actor>() && transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
			{
				if(projData.explodingBullets)
				{
                    col.gameObject.GetComponent<Actor>().TakeDamage(projData.projDamage);
                    aoe.ActivateExplosion("Enemy", projData.explosionDamage, projData.explosionRadius);

					//I hate myself
					int radius = 2;
					if(projData.explosionRadius < 3)
					{
						radius = 0;
					}
					else if(projData.explosionRadius < 5)
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
                    //if (col.GetComponent<Actor>() != projData.owner)
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
        else if (transform.position.x > screenTop.x || transform.position.x < screenBottom.x)
        {
            DeactivateProj();
        }
    }

    void DeactivateProj()
    {
        //target = null;
		finder.Reset ();
		aliveCooldown = 0.0f;
        gameObject.SetActive(false);
        enabled = false;        
    }

    public void SetProjectile(ProjectileData _data, Vector2 _direction,bool enemy = true)
    {
		projData = _data;
        spriteRenderer.sprite = projData.projSprite;

        if (_data.homingBullets)
        {
            finder.ActivateFinder((enemy) ? "Enemy" : "Player", _data.homingRadius);
        }
        direction = _direction;
        transform.up = _direction;
    }
}
