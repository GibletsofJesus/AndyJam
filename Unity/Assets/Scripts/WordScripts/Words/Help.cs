using UnityEngine;
using System.Collections;

public class Help : Word
{
    [SerializeField] private AbilityWord[] abilityWords = null;

    private void Awake()
    {
        thisWord = "help";
        wordActive = true;
    }

    protected override void Start()
    {

    }

    protected override void TriggerBehavior(float pitchMod = 1, float volumeMod = 1)
    {
        VisualCommandPanel.instance.AddLine('-');
        foreach (AbilityWord _w in abilityWords)
        {
            string _desc = _w.GetDescription();
            if(_desc != string.Empty)
            {
                VisualCommandPanel.instance.AddMessage(_desc);
            }
            else
            {
                break;
            }
        }
        VisualCommandPanel.instance.AddLine('-');
    }

    protected override void Behavior()
    {

    }
}
