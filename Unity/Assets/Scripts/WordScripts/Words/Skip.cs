using UnityEngine;
using System.Collections;

public class Skip : Word
{
    private void Awake()
    {
        thisWord = "skip.exe";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior()
    {
        Tutorial.instance.Skip();
        EndBehavior();
    }

    public void TutorialFinished()
    {
        wordActive = false;
    }

    protected override void Behavior()
    {

    }
}