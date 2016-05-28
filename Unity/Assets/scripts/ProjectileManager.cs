using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour 
{
    public static ProjectileManager instance = null;
    public GameObject projectile;
    List<Projectile> projList = new List<Projectile>();
   
    // Use this for initialization
	void Awake ()
    {
	    if (instance == null)
        {
            instance = this;
        }
	}
	public Projectile PoolingProjectile(Transform t)
    {
        for (int i=0;i<projList.Count;i++)
        {
            if (!projList[i].isActiveAndEnabled)
            {
                projList[i].enabled = true;
                return projList[i];
            }
        }

        GameObject newProj = Instantiate(projectile,t.position,t.rotation) as GameObject;
        newProj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Projectile p =newProj.GetComponent<Projectile>();
        projList.Add(p);
        return p;
    }
}
