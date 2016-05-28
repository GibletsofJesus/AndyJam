using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordHUD : MonoBehaviour 
{
	[SerializeField] private Text text = null;
	[SerializeField] private Image backgroundImage = null;
	[SerializeField] private Image cooldownImage = null;

	public void UpdateCooldown(float _fill)
	{
		cooldownImage.fillAmount = _fill;
	}

	public void TriggerSuccess()
	{
		cooldownImage.fillAmount = 0.0f;
	}

	public void Deactivate()
	{
		text.color = HUDData.instance.deactivateColour;
		backgroundImage.sprite = HUDData.instance.deactivateBackground;
		cooldownImage.sprite = HUDData.instance.deactivateCooldown;
	}

	public void Activate()
	{
		text.color = HUDData.instance.activateColour;
		backgroundImage.sprite = HUDData.instance.activateBackground;
		cooldownImage.sprite = HUDData.instance.activateCooldown;
	}

	public void UpdateWord(string _word)
	{
		text.text = _word;
	}
}
