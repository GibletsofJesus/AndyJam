using UnityEngine;
//using UnityEditor;
using System.Collections;

public class folder_boss : Boss {

    public GameObject[] Cannons;
    bool LcanDir, RcanDir;
    public GameObject headTop;
    public SpriteRenderer[] sprites;
    public AudioClip[] soundFx,cannonSounds;
    public float mouthCooldownMax=15;

    Color revertCol=Color.white;
    [SerializeField]
    private ProjectileData InfectedFileStats;
    public ParticleSystem vomitParticleSystem,drool;
    float mouthCD;
    int lastCannonSound=0;
    //2 modes, open mouth and closed
    //When mouth is open, stretch a bit on the Y axis
    //Fire LOADS of infected files outwards
    
	protected override void Awake ()
    {          
        base.Awake();
        shootCooldowns[0] = shootRate/2;
        shootCooldowns[1] = shootCooldown;
        InfectedFileStats.projDamage = projData.defaultProjDamage;
        InfectedFileStats.owner = this;
        InfectedFileStats.parentTag = tag;
    }

    public  override void OnSpawn()
    {
        base.OnSpawn();
        transform.position= new Vector3(0, 11.75f, 0);

        soundManager.instance.music.enabled = false;
    }

	protected override void Update ()
    {
        shootRate = 1.5f -(1f-(health/defaultHealth));

        revertCol = Color.Lerp(Color.white,Color.red,.6f*(1-health/defaultHealth));

        if (!bossDefeated)
        {
            //Move cannons
            #region cannon moving
            if (LcanDir)
                CannonRot(0, Cannons[0].transform.localRotation.eulerAngles.z + (Time.fixedDeltaTime * 50));
            else
                CannonRot(0, Cannons[0].transform.localRotation.eulerAngles.z - (Time.fixedDeltaTime * 50));

            if ((Cannons[0].transform.localRotation.eulerAngles.z < 330 && Cannons[0].transform.localRotation.eulerAngles.z > 300) || (Cannons[0].transform.localRotation.eulerAngles.z > 30 && Cannons[0].transform.localRotation.eulerAngles.z < 50))
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
            CannonsCoolDown();
            MouthCoolDown();

            if (mouthCD >= mouthCooldownMax)
            {
                StartCoroutine(mouthFire());
                mouthCD = 0;
            }
        }
    }

    IEnumerator mouthFire()
    {
        soundManager.instance.playSound(soundFx[Random.Range(0,1)]);
        yield return StartCoroutine(mouthOpenClose(true));
        vomitParticleSystem.Play();

        float filesFired = 0;

        CameraShake.instance.shakeDuration = 5;
        CameraShake.instance.shakeAmount = 1;
        while (filesFired < 50)
        {
            shootFile();
            soundManager.instance.playSound(Player.instance.shootSounds[Random.Range(0, 3)],1.5f);
            yield return new WaitForSeconds(0.1f);
            filesFired++;
            if (bossDefeated)
                break;
        }

        vomitParticleSystem.Stop();
        yield return StartCoroutine(mouthOpenClose(false));
        mouthCD = 0;
    }

    void MouthCoolDown()
    {
        mouthCD= (mouthCD + Time.deltaTime) > mouthCooldownMax ? mouthCooldownMax : (mouthCD + Time.deltaTime);
    }

    void CannonsCoolDown()
    {
        shootCooldowns[0] = (shootCooldowns[0] + Time.deltaTime) > shootRate ? shootRate : (shootCooldowns[0] + Time.deltaTime);
        shootCooldowns[1] = (shootCooldowns[1] + Time.deltaTime) > shootRate ? shootRate : (shootCooldowns[1] + Time.deltaTime);
    }

