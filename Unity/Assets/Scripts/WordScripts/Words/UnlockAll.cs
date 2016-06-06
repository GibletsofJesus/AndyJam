using UnityEngine;
using System.Collections;

public class UnlockAll : Word
{

    private void Awake()
    {
        thisWord = "landrover.exe";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior()
    {
        UpdateBehavior.instance.UnlockAll();
    }

    protected override void Behavior()
    {

    }
}
