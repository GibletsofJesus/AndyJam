using UnityEngine;
using System.Collections;


//Attach this to a camera
public class CameraScreenGrab : MonoBehaviour {
		
	//how chunky to make the screen
	public int pixelSize = 4;
    int oldPixelSize;
	public FilterMode filterMode = FilterMode.Point;
	public Camera[] otherCameras;
	public Material mat;
    public Canvas canvas;
    Texture2D tex;
	
	void Start ()
    {
        oldPixelSize = pixelSize;
        GetComponent<Camera>().pixelRect = new Rect(0, 0, Screen.width / pixelSize, Screen.height / pixelSize);
        for (int i = 0; i < otherCameras.Length; i++)
            otherCameras[i].pixelRect = new Rect(0, 0, Screen.width / pixelSize, Screen.height / pixelSize);
    }

    void Update()
    {
        if (oldPixelSize != pixelSize && pixelSize > 0)
        {
            GetComponent<Camera>().pixelRect = new Rect(0, 0, Screen.width / pixelSize, Screen.height / pixelSize);
            for (int i = 0; i < otherCameras.Length; i++)
                otherCameras[i].pixelRect = new Rect(0, 0, Screen.width / pixelSize, Screen.height / pixelSize);
            oldPixelSize = pixelSize;
        }
    }

    public void setPixelScale(int i)
    {
        pixelSize = i;
        oldPixelSize = i;
    }

	void OnGUI()
	{
        if (pixelSize > 1)
        {
            if (Event.current.type == EventType.Repaint)
                Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
        }
        else
        {
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    public void SwitchMode(bool retroMode)
    {

    }

    void OnPostRender()
    {
        if (pixelSize > 1)
        {
            // Draw a quad over the whole screen with the above shader
            GL.PushMatrix();
            GL.LoadOrtho();
            for (var i = 0; i < mat.passCount; ++i)
            {
                mat.SetPass(i);
                GL.Begin(GL.QUADS);
                GL.Vertex3(0, 0, 0.1f);
                GL.Vertex3(1, 0, 0.1f);
                GL.Vertex3(1, 1, 0.1f);
                GL.Vertex3(0, 1, 0.1f);
                GL.End();
            }
            GL.PopMatrix();
            DestroyImmediate(tex);

            tex = new Texture2D(Mathf.FloorToInt(GetComponent<Camera>().pixelWidth), Mathf.FloorToInt(GetComponent<Camera>().pixelHeight));
            tex.filterMode = filterMode;
            tex.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
            tex.Apply();
        }
    }
}
