using UnityEngine;
using UnityEditor;
using System.Collections;

public class folder_boss : Boss {

    public GameObject[] Cannons;
    bool LcanDir, RcanDir;
    public GameObject headTop;
    public SpriteRenderer[] sprites;
    
    //2 modes, open mouth and closed
    //When mouth is open, stretch a bit on the Y axis
    //Fire LOADS of infected files outwards


	protected override void Awake ()
    {
        base.Awake();
        //base.OnSpawn(); //Mb remove this line when we're done testing
    }

    public  override void OnSpawn()
    {
        base.OnSpawn();
        transform.position= Vector3.zero;
    }

	protected override void Update ()
    {
        //Move cannons
        #region cannon moving
        if (LcanDir)        
            CannonRot(0, Cannons[0].transform.localRotation.eulerAngles.z + (Time.fixedDeltaTime*50));        
        else
            CannonRot(0, Cannons[0].transform.localRotation.eulerAngles.z - (Time.fixedDeltaTime*50));

        if ((Cannons[0].transform.localRotation.eulerAngles.z < 330 && Cannons[0].transform.localRotation.eulerAngles.z > 300 )|| (Cannons[0].transform.localRotation.eulerAngles.z > 30 && Cannons[0].transform.localRotation.eulerAngles.z < 50))
        {
            LcanDir = !LcanDir;
        }

        if (!RcanDir)        
            CannonRot(1, Cannons[1].transform.localRotation.eulerAngles.z + (Time.fixedDeltaTime * 50));        
        else
            CannonRot(1, Cannons[1].transform.localRotation.eulerAngles.z - (Time.fixedDeltaTime * 50));

        if ((Cannons[1].transform.localRotation.eulerAngles.z < 330 && Cannons[1].transform.localRotation.eulerAngles.z > 300) || (Cannons[1].transform.localRotation.eulerAngles.z > 30 && Cannons[1].transform.localRotation.eulerAngles.z < 50))
        {
            RcanDir = !RcanDir;
        }
        #endregion
        base.Update();
    }
    public override void TakeDamage(float _damage)
    {
        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = Color.red;
        }
        Invoke("revertSpriteColours", .1f);
        base.TakeDamage(_damage);
    }

    void revertSpriteColours()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = Color.white;
        }
    }
    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (!bossDefeated)
        {
            if (shootCooldown >= shootRate)
            {
                for (int i = 0; i < _shootTransform.Length; i++)
                {
                        CameraShake.instance.shakeAmount = 0.35f;
                    if (CameraShake.instance.shakeDuration < 0.2f)
                    {
                        CameraShake.instance.shakeDuration = 0.2f;
                    }
                        Vector3 shootDir = _shootTransform[i].transform.position - transform.position;
                        
                    Projectile p = ProjectileManager.instance.PoolingEnemyProjectile(_shootTransform[i].transform);
                    p.SetProjectile(_projData, shootDir,false);
                    p.transform.position = _shootTransform[i].transform.position;
                    p.gameObject.SetActive(true);
                    shootCooldown = 0;
                }
                return true;
            }
        }
        return false;
    }

    public IEnumerator mouthOpenClose(bool open)
    {
        float lerpy = 0;
        while (lerpy < 1)
        {
            Vector3 v = Vector3.Lerp(headTop.transform.rotation.eulerAngles, new Vector3((open) ? 55 : 0, 0, 0), lerpy);
            headTop.transform.rotation = Quaternion.Euler(v);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2, (open) ? 2.25f : 2, 2), lerpy);
            lerpy += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void mouth(bool dir)
    {
        if (dir)
            headTop.transform.rotation = Quaternion.Euler(new Vector3(55, 0, 0));
        else
        headTop.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void CannonRot(int cannonIndex, float desiredRotation)
    {
        //desiredRotation = Mathf.Clamp(desiredRotation, -30, 30);
        Cannons[cannonIndex].transform.rotation = Quaternion.Euler(new Vector3(0, 0, desiredRotation));
    }

}

[CustomEditor(typeof(folder_boss))]
public class folderTester : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        folder_boss myScript = (folder_boss)target;
        if (GUILayout.Button("Open mouth"))
        {
            myScript.StartCoroutine(myScript.mouthOpenClose(true));
        }
        if (GUILayout.Button("Close mouth"))
        {
            myScript.StartCoroutine(myScript.mouthOpenClose(false));
        }

        if (GUILayout.Button("R cannon UP"))
        {
            myScript.CannonRot(1, 30);
        }
        if (GUILayout.Button("R cannon DOWN"))
        {
            myScript.CannonRot(1, -30);
        }
        if (GUILayout.Button("L cannon UP"))
        {
            myScript.CannonRot(0, 30);
        }
        if (GUILayout.Button("L cannon DOWN"))
        {
            myScript.CannonRot(0, -30);
        }

    }
}