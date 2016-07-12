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
    private float maxMenuCool = 0.35f;
    private float buttonCool = 0.35f;
    private float maxButtonCool = 0.35f;
    public bool leaderBool = false;

	void Update ()
    {        if (allowStart)
        {
            OptionSelect();
            ChangeMenuColour();
        }
        MenuCooldown();
        ButtonCoolDown();
        DisplayLeaderBoard();
        if (Input.GetButton("Fire2")&&ButtonCoolDown())
        {
            swapShipSound.PlayOneShot(swapSounds[(Random.value>0.5f) ? 0 : 1]);
            shipIndex++;
            if (shipIndex>Ships.Length-1)
            {
                shipIndex = 0;
            }
            ship.sprite = Ships[shipIndex];
           
            buttonCool = 0;
        }
        if (Input.GetButton("Fire1") && allowStart && ButtonCoolDown() && !transitioning)
        {
            switch (menuSelect)
            {
                case 0:
                    anim.StopPlayback();
                    GreenShip.instance.ship = ship.sprite;
                    anim.Play("splash_out");
                    foreach (Text t in options)
                    allowStart = false;
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
                        Application.Quit();
                        soundManager.instance.playSound(swapSounds[0]);
                    }
                    break;
            }
        }
    }

    bool transitioning;
    
    public void loadLevel()
    {
        SceneManager.LoadScene(3);
    }

    void OptionSelect()
    {
        if (Input.GetAxis("Vertical") != 0 && MenuCooldown() && !leaderBool)
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
            soundManager.instance.playSound(swapSounds[1]);
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
    }

    void ShowMenu()
    {
        StartCoroutine(FadeMenu());
    }

    IEnumerator FadeMenu()
    {
        while (options[0].color.a < 1)
        {
            foreach (Text t in options)
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
            buttonCool += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
