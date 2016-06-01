using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionManager : MonoBehaviour {

    public static ExplosionManager instance;
    public GameObject explosionPrefab;
    List<Explosion> exploList = new List<Explosion>();
    
    void Awake()
    {
        instance = this;
	}

    public Explosion PoolingExplosion(Transform t)
    {
        for (int i = 0; i < exploList.Count; i++)
        {
            if (!exploList[i].isActiveAndEnabled)
            {
                exploList[i].enabled = true;
                return exploList[i];
            }
        }

        GameObject newObj = Instantiate(explosionPrefab, t.position, t.rotation) as GameObject;
        newObj.transform.parent = transform.parent;
        //newProj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Explosion p = newObj.GetComponent<Explosion>();
        exploList.Add(p);
        return p;
    }
}