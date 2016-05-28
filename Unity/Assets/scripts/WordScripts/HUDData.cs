using UnityEngine;
using System.Collections;

public class HUDData : MonoBehaviour 
{
	private static HUDData staticInstance = null;
	public static HUDData instance {get {return staticInstance;} set{}}

	public Sprite deactivateBackground = null;
	public Sprite activateBackground = null;

	public Sprite deactivateCooldown = null;
	public Sprite activateCooldown = null;

	public Color deactivateColour = Color.white;
	public Color activateColour = Color.white;
	
	private void Start () 
	{
		staticInstance = this;
	}
}
