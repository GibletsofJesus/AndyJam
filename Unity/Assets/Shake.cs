using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shake : MonoBehaviour {

    public Text txt;
    public float shift;
	// Update is called once per frame
	void Update () {
        txt.rectTransform.anchoredPosition = new Vector2(Mathf.Sin(Random.value) * shift, Mathf.Sin(Random.value) * shift);
    }
}
