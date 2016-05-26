using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

    Rigidbody2D rig;

	// Use this for initialization
	void Start ()
    {
        rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    void Update()
    {
        inputThings();
	}

    void inputThings()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rig.AddForce(
            }

    }
}
