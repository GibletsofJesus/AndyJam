using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour 
{
    public static ProjectileManager instance = null;
    public GameObject projectile;
    List<Projectile> projList = new List<Projectile>();

    public GameObject enemyProjectile;
    List<Projectile> enemyProjList = new List<Projectile>();

    public GameObject infectedFile;
    List<Projectile> infectedFileList = new List<Projectile>();

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
        for (int i = 0; i < projList.Count; i++)
        {
            if (!projList[i].isActiveAndEnabled)
            {
                projList[i].enabled = true;
                return projList[i];
            }
        }

        GameObject newProj = Instantiate(projectile, t.position, t.rotation) as GameObject;
        newProj.transform.parent = transform;
        //newProj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Projectile p = newProj.GetComponent<Projectile>();
        projList.Add(p);
        return p;
    }

    public Projectile PoolingEnemyProjectile(Transform t)
    {
        for (int i = 0; i < enemyProjList.Count; i++)
        {
            if (!enemyProjList[i].isActiveAndEnabled)
            {
                enemyProjList[i].enabled = true;
                return enemyProjList[i];
            }
        }

        GameObject newProj = Instantiate(enemyProjectile, t.position, t.rotation) as GameObject;
        newProj.transform.parent = transform;
        //newProj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Projectile p = newProj.GetComponent<Projectile>();
        enemyProjList.Add(p);
        return p;
    }

    public Projectile PoolingInfectedFile(Transform t)
    {
        for (int i = 0; i < infectedFileList.Count; i++)
        {
            if (!infectedFileList[i].isActiveAndEnabled)
            {
                infectedFileList[i].enabled = true;
                return infectedFileList[i];
            }
        }

        GameObject newProj = Instantiate(infectedFile, t.position, t.rotation) as GameObject;
        newProj.transform.parent = transform;
        //newProj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Projectile p = newProj.GetComponent<Projectile>();
        infectedFileList.Add(p);
        return p;
    }
}
