using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroveseLevelSelectPrompt : Prompt
{
    public int levelIndex;

    protected override void TurnOn()
    {
        base.TurnOn();

        MinigameManagerScript.instance.SetCurrentLevel(this.levelIndex);
    }
}
