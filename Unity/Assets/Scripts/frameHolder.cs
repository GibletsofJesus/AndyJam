using UnityEngine;
using System.Collections;

//Create tiny pauses for certain events
//i.e. clashing of beaks, defeat of geese etc.
public class frameHolder : MonoBehaviour {

    public static frameHolder instance;

    public bool enabled = true;

    public void holdFrame(float time)
    {
        if (enabled)
        {
            holdDuration += time;
            if (Time.timeScale > 0)
                StartCoroutine(hold());
        }
    }

    bool running;
    float holdDuration;
    IEnumerator hold()
    {
        if (enabled)
        {
            Time.timeScale = 0.1f;

            while (holdDuration > 0)
            {
                holdDuration -= Time.fixedDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        Time.timeScale = 1;
    }

    // Use this for initialization
    void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}