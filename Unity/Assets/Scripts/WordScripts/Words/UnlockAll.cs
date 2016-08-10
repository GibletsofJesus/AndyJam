using UnityEngine;
using System.Collections;

public class UnlockAll : Word
{

    private void Awake()
    {
        thisWord = "landrover";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
        UpdateBehavior.instance.UnlockAll();
    }

    protected override void Behavior()
    {

    }
}
