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

            if (CameraShake.instance.shakeDuration < 0.2f)
            {
                CameraShake.instance.shakeDuration = 0.2f;
                CameraShake.instance.shakeAmount = 0.5f;
            }
        }
        Invoke("turnOff", 1f);
    }

    void turnOff() { 
        gameObject.SetActive(false);}
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