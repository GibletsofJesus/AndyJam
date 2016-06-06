using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 10.0f;
    
    private void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
        }
    }
 
    private void LateUpdate()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z);
        }
    }
}
