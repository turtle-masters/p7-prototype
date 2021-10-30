using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class LogableEvent
{
    public string classifier;
    public string name;
    public string source;
    public string sourceLevel;
    public string sourceScene;
    public string data;

    public string timestamp;  // this field is populated automatically when the object is initialized

    public LogableEvent(string _classifier, string _name, string _source, string _sourceLevel, string _sourceScene, string _data)
    {
        this.classifier =   _classifier;
        this.name =         _name;
        this.source =       _source;
        this.sourceLevel =  _sourceLevel;
        this.sourceScene =  _sourceScene;
        this.data =         _data;

        this.timestamp =    System.DateTime.Now
                                .ToString("yyyy-MM-dd HH:mm:ss.ffff")
                                .Replace('/', '-')
                                .Replace(":", "-");
    }

    public bool Equals(LogableEvent le)
    {
        return this.classifier == le.classifier
            && this.name == le.name
            && this.source == le.source
            && this.sourceLevel == le.sourceLevel
            && this.sourceScene == le.sourceScene
            && this.data == le.data;
            // timestamp should not be compared
    }

    public string ToStringPretty()
    {
        return $"{this.timestamp}: <({this.classifier}) {this.name}> in {this.sourceScene}->{this.sourceLevel} by {this.source}"
            + this.data != null ? $" with message: {this.data}" : "";
    }

    public override string ToString()
    {
        return $"{this.timestamp},{this.classifier},{this.name},{this.sourceScene},{this.sourceLevel},{this.source},{this.data}";
    }
}

public class Logger : MonoBehaviour  // class is almost entirely static
{
    private static bool logConsoleMessages = false;
    private static bool doLoggingOverIP = false;
    
    private static List<LogableEvent> logQueue = new List<LogableEvent>();
    private static bool fileSystemOperationInProgress = false;
    private static string logFileName = $"/LOG_MOVE_ME_{System.DateTime.Now.ToString("HH:mm:ss.ffff")}.csv";

    protected void Start()
    {
        Application.logMessageReceived  += LogConsoleMessage;
        Application.focusChanged        += LogFocusChanged;
        Application.lowMemory           += LogLowMemory;
        Application.unloading           += LogUnloading;
        Application.quitting            += OnApplicationQuit;

        //Debug.Log("Wrote data to file path " + Logger.GetFilePath() + Logger.logFileName);

        //_ = Logger.WriteLineToFile("timestamp,classifier,event,scene,level,source,data\n");
    }

    // ===== THE DIFFERENT CLASSIFIERS- AND SUB-CLASSIFIERS =====
    public static void Log(Classifier.Metadata category, string data)
    {
        // ...
    }

    public static void Log(Classifier.Console category, string message)
    {
        // ...
    }

    public static void Log(Classifier.Answer category, string data)
    {
        // ...
    }

    public static void Log(Classifier.Player category, GameObject data)
    {
        // ...
    }

    public static void Log(Classifier.Prompt category, Prompt prompt)
    {
        // ...
    }

    public static void Log(Classifier.Task category, Task task)
    {
        // ...
    }

    public static void Log(Classifier.Level category, Level level)
    {
        // ...
    }

    // ===== THE BOTTLENECK LOG METHOD =====
    private static void Log(LogableEvent le)
    {
        Logger.logQueue.Add(le);

        if (!Logger.fileSystemOperationInProgress)
        {
            List<LogableEvent> logQueueCopy = new List<LogableEvent>(Logger.logQueue);
            Logger.logQueue.Clear();

            Logger.WriteLogableEventsToFile(new List<LogableEvent>(logQueueCopy));
            // TODO: add LoIP call here...
        }
    }

    // ===== THE SUBSCRIBED LOG EVENTS =====
    private static void LogConsoleMessage(string logString, string stackTrace, LogType type)
    {
        if (!Logger.logConsoleMessages) return;

        Logger.Log(new LogableEvent(
            "Console",
            type.ToString(),
            "null",
            Level.activeLevel.name,
            SceneManager.GetActiveScene().name,
            $"\"{logString}\" in {stackTrace}"  // <- this will most likely need a .Replace to remove newlines...
        ));
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

    // ===== THE LOWER-LEVEL FILE SYSTEM AND NETWORKING OPERATIONS =====
    private static bool WriteLogableEventsToFile(List<LogableEvent> eventQueue)
    {
        List<string> LogableStrings = new List<string>();
        foreach (LogableEvent le in eventQueue)
            LogableStrings.Add(le.ToString());

        return Logger.WriteLinesToFile(LogableStrings);
    }

    private static string GetFilePath()
    {
        return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
    }

    private static bool LogFileExists()
    {
        return File.Exists(Logger.GetFullFilePath());
    }

    private static string GetFullFilePath()
    {
        return Path.Combine(Logger.GetFilePath() + Logger.logFileName);
    }

    private static bool WriteLinesToFile(List<string> lines)
    {
        Logger.fileSystemOperationInProgress = true;

        try
        {
            if (!Logger.LogFileExists()) { }
                File.Create(Logger.GetFullFilePath());

            /*using (StreamWriter file = File.AppendText(Logger.GetFullFilePath()))
            //using (var file = new StreamWriter(Logger.GetFullFilePath(), true))
            {
                foreach (string line in lines)
                    file.WriteLine(line);
            }*/

            Logger.fileSystemOperationInProgress = false;
            return true;
        }
        catch (InvalidDataException e)
        {
            Debug.LogError("Target log path exists but is read-only\n" + e);
        }
        catch (PathTooLongException e)
        {
            Debug.LogError("Target log path name may be too long\n" + e);
        }
        catch (IOException e)
        {
            Debug.LogError("The disk may be full\n" + e);
        }

        // TODO: revert log file if write operations fail...

        Logger.fileSystemOperationInProgress = false;
        return false;
    }

    private static bool WriteLineToFile(string line)
    {
        return Logger.WriteLinesToFile(new List<string>() { line });
    }
}
