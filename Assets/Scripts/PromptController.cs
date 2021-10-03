using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptController : LogController
{
    // NOTE: some child of this should implement methods "Start" and "Update" ...

    public override string GetVar()
    {
        return "PromptController " + testVar;
    }
}
