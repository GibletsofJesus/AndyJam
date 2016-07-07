using UnityEngine;
using System.Collections;

public class folderboss_animator : MonoBehaviour {

    public void shake()
    {
        CameraShake.instance.shakeDuration = 1;
        CameraShake.instance.shakeAmount = 1;
    }
}
