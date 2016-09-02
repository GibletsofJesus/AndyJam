using UnityEngine;
using System.Collections;

public class Adblocker : Word
{
    private void Awake()
    {
        thisWord = "adblocker";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
        Player.instance.IncreaseScore(-1337);
        GameStateManager.instance.cheat = true;
        AdManager.instance.EnableAdBlock();
    }

    protected override void Behavior()
    {

    }
}
