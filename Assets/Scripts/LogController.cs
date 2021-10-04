using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Children will report user actions and game events to this class, which will then log them and hand them off to a web server (Logging Over IP)
 */
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
