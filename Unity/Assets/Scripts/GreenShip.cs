using UnityEngine;
using System.Collections;

public class GreenShip : MonoBehaviour 
{
    public static GreenShip instance;

    public Sprite ship;
	// Use this for initialization
	void Awake () 
    {
	    if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
	}
}
