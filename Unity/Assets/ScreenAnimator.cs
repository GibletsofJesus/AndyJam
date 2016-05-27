using UnityEngine;
using System.Collections;

public class ScreenAnimator : MonoBehaviour {

    CameraScreenGrab screenEffect;

    void Start()
    {
        screenEffect = Camera.main.GetComponent<CameraScreenGrab>();
    }

    void FixedUpdate()
    {
        if (screenEffect.pixelSize > 1)
        {
            screenEffect.pixelSize = (int)Mathf.Lerp(screenEffect.pixelSize, 1, Time.fixedDeltaTime);
        }
    }

	public void SetPixelSize(int f)
    {
        Camera.main.GetComponent<CameraScreenGrab>().pixelSize = f;
    }
}
