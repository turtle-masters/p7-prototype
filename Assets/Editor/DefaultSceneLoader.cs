#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

/**
 * CREDIT: https://stackoverflow.com/a/48817315
 */
[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            if (EditorSceneManager.GetActiveScene().buildIndex == 0) return;

            EditorSceneManager.LoadScene(0);
            Level.totalSceneChanges = 0;
        }
    }
}
#endif