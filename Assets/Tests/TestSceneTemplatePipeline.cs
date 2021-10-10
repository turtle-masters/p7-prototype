using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneTemplatePipeline : ISceneTemplatePipeline
{
    public void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
    {
        // ...
    }

    public void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
    {
        if (sceneTemplateAsset)
        {
            //Debug.Log($"After Template Pipeline {sceneTemplateAsset.name} scene: {scene} isAdditive: {isAdditive} sceneName: {sceneName}");
            /*IEnumerator gameObjectEnumerator = scene.GetRootGameObjects().GetEnumerator();
            do
            {
                GameObject currentGameObject = (GameObject)gameObjectEnumerator.Current;
                Level currentLevel = currentGameObject.GetComponent<Level>();
                if (currentGameObject != null) currentLevel.IsDebugging();
            }
            while (gameObjectEnumerator.MoveNext());*/
        }
    }

    public bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
    {
        return true;
    }
}
