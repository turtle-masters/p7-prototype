using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [Tooltip("The first Prompt to be activated when the level is loaded.")]
    public Prompt entryPrompt;

    public static Level activeLevel;
    private static int totalSceneChanges = 0;

    /*public delegate void ActiveSceneChangedEvent();
    public static event ActiveSceneChangedEvent OnActiveSceneChanged;
    public delegate void SceneLoadedEvent();
    public static event SceneLoadedEvent OnSceneLoaded;
    public delegate void SceneUnloadedEvent();
    public static event SceneUnloadedEvent OnSceneUnloaded;*/

    static Level()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Level.totalSceneChanges++;

        // find all Level components in the Scene
        List<Level> levelsInScene = new List<Level>();
        IEnumerator ie = newScene.GetRootGameObjects().GetEnumerator();
        ie.Reset();
        while (ie.MoveNext())
        {
            GameObject currentObject = (GameObject) ie.Current;
            if (currentObject != null && currentObject.GetComponent<Level>())
                levelsInScene.Add(currentObject.GetComponent<Level>());
        }
        
        // use totalSceneLoads to figure out what level to load
        switch (Level.totalSceneChanges)
        {
            case 1:
                Level.activeLevel = levelsInScene[0];

                // start loading the "Tutorial" scene in the background
                if (Application.isPlaying) {
                    /*SceneManager.UnloadSceneAsync("Tutorial");
                    Level.activeLevel.StartCoroutine(Level.LoadNextLevel());*/
                }
                break;
            case 2:
                Level.activeLevel = levelsInScene[0];
                break;
            case 3:
            case 4:
                Level.activeLevel = levelsInScene[1];
                break;
            case 5:
            case 6:
                Level.activeLevel = levelsInScene[2];
                break;
            case 7:
                Level.activeLevel = levelsInScene[3];

                // start loading the "Microverse" scene in the background
                if (Application.isPlaying)
                {
                    /*SceneManager.UnloadSceneAsync("Microverse");
                    Level.activeLevel.StartCoroutine(Level.LoadNextLevel());*/
                }
                break;
            default:
                Level.activeLevel = levelsInScene[0];  // other scenes than "Village" and "Microverse" should only have one Level
                break;

        }
        Level.activeLevel.Activate();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " loaded in the background in mode " + mode);
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        // ...
    }

    private static void Continue()
    {
        // the following code will error during unit testing
        try
        {
            Scene nextScene = SceneManager.GetSceneByName(Level.GetNextSceneName());
            if (nextScene != null && nextScene.isLoaded)  // these two conditions are probably the same thing...
                SceneManager.SetActiveScene(nextScene);
            else
            {
                // load next level (and scene) if async load didn't work for some reason
                SceneManager.LoadScene(Level.GetNextSceneName());
            }
        }
        catch (System.InvalidOperationException e)
        {
            // we just pretend the level changed here
            Level.totalSceneChanges++;
            Debug.LogWarning("Level tried to continue to next level but failed: " + e);
        }
    }

    private static IEnumerator LoadNextLevel()
    {
        // use totalSceneLoads to figure out what scene to load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Level.GetNextSceneName());
        asyncLoad.allowSceneActivation = false;
        
        // wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private static string GetNextSceneName()
    {
        switch (Level.totalSceneChanges)
        {
            // first scene (tutorial) will be loaded automatically
            case 0:
            case 2:
            case 4:
            case 6:
                return "Village";
            case 1:
            case 3:
            case 5:
                return "Microverse";
            case 7:
                return "Questionnaire";
            default:
                // this is just here to make the compiler happy...
                return "A Scene That Doesn't Exist...";
        }
    }

    /*
     * Activates this Level
     * Should only be done from inside the static Level instance
     */
    public void Activate()
    {
        if (entryPrompt != null) entryPrompt.Activate();
        else Debug.LogError(this + " was activated but no initial Prompt was given. Did you foget to reference the entry Prompt?");
    }

    /*
     * Call when the Level is considered complete and the player is ready to move on to the next
     */
    public void Complete(Prompt p)
    {
        // resolve all unresolved prompts before continuing
        foreach (Prompt ap in new List<Prompt>(Prompt.activePrompts))
            p.Resolve();

        // run preloaded scene
        Level.Continue();
    }
}
