using UnityEngine;
using System.Collections;

public class playerMovement : Actor {

    public GameObject parent;

    Rigidbody2D rig;
    public float moveSpeed;

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

        //If difference in y rotation is voer 60 degrees 
        if (transform.rotation.eulerAngles.y < 40 && rotLerp.y == -45)
        {
            StartCoroutine(doABarrrelRoll(1));
        }
        else if (transform.rotation.eulerAngles.y > 315 && rotLerp.y == 45)
        {
            StartCoroutine(doABarrrelRoll(-1));
        }

        if (!rolling)
            angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotLerp.y, Time.deltaTime * 5);


        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
    bool rolling;
    IEnumerator doABarrrelRoll(int dir)
    {
        rolling = true;
        Vector3 initialRot = parent.transform.rotation.eulerAngles;
        Vector3 goalRot = rotLerp;
        float t=0;
        if (dir > 0)
        {
            goalRot.y += 360;
        }
        else
        {
            goalRot.y -= 360;
        }

        Vector3 currentRot = initialRot;

        while (t < .5f)
        {
            currentRot.y = Mathf.Lerp(initialRot.y, goalRot.y, t / .5f);
            transform.rotation= Quaternion.Euler(currentRot);
            t += Time.deltaTime;
            yield return null;
        }

        rolling = false;
        //parent.transform.Rotate(Vector3.left);
    }

    void inputThings()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rig.AddForce(-Vector2.right * moveSpeed, ForceMode2D.Impulse);
            rotLerp = new Vector3(0, 45,0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rig.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
            rotLerp = new Vector3(0,-45,0);
        }
        else
        {
            rotLerp = Vector3.zero ;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rig.AddForce(-Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
    }
}
