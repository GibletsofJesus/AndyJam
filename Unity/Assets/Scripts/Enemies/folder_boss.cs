using UnityEngine;
using UnityEditor;
using System.Collections;

public class folder_boss : MonoBehaviour {

    public GameObject[] Cannons;
    public GameObject headTop;
    public Animator headAnimator;
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void mouth(bool dir)
    {
        if (dir)
        headTop.transform.rotation = Quaternion.Euler(new Vector3(55, 0, 0));
        else
        headTop.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    //public void CannonRot()

}

[CustomEditor(typeof(folder_boss))]
public class folderTester : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        folder_boss myScript = (folder_boss)target;
        if (GUILayout.Button("Open mouth"))
        {
            myScript.mouth(true);
        }
        if (GUILayout.Button("Close mouth"))
        {
            myScript.mouth(false);
        }
    }
}