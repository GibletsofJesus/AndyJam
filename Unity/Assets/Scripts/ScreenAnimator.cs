using UnityEngine;
using System.Collections;

public class ScreenAnimator : MonoBehaviour {

    CameraScreenGrab screenEffect;

    void Start()
    {
        screenEffect = Camera.main.GetComponent<CameraScreenGrab>();
        once = true;
    }
    bool once;
    void FixedUpdate()
    {
        if (once)
        {
            if (screenEffect.pixelSize > 4)
            {
                screenEffect.pixelSize = (int)Mathf.Lerp(screenEffect.pixelSize, 1, Time.fixedDeltaTime);
            }
            else
            {
                once = false;
            }
        }
    }

	public void SetPixelSize(int f)
    {
        Camera.main.GetComponent<CameraScreenGrab>().pixelSize = f;
    }
}
