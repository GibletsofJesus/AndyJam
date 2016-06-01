using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ad : MonoBehaviour 
{
	[SerializeField] private RectTransform rectTrans = null;
	[SerializeField] private float startScale = 0.01f;
	[SerializeField] private float endScale = 1.0f;
	[SerializeField] private float moveTime = 0.5f;
	private float currentTime = 0.0f;

	private Vector3 start;
	private Vector3 target;
	private float startingScale;
	private float targetScale;
	private bool move = false;
	private bool closing = false;

	private Camera cam;
	private Canvas canvas;

	public void SetCamera(Camera _cam, Canvas _canvas)
	{
		cam = _cam;
		canvas = _canvas;
	}

	public void SpawnAd(Vector3 _start, Vector3 _target)
	{
		gameObject.SetActive (true);
		start = _start;
		target = _target;
		move = true;
		startingScale = startScale;
		targetScale = endScale;
	}

	public void CloseAd(Vector3 _target)
	{
		move = true;
		closing = true;
		start = transform.position;
		target = _target;
		startingScale = endScale;
		targetScale = startScale;
	}

	public void Update()
	{
		if(move)
		{
			currentTime = Mathf.Min (currentTime + Time.deltaTime, moveTime);
			RectTransform canvasRect = canvas.GetComponent<RectTransform>();
			Vector2 _viewport = cam.WorldToViewportPoint(start + ((target - start) * (currentTime / moveTime)));
			Vector2 _screenPos = new Vector2(((_viewport.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
			                                 ((_viewport.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)));
			rectTrans.anchoredPosition=_screenPos;
			//rectTrans.anchorMin = _screenPos;
			//rectTrans.anchorMax = _screenPos;
			//Debug.Log (_screenPos);

			transform.localScale = Vector3.one * (startingScale + ((targetScale - startingScale) * (currentTime / moveTime)));
			if(currentTime == moveTime)
			{
				move = false;
				currentTime = 0.0f;
				if(closing)
				{
					closing = false;
					gameObject.SetActive(false);
				}
			}
		}
	}
}
