using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour 
{
    public static ProjectileManager instance = null;
    public Projectile projectile;
    List<Projectile> projList = new List<Projectile>();
   
    // Use this for initialization
	void Awake ()
    {
	    if (instance == null)
        {
            instance = this;
        }
	}
	public Projectile PoolingProjectile()
    {
        Debug.Log(projList.Count);
        for (int i=0;i<projList.Count;i++)
        {
            if (!projList[i].isActiveAndEnabled)
            {
                projList[i].enabled = true;
                return projList[i];
            }
        }

        Projectile p = Instantiate(projectile);
        projList.Add(p);
        return p;
    }
}
