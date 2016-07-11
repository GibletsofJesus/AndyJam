using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{
	private static AdManager staticInstance = null;
	public static AdManager instance {get {return staticInstance;} set{}}

	[SerializeField] private Transform canvasParent = null;
	[SerializeField] private GameObject[] adPrefabs = null;
	private List<Ad>[] pooledAds;
	
	private List<Ad> activeAds = new List<Ad>();
	private float adPercent = 0.0f;
	private bool adblock = false;

	[SerializeField] private Vector3 closePoint = Vector3.zero;
	[SerializeField] private Canvas mainCanvas = null;
	[SerializeField] private Camera mainCam = null;

	private void Awake()
	{
		staticInstance = this;
		pooledAds = new List<Ad>[adPrefabs.Length];
		for(int i = 0; i < adPrefabs.Length; ++i)
		{
			pooledAds[i] = new List<Ad>();
		}
		//TryGenerateAd (new Vector3 (25,25, 0));
	}

	public void TryGenerateAd(Vector3 _pos)
	{
		if(!adblock)
		{
			if((100.0f - adPercent) > Random.Range(0.0f, 100.0f))
			{
				GenerateAd(_pos);
			}
		}
	}

    public void TryGenerateAd(Vector3 _pos, int _index)
    {
        if (!adblock)
        {
            if ((100.0f - adPercent) > Random.Range(0.0f, 100.0f))
            {
                GenerateAd(_pos);
            }
        }
    }

    private void GenerateAd(Vector3 _pos)
	{
		Ad _ad = AdPooling (Random.Range (0, adPrefabs.Length));
		_ad.SpawnAd (_pos, new Vector3(Random.Range(-7.0f, 7.0f),Random.Range(-14.0f, 14.0f),0.0f));
		activeAds.Add (_ad);
	}

    private void GenerateAd(Vector3 _pos, int _index)
    {
        Ad _ad = AdPooling(_index);
        _ad.SpawnAd(_pos, new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-10.0f, 10.0f), 0.0f));
        activeAds.Add(_ad);
    }

    private Ad AdPooling(int _index)
	{
		for (int i = 0; i < pooledAds[_index].Count; i++)
		{
			if (!pooledAds[_index][i].isActiveAndEnabled)
			{
				return pooledAds[_index][i];
			}
		}
		GameObject _obj = Instantiate (adPrefabs [_index]);
		_obj.transform.SetParent (canvasParent);
		_obj.SetActive (false);
		_obj.hideFlags = HideFlags.HideInHierarchy;
		Ad _ad = _obj.GetComponent<Ad> ();
		_ad.SetCamera (mainCam, mainCanvas);
		pooledAds[_index].Add(_ad);
		return _ad;
	}

	public void closeAd()
	{
		if(activeAds.Count > 0)
		{
			activeAds[activeAds.Count - 1].CloseAd(closePoint);
			activeAds.RemoveAt(activeAds.Count - 1);
		}
	}

    public void EnableAdBlock()
    {
        if (!adblock)
        {
            VisualCommandPanel.instance.AddMessage("Adblocker enabled");
            for (int i = 0; i < activeAds.Count; ++i)
            {
                activeAds[i].CloseAd(closePoint);
            }
            activeAds.Clear();
            adblock = true;
        }
    }

    public void DisableAdBlock()
    {
        if(!adblock)
        {
            for (int i = 0; i < activeAds.Count; ++i)
            {
                activeAds[i].CloseAd(closePoint);
            }
            activeAds.Clear();
        }
        adblock = false;
    }

}
