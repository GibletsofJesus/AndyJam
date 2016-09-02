using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private static PlayerHUD staticInstance = null;
    public static PlayerHUD instance { get { return staticInstance; } set { } }

    [SerializeField]
    private Image healthBar = null;
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Image[] lives = null;
    [SerializeField]
    private Sprite liveSprite = null;
    [SerializeField]
    private Sprite notLiveSprite = null;

	private const int pixels = 100;
	private float pixelCooldown;

    private void Awake()
    {
        staticInstance = this;
        pixelCooldown = (1.0f / (float)pixels) * 1000.0f;
        if (GreenShip.instance)
            liveSprite = GreenShip.instance.ship;
        foreach (Image i in lives)
        {
            i.sprite = liveSprite;
        }
    }

    public void UpdateHealth(float _healthPercent)
    {
		healthBar.fillAmount = (((_healthPercent * 1000.0f) - ((_healthPercent * 1000.0f) % pixelCooldown)) / 1000.0f);
    }

    public void UpdateLives(int _lives)
    {
        for (int i = 0; i < lives.Length; ++i)
        {
            lives[i].sprite = i < _lives ? liveSprite : notLiveSprite;
        }
    }

    void Update()
    {
        if (scoreToAdd > 0)
        {
            scoreToAdd -= 10;
            currentScore += 10;
        }
        scoreText.text = currentScore.ToString();
    }

    int currentScore, scoreToAdd;

    public void UpdateScore(int _score)
    {
        scoreToAdd = _score - currentScore;
    }
}
