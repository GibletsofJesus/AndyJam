using UnityEngine;
using System.Collections;

//Yes, this may seem like a waste, if we used interfaces this would have been nicer
public class FirewallObject : Actor
{
    //Gradually rotate firewall
    protected override void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
    //Do not perform y rotation of firewall
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z);
    }
}
