using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordHUD : MonoBehaviour 
{
	[SerializeField] private Text text = null;
	[SerializeField] private Image cooldownImage = null;

	public void UpdateCooldown(float _fill)
	{
		cooldownImage.fillAmount = _fill;
	}

	public void TriggerSuccess()
	{
		cooldownImage.fillAmount = 0.0f;
	}
}
