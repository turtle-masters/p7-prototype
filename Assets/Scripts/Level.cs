using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public static GameObject globalPlayerObject;
    //[HideInInspector]
    public bool isActive;

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

    /*private void Await()
    {
        this.SetVisibilityOfAllChildren(false);
    }*/

    private void OnEnable()
    {
        this.SetVisibilityOfAllChildren(false);
    }

    private void Start()
    {
        IEnumerator rootEnumerator = this.gameObject.scene.GetRootGameObjects().GetEnumerator();
        rootEnumerator.Reset();
        GameObject playerGameObject = null;
        while (rootEnumerator.MoveNext())
        {
            GameObject current = (GameObject)rootEnumerator.Current;
            if (current.tag == "Player")
            {
                playerGameObject = current;
                break;
            }
        }
        if (Level.globalPlayerObject == null) Level.globalPlayerObject = playerGameObject;
        if (DebugPlayer.isActive || Level.totalSceneChanges < 2) playerGameObject.SetActive(true);
        else playerGameObject.SetActive(false);
    }

    private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
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
            case 0:
            case 1:
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
                break;
            default:  // case 8
                Level.activeLevel = levelsInScene[1];
                break;
        }

        Level.LoadNextLevel(Level.GetNextSceneName());
        Level.activeLevel.Activate();
        Level.totalSceneChanges++;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " loaded in mode " + mode);
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        // ...
    }

    private static void Continue()
    {
        try
        {
            Scene nextScene = SceneManager.GetSceneByName(Level.GetNextSceneName());
            if (nextScene != null && nextScene.isLoaded)  // these two conditions are probably the same thing...
                SceneManager.SetActiveScene(nextScene);
            else
                // load next level (and scene) if async load didn't work for some reason
                SceneManager.LoadScene(Level.GetNextSceneName());
        }
        catch (System.InvalidOperationException e)
        {
            // we just pretend the level changed here
            Level.totalSceneChanges++;
            Debug.LogWarning("Level tried to continue to next level but failed: " + e);
        }
    }

    private static IEnumerator LoadNextLevel(string sceneName)
    {
        // use totalSceneLoads to figure out what scene to load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        // wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
            yield return null;
    }

    private static string GetNextSceneName()
    {
        switch (Level.totalSceneChanges)
        {
            // first scene (tutorial) will be loaded automatically
            case 1:
            case 3:
            case 5:
            case 7:
                return "Village";
            case 2:
            case 4:
            case 6:
                return "Microverse";
            default:  // 7
                return "Room";
        }
    }

    public void SetVisibilityOfAllChildren(bool isVisible)
    {
        Debug.Log(this.name + "->SetVisibilityOfAllChildren->" + isVisible);
        this.isActive = isVisible;

        foreach (Transform childTransform in this.transform)
        {
            if (isVisible) childTransform.gameObject.layer = 0;
            else childTransform.gameObject.layer = 1;
            if (childTransform.GetComponent<Renderer>() != null)
                if (childTransform.GetComponent<InteractionTarget>() == null || !isVisible)
                    childTransform.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    /*
     * Activates this Level
     */
    public void Activate()
    {
        Debug.Log(this.name + "->Activate");
        this.SetVisibilityOfAllChildren(true);
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

        this.SetVisibilityOfAllChildren(false);

        // run preloaded scene
        Level.Continue();
    }
}
