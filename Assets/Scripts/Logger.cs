using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Classifier
{
    public enum Metadata    // metadata about the application and environment, unity version, fps...
    {
        // morrored from Application: https://docs.unity3d.com/ScriptReference/Application.html
        DataPath,
        InternetReachability,
        IsFocused,
        Platform,
        SystemLanguage,
        UnityVersion,
        Version,
        FrameCount,
    }

    public enum Console     // everything that is logged to the console...
    {
        Error,      //
        Assert,     //
        Warning,    //      mirrored from LogType: https://docs.unity3d.com/ScriptReference/LogType.html
        Log,        //
        Exception,  //
    }

    public enum Answer      // the player's answers to the questionnaires...
    {
        ChoiceAvailable,
        Selected,
        Comitted,
    }

    public enum Player      // location of player head and hands, look direction, buttons pressed...
    {
        PlayerHeadPosition,
        PlayerHeadForwardVector,
        PlayerLeftHandPosition,
        PlayerLeftHandRotation,
        PlayerLeftHandInputEngaged,
        PlayerLeftHandInputReleased,
        PlayerRightHandPosition,
        PlayerRightHandRotation,
        PlayerRightHandInputEngaged,
        PlayerRightHandInputReleased,
    }

    public enum Prompt      // active and resolved prompts, sounds playing, amount of text on screen...
    {
        Activated,
        Resolved,
        SoundPlaying,
        SoundStopped,
        TextShowing,
        TextHidden,
    }

    public enum Task        // objects being touched...
    {
        Touching,
        NoLongerTouching,
        Grabbed,
        Released,
    }

    public enum Level       // active level, load events, completion %...
    {
        Loaded,
        Unloaded,
        Started,
        Completed,
        HasBecomeActive,
        HasBecomeInactive,
    }
}

public static class Logger
{
    static Logger()
    {
        Application.logMessageReceived  += LogConsoleMessage;
        Application.focusChanged        += LogFocusChanged;
        Application.lowMemory           += LogLowMemory;
        Application.unloading           += LogUnloading;
        Application.quitting            += OnApplicationQuit;

        // ...
    }

    private static void Log(string classifier, string data)
    {
        // ...
    }

    private static void LogConsoleMessage(string logString, string stackTrace, LogType type)
    {
        // ...
    }

    private static void LogFocusChanged(bool isFocused)
    {
        // ...
    }

    private static void LogLowMemory()
    {
        // ...
    }

    private static void LogUnloading()
    {
        // ...
    }

    private static void OnApplicationQuit()
    {
        // ...
    }
}
