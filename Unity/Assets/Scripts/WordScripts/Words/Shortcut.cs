using UnityEngine;
using System.Collections;

public class Shortcut : Word
{
    private void Awake()
    {
        thisWord = "shortcut";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
        GameStateManager.instance.cheat = true;
        EnemyManager.instance.ShortCut();
    }

    protected override void Behavior()
    {

    }
}
