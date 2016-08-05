using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class splashScreen : MonoBehaviour
{
    public AudioClip MenuBoop;
    public Color highLightCol;
    public Animator anim;
    public SpriteRenderer ship;
    public Sprite[] Ships;
    int shipIndex=0;
    public bool allowStart;
    public AudioSource swapShipSound;
    public AudioClip[] swapSounds;
    [SerializeField] private Text[] options = null;
    [SerializeField] Text[] scores = null;
    [SerializeField] Text[] names = null;
    [SerializeField] Text[] rank = null;
    [SerializeField]
    bool once = false;
    private int menuSelect = 0;
    private float menuCool = 0.5f;
    private float maxMenuCool = 0.35f;
    private float buttonCool = 0.35f;
    private float maxButtonCool = 0.35f;
    private float shipToggleCool = 0.35f;
    private float shipToggleMaxCool = 0.35f;
    public bool leaderBool = false;

	void Update()
    {
        if (allowStart)
        {
            OptionSelect();
            ChangeMenuColour();
        }
        MenuCooldown();
        buttonCool += Time.deltaTime;
        DisplayLeaderBoard();

        if ((!(shipToggleCool < shipToggleMaxCool)) && allowStart && ConvertToPos())
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                swapShipSound.PlayOneShot(swapSounds[(Random.value > 0.5f) ? 0 : 1]);
                ++shipIndex;
                if (shipIndex == Ships.Length)
                {
                    shipIndex = 0;
                }
                ship.sprite = Ships[shipIndex];
                shipToggleCool = 0.0f;
            }
            else if(Input.GetAxis("Horizontal") < 0)
            {
                swapShipSound.PlayOneShot(swapSounds[(Random.value > 0.5f) ? 0 : 1]);
                --shipIndex;
                if (shipIndex == -1)
                {
                    shipIndex = Ships.Length - 1;
                }
                ship.sprite = Ships[shipIndex];
                shipToggleCool = 0.0f;
            }
        }
        else
        {
            shipToggleCool += Time.deltaTime;
        }
        if (Input.GetButton("Fire1") && allowStart && ButtonCoolDown() && !transitioning)
        {
            switch (menuSelect)
            {
                case 0:
                    anim.StopPlayback();
                    GreenShip.instance.ship = ship.sprite;
                    anim.Play("splash_out");
                    for (int i = 0; i < options.Length; ++i)
                    {
                        allowStart = false;
                    }
                    break;
                case 1:
                    leaderBool = !leaderBool;
                    ShowLeaderBoard(leaderBool);
                    buttonCool = 0;
                    soundManager.instance.playSound(swapSounds[0]);
                    break;
                case 2:
                    if (Input.GetButton("Fire1") && allowStart && MenuCooldown())
                    {
                        //Application.Quit();
                        //soundManager.instance.playSound(swapSounds[0]);
                    }
                    break;
            }
        }
        if (!Input.anyKey && Input.GetAxis("Horizontal") < 0.1f)
        {
            idletime += Time.deltaTime;
        }
        else
            idletime = 0;

        if (idletime > 180)
            SceneManager.LoadScene(0);
    }

    float idletime;

    bool transitioning;
    
    public void loadLevel()
    {
        SceneManager.LoadScene(4);
    }

    void OptionSelect()
    {
        if (!ConvertToPos())
        {
            if (Input.GetAxis("Vertical") != 0 && MenuCooldown() && !leaderBool)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    menuSelect++;
                    if (menuSelect > options.Length - 2)
                    {
                        menuSelect = 0;
                    }
                }
                if (Input.GetAxis("Vertical") > 0)
                {
                    menuSelect--;
                    if (menuSelect < 0)
                    {
                        menuSelect = options.Length - 2;
                    }
                }
                menuCool = 0;
                soundManager.instance.playSound(swapSounds[1]);
            }
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
                options[i].color = highLightCol;
            }
            else
            {
                options[i].color = Color.white;
            }
        }
        options[2].color = Color.grey;
    }

    void ShowMenu()
    {
        StartCoroutine(FadeMenu());
    }

    IEnumerator FadeMenu()
    {
        foreach (Text t in options)
        {
            while (t.color.a < 1)
            {
                Color col = t.color;
                col.a += Time.deltaTime;
                t.color = col;
            }
            yield return new WaitForEndOfFrame();
        }
        allowStart = true;
    }

    void DisplayLeaderBoard()
    {
        if (!once)
        {
            List<KeyValuePair<string, int>> k = LeaderBoard.instance.ReturnLeaderBoard();
            for (int i = 0; i < k.Count; i++)
            {
                rank[i].text = (i + 1).ToString();
                scores[i].text = k[i].Value.ToString();

                string s = k[i].Key;
                s = s.Insert(4, " ");
                s = s.Insert(3, " ");
                names[i].text = s;

                rank[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
                scores[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
                names[i].color = Color.Lerp(Color.green, Color.red, (float)(i * 0.05f));
            }
        }
    }
    void ShowLeaderBoard(bool _onOff)
    {
        if (!transitioning)
            StartCoroutine(leadboardPan(_onOff));
    }

    IEnumerator leadboardPan(bool InOut)
    {
        transitioning = true;
        Vector3 mainMenu = new Vector3(0.5f, -0.87f, -10);
        Vector3 leadboardView = new Vector3(5.5f, -9.53f, -10);

        float lerpVal = 0;
        while (lerpVal < 1)
        {
            if (InOut)
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, leadboardView, lerpVal);
            else
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, mainMenu, lerpVal);

            lerpVal += Time.deltaTime * 0.8f;
            yield return new WaitForEndOfFrame();
        }
        transitioning = false;
    }

    bool ButtonCoolDown()
    {
        if (buttonCool<maxButtonCool)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    bool ConvertToPos()
    {
        float hori = Input.GetAxis("Horizontal");
        float verti = Input.GetAxis("Vertical");
        if (hori < 0)
        {
            hori -= hori * 2;
        }
        if (verti < 0)
        {
            verti -= verti * 2;
        }
        if (hori > verti)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
