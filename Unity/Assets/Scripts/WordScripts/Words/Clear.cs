using UnityEngine;
using System.Collections;

public class Clear : Word
{
    private void Awake()
    {
        thisWord = "clear";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior()
    {
        VisualCommandPanel.instance.ClearPanel();
    }

    protected override void Behavior()
    {

    }
}
