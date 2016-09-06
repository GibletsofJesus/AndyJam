using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterName : MonoBehaviour
{
    public Text[] box;
    int[] currentCharacter;
    public Text score;
    int selectBox = 0;
    int selectChar= 65;
    float coolDown = 0;
    float maxCool = 0.2f;
    string theName = string.Empty;
    [SerializeField] private Image[] charArrows = null;

    // Use this for initialization
    void Awake () 
    {
        coolDown = maxCool;
        currentCharacter = new int[box.Length];
        for (int i = 0; i < currentCharacter.Length; i++)
        {
            currentCharacter[i] = 65;
        }
        if(GameStateManager.instance.aiTyper)
        {
            currentCharacter[3] = 65; //A
            currentCharacter[4] = 45; //-
            currentCharacter[5] = 73; //I

            for(int i = 3; i < 6; ++i)
            {
                box[i].text = ((char)currentCharacter[i]).ToString();
                charArrows[i * 2].gameObject.SetActive(false);
                charArrows[(i * 2) + 1].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MenuInput();
        box[selectBox].text = ((char)currentCharacter[selectBox]).ToString();
        ChangeTextColour();
        score.text = "Your Score:\n" + Player.instance.score.ToString();
        if (Input.GetButtonDown("Fire1"))
            SelectName();
    }

    void MenuInput()
    {
        if (ConvertToPos())
        {
            if (Input.GetAxis("Horizontal") != 0 && SelectCoolDown())
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (selectBox == (GameStateManager.instance.aiTyper ? box.Length - 4 : box.Length - 1))
                    {
                        selectBox = 0;
                    }
                    else
                    {
                        selectBox++;
                    }
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    if (selectBox == 0)
                    {
                        if (GameStateManager.instance.aiTyper)
                        {
                            selectBox = box.Length - 4;
                        }
                        else
                        {
                            selectBox = box.Length - 1;
                        }
                    }
                    else
                    {
                        selectBox--;
                    }
                }
                selectChar = currentCharacter[selectBox];
                coolDown = 0;
            }
        }
        else
        {
            if (Input.GetAxis("Vertical") != 0 && SelectCoolDown())
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    charArrows[(selectBox * 2) + 1].color = Color.grey;
                    charArrows[(selectBox * 2) + 1].rectTransform.sizeDelta = new Vector2(60, 200);
                    StartCoroutine(revertImageColour(charArrows[(selectBox * 2) + 1]));

                    if (selectChar > 65)
                        selectChar--;

                    else
                        selectChar = 90;
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    charArrows[selectBox * 2].color = Color.grey;
                    charArrows[selectBox * 2].rectTransform.sizeDelta = new Vector2(60, 200);
                    StartCoroutine(revertImageColour(charArrows[selectBox * 2]));

                    if (selectChar < 90)
                        selectChar++;

                    else
                        selectChar = 65;
                }
                coolDown = 0;
                currentCharacter[selectBox] = selectChar;
            }
            if(Input.inputString.Length > 0)
            {
                int _charInt = (int)char.ToUpper(Input.inputString[0]);
                if (_charInt > 64 && _charInt < 91)
                {
                    selectChar = _charInt;
                    currentCharacter[selectBox] = selectChar;
                    box[selectBox].text = ((char)currentCharacter[selectBox]).ToString();
                    selectBox = selectBox == (GameStateManager.instance.aiTyper ? box.Length - 4 : box.Length - 1) ? selectBox : selectBox + 1;
                    selectChar = currentCharacter[selectBox];
                }
            }
        }
    }

    IEnumerator revertImageColour(Image i)
    {
        yield return new WaitForSeconds(0.075f);
        i.color = Color.white;
        i.rectTransform.sizeDelta = new Vector2(42, 200);
    }


    bool SelectCoolDown()
    {
        if (coolDown<maxCool)
        {
            coolDown += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    void ChangeTextColour()
    {
       for (int i=0;i<box.Length;i++)
       {
           if (selectBox == i)
           {
               box[i].color = Color.green;
           }
           else
           {
               box[i].color = Color.white;
           }
        }
    }
    void SelectName()
    {
        for (int i = 0; i < box.Length; i++)
        {
            theName = theName + box[i].text;
        }
       
        theName =  theName.Insert(3, "&");
        LeaderBoard.instance.SetName(theName);
        LeaderBoard.instance.AddNewScoreToLB();
        GameStateManager.instance.RunGameOver();
        transform.parent.gameObject.SetActive(false);
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
