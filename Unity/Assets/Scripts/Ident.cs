using UnityEngine;
using UnityEngine.SceneManagement;

public class Ident : MonoBehaviour
{
    public AudioClip[] honks;
    public AudioSource honkSound;
    [SerializeField] private GameObject blackScreen = null;
    private bool skip = false;


    public void Start()
    {
        Cursor.visible = false;
    }

	public void loadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void selectRandomHonk()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        honkSound.clip = honks[Random.Range(0, honks.Length)];
    }

    private void Update()
    {
        if (skip)
        {
            loadLevel(3);
        }
        if (Input.GetButton("Fire1"))
        {
            blackScreen.gameObject.SetActive(true);
            skip = true;
        }
    }

}
