using UnityEngine;
using System.Collections;

public class Skip : Word
{
    private void Awake()
    {
        thisWord = "skip";
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

    public override void Reset()
    {
        //This is needed to override base behavior to prevent if from happening
    }
}