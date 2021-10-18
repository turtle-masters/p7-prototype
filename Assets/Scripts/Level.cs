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
    
    [HideInInspector]
    public static GameObject globalPlayerObject;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public static Level activeLevel;
    [HideInInspector]
    public static int totalSceneChanges = 0;

    static Level()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

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

        //Debug.Log("Active Level is now " + Level.activeLevel.name + " in Scene " + SceneManager.GetActiveScene().name);

        Level.LoadNextLevel(Level.GetNextSceneName());
        Level.activeLevel.Activate();
        Level.totalSceneChanges++;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + " loaded in mode " + mode);
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
            default:  // 8
                return "Room";
        }
    }

    private void SetVisibilityRecursively(GameObject node, bool isVisible, bool keepDisabled = false)
    {
        if (node.GetComponent<InteractionTarget>() != null) 
            keepDisabled = true;
        else if (node.GetComponent<Task>() != null && node.GetComponent<Task>().hideUntilActive)
            keepDisabled = true;

        /*if (isVisible) node.layer = 0;
        else node.layer = 1;*/

        if (!keepDisabled || node.GetComponent<InteractionTarget>() == null || !isVisible)
        {
            if (node.GetComponent<Renderer>() != null)
                node.GetComponent<Renderer>().enabled = isVisible;
            if (node.GetComponent<Collider>() != null)
                node.GetComponent<Collider>().enabled = isVisible;
        }

        for (int i = 0; i < node.transform.childCount; i++)
            this.SetVisibilityRecursively(node.transform.GetChild(i).gameObject, isVisible, keepDisabled);

        if (node.GetComponent<Renderer>() != null && keepDisabled) 
            node.GetComponent<Renderer>().enabled = false;
    }

    /*
     * Toggles Renderer- and Collider components in all children of this Level
     */
    public void SetVisibilityOfAllChildren(bool isVisible)
    {
        //Debug.Log(this.name + "->SetVisibilityOfAllChildren->" + isVisible);

        this.isActive = isVisible;

        this.SetVisibilityRecursively(this.gameObject, isVisible);
    }

    /*
     * Activates this Level
     */
    public void Activate()
    {
        //Debug.Log(this.name + "->Activate");

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