    public override void TakeDamage(float _damage)
    {
        if (IsInvoking("revertSpriteColours"))
        {
            CancelInvoke("revertSpriteColours");
        }
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
            sr.color = revertCol;
        }
    }

    float[] shootCooldowns = new float[2];

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform, bool b = false)
    {
        if (!bossDefeated)
        {
            for (int i = 0; i < _shootTransform.Length; i++)
            {
                if (shootCooldowns[i] >= shootRate)
                {
                    lastCannonSound += (Random.value > .5f) ? 1 : -1;
                    if (lastCannonSound < 0)
                        lastCannonSound = cannonSounds.Length - 1;
                    else if (lastCannonSound > cannonSounds.Length - 1)
                        lastCannonSound = 0;

                    soundManager.instance.playSound(cannonSounds[lastCannonSound]);

                    CameraShake.instance.shakeAmount = 0.35f;
                    if (CameraShake.instance.shakeDuration < 0.2f)
                    {
                        CameraShake.instance.shakeDuration = 0.2f;
                    }
                    Vector3 shootDir = _shootTransform[i].transform.position - transform.position;

                    Projectile p = ProjectileManager.instance.PoolingEnemyProjectile(_shootTransform[i].transform);
                    p.SetProjectile(_projData, shootDir, false);
                    p.transform.position = _shootTransform[i].transform.position;
                    p.gameObject.SetActive(true);
                    shootCooldowns[i] = 0;
                    return true;
                }
            }
        }
        return false;
    }

    public void shootFile()
    {
        Vector3 randomDir = new Vector3(Random.Range(-6.5f,6.5f),-10, 0);
        Projectile p = ProjectileManager.instance.PoolingInfectedFile(vomitParticleSystem.transform);
        p.SetProjectile(InfectedFileStats, randomDir, false);
        p.transform.position = vomitParticleSystem.transform.position;
        p.gameObject.SetActive(true);
    }

    public bool mouthInMotion;
    public IEnumerator mouthOpenClose(bool open)
    {
        mouthInMotion = true;
        float lerpy = 0;
        while (lerpy < 1)
        {
            Vector3 v = Vector3.Lerp(headTop.transform.rotation.eulerAngles, new Vector3((open) ? 55 : 0, 0, 0), lerpy);
            headTop.transform.rotation = Quaternion.Euler(v);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, (open) ? 1.125f : 1, 1), lerpy);
            lerpy += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mouthInMotion = false;
    }
    public void CannonRot(int cannonIndex, float desiredRotation)
    {
        //desiredRotation = Mathf.Clamp(desiredRotation, -30, 30);
        Cannons[cannonIndex].transform.rotation = Quaternion.Euler(new Vector3(0, 0, desiredRotation));
    }

    protected override  IEnumerator bossDeath()
    {
        drool.Stop();
        drool.gameObject.SetActive(false);
        Explosion ex;

        foreach (GameObject go in base.shootTransform)
        {
            if (Random.value > .5f)
                ex = ExplosionManager.instance.PoolingExplosion(go.transform, 0);
            else
                ex = ExplosionManager.instance.PoolingExplosion(go.transform, 1);

            ex.transform.position = go.transform.position;
            ex.gameObject.SetActive(true);
            ex.explode();
            yield return new WaitForSeconds(Random.Range(0.4f, .8f));
        }
        soundManager.instance.playSound(soundFx[2], 2);
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("boss_folder_death");

        yield return new WaitForSeconds(2.5f);
        GetComponent<Animator>().enabled = false;

        //EnemyManager.instance.NextLevel();
        StartCoroutine(base.bossDeath());
    }

}

//[CustomEditor(typeof(folder_boss))]
//public class folderTester : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        folder_boss myScript = (folder_boss)target;
//        if (GUILayout.Button("Open mouth"))
//        {
//            if (!myScript.mouthInMotion)
//            myScript.StartCoroutine(myScript.mouthOpenClose(true));
//        }
//        if (GUILayout.Button("Close mouth"))
//        {
//            if (!myScript.mouthInMotion)
//                myScript.StartCoroutine(myScript.mouthOpenClose(false));
//        }
//        if (GUILayout.Button("Fire File"))
//        {
//            if (!myScript.mouthInMotion)
//                myScript.shootFile();
//        }
//    }
//}