using UnityEngine;
using System.Collections;

public class playerMovement : Actor {

    Rigidbody2D rig;
    public float moveSpeed;

    public AudioClip[] shootSounds;

    Vector3 rotLerp;

	// Use this for initialization
	void Start ()
    {
        rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    void Update()
    {
        inputThings();

        Quaternion q = Quaternion.Euler(rotLerp);
        float angle=0;
        
        if (!rolling)
            angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotLerp.y, Time.deltaTime * 5);

        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    bool rolling;
    IEnumerator doABarrrelRoll(int dir)
    {
        rolling = true;
        Vector3 initialRot = transform.rotation.eulerAngles;
        Vector3 goalRot = rotLerp;
        float t=0;

        Vector3 currentRot = initialRot;

        while (t < .5f)
        {
            currentRot.y = Mathf.Lerp(initialRot.y, rotLerp.y + ((dir > 0) ? 360 : -90), t / .5f);
            transform.rotation = Quaternion.Euler(currentRot);
            t += Time.deltaTime;
            yield return null;
        }

        rolling = false;
    }

    void inputThings()
    {
        #region move
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!rolling)
            {
                if (transform.rotation.eulerAngles.y > 314 && transform.rotation.eulerAngles.y < 317)
                    StartCoroutine(doABarrrelRoll(-1));

                rotLerp = new Vector3(0, 45, 0);
            }
            rig.AddForce(-Vector2.right * moveSpeed, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!rolling)
            {
                if (transform.rotation.eulerAngles.y > 42 && transform.rotation.eulerAngles.y < 45)
                    StartCoroutine(doABarrrelRoll(1));
                rotLerp = new Vector3(0, -45, 0);
            }
            
            rig.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rotLerp = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rig.AddForce(-Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            soundManager.instance.playSound(shootSounds[Random.Range(0, shootSounds.Length - 1)]);

            if (CameraShake.instance.shakeDuration < 0.2f)
                CameraShake.instance.shakeDuration += 0.2f;
        }
    }
}
