using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

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
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(rotLerp), Time.deltaTime * 5);
        //If difference in y rotation is voer 60 degrees 
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
