using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordHUD : MonoBehaviour 
{
	[SerializeField] private Text text = null;
	[SerializeField] private Image backgroundImage = null;
	[SerializeField] private Image cooldownImage = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private Transform originalIconPosition = null;

    private bool displayIcon = true;
    private float lerpValue = 0;
    private float lerpSpeed = 2.0f;

    private void Start()
    {
        //originalIconPosition.position = iconImage.rectTransform.position;
    }

    public void UpdateCooldown(float _fill)
	{
		cooldownImage.fillAmount = _fill;
	}

	public void TriggerSuccess()
	{
		cooldownImage.fillAmount = 0.0f;
        displayIcon = false;
	}

    public void CooldownFinished()
    {
         displayIcon = true;
    }

	public void Deactivate()
	{
        text.text = "unknown-command";
		text.color = HUDData.instance.deactivateColour;
		backgroundImage.sprite = HUDData.instance.deactivateBackground;
		cooldownImage.sprite = HUDData.instance.deactivateCooldown;
        displayIcon = false;
    }

	public void Activate()
	{
		text.color = HUDData.instance.activateColour;
		backgroundImage.sprite = HUDData.instance.activateBackground;
		cooldownImage.sprite = HUDData.instance.activateCooldown;
        displayIcon = true;
    }

	public void UpdateWord(string _word)
	{
		text.text = _word;
	}

    private void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!displayIcon)
            {
                lerpValue -= Time.deltaTime * lerpSpeed;
                lerpValue = lerpValue <= 0 ? 0 : lerpValue;
            }
            else
            {
                lerpValue += Time.deltaTime * lerpSpeed;
                lerpValue = lerpValue >= 1 ? 1 : lerpValue;
            }
            //iconImage.rectTransform.position = Vector3.Lerp(originalIconPosition.position + (Vector3.left * iconImage.rectTransform.rect.width), originalIconPosition.position, lerpValue);
        }
    }
}
