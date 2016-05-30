using UnityEngine;
using System.Collections;

public class splashScreen : MonoBehaviour {

    public Animator anim;
    public bool allowStart;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Return) && allowStart)
        {
            anim.StopPlayback();
            anim.Play("splash_out");
            allowStart = false;
        }
	}

    public void letPlayerStart()
    {
        allowStart = true;
    }

    public void loadLevel()
    {
        Application.LoadLevel(1);
    }
}
