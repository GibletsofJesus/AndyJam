using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PwIdent : MonoBehaviour 
{
    public RawImage ident;
    MovieTexture mTex;
	// Use this for initialization
	void Awake ()
    {
        mTex = (MovieTexture)ident.mainTexture;
        if (!mTex.isPlaying)
        {
            mTex.Play();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	  if (!mTex.isPlaying)
      {
          mTex.Play();
      }
	}
}
