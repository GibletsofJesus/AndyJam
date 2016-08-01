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
    private bool spamfilter = false;

    private float alertTime = 0;
    private float alertCooldown = 10.0f;

    [SerializeField] private Vector3 closePoint = Vector3.zero;
	[SerializeField] private Canvas mainCanvas = null;
	[SerializeField] private Camera mainCam = null;

    public int numActiveAds { get { return activeAds.Count; } }

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

	public void TryGenerateAd(Vector3 _pos, bool _limitToLower = false)
	{
		if(!adblock && !spamfilter)
		{
			if((100.0f - adPercent) > Random.Range(0.0f, 100.0f))
			{
				GenerateAd(_pos, _limitToLower);
			}
		}
	}

    public void TryGenerateAd(Vector3 _pos, int _index, bool _limitToLower = false)
    {
        if (!adblock && !spamfilter)
        {
            if ((100.0f - adPercent) > Random.Range(0.0f, 100.0f))
            {
                GenerateAd(_pos, _limitToLower);
            }
        }
    }

    private void GenerateAd(Vector3 _pos, bool _limitToLower)
	{
		Ad _ad = AdPooling (Random.Range (0, adPrefabs.Length));
		_ad.SpawnAd (_pos, new Vector3(Random.Range(-7.0f, 7.0f),Random.Range(-14.0f, _limitToLower ? 0.0f : 14.0f),0.0f));
		activeAds.Add (_ad);
	}

    private void GenerateAd(Vector3 _pos, int _index, bool _limitToLower)
    {
        Ad _ad = AdPooling(_index);
        _ad.SpawnAd(_pos, new Vector3(Random.Range(-7.0f, 7.0f), Random.Range(-14.0f, _limitToLower ? 0.0f : 14.0f), 0.0f));
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
        alertTime = 0.0f;
        int _range = Random.Range(1, 4);
        for (int i = 0; i < _range; ++i)
        {
            if (activeAds.Count > 0)
            {
                activeAds[activeAds.Count - 1].CloseAd(closePoint);
                activeAds.RemoveAt(activeAds.Count - 1);
            }
            else
            {
                return;
            }
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
            alertTime = 0;
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
        alertTime = 0;
    }

    public void SpamFilter(bool _on)
    {
        spamfilter = _on;
    }


    private void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (activeAds.Count > 0)
            {
                alertTime += Time.deltaTime;
                if (alertTime >= alertCooldown)
                {
                    alertTime = 0;
                    VisualCommandPanel.instance.TryMessage("Type close to remove ads");
                }
            }
        }
    }

}
