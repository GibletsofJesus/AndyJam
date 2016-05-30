using UnityEngine;
using System.Collections;

public class splashScreen : MonoBehaviour {

    public Animator anim;
    public SpriteRenderer ship;
    public Sprite ShipA, ShipB;
    public bool allowStart;
    public AudioSource swapShipSound;
    public AudioClip[] swapSounds;
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Return) && allowStart)
        {
            anim.StopPlayback();
            anim.Play("splash_out");
            allowStart = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            swapShipSound.PlayOneShot(swapSounds[(ship.sprite == ShipA) ? 0 : 1]);
            if (ship.sprite == ShipA)
            {
                ship.sprite =ShipB;
            }
            else
            {
                ship.sprite = ShipA;
            }
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
