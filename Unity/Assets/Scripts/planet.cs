using UnityEngine;
using System.Collections;

public class planet : MonoBehaviour {
    Vector3 killPos;
    float speed;

    // Use this for initialization
    void Start()
    {
        speed = Random.Range(1, 10);
        killPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, -0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y-(Time.deltaTime*speed), transform.position.z);
        if (transform.position.y < killPos.y)
        {
            planetMaker.instance.RemovePlanetFromList(gameObject);
            Destroy(gameObject);
        }
    }
}
