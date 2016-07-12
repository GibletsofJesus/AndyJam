using UnityEngine;
using System.Collections;

public class SuperCooldowns : Word
{
    private void Awake()
    {
        thisWord = "administrator.exe";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior()
    {
        foreach(AbilityWord _w in FindObjectsOfType<AbilityWord>())
        {
            _w.BetterCooldown();
        }
    }

    protected override void Behavior()
    {

    }
}
