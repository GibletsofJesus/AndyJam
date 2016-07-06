using UnityEngine;
using System.Collections;

public class physicsForcer : MonoBehaviour {

    bool once;
	void Update()
    {
        if (gameObject.activeInHierarchy && !once)
        {
            once = true;
            GetComponent<Rigidbody2D>().AddForceAtPosition(transform.position, Vector2.zero, ForceMode2D.Impulse);
        }
    }
}
