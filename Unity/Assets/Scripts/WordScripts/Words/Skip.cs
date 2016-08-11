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

    protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
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