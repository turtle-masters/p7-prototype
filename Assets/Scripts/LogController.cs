using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public int testVar;

    public LogController()
    {
        this.testVar = 1;
    }

    public virtual string GetVar()
    {
        return "LogController " + testVar;
    }
}
