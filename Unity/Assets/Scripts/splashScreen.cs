using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class splashScreen : MonoBehaviour
{

    public Animator anim;
    public SpriteRenderer ship;
    public Sprite ShipA, ShipB;
    public bool allowStart;
    public AudioSource swapShipSound;
    public AudioClip[] swapSounds;
    [SerializeField] private Text[] options;
    [SerializeField] private GameObject[] titles;
    [SerializeField] Text[] scores;
    [SerializeField] Text[] names;
    [SerializeField] Text[] rank;
    [SerializeField]
    Canvas lBoard;
    bool once = false;
    private int menuSelect = 0;
    private float menuCool = 0.5f;
    private float maxMenuCool = 0.5f;
    private float buttonCool = 0.5f;
    private float maxButtonCool = 0.5f;
    bool leaderBool = false;
    // Update is called once per frame
    void Awake()
    {
       
    }
	void Update ()
    {
       
        if (allowStart)
        {
            OptionSelect();
            ChangeMenuColour();
        }
        MenuCooldown();
        ButtonCoolDown();
        DisplayLeaderBoard();
        if (Input.GetButton("Fire2")&&ButtonCoolDown())
        {
            swapShipSound.PlayOneShot(swapSounds[(ship.sprite == ShipA) ? 0 : 1]);
            if (ship.sprite == ShipA)
            {
                ship.sprite =ShipB;
            }
            else
            {
                ship.sprite = ShipA;
            }
            buttonCool = 0;
        }
        if (Input.GetButton("Fire1") && allowStart && ButtonCoolDown())
        {
            switch (menuSelect)
            {
                case 0:
                    {
                        anim.StopPlayback();
                        GreenShip.instance.ship = ship.sprite;
                        anim.Play("splash_out");
                        foreach (Text t in options)
                        {
                            t.CrossFadeColor(new Color(0, 0, 0, 0), 0.3f, false, true);
                        }
                        allowStart = false;

                        break;
                    }
                case 1:
                    {
                        leaderBool = !leaderBool;
                        ShowLeaderBoard(leaderBool);
                        buttonCool = 0;
                        break;
                    }
                case 2:
                    {
                        if (Input.GetButton("Fire1") && allowStart && MenuCooldown())
                            Application.Quit();
                        break;
                    }
            }
        }
	}

    public void letPlayerStart()
    {
        allowStart = true;
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(3);
    }

    void OptionSelect()
    {
        if (Input.GetAxis("Vertical") != 0 && MenuCooldown())
        {
            if (Input.GetAxis("Vertical")<0)
            {
                menuSelect++;
                if (menuSelect>options.Length-1)
                {
                    menuSelect = 0;
                }
            }
            if (Input.GetAxis("Vertical")>0)
            {
                menuSelect--;
                if (menuSelect<0)
                {
                    menuSelect = options.Length-1;
                }
            }
            menuCool = 0;
        }
    }

    bool MenuCooldown()
    {
        if (menuCool<maxMenuCool)
        {
            menuCool += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
    void ChangeMenuColour()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i == menuSelect)
            {
                options[i].color = Color.blue;
            }
            else
            {
                options[i].color = Color.white;
            }
        }
    }

    void ShowMenu()
    {
      foreach(Text t in options)
            {
                t.gameObject.SetActive(true);
            }
    }
  // Update is called once per frame
    void DisplayLeaderBoard()
    {
        if (!once)
        {
            List<KeyValuePair<string, int>> k = LeaderBoard.instance.ReturnLeaderBoard();
            for (int i = 0; i < k.Count; i++)
            {
                rank[i].text = (i + 1).ToString();
                scores[i].text = k[i].Value.ToString();
                names[i].text = k[i].Key;
                rank[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
                scores[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
                names[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
            }
        }
    }
    void ShowLeaderBoard(bool _onOff)
    {
        if (_onOff)
        {
            foreach (GameObject g in titles)
            {
                g.SetActive(false);
            }
            lBoard.gameObject.SetActive(true);
        }
        else
        {
             lBoard.gameObject.SetActive(false);
            
            foreach (GameObject g in titles)
            {
                g.SetActive(true);
            }
        }
    }
    bool ButtonCoolDown()
    {
        if (buttonCool<maxButtonCool)
        {
            buttonCool += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
