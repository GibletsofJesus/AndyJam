using UnityEngine;
using System.Collections;

public class Enemy_Adware : Enemy
{

	// Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
        base.Update();
	}

    protected override void Movement()
    {
        Vector2 movement = -transform.up * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            _col.gameObject.GetComponent<Actor>().TakeDamage(contactHitDamage);
            Player.instance.advertAttack = true;
            Death();
        }
    }

    IEnumerator AllTheAdds()
    {
        Death();
      
            AdManager.instance.TryGenerateAd(new Vector3(25, 25, 0));
            yield return new WaitForSeconds(Random.Range(2, 5));
           
  
    }
}
