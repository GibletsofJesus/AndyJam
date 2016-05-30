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

    private void Awake()
    {
        staticInstance = this;
    }

    public void UpdateHealth(float _healthPercent)
    {
        healthBar.fillAmount = _healthPercent;
    }

    public void UpdateLives(int _lives)
    {
        for (int i = 0; i < lives.Length; ++i)
        {
            lives[i].sprite = i < _lives ? liveSprite : notLiveSprite;
        }
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }
}
