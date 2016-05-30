using UnityEngine;
using System.Collections;
using UnityEditor;

public class Explosion : MonoBehaviour
{
    public ParticleSystem[] particles;
    public void explode()
    {
        foreach (ParticleSystem p in particles)
        {
            p.Play();
        }
    }
}

[CustomEditor(typeof(Explosion))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Explosion obj = (Explosion)target;
        if (GUILayout.Button("Explode"))
        {
            obj.explode();
        }
    }
}